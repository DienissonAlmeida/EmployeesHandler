using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.Commands;
using EmployeeManagement.Application.Dtos;

namespace EmployeeManagement.Application.Contracts
{
    public interface IEmployeeService
    {
        //We can use a result pattern here, but for simplicity, we will return the DTO directly.
        Task<EmployeeDto> CreateAsync(CreateEmployeeCommand request, Guid currentUserId);
        //Task<EmployeeDto?> GetByIdAsync(Guid id);
        //Task<IEnumerable<EmployeeDto>> GetAllAsync();
        //Task DeleteAsync(Guid id);
        //Task UpdateAsync(Guid id, CreateEmployeeCommand request);
    }
}
