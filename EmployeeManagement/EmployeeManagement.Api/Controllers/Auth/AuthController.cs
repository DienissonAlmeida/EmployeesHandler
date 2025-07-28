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

        public AuthController(IEmployeeService service, ITokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var employee = await _service.GetByEmailAsync(request.Email);

            if (employee == null || employee.Password != request.Password)
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
