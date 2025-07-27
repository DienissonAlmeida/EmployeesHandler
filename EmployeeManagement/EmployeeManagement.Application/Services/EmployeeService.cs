using EmployeeManagement.Application.Commands;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Domain.Contracts;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;

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

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeCommand command, Guid currentUserId)
        {
            //var currentUser = await _repository.GetByIdAsync(currentUserId);
            //if (currentUser == null) throw new Exception("Usuário atual não encontrado.");

            var newRole = Enum.Parse<Role>(command.Role);
            //if (newRole > currentUser.Role)
            //    throw new Exception("Você não pode criar um funcionário com permissão superior à sua.");

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

            return MapToDto(entity);
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
