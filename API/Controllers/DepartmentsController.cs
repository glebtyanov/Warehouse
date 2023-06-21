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
        private readonly ILogger<DepartmentsController> logger;

        public DepartmentsController(DepartmentService departmentService, ILogger<DepartmentsController> logger)
        {
            this.departmentService = departmentService;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Retrieving all departments");
            var departments = await departmentService.GetAllAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation("Retrieving department with ID {Id}", id);
            var department = await departmentService.GetByIdAsync(id);
            if (department == null)
            {
                logger.LogWarning("Department with ID {Id} not found", id);
                return NotFound();
            }

            return Ok(department);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving department details for ID {Id}", id);
            var foundDepartment = await departmentService.GetDetailsByIdAsync(id);
            if (foundDepartment is null)
            {
                logger.LogWarning("Department with ID {Id} not found", id);
                return NotFound("Department not found");
            }

            return Ok(foundDepartment);
        }

        [HttpPost]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Add(DepartmentAddingDTO departmentToAdd)
        {
            logger.LogInformation("Adding new department");
            var addedDepartment = await departmentService.AddAsync(departmentToAdd);

            logger.LogInformation("Department with ID {Id} added", addedDepartment.DepartmentId);
            return CreatedAtAction(nameof(GetById), new { id = addedDepartment.DepartmentId }, addedDepartment);
        }

        [HttpPost("addWorker")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> AddWorkerToDepartment(DepartmentWorkerAddingDTO departmentWorkerToAdd)
        {
            logger.LogInformation("Adding worker to department");
            var isAdded = await departmentService.AddWorkerToDepartmentAsync(departmentWorkerToAdd);

            if (!isAdded)
            {
                logger.LogWarning("Failed to add worker to department. Worker is already in the department or departmentId or workerId is invalid");
                return BadRequest("Worker is already in the department or departmentId or workerId is invalid");
            }

            logger.LogInformation("Worker successfully added to the department");
            return Ok("Worker successfully added to the department");
        }

        [HttpPut]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Update(DepartmentPlainDTO departmentToUpdate)
        {
            logger.LogInformation("Updating department with ID {Id}", departmentToUpdate.DepartmentId);
            var updatedDepartment = await departmentService.UpdateAsync(departmentToUpdate);
            if (updatedDepartment == null)
            {
                logger.LogWarning("Department with ID {Id} not found", departmentToUpdate.DepartmentId);
                return NotFound();
            }

            return Ok(updatedDepartment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Delete(int id)
        {
            logger.LogInformation("Deleting department with ID {Id}", id);
            var isDeleted = await departmentService.DeleteAsync(id);
            if (!isDeleted)
            {
                logger.LogWarning("Department with ID {Id} not found", id);
                return NotFound();
            }

            logger.LogInformation("Department with ID {Id} deleted", id);
            return NoContent();
        }
    }
}
