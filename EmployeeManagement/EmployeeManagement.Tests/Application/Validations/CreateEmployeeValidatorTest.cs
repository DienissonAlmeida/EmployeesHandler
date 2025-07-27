using EmployeeManagement.Application.Commands;
using EmployeeManagement.Application.Validations;
using EmployeeManagement.Domain.Contracts;
using FluentAssertions;
using Moq;

namespace EmployeeManagement.Tests.Application.Validations
{
    public class CreateEmployeeCommandValidatorTests
    {
        private readonly CreateEmployeeValidator _validator;
        private readonly Mock<IEmployeeRepository> _repositoryMock;

        public CreateEmployeeCommandValidatorTests()
        {
            _repositoryMock = new Mock<IEmployeeRepository>();
            _validator = new CreateEmployeeValidator(_repositoryMock.Object);


        }

        [Theory]
        [InlineData("", "First name is required.")]
        [InlineData(null, "First name is required.")]
        [InlineData("FirstName", null)]
        public async Task Should_Have_Error_When_FirstName_Is_Empty(string firstName, string? messageError)
        {
            var command = new CreateEmployeeCommand
            {
                FirstName = firstName,
                LastName = "Silva",
                Email = "test@example.com",
                DocumentNumber = "12345678900",
                Password = "Secure@123",
                BirthDate = DateTime.Now.AddYears(-20),
            };

            var result = await _validator.ValidateAsync(command);

            if (messageError is not null)
            {
                result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
                result.Errors[0].ErrorMessage.Should().Be(messageError);
                result.IsValid.Should().BeFalse();
            }
            else
                result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null, "Email is required.")]
        [InlineData("", "A valid email is required.")]
        [InlineData("plainaddress", "A valid email is required.")]
        [InlineData("test@.com", null)] // Considered valid by FluentValidation
        [InlineData("test@domain", null)] // Considered valid by FluentValidation
        [InlineData("test@domain.", null)] // Considered valid by FluentValidation
        [InlineData("test@domain.com", null)]
        [InlineData("user.name+tag@sub.domain.com", null)]
        public async Task Should_Validate_Email_Scenarios(string email, string? expectedError)
        {
            var command = new CreateEmployeeCommand
            {
                FirstName = "João",
                LastName = "Silva",
                Email = email,
                DocumentNumber = "12345678900",
                Password = "Secure@123",
                BirthDate = DateTime.Now.AddYears(-20),
            };

            var result = await _validator.ValidateAsync(command);

            if (expectedError is not null)
            {
                result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage == expectedError);
                result.IsValid.Should().BeFalse();
            }
            else
            {
                result.Errors.Should().NotContain(e => e.PropertyName == "Email");
                result.IsValid.Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(null, false, "Document number is required.")]
        [InlineData("", false, "Document number is required.")]
        [InlineData("12345678900", true, "Document number must be unique.")]
        [InlineData("12345678900", false, null)]
        [InlineData("00000000000", true, "Document number must be unique.")]
        [InlineData("00000000000", false, null)]
        public async Task Should_Validate_DocumentNumber_Scenarios(string documentNumber, bool exists, string? expectedError)
        {
            _repositoryMock.Setup(r => r.ExistsByDocumentAsync(documentNumber)).ReturnsAsync(exists);

            var command = new CreateEmployeeCommand
            {
                FirstName = "João",
                LastName = "Silva",
                Email = "test@example.com",
                DocumentNumber = documentNumber,
                Password = "Secure@123",
                BirthDate = DateTime.Now.AddYears(-20),
            };

            var result = await _validator.ValidateAsync(command);

            if (expectedError is not null)
            {
                result.Errors.Should().Contain(e => e.PropertyName == "DocumentNumber" && e.ErrorMessage == expectedError);
                result.IsValid.Should().BeFalse();
            }
            else
            {
                result.Errors.Should().NotContain(e => e.PropertyName == "Document");
                result.IsValid.Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(null, "Password is required.")]
        [InlineData("", "Password is required.")]
        [InlineData("123", "Password must be at least 6 characters.")]
        [InlineData("abcde", "Password must be at least 6 characters.")]
        [InlineData("abcdef", null)]
        [InlineData("Secure@123", null)]
        public async Task Should_Validate_Password_Scenarios(string password, string? expectedError)
        {
            var command = new CreateEmployeeCommand
            {
                FirstName = "João",
                LastName = "Silva",
                Email = "test@example.com",
                DocumentNumber = "12345678900",
                Password = password,
                BirthDate = DateTime.Now.AddYears(-20),
            };

            var result = await _validator.ValidateAsync(command);

            if (expectedError is not null)
            {
                result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage == expectedError);
                result.IsValid.Should().BeFalse();
            }
            else
            {
                result.Errors.Should().NotContain(e => e.PropertyName == "Password");
                result.IsValid.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Should_Pass_Validation_When_phoneNumbers_are_Valid()
        {
            _repositoryMock.Setup(r => r.ExistsByDocumentAsync("12345678900")).ReturnsAsync(false);

            var command = new CreateEmployeeCommand
            {
                FirstName = "João",
                LastName = "Silva",
                Email = "test@example.com",
                DocumentNumber = "12345678900",
                Password = "Secure@123",
                PhoneNumbers = new List<string> { "11988887777" },
                BirthDate = DateTime.Now.AddYears(-20),
            };

            var result = await _validator.ValidateAsync(command);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null, "Phone number cannot be empty.")]
        [InlineData("", "Phone number cannot be empty.")]
        [InlineData("11988887777", null)]
        [InlineData("21999998888", null)]
        public async Task Should_Validate_PhoneNumbers_Scenarios(string phoneNumber, string? expectedError)
        {
            var command = new CreateEmployeeCommand
            {
                FirstName = "João",
                LastName = "Silva",
                Email = "test@example.com",
                DocumentNumber = "12345678900",
                Password = "Secure@123",
                PhoneNumbers = new List<string> { phoneNumber },
                BirthDate = DateTime.Now.AddYears(-20),
            };

            var result = await _validator.ValidateAsync(command);

            if (expectedError is not null)
            {
                result.Errors.Should().Contain(e => e.PropertyName == "PhoneNumbers[0]" && e.ErrorMessage == expectedError);
                result.IsValid.Should().BeFalse();
            }
            else
            {
                result.Errors.Should().NotContain(e => e.PropertyName == "PhoneNumbers[0]");
                result.IsValid.Should().BeTrue();
            }
        }

        [Theory]
        [InlineData("2008-01-01", "Employee must be at least 18 years old.")]
        [InlineData("2020-12-31", "Employee must be at least 18 years old.")]
        [InlineData("2000-01-01", null)]
        [InlineData("1990-05-20", null)]
        public async Task Should_Validate_BirthDate_Scenarios(string birthDateString, string? expectedError)
        {
            var birthDate = DateTime.Parse(birthDateString);
            var command = new CreateEmployeeCommand
            {
                FirstName = "João",
                LastName = "Silva",
                Email = "test@example.com",
                DocumentNumber = "12345678900",
                Password = "Secure@123",
                BirthDate = birthDate
            };

            var result = await _validator.ValidateAsync(command);

            if (expectedError is not null)
            {
                result.Errors.Should().Contain(e => e.PropertyName == "BirthDate" && e.ErrorMessage == expectedError);
                result.IsValid.Should().BeFalse();
            }
            else
            {
                result.Errors.Should().NotContain(e => e.PropertyName == "BirthDate");
                result.IsValid.Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(true, false, "Manager must be a valid employee.")]
        [InlineData(true, true, null)]
        [InlineData(false, false, null)] // ManagerId is null, so no validation
        public async Task Should_Validate_ManagerId_Scenarios(bool hasManagerId, bool exists, string? expectedError)
        {
            Guid? managerId = hasManagerId ? Guid.NewGuid() : null;
            if (hasManagerId)
                _repositoryMock.Setup(r => r.ExistsByIdAsync(managerId.Value)).ReturnsAsync(exists);

            var command = new CreateEmployeeCommand
            {
                FirstName = "João",
                LastName = "Silva",
                Email = "test@example.com",
                DocumentNumber = "12345678900",
                Password = "Secure@123",
                BirthDate = DateTime.Now.AddYears(-20),
                ManagerId = managerId
            };

            var result = await _validator.ValidateAsync(command);

            if (expectedError is not null)
            {
                result.Errors.Should().Contain(e => e.PropertyName == "ManagerId.Value" && e.ErrorMessage == expectedError);
                result.IsValid.Should().BeFalse();
            }
            else
            {
                result.Errors.Should().NotContain(e => e.PropertyName == "ManagerId.Value");
                result.IsValid.Should().BeTrue();
            }
        }
    }

}
