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

            return Ok(foundCustomer);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Retrieving customer details for ID {Id}", id);
            var foundCustomer = await customerService.GetDetailsByIdAsync(id);

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

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("Deleting customer with ID {Id}", id);
            customerService.DeleteAsync(id);

            logger.LogInformation("Customer with ID {Id} deleted", id);

            return Ok("Customer successfully deleted");
        }
    }
}
