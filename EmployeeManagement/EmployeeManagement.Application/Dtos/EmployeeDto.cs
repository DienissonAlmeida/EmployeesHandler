namespace EmployeeManagement.Application.Dtos
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public List<string> PhoneNumbers { get; set; } = new();
        public Guid? ManagerId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; } = null!;
    }

}
