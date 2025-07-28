using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Commands
{
    public class CreateEmployeeCommand
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public List<string> PhoneNumbers { get; set; } = new();
        public Guid? ManagerId { get; set; }
        public string? Password { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Role { get; set; } = null!;
    }
}
