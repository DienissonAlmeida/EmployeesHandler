using EmployeeManagement.Application.Commands;
using EmployeeManagement.Domain.Dtos;
using static EmployeeManagement.Domain.Dtos.CreateEmployeeResponseDto;

namespace EmployeeManagement.Application.Contracts
{
    public interface IEmployeeService
    {
        //We can use a result pattern here, but for simplicity, we will return the DTO directly.
        Task<CreateEmployeeResponse> CreateAsync(CreateEmployeeCommand command, Guid currentUserId);
        Task<List<EmployeeDto>> GetAllAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Guid id, CreateEmployeeCommand request);
        Task<EmployeeDto> GetByEmailAsync(string requestEmail);

        Task<List<EmployeeDto>> GetAllToLinkAsync(Guid id);
    }
}
