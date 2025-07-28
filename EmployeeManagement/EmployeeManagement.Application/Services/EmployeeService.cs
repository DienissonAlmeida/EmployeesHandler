using EmployeeManagement.Application.Commands;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Application.Validations;
using EmployeeManagement.Domain.Contracts;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using static EmployeeManagement.Domain.Dtos.CreateEmployeeResponseDto;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly CreateEmployeeValidator _validator;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository repository,
            CreateEmployeeValidator validator,
            IPasswordHasherService passwordHasherService,
            ILogger<EmployeeService> logger)
        {
            _repository = repository;
            _validator = validator;
            _passwordHasherService = passwordHasherService;
            _logger = logger;
        }

        public async Task<CreateEmployeeResponse> CreateAsync(CreateEmployeeCommand command, Guid currentUserId)
        {
            _logger.LogInformation("Starting process of employee creation");

            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(";", validationResult.Errors.Select(x => x.ErrorMessage));
                _logger.LogError($"Errors in validation: {errors}");

                return new CreateEmployeeResponse()
                {
                    Success = false,
                    ErrorMessage = errors
                };
            }

            var currentUserRole = await _repository.GetRoleById(currentUserId);

            var newRole = Enum.Parse<Role>(command.Role);

            if (newRole > currentUserRole)
            {
                var error = "You cannot create an employee with a higher permission than yours.";
                _logger.LogError("Error in validation: {0}", error);

                return new CreateEmployeeResponse()
                {
                    Success = false,
                    ErrorMessage = error
                };
            }

            var entity = new Employee
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                DocumentNumber = command.DocumentNumber,
                PhoneNumbers = command.PhoneNumbers,
                ManagerId = command.ManagerId,
                PasswordHash = _passwordHasherService.HashPassword(command.Password!),
                BirthDate = command.BirthDate,
                Role = newRole
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var dto = MapToDto(entity);

            _logger.LogInformation($"Employee registered successfully: {dto.FirstName}");

            return new CreateEmployeeResponse()
            {
                Success = true,
                Employee = dto
            };
        }

        public async Task<List<EmployeeDto>> GetAllAsync(Guid id)
        {
            _logger.LogInformation("Getting all employees for employeeId {EmployeeId}", id);
            var currentEmployeeRole = await _repository.GetRoleById(id);
            var result = await _repository.GetAllRoleBellowAndItself(currentEmployeeRole, id);
            _logger.LogInformation("Retrieved {Count} employees for employeeId {EmployeeId}", result.Count, id);
            return result;
        }
        public async Task<List<EmployeeDto>> GetAllToLinkAsync(Guid id)
        {
            _logger.LogInformation("Getting all employees to link for employeeId {EmployeeId}", id);
            var currentEmployeeRole = await _repository.GetRoleById(id);
            var result = await _repository.GetAllRoleAboveAndItself(currentEmployeeRole, id);
            _logger.LogInformation("Retrieved {Count} employees to link for employeeId {EmployeeId}", result.Count, id);
            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting employee with id {EmployeeId}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Deleted employee with id {EmployeeId}", id);
        }

        public async Task UpdateAsync(Guid id, CreateEmployeeCommand request)
        {
            _logger.LogInformation("Updating employee with id {EmployeeId}", id);
            var employeeToUpdate = await _repository.GetByIdAsync(id);

            var hashedPassword = request.Password is not null
                ? _passwordHasherService.HashPassword(request.Password!)
                : employeeToUpdate!.PasswordHash;

            employeeToUpdate!.UpdateProperties(request, hashedPassword);

            _repository.UpdateAsync(employeeToUpdate);

            await _repository.SaveChangesAsync();
            _logger.LogInformation("Updated employee with id {EmployeeId}", id);
        }

        public async Task<EmployeeDto> GetByEmailAsync(string requestEmail)
        {
            _logger.LogInformation("Getting employee by email {Email}", requestEmail);
            var employee = await _repository.GetByEmail(requestEmail);
            if (employee != null)
                _logger.LogInformation("Found employee with email {Email}", requestEmail);
            else
                _logger.LogWarning("No employee found with email {Email}", requestEmail);
            return employee!;
        }

        private EmployeeDto MapToDto(Employee e) => new()
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            DocumentNumber = e.DocumentNumber,
            PhoneNumbers = e.PhoneNumbers,
            ManagerId = e.ManagerId,
            BirthDate = e.BirthDate,
            Role = e.Role.ToString()
        };

    }

}
