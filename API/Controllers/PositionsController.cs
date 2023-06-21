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

        public PositionsController(PositionService positionService)
        {
            this.positionService = positionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await positionService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var position = await positionService.GetByIdAsync(id);
            if (position == null)
                return NotFound();

            return Ok(position);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PositionAddingDTO positionToAdd)
        {
            var addedPosition = await positionService.AddAsync(positionToAdd);

            return CreatedAtAction(nameof(GetById), new { id = addedPosition.PositionId }, addedPosition);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PositionPlainDTO positionToUpdate)
        {
            var updatedPosition = await positionService.UpdateAsync(positionToUpdate);
            if (updatedPosition == null)
                return NotFound();

            return Ok(updatedPosition);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await positionService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
