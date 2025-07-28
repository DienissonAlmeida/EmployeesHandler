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

        [HttpPost("{id}")]
        public async Task<IActionResult> Create(Guid id, [FromBody] CreateEmployeeCommand request)
        {
            var result = await _service.CreateAsync(request, id);
            if (result.Success)
                return Created("id", result);

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            var result = await _service.GetAllAsync(id);
            return Ok(result);
        }

        [HttpGet("link/{id}")]
        public async Task<IActionResult> GetAllEmployesToLink(Guid id)
        {
            var result = await _service.GetAllToLinkAsync(id);
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
