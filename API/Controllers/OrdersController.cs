using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService orderService;

        public OrdersController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await orderService.GetAllAsync());
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var foundOrder = await orderService.GetDetailsByIdAsync(id);
            if (foundOrder is null)
                return NotFound("Order not found");

            return Ok(foundOrder);
        }

        [HttpGet("worker/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAllOfGivenWorker(int id)
        {
            if (!HttpContext.User.IsInRole("CEO") &&
                !HttpContext.User.IsInRole("Manager"))
            {
                if (HttpContext.User.FindFirstValue("id") != id.ToString())
                    return Forbid();
            }

            return Ok(await orderService.GetAllOfGivenWorker(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderAddingDTO orderToAdd)
        {
            var addedOrder = await orderService.AddAsync(orderToAdd);

            if (addedOrder is null)
                return BadRequest("Order creation failed.");

            return CreatedAtAction(nameof(GetById), new { id = addedOrder.OrderId }, addedOrder);
        }

        [HttpPost("addWorker")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> AddProductToOrder(OrderProductAddingDTO orderProductToAdd)
        {
            var isAdded = await orderService.AddOrderToProductAsync(orderProductToAdd);

            if (!isAdded)
                return BadRequest("Product is already in the order or orderId or productId is invalid");

            return Ok("Product successfully added to the order");
        }

        [HttpPut]
        public async Task<IActionResult> Update(OrderPlainDTO orderToUpdate)
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

            return Ok("Order successfuly deleted");
        }
    }
}
