using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            return await _context.Employees
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
                .ToListAsync();
        }

        public void UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            //var employee = await _context.Employees.FindAsync(id);
            //if (employee != null)
            //{
            //    _context.Employees.Remove(employee);
            //    await _context.SaveChangesAsync();
            //}

            return await _context.Employees.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
