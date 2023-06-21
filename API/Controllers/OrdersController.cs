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
        private readonly ILogger<OrdersController> logger;

        public OrdersController(OrderService orderService, ILogger<OrdersController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Retrieving all orders");
            var orders = await orderService.GetAllAsync();
            return Ok(orders);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation("Retrieving order with ID {Id}", id);
            var order = await orderService.GetByIdAsync(id);
            if (order == null)
            {
                logger.LogWarning("Order with ID {Id} not found", id);
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving order details for ID {Id}", id);
            var foundOrder = await orderService.GetDetailsByIdAsync(id);
            if (foundOrder is null)
            {
                logger.LogWarning("Order with ID {Id} not found", id);
                return NotFound("Order not found");
            }

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

            logger.LogInformation("Retrieving all orders for worker with ID {Id}", id);
            var orders = await orderService.GetAllOfGivenWorker(id);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderAddingDTO orderToAdd)
        {
            logger.LogInformation("Adding new order");
            var addedOrder = await orderService.AddAsync(orderToAdd);

            if (addedOrder is null)
            {
                logger.LogWarning("Order creation failed");
                return BadRequest("Order creation failed.");
            }

            logger.LogInformation("Order with ID {Id} added", addedOrder.OrderId);
            return CreatedAtAction(nameof(GetById), new { id = addedOrder.OrderId }, addedOrder);
        }

        [HttpPost("addWorker")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> AddProductToOrder(OrderProductAddingDTO orderProductToAdd)
        {
            logger.LogInformation("Adding product to order");
            var isAdded = await orderService.AddOrderToProductAsync(orderProductToAdd);

            if (!isAdded)
            {
                logger.LogWarning("Failed to add product to order. Product is already in the order or orderId or productId is invalid");
                return BadRequest("Product is already in the order or orderId or productId is invalid");
            }

            logger.LogInformation("Product successfully added to the order");
            return Ok("Product successfully added to the order");
        }

        [HttpPut]
        public async Task<IActionResult> Update(OrderPlainDTO orderToUpdate)
        {
            logger.LogInformation("Updating order with ID {Id}", orderToUpdate.OrderId);
            var updatedOrder = await orderService.UpdateAsync(orderToUpdate);
            if (updatedOrder == null)
            {
                logger.LogWarning("Order with ID {Id} not found", orderToUpdate.OrderId);
                return NotFound();
            }

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            logger.LogInformation("Deleting order with ID {Id}", id);
            var isDeleted = await orderService.DeleteAsync(id);
            if (!isDeleted)
            {
                logger.LogWarning("Order with ID {Id} not found", id);
                return NotFound();
            }

            logger.LogInformation("Order with ID {Id} deleted", id);
            return Ok("Order successfully deleted");
        }
    }
}
