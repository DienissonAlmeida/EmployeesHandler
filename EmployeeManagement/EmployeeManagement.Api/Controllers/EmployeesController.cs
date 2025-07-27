using EmployeeManagement.Application.Commands;
using EmployeeManagement.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {

        //TODO: We can use mrdiatR for better separation of concerns
        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand request)
        {
            // Simulando ID do usuário atual (autenticação virá depois)
            var currentUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            var result = await _service.CreateAsync(request, currentUserId);
            //return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);

            return Created("id", result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            //var result = await _service.GetByIdAsync(id);
            //if (result == null) return NotFound();
            //return Ok(result);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateEmployeeCommand request)
        {
            await _service.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
