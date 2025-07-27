using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Domain.Dtos
{
    public class CreateEmployeeResponseDto
    {
        public class CreateEmployeeResponse
        {
            public bool Success { get; set; }
            public string? ErrorMessage { get; set; }
            public EmployeeDto? Employee { get; set; }
        }
    }
}
