using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Utilities;

namespace Tests.DAL.Repositories
{
    [Collection("DisableParallelizationCollection")]
    public class CustomerRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture databaseFixture;
        private readonly List<Customer> customers;

        public CustomerRepositoryTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;

            // Initialize a list of customers for testing
            customers = new List<Customer>
            {
                new Customer { CustomerId = 1, Name = "Customer 1", Address = "Address 1", ContactNumber = "12345", Email = "customer1@example.com" },
                new Customer { CustomerId = 2, Name = "Customer 2", Address = "Address 2", ContactNumber = "67890", Email = "customer2@example.com" }
            };

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                // Act
                var result = await customerRepository.GetAllAsync();

                // Assert
                Assert.Equal(customers.Count, result.Count);
                Assert.All(customers, c => Assert.Contains(result, r => r.CustomerId == c.CustomerId));
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMatchingCustomer()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var customerId = 1;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                // Act
                var result = await customerRepository.GetByIdAsync(customerId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(customerId, result.CustomerId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var invalidCustomerId = 3;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                // Act
                var result = await customerRepository.GetByIdAsync(invalidCustomerId);

                // Assert
                Assert.Null(result);
            }
        }
        [Fact]
        public async Task AddAsync_ShouldAddCustomer()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var newCustomer = new Customer
            {
                CustomerId = 3,
                Name = "New Customer",
                Address = "New Address",
                ContactNumber = "98765",
                Email = "newcustomer@example.com"
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                var customerRepository = new CustomerRepository(dbContext);

                // Act
                await customerRepository.AddAsync(newCustomer);
                await dbContext.SaveChangesAsync();

                var result = await customerRepository.GetByIdAsync(newCustomer.CustomerId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(newCustomer.CustomerId, result.CustomerId);
                Assert.Equal(newCustomer.Name, result.Name);
                Assert.Equal(newCustomer.Address, result.Address);
                Assert.Equal(newCustomer.ContactNumber, result.ContactNumber);
                Assert.Equal(newCustomer.Email, result.Email);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateCustomer()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var customerId = 1;
            var updatedCustomer = new Customer
            {
                CustomerId = customerId,
                Name = "Updated Customer",
                Address = "Updated Address",
                ContactNumber = "54321",
                Email = "updatedcustomer@example.com"
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                dbContext.Entry(dbContext.Customers.First()).State = EntityState.Detached;
                // Act
                await customerRepository.UpdateAsync(updatedCustomer);
                await dbContext.SaveChangesAsync();

                var result = await customerRepository.GetByIdAsync(customerId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedCustomer.CustomerId, result.CustomerId);
                Assert.Equal(updatedCustomer.Name, result.Name);
                Assert.Equal(updatedCustomer.Address, result.Address);
                Assert.Equal(updatedCustomer.ContactNumber, result.ContactNumber);
                Assert.Equal(updatedCustomer.Email, result.Email);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var customerId = -1;
            var updatedCustomer = new Customer
            {
                CustomerId = customerId,
                Name = "Updated Customer",
                Address = "Updated Address",
                ContactNumber = "54321",
                Email = "updatedcustomer@example.com"
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                // Act
                await customerRepository.UpdateAsync(updatedCustomer);
                await dbContext.SaveChangesAsync();

                var result = await customerRepository.GetByIdAsync(customerId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteCustomer()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var customerId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                // Act
                var customerBeforeDeletion = await customerRepository.GetByIdAsync(customerId);

                var isDeleted = await customerRepository.DeleteAsync(customerId);
                await dbContext.SaveChangesAsync();

                var customerAfterDeletion = await customerRepository.GetByIdAsync(customerId);

                // Assert
                Assert.NotNull(customerBeforeDeletion);
                Assert.True(isDeleted);
                Assert.Null(customerAfterDeletion);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldDeleteCustomer()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var customerId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                // Act
                var customerBeforeDeletion = await customerRepository.GetByIdAsync(customerId);

                var isDeleted = await customerRepository.DeleteAsync(customerId);
                await dbContext.SaveChangesAsync();

                var customerAfterDeletion = await customerRepository.GetByIdAsync(customerId);

                // Assert
                Assert.Null(customerBeforeDeletion);
                Assert.False(isDeleted);
                Assert.Null(customerAfterDeletion);
            }
        }

        [Fact]
        public async Task GetDetailsAsync_WithValidId_ShouldReturnCustomerWithNavigationalProperties()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var customerId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                // Act
                var result = await customerRepository.GetDetailsAsync(customerId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(customers[0].CustomerId, result.CustomerId);
                Assert.Equal(customers[0].Name, result.Name);
                Assert.Equal(customers[0].Address, result.Address);
                Assert.Equal(customers[0].ContactNumber, result.ContactNumber);
                Assert.Equal(customers[0].Email, result.Email);
                Assert.NotNull(result.Orders);
            }
        }

        [Fact]
        public async Task GetDetailsAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var customerId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var customerRepository = new CustomerRepository(dbContext);

                // Act
                var result = await customerRepository.GetDetailsAsync(customerId);

                // Assert
                Assert.Null(result);
            }
        }
    }
}
