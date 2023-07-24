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
    public class PositionsController : ControllerBase
    {
        private readonly PositionService positionService;
        private readonly ILogger<PositionsController> logger;

        public PositionsController(PositionService positionService, ILogger<PositionsController> logger)
        {
            this.positionService = positionService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Retrieving all positions");
            var positions = await positionService.GetAllAsync();

            return Ok(positions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation("Retrieving position with ID {Id}", id);
            var position = await positionService.GetByIdAsync(id);

            return Ok(position);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving position details for ID {Id}", id);
            var foundPosition = await positionService.GetDetailsByIdAsync(id);

            return Ok(foundPosition);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PositionAddingDTO positionToAdd)
        {
            logger.LogInformation("Adding new position");
            var addedPosition = await positionService.AddAsync(positionToAdd);

            logger.LogInformation("Position with ID {Id} added", addedPosition.PositionId);

            return CreatedAtAction(nameof(GetById), new { id = addedPosition.PositionId }, addedPosition);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PositionPlainDTO positionToUpdate)
        {
            logger.LogInformation("Updating position with ID {Id}", positionToUpdate.PositionId);
            var updatedPosition = await positionService.UpdateAsync(positionToUpdate);

            return Ok(updatedPosition);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("Deleting position with ID {Id}", id);
            positionService.DeleteAsync(id);

            logger.LogInformation("Position with ID {Id} deleted", id);

            return Ok("Position successfully deleted");
        }
    }
}
