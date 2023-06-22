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

            return Ok(department);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving department details for ID {Id}", id);
            var foundDepartment = await departmentService.GetDetailsByIdAsync(id);

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
        public IActionResult AddWorkerToDepartment(DepartmentWorkerAddingDTO departmentWorkerToAdd)
        {
            logger.LogInformation("Adding worker to department");
            departmentService.AddWorkerToDepartmentAsync(departmentWorkerToAdd);

            logger.LogInformation("Worker successfully added to the department");

            return Ok("Worker successfully added to the department");
        }

        [HttpPut]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Update(DepartmentPlainDTO departmentToUpdate)
        {
            logger.LogInformation("Updating department with ID {Id}", departmentToUpdate.DepartmentId);
            var updatedDepartment = await departmentService.UpdateAsync(departmentToUpdate);

            return Ok(updatedDepartment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CEO")]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("Deleting department with ID {Id}", id);
            departmentService.DeleteAsync(id);

            logger.LogInformation("Department with ID {Id} deleted", id);

            return Ok("Department successfully deleted");
        }
    }
}
