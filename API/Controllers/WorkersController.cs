using BLL.DTO.Plain;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkersController : ControllerBase
    {
        private readonly WorkerService workerService;
        private readonly ILogger<WorkersController> logger;

        public WorkersController(WorkerService workerService, ILogger<WorkersController> logger)
        {
            this.workerService = workerService;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Retrieving all customers");

            var workers = await workerService.GetAllAsync();
            return Ok(workers);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation("Retrieving worker details for ID {Id}", id);

            var worker = await workerService.GetByIdAsync(id);
            if (worker == null)
            {
                logger.LogWarning("Worker with Id {Id} not found.", id);
                return NotFound();
            }

            return Ok(worker);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving worker details for ID {Id}", id);

            var foundWorker = await workerService.GetDetailsByIdAsync(id);
            if (foundWorker is null)
            {
                logger.LogWarning("Worker with Id {Id} not found.", id);
                return NotFound("Worker not found");
            }

            return Ok(foundWorker);
        }

        [HttpPut]
        public async Task<IActionResult> Update(WorkerPlainDTO workerToUpdate)
        {
            logger.LogInformation("Updating customer with ID {Id}", workerToUpdate.WorkerId);

            var updatedWorker = await workerService.UpdateAsync(workerToUpdate);
            if (updatedWorker == null)
            {
                logger.LogWarning("Worker with Id {WorkerId} not found.", workerToUpdate.WorkerId);
                return NotFound("Worker not found");
            }

            return Ok(updatedWorker);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            logger.LogInformation("Deleting worker with ID {Id}", id);

            var isDeleted = await workerService.DeleteAsync(id);
            if (!isDeleted)
            {
                logger.LogWarning("Worker with Id {WorkerId} not found.", id);
                return NotFound();
            }

            logger.LogInformation("Worker with Id {WorkerId} deleted successfully.", id);
            return NoContent();
        }
    }
}
