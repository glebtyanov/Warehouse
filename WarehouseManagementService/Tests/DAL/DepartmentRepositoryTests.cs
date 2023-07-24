using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Utilities;

namespace Tests.DAL.Repositories
{
    [Collection("DisableParallelizationCollection")]
    public class DepartmentRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture databaseFixture;
        private readonly List<Department> departments;

        public DepartmentRepositoryTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;

            // Initialize a list of departments for testing
            departments = new List<Department>
            {
                new Department { DepartmentId = 1, Name = "Department 1", Location = "Location 1", Capacity = 10 },
                new Department { DepartmentId = 2, Name = "Department 2", Location = "Location 2", Capacity = 15 }
            };

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                var result = await departmentRepository.GetAllAsync();

                // Assert
                Assert.Equal(departments.Count, result.Count);
                Assert.All(departments, d => Assert.Contains(result, r => r.DepartmentId == d.DepartmentId));
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMatchingDepartment()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var departmentId = 1;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                var result = await departmentRepository.GetByIdAsync(departmentId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(departmentId, result.DepartmentId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var invalidDepartmentId = 3;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                var result = await departmentRepository.GetByIdAsync(invalidDepartmentId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddAsync_ShouldAddDepartment()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var newDepartment = new Department
            {
                DepartmentId = 3,
                Name = "New Department",
                Location = "New Location",
                Capacity = 20
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                await departmentRepository.AddAsync(newDepartment);
                await dbContext.SaveChangesAsync();

                var result = await departmentRepository.GetByIdAsync(newDepartment.DepartmentId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(newDepartment.DepartmentId, result.DepartmentId);
                Assert.Equal(newDepartment.Name, result.Name);
                Assert.Equal(newDepartment.Location, result.Location);
                Assert.Equal(newDepartment.Capacity, result.Capacity);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateDepartment()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var departmentId = 1;
            var updatedDepartment = new Department
            {
                DepartmentId = departmentId,
                Name = "Updated Department",
                Location = "Updated Location",
                Capacity = 25
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                dbContext.Entry(dbContext.Departments.First()).State = EntityState.Detached;
                // Act
                await departmentRepository.UpdateAsync(updatedDepartment);
                await dbContext.SaveChangesAsync();

                var result = await departmentRepository.GetByIdAsync(departmentId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedDepartment.DepartmentId, result.DepartmentId);
                Assert.Equal(updatedDepartment.Name, result.Name);
                Assert.Equal(updatedDepartment.Location, result.Location);
                Assert.Equal(updatedDepartment.Capacity, result.Capacity);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var departmentId = -1;
            var updatedDepartment = new Department
            {
                DepartmentId = departmentId,
                Name = "Updated Department",
                Location = "Updated Location",
                Capacity = 25
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                await departmentRepository.UpdateAsync(updatedDepartment);
                await dbContext.SaveChangesAsync();

                var result = await departmentRepository.GetByIdAsync(departmentId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteDepartment()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var departmentId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                var departmentBeforeDeletion = await departmentRepository.GetByIdAsync(departmentId);

                var isDeleted = await departmentRepository.DeleteAsync(departmentId);
                await dbContext.SaveChangesAsync();

                var departmentAfterDeletion = await departmentRepository.GetByIdAsync(departmentId);

                // Assert
                Assert.NotNull(departmentBeforeDeletion);
                Assert.True(isDeleted);
                Assert.Null(departmentAfterDeletion);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldDeleteDepartment()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var departmentId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                var departmentBeforeDeletion = await departmentRepository.GetByIdAsync(departmentId);

                var isDeleted = await departmentRepository.DeleteAsync(departmentId);
                await dbContext.SaveChangesAsync();

                var departmentAfterDeletion = await departmentRepository.GetByIdAsync(departmentId);

                // Assert
                Assert.Null(departmentBeforeDeletion);
                Assert.False(isDeleted);
                Assert.Null(departmentAfterDeletion);
            }
        }
        [Fact]
        public async Task GetDetailsAsync_WithValidId_ShouldReturnDepartmentWithNavigationalProperties()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var deparmentId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                var result = await departmentRepository.GetDetailsAsync(deparmentId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(departments[0].DepartmentId, result.DepartmentId);
                Assert.Equal(departments[0].Name, result.Name);
                Assert.Equal(departments[0].Location, result.Location);
                Assert.Equal(departments[0].Capacity, result.Capacity);
                Assert.NotNull(result.Workers);
            }
        }

        [Fact]
        public async Task GetDetailsAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var deparmentId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Departments.AddRange(departments);
                dbContext.SaveChanges();

                var departmentRepository = new DepartmentRepository(dbContext);

                // Act
                var result = await departmentRepository.GetDetailsAsync(deparmentId);

                // Assert
                Assert.Null(result);
            }
        }
    }
}
