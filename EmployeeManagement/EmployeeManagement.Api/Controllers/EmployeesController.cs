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
            //return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            if (result.Success)
                return Created("id", result);

            return BadRequest(result);
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
