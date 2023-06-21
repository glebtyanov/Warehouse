using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "CEO")]
    public class StatusesController : ControllerBase
    {
        private readonly StatusService statusService;

        public StatusesController(StatusService statusService)
        {
            this.statusService = statusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await statusService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await statusService.GetByIdAsync(id);
            if (status == null)
                return NotFound();

            return Ok(status);
        }

        [HttpPost]
        public async Task<IActionResult> Add(StatusAddingDTO statusToAdd)
        {
            var addedStatus = await statusService.AddAsync(statusToAdd);

            return CreatedAtAction(nameof(GetById), new { id = addedStatus.StatusId }, addedStatus);
        }

        [HttpPut]
        public async Task<IActionResult> Update(StatusPlainDTO statusToUpdate)
        {
            var updatedStatus = await statusService.UpdateAsync(statusToUpdate);
            if (updatedStatus == null)
                return NotFound();

            return Ok(updatedStatus);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await statusService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
