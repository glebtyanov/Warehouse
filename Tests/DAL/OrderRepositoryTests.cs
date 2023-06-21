using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Utilities;

namespace Tests.DAL.Repositories
{
    [Collection("DisableParallelizationCollection")]
    public class OrderRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture databaseFixture;
        private readonly List<Order> orders;

        public OrderRepositoryTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;

            // Initialize a list of orders for testing
            orders = new List<Order>
            {
                new Order { OrderId = 1, CustomerId = 1, OrderDate = DateTime.Now, ProductAmount = 100.0, StatusId = 1, WorkerId = 1 },
                new Order { OrderId = 2, CustomerId = 2, OrderDate = DateTime.Now, ProductAmount = 200.0, StatusId = 2, WorkerId = 2 }
            };
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrders()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var orderRepository = new OrderRepository(dbContext);

                // Act
                var result = await orderRepository.GetAllAsync();

                // Assert
                Assert.Equal(orders.Count, result.Count);
                Assert.All(orders, o => Assert.Contains(result, r => r.OrderId == o.OrderId));
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMatchingOrder()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var orderId = 1;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var orderRepository = new OrderRepository(dbContext);

                // Act
                var result = await orderRepository.GetByIdAsync(orderId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(orderId, result.OrderId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var invalidOrderId = 3;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var orderRepository = new OrderRepository(dbContext);

                // Act
                var result = await orderRepository.GetByIdAsync(invalidOrderId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddAsync_ShouldAddOrder()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var newOrder = new Order
            {
                OrderId = 3,
                CustomerId = 3,
                OrderDate = DateTime.Now,
                ProductAmount = 300.0,
                StatusId = 1,
                WorkerId = 1
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                var orderRepository = new OrderRepository(dbContext);

                // Act
                await orderRepository.AddAsync(newOrder);
                await dbContext.SaveChangesAsync();

                var result = await orderRepository.GetByIdAsync(newOrder.OrderId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(newOrder.OrderId, result.OrderId);
                Assert.Equal(newOrder.CustomerId, result.CustomerId);
                Assert.Equal(newOrder.OrderDate, result.OrderDate);
                Assert.Equal(newOrder.ProductAmount, result.ProductAmount);
                Assert.Equal(newOrder.StatusId, result.StatusId);
                Assert.Equal(newOrder.WorkerId, result.WorkerId);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateOrder()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var orderId = 1;
            var updatedOrder = new Order
            {
                OrderId = orderId,
                CustomerId = 2,
                OrderDate = DateTime.Now.AddDays(-1),
                ProductAmount = 150.0,
                StatusId = 2,
                WorkerId = 2
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var orderRepository = new OrderRepository(dbContext);

                dbContext.Entry(dbContext.Orders.First()).State = EntityState.Detached;
                // Act
                await orderRepository.UpdateAsync(updatedOrder);
                await dbContext.SaveChangesAsync();

                var result = await orderRepository.GetByIdAsync(orderId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedOrder.OrderId, result.OrderId);
                Assert.Equal(updatedOrder.CustomerId, result.CustomerId);
                Assert.Equal(updatedOrder.OrderDate, result.OrderDate);
                Assert.Equal(updatedOrder.ProductAmount, result.ProductAmount);
                Assert.Equal(updatedOrder.StatusId, result.StatusId);
                Assert.Equal(updatedOrder.WorkerId, result.WorkerId);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var orderId = -1;
            var updatedOrder = new Order
            {
                OrderId = orderId,
                CustomerId = 2,
                OrderDate = DateTime.Now.AddDays(-1),
                ProductAmount = 150.0,
                StatusId = 2,
                WorkerId = 2
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var orderRepository = new OrderRepository(dbContext);

                // Act
                await orderRepository.UpdateAsync(updatedOrder);
                await dbContext.SaveChangesAsync();

                var result = await orderRepository.GetByIdAsync(orderId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteOrder()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var orderId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var orderRepository = new OrderRepository(dbContext);

                // Act
                var orderBeforeDeletion = await orderRepository.GetByIdAsync(orderId);

                var isDeleted = await orderRepository.DeleteAsync(orderId);
                await dbContext.SaveChangesAsync();

                var orderAfterDeletion = await orderRepository.GetByIdAsync(orderId);

                // Assert
                Assert.NotNull(orderBeforeDeletion);
                Assert.True(isDeleted);
                Assert.Null(orderAfterDeletion);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldDeleteOrder()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var orderId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var orderRepository = new OrderRepository(dbContext);

                // Act
                var orderBeforeDeletion = await orderRepository.GetByIdAsync(orderId);

                var isDeleted = await orderRepository.DeleteAsync(orderId);
                await dbContext.SaveChangesAsync();

                var orderAfterDeletion = await orderRepository.GetByIdAsync(orderId);

                // Assert
                Assert.Null(orderBeforeDeletion);
                Assert.False(isDeleted);
                Assert.Null(orderAfterDeletion);
            }
        }
        [Fact]
        public async Task GetDetailsAsync_WithValidId_ShouldReturnOrderWithNavigationalProperties()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var orderId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();

                var orderRepository = new OrderRepository(dbContext);

                // Act
                var result = await orderRepository.GetDetailsAsync(orderId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(orders[0].OrderId, result.OrderId);
                Assert.Equal(orders[0].CustomerId, result.CustomerId);
                Assert.Equal(orders[0].OrderDate, result.OrderDate);
                Assert.Equal(orders[0].ProductAmount, result.ProductAmount);
                Assert.Equal(orders[0].StatusId, result.StatusId);
                Assert.Equal(orders[0].WorkerId, result.WorkerId);
                Assert.NotNull(result.Products);
            }
        }

        [Fact]
        public async Task GetDetailsAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var orderId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();
                dbContext.Entry(dbContext.Orders.First()).State = EntityState.Detached;

                var orderRepository = new OrderRepository(dbContext);

                // Act
                var result = await orderRepository.GetDetailsAsync(orderId);

                // Assert
                Assert.Null(result);
            }
        }
    }
}

