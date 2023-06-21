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

        public CustomersController(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await customerService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var foundCustomer = await customerService.GetByIdAsync(id);
            if (foundCustomer is null)
                return NotFound("Customer not found");

            return Ok(foundCustomer);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var foundCustomer = await customerService.GetDetailsByIdAsync(id);
            if (foundCustomer is null)
                return NotFound("Customer not found");

            return Ok(foundCustomer);
        }

        [HttpPost]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> Add(CustomerAddingDTO customerToAdd)
        {
            return Ok(await customerService.AddAsync(customerToAdd));
        }

        [HttpPut]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> Update(CustomerPlainDTO customerToUpdate)
        {
            var updatedCustomer = await customerService.UpdateAsync(customerToUpdate);

            if (updatedCustomer is null)
                return NotFound("Customer not found");

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CEO, Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleteSuccessful = await customerService.DeleteAsync(id);

            if (!isDeleteSuccessful)
                return NotFound("Customer not found");

            return Ok("Customer successfuly deleted");
        }
    }
}
