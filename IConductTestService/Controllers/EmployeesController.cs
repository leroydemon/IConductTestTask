using Microsoft.AspNetCore.Mvc;
using IConductTestService.Dtos;
using IConductTestService.Interfaces;

namespace IConductTestService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }

        // 1. GET /api/employees/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id, CancellationToken cancellationToken)
        {
            var employee = await _service.GetEmployeeTreeAsync(id, cancellationToken);

            if (employee == null)
                return NotFound();

            return Ok(employee);  
        }

        // 2. POST /api/employees/{id}/enable?enable=true
        [HttpPost("{id:int}/enable")]
        public async Task<IActionResult> EnableEmployee(int id, bool enable, CancellationToken cancellationToken)
        {
            var result = await _service.SetEmployeeEnableAsync(id, enable, cancellationToken);

            return result switch
            {
                SetEmployeeEnableResult.NotFound => NotFound(),
                SetEmployeeEnableResult.Success => NoContent(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
    }
}

