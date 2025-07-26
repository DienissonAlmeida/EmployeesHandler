using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Contracts
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task<Employee?> GetByIdAsync(Guid id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
