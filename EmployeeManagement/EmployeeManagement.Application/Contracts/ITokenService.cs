using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(EmployeeDto employee);
    }
}
