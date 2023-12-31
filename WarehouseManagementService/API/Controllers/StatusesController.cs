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
        private readonly ILogger<StatusesController> logger;

        public StatusesController(StatusService statusService, ILogger<StatusesController> logger)
        {
            this.statusService = statusService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Fetching all statuses");

            return Ok(await statusService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation("Fetching status by ID: {Id}", id);
            var status = await statusService.GetByIdAsync(id);

            return Ok(status);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Fetching status details by ID: {Id}", id);
            var foundStatus = await statusService.GetDetailsByIdAsync(id);

            return Ok(foundStatus);
        }

        [HttpPost]
        public async Task<IActionResult> Add(StatusAddingDTO statusToAdd)
        {
            logger.LogInformation("Adding a new status");
            var addedStatus = await statusService.AddAsync(statusToAdd);

            logger.LogInformation("Status created successfully");

            return CreatedAtAction(nameof(GetById), new { id = addedStatus.StatusId }, addedStatus);
        }

        [HttpPut]
        public async Task<IActionResult> Update(StatusPlainDTO statusToUpdate)
        {
            logger.LogInformation("Updating status with ID: {Id}", statusToUpdate.StatusId);
            var updatedStatus = await statusService.UpdateAsync(statusToUpdate);

            logger.LogInformation("Status updated successfully");

            return Ok(updatedStatus);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("Deleting status with ID: {Id}", id);
            statusService.DeleteAsync(id);

            logger.LogInformation("Status with ID {Id} deleted", id);

            return Ok("Status successfully deleted");
        }
    }
}
