using DotNetWebApiPractise.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetWebApiPractise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllEmployee()
        {
            var a = _employeeService.GetAllEmployees();
            return Ok(a);
        }
    }
}
