using Microsoft.AspNetCore.Mvc;
using BLL.DTO;
using BLL.DTO.Adding;
using BLL.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService customerService;

        public CustomersController(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            return Ok(await customerService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerByIdAsync(int id)
        {
            var foundCustomer = await customerService.GetByIdAsync(id);
            if (foundCustomer is null)
                return NotFound();

            return Ok(foundCustomer);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerAsync(CustomerAddingDTO customerToAdd)
        {
            return Ok(await customerService.AddAsync(customerToAdd));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomerAsync(CustomerDTO customerToUpdate)
        {
            var updatedCustomer = await customerService.UpdateAsync(customerToUpdate);

            if (updatedCustomer is null)
                return NotFound("Customer not found");

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            var isDeleteSuccessful = await customerService.DeleteAsync(id);

            if (!isDeleteSuccessful)
                return NotFound("Customer not found");

            return Ok("Customer successfuly deleted");
        }
    }
}
