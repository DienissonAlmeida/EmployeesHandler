using System.Data;
using EmployeeManagement.Application.Commands;

namespace EmployeeManagement.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public List<string> PhoneNumbers { get; set; } = new();
        public Guid? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime BirthDate { get; set; }

        public Role Role { get; set; } 


        public void UpdateProperties(CreateEmployeeCommand request)
        {
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;
            PhoneNumbers = request.PhoneNumbers;
            DocumentNumber = request.DocumentNumber;
            ManagerId = request.ManagerId;
            PasswordHash = request.Password;
            Role = (Role)Enum.Parse(typeof(Role), request.Role);
        }
    }

    public enum Role
    {
        Employee = 1,
        Leader = 2,
        Director = 3
    }
}
