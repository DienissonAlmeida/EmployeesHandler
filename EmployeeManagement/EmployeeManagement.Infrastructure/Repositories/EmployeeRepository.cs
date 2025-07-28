using System.Reflection.Metadata;
using EmployeeManagement.Domain.Contracts;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            //TODO: Maybe split saveChangesAsync into a separate method to handle transaction management
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }
        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<EmployeeDto>> GetAllRoleBellowAndItself(Role role, Guid currentUserId)
        {
            return await _context.Employees.Where(e =>
                        e.Role < role || // above role
                        e.Id == currentUserId
                )
                .Select(x => new EmployeeDto()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BirthDate = x.BirthDate,
                    DocumentNumber = x.DocumentNumber,
                    Email = x.Email,
                    PhoneNumbers = x.PhoneNumbers,
                    Role = x.Role.ToString(),
                    ManagerId = x.ManagerId,
                })
                .ToListAsync();
        }
        public async Task<List<EmployeeDto>> GetAllRoleAboveAndItself(Role role, Guid currentUserId)
        {
            return await _context.Employees.Where(e =>
                    e.Role == Role.Director || e.Role == Role.Leader
                )
                .Select(x => new EmployeeDto()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                })
                .ToListAsync();
        }

        public void UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _context.Employees.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByDocumentAsync(string document)
        {
            return await _context.Employees.AnyAsync(x => x.DocumentNumber == document);

        }

        public async Task<bool> ExistsByIdAsync(Guid managerId)
        {
            return await _context.Employees.AnyAsync(x => x.Id == managerId);
        }
        public async Task<Role> GetRoleById(Guid id)
        {
            return await _context.Employees
                .Where(x => x.Id == id)
                .Select(x => x.Role)
                .SingleOrDefaultAsync();
        }

        public async Task<EmployeeDto> GetByEmail(string requestEmail)
        {
            return await _context.Employees
                .Where(x => x.Email == requestEmail)
                .Select(x => new EmployeeDto()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BirthDate = x.BirthDate,
                    DocumentNumber = x.DocumentNumber,
                    Email = x.Email,
                    PhoneNumbers = x.PhoneNumbers,
                    Role = x.Role.ToString(),
                    ManagerId = x.ManagerId,
                    Password = x.PasswordHash
                })
                .SingleAsync();
        }
    }


}
