using EmployeeManagement.Application.Commands;
using EmployeeManagement.Domain.Dtos;

namespace EmployeeManagement.Application.Contracts
{
    public interface IEmployeeService
    {
        //We can use a result pattern here, but for simplicity, we will return the DTO directly.
        Task<EmployeeDto> CreateAsync(CreateEmployeeCommand request, Guid currentUserId);
        Task<List<EmployeeDto>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Guid id, CreateEmployeeCommand request);
    }
}
