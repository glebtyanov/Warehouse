using BLL.DTO;
using BLL.DTO.Adding;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await departmentService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await departmentService.GetByIdAsync(id);
            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DepartmentAddingDTO departmentToAdd)
        {
            var addedDepartment = await departmentService.AddAsync(departmentToAdd);
            return CreatedAtAction(nameof(GetById), new {id = addedDepartment.DepartmentId}, addedDepartment);
        }

        [HttpPut]
        public async Task<IActionResult> Update(DepartmentDTO departmentToUpdate)
        {
            var updatedDepartment = await departmentService.UpdateAsync(departmentToUpdate);
            if (updatedDepartment == null)
                return NotFound();

            return Ok(updatedDepartment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await departmentService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
