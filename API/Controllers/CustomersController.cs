using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService customerService;
        private readonly ILogger<CustomersController> logger;

        public CustomersController(CustomerService customerService, ILogger<CustomersController> logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Retrieving all customers");
            var customers = await customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation("Retrieving customer with ID {Id}", id);
            var foundCustomer = await customerService.GetByIdAsync(id);
            if (foundCustomer is null)
            {
                logger.LogWarning("Customer with ID {Id} not found", id);
                return NotFound("Customer not found");
            }

            return Ok(foundCustomer);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving customer details for ID {Id}", id);
            var foundCustomer = await customerService.GetDetailsByIdAsync(id);
            if (foundCustomer is null)
            {
                logger.LogWarning("Customer with ID {Id} not found", id);
                return NotFound("Customer not found");
            }

            return Ok(foundCustomer);
        }

        [HttpPost]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> Add(CustomerAddingDTO customerToAdd)
        {
            logger.LogInformation("Adding new customer");
            var addedCustomer = await customerService.AddAsync(customerToAdd);

            logger.LogInformation("Customer with ID {Id} added", addedCustomer.CustomerId);
            return Ok(addedCustomer);
        }

        [HttpPut]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> Update(CustomerPlainDTO customerToUpdate)
        {
            logger.LogInformation("Updating customer with ID {Id}", customerToUpdate.CustomerId);
            var updatedCustomer = await customerService.UpdateAsync(customerToUpdate);
            if (updatedCustomer is null)
            {
                logger.LogWarning("Customer with ID {Id} not found", customerToUpdate.CustomerId);
                return NotFound("Customer not found");
            }

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            logger.LogInformation("Deleting customer with ID {Id}", id);
            var isDeleteSuccessful = await customerService.DeleteAsync(id);
            if (!isDeleteSuccessful)
            {
                logger.LogWarning("Customer with ID {Id} not found", id);
                return NotFound("Customer not found");
            }

            logger.LogInformation("Customer with ID {Id} deleted successfully", id);
            return Ok("Customer successfully deleted");
        }
    }
}
