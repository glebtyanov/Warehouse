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

            return Ok(worker);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving worker details for ID {Id}", id);

            var foundWorker = await workerService.GetDetailsByIdAsync(id);

            return Ok(foundWorker);
        }

        [HttpPut]
        public async Task<IActionResult> Update(WorkerPlainDTO workerToUpdate)
        {
            logger.LogInformation("Updating customer with ID {Id}", workerToUpdate.WorkerId);

            var updatedWorker = await workerService.UpdateAsync(workerToUpdate);

            return Ok(updatedWorker);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("Deleting worker with ID {Id}", id);

            workerService.DeleteAsync(id);

            logger.LogInformation("Worker with Id {WorkerId} deleted successfully.", id);
            return Ok("Worker successfully deleted");
        }
    }
}
