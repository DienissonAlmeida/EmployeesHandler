using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Dtos;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace EmployeeManagement.Tests.Application.Services
{
    public class TokenServiceTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TokenService _tokenService;

        public TokenServiceTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_test_key_1234567890");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            _tokenService = new TokenService(_configurationMock.Object);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken()
        {
            var employee = new EmployeeDto
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Role = "Employee"
            };

            var token = _tokenService.GenerateToken(employee);

            token.Should().NotBeNullOrWhiteSpace();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            jwtToken.Issuer.Should().Be("TestIssuer");
            jwtToken.Audiences.Should().Contain("TestAudience");
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == employee.Id.ToString());
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == employee.Email);
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == employee.FirstName);
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == employee.Role);
        }
    }
}