using EmployeeManagement.Application.Commands;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Application.Validations;
using EmployeeManagement.Domain.Contracts;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using static EmployeeManagement.Domain.Dtos.CreateEmployeeResponseDto;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly CreateEmployeeValidator _validator;
        private readonly IPasswordHasherService _passwordHasherService;

        public EmployeeService(IEmployeeRepository repository, CreateEmployeeValidator validator, IPasswordHasherService passwordHasherService)
        {
            _repository = repository;
            _validator = validator;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<CreateEmployeeResponse> CreateAsync(CreateEmployeeCommand command, Guid currentUserId)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
                return new CreateEmployeeResponse()
                {
                    Success = false,
                    ErrorMessage = string.Join(";", validationResult.Errors.Select(x => x.ErrorMessage))
                };

            var currentUserRole = await _repository.GetRoleById(currentUserId);

            var newRole = Enum.Parse<Role>(command.Role);

            if (newRole > currentUserRole)
                return new CreateEmployeeResponse()
                {
                    Success = false,
                    ErrorMessage = "Você não pode criar um funcionário com permissão superior à sua."
                };

            var entity = new Employee
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                DocumentNumber = command.DocumentNumber,
                PhoneNumbers = command.PhoneNumbers,
                ManagerId = command.ManagerId,
                PasswordHash = _passwordHasherService.HashPassword(command.Password),
                BirthDate = command.BirthDate,
                Role = newRole
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var dto = MapToDto(entity);

            return new CreateEmployeeResponse()
            {
                Success = true,
                Employee = dto
            };
        }

        public async Task<List<EmployeeDto>> GetAllAsync(Guid id)
        {
            var currentEmployeeRole = await _repository.GetRoleById(id);

            return await _repository.GetAllRoleBellowAndItself(currentEmployeeRole, id);
        }
        public async Task<List<EmployeeDto>> GetAllToLinkAsync(Guid id)
        {
            var currentEmployeeRole = await _repository.GetRoleById(id);

            return await _repository.GetAllRoleAboveAndItself(currentEmployeeRole, id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateAsync(Guid id, CreateEmployeeCommand request)
        {
            var employeeToUpdate = await _repository.GetByIdAsync(id);

            var hashedPassword = request.Password is not null ?
                _passwordHasherService.HashPassword(request.Password!)
                : employeeToUpdate!.PasswordHash;

            employeeToUpdate!.UpdateProperties(request, hashedPassword);

            _repository.UpdateAsync(employeeToUpdate);

            await _repository.SaveChangesAsync();
        }


        public Task<EmployeeDto> GetByEmailAsync(string requestEmail)
        {
            return _repository.GetByEmail(requestEmail);
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
