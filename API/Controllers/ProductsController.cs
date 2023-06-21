using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "CEO, Manager")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductsController(ProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await productService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var foundProduct = await productService.GetDetailsByIdAsync(id);
            if (foundProduct is null)
                return NotFound("Product not found");

            return Ok(foundProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductAddingDTO productToAdd)
        {
            var addedProduct = await productService.AddAsync(productToAdd);

            return CreatedAtAction(nameof(GetById), new { id = addedProduct.ProductId }, addedProduct);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductPlainDTO productToUpdate)
        {
            var updatedProduct = await productService.UpdateAsync(productToUpdate);
            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await productService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
