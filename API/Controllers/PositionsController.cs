using BLL.DTO;
using BLL.DTO.Adding;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService orderService;

        public OrdersController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await orderService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderAddingDTO orderToAdd)
        {
            var addedOrder = await orderService.AddAsync(orderToAdd);

            if (addedOrder is null)
                return BadRequest("Order creation failed.");

            return CreatedAtAction(nameof(GetById), new {id = addedOrder.OrderId}, addedOrder);
        }

        [HttpPut]
        public async Task<IActionResult> Update(OrderDTO orderToUpdate)
        {
            var updatedOrder = await orderService.UpdateAsync(orderToUpdate);
            if (updatedOrder == null)
                return NotFound();

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await orderService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
