using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Utilities;
using Xunit;

namespace Tests.DAL.Repositories
{
    [Collection("DisableParallelizationCollection")]
    public class ProductRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture databaseFixture;
        private readonly List<Product> products;

        public ProductRepositoryTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;

            // Initialize a list of products for testing
            products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Product 1", Description = "Description 1", Quantity = 10, Price = 100.0 },
                new Product { ProductId = 2, Name = "Product 2", Description = "Description 2", Quantity = 20, Price = 200.0 }
            };
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var productRepository = new ProductRepository(dbContext);

                // Act
                var result = await productRepository.GetAllAsync();

                // Assert
                Assert.Equal(products.Count, result.Count);
                Assert.All(products, p => Assert.Contains(result, r => r.ProductId == p.ProductId));
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMatchingProduct()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var productId = 1;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var productRepository = new ProductRepository(dbContext);

                // Act
                var result = await productRepository.GetByIdAsync(productId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(productId, result.ProductId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var invalidProductId = 3;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var productRepository = new ProductRepository(dbContext);

                // Act
                var result = await productRepository.GetByIdAsync(invalidProductId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var newProduct = new Product
            {
                ProductId = 3,
                Name = "Product 3",
                Description = "Description 3",
                Quantity = 30,
                Price = 300.0
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                var productRepository = new ProductRepository(dbContext);

                // Act
                await productRepository.AddAsync(newProduct);
                await dbContext.SaveChangesAsync();

                var result = await productRepository.GetByIdAsync(newProduct.ProductId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(newProduct.ProductId, result.ProductId);
                Assert.Equal(newProduct.Name, result.Name);
                Assert.Equal(newProduct.Description, result.Description);
                Assert.Equal(newProduct.Quantity, result.Quantity);
                Assert.Equal(newProduct.Price, result.Price);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateProduct()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var productId = 1;
            var updatedProduct = new Product
            {
                ProductId = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Quantity = 5,
                Price = 50.0
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var productRepository = new ProductRepository(dbContext);

                dbContext.Entry(dbContext.Products.First()).State = EntityState.Detached;
                // Act
                await productRepository.UpdateAsync(updatedProduct);
                await dbContext.SaveChangesAsync();

                var result = await productRepository.GetByIdAsync(productId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedProduct.ProductId, result.ProductId);
                Assert.Equal(updatedProduct.Name, result.Name);
                Assert.Equal(updatedProduct.Description, result.Description);
                Assert.Equal(updatedProduct.Quantity, result.Quantity);
                Assert.Equal(updatedProduct.Price, result.Price);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var productId = -1;
            var updatedProduct = new Product
            {
                ProductId = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Quantity = 5,
                Price = 50.0
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var productRepository = new ProductRepository(dbContext);

                // Act
                await productRepository.UpdateAsync(updatedProduct);
                await dbContext.SaveChangesAsync();

                var result = await productRepository.GetByIdAsync(productId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteProduct()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var productId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var productRepository = new ProductRepository(dbContext);

                // Act
                var productBeforeDeletion = await productRepository.GetByIdAsync(productId);

                var isDeleted = await productRepository.DeleteAsync(productId);
                await dbContext.SaveChangesAsync();

                var productAfterDeletion = await productRepository.GetByIdAsync(productId);

                // Assert
                Assert.NotNull(productBeforeDeletion);
                Assert.True(isDeleted);
                Assert.Null(productAfterDeletion);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldDeleteProduct()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var productId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var productRepository = new ProductRepository(dbContext);

                // Act
                var productBeforeDeletion = await productRepository.GetByIdAsync(productId);

                var isDeleted = await productRepository.DeleteAsync(productId);
                await dbContext.SaveChangesAsync();

                var productAfterDeletion = await productRepository.GetByIdAsync(productId);

                // Assert
                Assert.Null(productBeforeDeletion);
                Assert.False(isDeleted);
                Assert.Null(productAfterDeletion);
            }
        }
        [Fact]
        public async Task GetDetailsAsync_WithValidId_ShouldReturnDepartmentWithNavigationalProperties()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var productId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var productRepository = new ProductRepository(dbContext);

                // Act
                var result = await productRepository.GetDetailsAsync(productId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(products[0].ProductId, result.ProductId);
                Assert.Equal(products[0].Name, result.Name);
                Assert.Equal(products[0].Description, result.Description);
                Assert.Equal(products[0].Price, result.Price);
                Assert.Equal(products[0].Quantity, result.Quantity);
                Assert.NotNull(result.Orders);
            }
        }

        [Fact]
        public async Task GetDetailsAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var productId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                var result = await departmentRepository.GetDetailsAsync(productId);

                // Assert
                Assert.Null(result);
            }
        }
    }
}
