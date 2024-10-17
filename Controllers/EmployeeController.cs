using JWT_Token.Bl.InterFace;
using JWT_Token.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneretJWT_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employee;

        public EmployeeController(IEmployee Iemployee)
        {
            _employee = Iemployee;
        }
        [HttpGet("GetEmployee")]
        public async Task<IActionResult>GetData()
        {
            var res = await _employee.GetEmployee();
            return Ok(res);
        }
        [HttpGet("GetByEmployeeID")]
        public async Task<IActionResult>GetByID(int id)
        {
            var res = await _employee.GetEmployeeByID(id);
            return Ok(res);
        }
        [HttpPost("postData")]
        public async Task<IActionResult>AddData(EmployeeDto employeeDto)
        {
            var res =await _employee.AddEmployee(employeeDto);
            return Ok(res);
        }
        [HttpPut("UpdateData")]
        public async Task<IActionResult>UpdateEmpData(EmployeeDto employeeDto)
        {
            var res =await _employee.UpdateEmployee(employeeDto);
            return Ok(res);
        }
        [HttpDelete("Delete Data")]
        public async Task<IActionResult>DeleteData(int id)
        {
            await _employee.DeleteEmployee(id);
            return Ok("Data Deleted");
        }
    }
}
