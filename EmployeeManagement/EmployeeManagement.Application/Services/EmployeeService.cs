using EmployeeManagement.Application.Commands;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Domain.Contracts;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;
using static EmployeeManagement.Domain.Dtos.CreateEmployeeResponseDto;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        //private readonly IPasswordHasher _passwordHasher;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
            //_passwordHasher = passwordHasher;
        }

        public async Task<CreateEmployeeResponse> CreateAsync(CreateEmployeeCommand command, Guid currentUserId)
        {
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
                PasswordHash = command.Password,
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

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateAsync(Guid id, CreateEmployeeCommand request)
        {
            var employeeToUpdate = await _repository.GetByIdAsync(id);

            employeeToUpdate!.UpdateProperties(request);

            _repository.UpdateAsync(employeeToUpdate);

            await _repository.SaveChangesAsync();
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


        // Implement GetByIdAsync, GetAllAsync, UpdateAsync, DeleteAsync simlarly
    }

}
