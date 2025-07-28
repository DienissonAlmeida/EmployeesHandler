using EmployeeManagement.Application.Commands;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Application.Validations;
using EmployeeManagement.Domain.Contracts;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;
using FluentAssertions;
using Moq;

namespace EmployeeManagement.Tests.Application.Services
{
    public class EmployeeServiceTest
    {
        private readonly Mock<IEmployeeRepository> _repositoryMock = new();
        private readonly EmployeeService _service;

        public EmployeeServiceTest()
        {
            _service = new EmployeeService(_repositoryMock.Object, new CreateEmployeeValidator(_repositoryMock.Object));
        }

        [Fact]
        public async Task Should_createAsync_successfull()
        {
            //arrange
            var request = new CreateEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "teste@teste",
                DocumentNumber = "123456789",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-20),
                PhoneNumbers = new List<string>() { "454564564" },
                Role = "Employee"
            };

            var currentId = Guid.NewGuid();
            _repositoryMock.Setup(x => x.GetRoleById(currentId)).ReturnsAsync(Role.Employee);

            // Act
            var result = await _service.CreateAsync(request, currentId);

            // Assert
            result.Employee.Should().BeEquivalentTo(request, opt => opt.Excluding(y => y.Password));

            _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>()), Times.Once);
            _repositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_getAllAsync_successfull()
        {
            //arrange
            var employees = new List<EmployeeDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@company.com",
                    DocumentNumber = "987654321",
                    PhoneNumbers = new List<string> { "123456789" },
                    BirthDate = DateTime.Now.AddYears(-30),
                    Role = "Employee"
                }
            };

            _repositoryMock.Setup(r => r.GetAllRoleBellowAndItself(It.IsAny<Role>(), It.IsAny<Guid>())).ReturnsAsync(employees);

            //act
            var result = await _service.GetAllAsync(Guid.NewGuid());

            //assert
            result.Should().BeEquivalentTo(employees);
            _repositoryMock.Verify(r => r.GetAllRoleBellowAndItself(It.IsAny<Role>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteAndSaveChanges()
        {
            var employeeId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.DeleteAsync(employeeId)).ReturnsAsync(1);

            await _service.DeleteAsync(employeeId);

            _repositoryMock.Verify(r => r.DeleteAsync(employeeId), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEmployeeAndSaveChanges()
        {
            //arrange
            var employeeId = Guid.NewGuid();
            var existingEmployee = new Employee
            {
                Id = employeeId,
                FirstName = "Old",
                LastName = "Name",
                Email = "old@email.com",
                DocumentNumber = "111111111",
                PhoneNumbers = new List<string> { "111111111" },
                BirthDate = DateTime.Now.AddYears(-40),
                Role = Role.Employee
            };

            var updateCommand = new CreateEmployeeCommand
            {
                FirstName = "New",
                LastName = "Name",
                Email = "new@email.com",
                DocumentNumber = "222222222",
                PhoneNumbers = new List<string> { "222222222" },
                BirthDate = DateTime.Now.AddYears(-30),
                Role = "Employee"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(employeeId)).ReturnsAsync(existingEmployee);

            //act
            await _service.UpdateAsync(employeeId, updateCommand);

            //assert
            _repositoryMock.Verify(r => r.GetByIdAsync(employeeId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Employee>(e =>
                CheckEmployeeUpdated(e, updateCommand)
            )), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        private static bool CheckEmployeeUpdated(Employee e, CreateEmployeeCommand updateCommand)
        {
            return e.FirstName == updateCommand.FirstName &&
                   e.LastName == updateCommand.LastName &&
                   e.Email == updateCommand.Email &&
                   e.DocumentNumber == updateCommand.DocumentNumber &&
                   e.PhoneNumbers.SequenceEqual(updateCommand.PhoneNumbers);
        }
    }
}
