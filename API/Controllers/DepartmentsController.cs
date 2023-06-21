using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await departmentService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await departmentService.GetByIdAsync(id);
            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var foundDepartment = await departmentService.GetDetailsByIdAsync(id);
            if (foundDepartment is null)
                return NotFound("Department not found");

            return Ok(foundDepartment);
        }

        [HttpPost]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Add(DepartmentAddingDTO departmentToAdd)
        {
            var addedDepartment = await departmentService.AddAsync(departmentToAdd);

            return CreatedAtAction(nameof(GetById), new { id = addedDepartment.DepartmentId }, addedDepartment);
        }

        [HttpPost("addWorker")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> AddWorkerToDepartment(DepartmentWorkerAddingDTO departmentWorkerToAdd)
        {
            var isAdded = await departmentService.AddWorkerToDepartmentAsync(departmentWorkerToAdd);
            
            if (!isAdded)
                return BadRequest("Worker is already in the department or departmentId or workerId is invalid");

            return Ok("Worker successfully added to the department");
        }

        [HttpPut]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Update(DepartmentPlainDTO departmentToUpdate)
        {
            var updatedDepartment = await departmentService.UpdateAsync(departmentToUpdate);
            if (updatedDepartment == null)
                return NotFound();

            return Ok(updatedDepartment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await departmentService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return Ok("Department successfuly deleted");
        }
    }
}
