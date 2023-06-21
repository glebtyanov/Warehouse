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

        [HttpPost]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> Add(DepartmentAddingDTO departmentToAdd)
        {
            var addedDepartment = await departmentService.AddAsync(departmentToAdd);
            return CreatedAtAction(nameof(GetById), new { id = addedDepartment.DepartmentId }, addedDepartment);
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

            return NoContent();
        }
    }
}
