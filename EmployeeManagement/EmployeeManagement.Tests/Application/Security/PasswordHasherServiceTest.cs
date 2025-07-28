using EmployeeManagement.Application.Security;
using FluentAssertions;

namespace EmployeeManagement.Tests.Application.Security
{
    public class PasswordHasherServiceTest
    {
        private readonly PasswordHasherService _service = new();

        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // arrange
            var password = "MySecurePassword123!";

            // act
            var hash = _service.HashPassword(password);

            // assert
            hash.Should().NotBeNullOrWhiteSpace();
            hash.Should().NotBe(password);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            // arrange
            var password = "MySecurePassword123!";
            var hash = _service.HashPassword(password);

            // act
            var result = _service.VerifyPassword(hash, password);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            // arrange
            var password = "MySecurePassword123!";
            var wrongPassword = "WrongPassword!";
            var hash = _service.HashPassword(password);

            // act
            var result = _service.VerifyPassword(hash, wrongPassword);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForEmptyPassword()
        {
            // arrange
            var password = "MySecurePassword123!";
            var hash = _service.HashPassword(password);

            // act
            var result = _service.VerifyPassword(hash, "");

            // assert
            result.Should().BeFalse();
        }
    }
}