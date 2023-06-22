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
        private readonly ILogger<ProductsController> logger;

        public ProductsController(ProductService productService, ILogger<ProductsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Fetching all products");

            return Ok(await productService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation("Fetching product by ID: {Id}", id);
            var product = await productService.GetByIdAsync(id);

            return Ok(product);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Fetching product details by ID: {Id}", id);
            var foundProduct = await productService.GetDetailsByIdAsync(id);

            return Ok(foundProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductAddingDTO productToAdd)
        {
            logger.LogInformation("Adding a new product");
            var addedProduct = await productService.AddAsync(productToAdd);

            logger.LogInformation("Product created successfully");

            return CreatedAtAction(nameof(GetById), new { id = addedProduct.ProductId }, addedProduct);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductPlainDTO productToUpdate)
        {
            logger.LogInformation("Updating product with ID: {Id}", productToUpdate.ProductId);
            var updatedProduct = await productService.UpdateAsync(productToUpdate);

            logger.LogInformation("Product updated successfully");

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("Deleting product with ID: {Id}", id);
            productService.DeleteAsync(id);

            logger.LogInformation("Product with ID {Id} deleted", id);

            return Ok("Product successfully deleted");
        }
    }
}
