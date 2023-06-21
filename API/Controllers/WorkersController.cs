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
    public class WorkersController : ControllerBase
    {
        private readonly WorkerService workerService;

        public WorkersController(WorkerService workerService)
        {
            this.workerService = workerService;
        }

        [HttpGet]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await workerService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var worker = await workerService.GetByIdAsync(id);
            if (worker == null)
                return NotFound();

            return Ok(worker);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var foundWorker = await workerService.GetDetailsByIdAsync(id);
            if (foundWorker is null)
                return NotFound("Worker not found");

            return Ok(foundWorker);
        }

        [HttpPut]
        public async Task<IActionResult> Update(WorkerPlainDTO workerToUpdate)
        {
            var updatedWorker = await workerService.UpdateAsync(workerToUpdate);
            if (updatedWorker == null)
                return NotFound();

            return Ok(updatedWorker);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await workerService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
