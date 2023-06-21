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

        //[HttpPost]
        //public async Task<IActionResult> Add(WorkerAddingDTO workerToAdd)
        //{
        //    var addedWorker = await workerService.AddAsync(workerToAdd);

        //    if (addedWorker is null)
        //        return BadRequest("Worker creation failed.");

        //    return CreatedAtAction(nameof(GetById), new { id = addedWorker.WorkerId }, addedWorker);
        //}

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
