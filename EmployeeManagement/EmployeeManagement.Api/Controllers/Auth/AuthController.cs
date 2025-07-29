using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Domain.Dtos.Login;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasherService _passwordHasherService;


        public AuthController(IEmployeeService service, ITokenService tokenService, IPasswordHasherService passwordHasherService)
        {
            _service = service;
            _tokenService = tokenService;
            _passwordHasherService = passwordHasherService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var employee = await _service.GetByEmailAsync(request.Email);

            if (!_passwordHasherService.VerifyPassword(employee.Password, request.Password))
                return Unauthorized("Credenciais inválidas");

            var token = _tokenService.GenerateToken(employee);

            return Ok(new LoginResponse
            {
                Token = token,
                FirstName = employee.FirstName,
                Role = employee.Role
            });
        }
    }
}
