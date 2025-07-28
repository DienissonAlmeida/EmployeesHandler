using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Contracts
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task<Employee?> GetByIdAsync(Guid id);
        Task<List<EmployeeDto>> GetAllRoleBellowAndItself(Role role, Guid currentUserId);
        void UpdateAsync(Employee employee);
        Task<int> DeleteAsync(Guid id);
        Task SaveChangesAsync();
        Task<bool> ExistsByDocumentAsync(string document);
        Task<bool> ExistsByIdAsync(Guid managerId);
        Task<Role> GetRoleById(Guid id);
        Task<EmployeeDto> GetByEmail(string requestEmail);
        Task<List<EmployeeDto>> GetAllRoleAboveAndItself(Role role, Guid currentUserId);
    }
}
