using Azure;
using Employee.DAL.Interfaces;
using Employee.Domain.Models;
using Employee.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeRepository employeeRepository, 
            IEmployeeService employeeService
            )
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }

        [HttpGet("find-employees-company/company/{companyName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FindEmployeesCompany(string companyName)
        {
            var response = await _employeeService.GetEmployeesByCompanyServiceAsync(companyName);

            if (response.StatusCode == 400)
                return BadRequest(response);

            if (response.StatusCode == 500)
                return Problem(response.Description);

            return Ok(response);
        }

        [HttpGet("find-employees-deportment/company/{companyName}/deportment/{deportmentName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FindEmployeesDeportment(string companyName, string deportmentName)
        {
            var response = await _employeeService.GetEmployeesByDepartmentServiceAsync(companyName,deportmentName);

            if (response.StatusCode == 400)
                return BadRequest(response);

            if (response.StatusCode == 500)
                return Problem(response.Description);

            return Ok(response);
        }

        [HttpPatch("update-employee/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(int id, [FromBody] JsonPatchDocument<EmployeeModel> patchDocument)
        {
            try
            {
                if (id <= 0 || patchDocument == null)
                    return BadRequest("check the data");

                var model = await _employeeRepository.FindEmployeeById(id);

                if (model == null)
                    return NotFound($"check the data: data is {id}");

                patchDocument.ApplyTo(model);

                var request = await _employeeRepository.UpdateEmployeeAsync(id, model);

                return Ok(request);
            }
            catch(Exception ex)
            {
                return Problem("Server error");
            }
        }

        [HttpDelete("delete-employee/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _employeeService.EmployeeDeleteServiceAsync(id);

            if (response.StatusCode == 400)
                return BadRequest(response);

            if (response.StatusCode == 404)
                return NotFound(response);

            if (response.StatusCode == 500)
                return Problem(response.Description);

            return Ok(response);
        }

        [HttpPost("create-employee/{companyName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateEmployee(string companyName, [FromBody] EmployeeModel employeeModel)
        {
            var response = await _employeeService.EmployeeCreateServiceAsync(companyName,employeeModel);
            if (response.StatusCode == 400)
                return BadRequest(response);

            if (response.StatusCode == 500)
                return Problem(response.Description);

            return Ok(response);
        }

    }
}
