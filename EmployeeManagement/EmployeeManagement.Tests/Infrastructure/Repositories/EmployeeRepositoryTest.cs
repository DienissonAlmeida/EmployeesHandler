using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Tests.Infrastructure.Repositories
{
    public class EmployeeRepositoryTest
    {
        private readonly SqliteConnection _connection;
        private readonly EmployeeDbContext _context;
        private readonly EmployeeRepository _repository;

        public EmployeeRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EmployeeDbContext(options);
            _repository = new EmployeeRepository(_context);

        }

        [Fact]
        public async Task AddAsync_ShouldAddEmployee()
        {
            // arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@user.com",
                DocumentNumber = "123456789",
                PhoneNumbers = new List<string> { "123456789" },
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Role = Role.Employee,
                PasswordHash = "hash"
            };

            // act
            await _repository.AddAsync(employee);

            // assert
            var found = await _context.Employees.FindAsync(employee.Id);
            found.Should().NotBeNull();
            found!.Email.Should().Be("test@user.com");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmployee_WhenExists()
        {
            // arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@company.com",
                DocumentNumber = "987654321",
                PhoneNumbers = new List<string> { "123456789" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Employee,
                PasswordHash = "hash"
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // act
            var result = await _repository.GetByIdAsync(employee.Id);

            // assert
            result.Should().NotBeNull();
            result!.Email.Should().Be(employee.Email);
        }

        [Fact]
        public async Task ExistsByDocumentAsync_ShouldReturnTrue_WhenExists()
        {
            // arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@company.com",
                DocumentNumber = "987654321",
                PhoneNumbers = new List<string> { "123456789" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Employee,
                PasswordHash = "hash"
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // act
            var exists = await _repository.ExistsByDocumentAsync("987654321");

            // assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsByDocumentAsync_ShouldReturnFalse_WhenNotExists()
        {
            // act
            var exists = await _repository.ExistsByDocumentAsync("notfound");

            // assert
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsByIdAsync_ShouldReturnTrue_WhenExists()
        {
            // arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@company.com",
                DocumentNumber = "987654321",
                PhoneNumbers = new List<string> { "123456789" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Employee,
                PasswordHash = "hash"
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // act
            var exists = await _repository.ExistsByIdAsync(employee.Id);

            // assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsByIdAsync_ShouldReturnFalse_WhenNotExists()
        {
            // act
            var exists = await _repository.ExistsByIdAsync(Guid.NewGuid());

            // assert
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task GetRoleById_ShouldReturnRole_WhenExists()
        {
            // arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@company.com",
                DocumentNumber = "987654321",
                PhoneNumbers = new List<string> { "123456789" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Director,
                PasswordHash = "hash"
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // act
            var role = await _repository.GetRoleById(employee.Id);

            // assert
            role.Should().Be(Role.Director);
        }

        [Fact(Skip = "ExecuteDeleteAsync is not supported with in memory approach ")]
        public async Task DeleteAsync_ShouldRemoveEmployee()
        {
            // arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@company.com",
                DocumentNumber = "987654321",
                PhoneNumbers = new List<string> { "123456789" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Employee,
                PasswordHash = "hash"
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // act
            var deletedCount = await _repository.DeleteAsync(employee.Id);

            // assert
            deletedCount.Should().Be(1);
            var found = await _context.Employees.FindAsync(employee.Id);
            found.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEmployee()
        {
            // arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@company.com",
                DocumentNumber = "987654321",
                PhoneNumbers = new List<string> { "123456789" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Employee,
                PasswordHash = "hash"
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // act
            employee.FirstName = "Updated";
            _repository.UpdateAsync(employee);
            await _context.SaveChangesAsync();

            // assert
            var found = await _context.Employees.FindAsync(employee.Id);
            found!.FirstName.Should().Be("Updated");
        }

        [Fact]
        public async Task GetAllRoleBellowAndItself_ShouldReturnCorrectEmployees()
        {
            // arrange
            var director = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Director",
                LastName = "Boss",
                Email = "director@company.com",
                DocumentNumber = "111",
                PhoneNumbers = new List<string> { "111" },
                BirthDate = DateTime.UtcNow.AddYears(-50),
                Role = Role.Director,
                PasswordHash = "hash"
            };
            var leader = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Leader",
                LastName = "Lead",
                Email = "leader@company.com",
                DocumentNumber = "222",
                PhoneNumbers = new List<string> { "222" },
                BirthDate = DateTime.UtcNow.AddYears(-40),
                Role = Role.Leader,
                PasswordHash = "hash"
            };
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Employee",
                LastName = "Worker",
                Email = "employee@company.com",
                DocumentNumber = "333",
                PhoneNumbers = new List<string> { "333" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Employee,
                PasswordHash = "hash"
            };
            _context.Employees.AddRange(director, leader, employee);
            await _context.SaveChangesAsync();

            // act
            var result = await _repository.GetAllRoleBellowAndItself(Role.Leader, leader.Id);

            // assert
            result.Should().ContainSingle(x => x.Id == leader.Id);
            result.Should().ContainSingle(x => x.Id == employee.Id);
            result.Should().NotContain(x => x.Id == director.Id);
        }

        [Fact]
        public async Task GetAllRoleAboveAndItself_ShouldReturnDirectorsAndLeaders()
        {
            // arrange
            var director = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Director",
                LastName = "Boss",
                Email = "director@company.com",
                DocumentNumber = "111",
                PhoneNumbers = new List<string> { "111" },
                BirthDate = DateTime.UtcNow.AddYears(-50),
                Role = Role.Director,
                PasswordHash = "hash"
            };
            var leader = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Leader",
                LastName = "Lead",
                Email = "leader@company.com",
                DocumentNumber = "222",
                PhoneNumbers = new List<string> { "222" },
                BirthDate = DateTime.UtcNow.AddYears(-40),
                Role = Role.Leader,
                PasswordHash = "hash"
            };
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Employee",
                LastName = "Worker",
                Email = "employee@company.com",
                DocumentNumber = "333",
                PhoneNumbers = new List<string> { "333" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Employee,
                PasswordHash = "hash"
            };
            _context.Employees.AddRange(director, leader, employee);
            await _context.SaveChangesAsync();

            // act
            var result = await _repository.GetAllRoleAboveAndItself(Role.Employee, employee.Id);

            // assert
            result.Should().Contain(x => x.Id == director.Id);
            result.Should().Contain(x => x.Id == leader.Id);
            result.Should().NotContain(x => x.Id == employee.Id);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnEmployeeDto_WhenExists()
        {
            // arrange
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@company.com",
                DocumentNumber = "987654321",
                PhoneNumbers = new List<string> { "123456789" },
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Role = Role.Employee,
                PasswordHash = "hash"
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // act
            var result = await _repository.GetByEmail(employee.Email);

            // assert
            result.Should().NotBeNull();
            result.Email.Should().Be(employee.Email);
            result.FirstName.Should().Be(employee.FirstName);
        }
    }
}