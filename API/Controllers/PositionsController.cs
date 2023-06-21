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
            if (position == null)
            {
                logger.LogWarning("Position with ID {Id} not found", id);
                return NotFound();
            }

            return Ok(position);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving position details for ID {Id}", id);
            var foundPosition = await positionService.GetDetailsByIdAsync(id);
            if (foundPosition is null)
            {
                logger.LogWarning("Position with ID {Id} not found", id);
                return NotFound("Position not found");
            }

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
            if (updatedPosition == null)
            {
                logger.LogWarning("Position with ID {Id} not found", positionToUpdate.PositionId);
                return NotFound();
            }

            return Ok(updatedPosition);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            logger.LogInformation("Deleting position with ID {Id}", id);
            var isDeleted = await positionService.DeleteAsync(id);
            if (!isDeleted)
            {
                logger.LogWarning("Position with ID {Id} not found", id);
                return NotFound();
            }

            logger.LogInformation("Position with ID {Id} deleted", id);
            return NoContent();
        }
    }
}
