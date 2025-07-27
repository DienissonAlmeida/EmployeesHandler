using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Contracts
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task<Employee?> GetByIdAsync(Guid id);
        Task<List<EmployeeDto>> GetAllAsync();
        void UpdateAsync(Employee employee);
        Task<int> DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
