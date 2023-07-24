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
    public class PositionRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture databaseFixture;
        private readonly List<Position> positions;

        public PositionRepositoryTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;

            // Initialize a list of positions for testing
            positions = new List<Position>
            {
                new Position { PositionId = 1, Name = "Manager" },
                new Position { PositionId = 2, Name = "Supervisor" }
            };
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPositions()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                // Act
                var result = await positionRepository.GetAllAsync();

                // Assert
                Assert.Equal(positions.Count, result.Count);
                Assert.All(positions, p => Assert.Contains(result, r => r.PositionId == p.PositionId));
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMatchingPosition()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var positionId = 1;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                // Act
                var result = await positionRepository.GetByIdAsync(positionId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(positionId, result.PositionId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var invalidPositionId = 3;
            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                // Act
                var result = await positionRepository.GetByIdAsync(invalidPositionId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddAsync_ShouldAddPosition()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var newPosition = new Position
            {
                PositionId = 3,
                Name = "Associate"
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                var positionRepository = new PositionRepository(dbContext);

                // Act
                await positionRepository.AddAsync(newPosition);
                await dbContext.SaveChangesAsync();

                var result = await positionRepository.GetByIdAsync(newPosition.PositionId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(newPosition.PositionId, result.PositionId);
                Assert.Equal(newPosition.Name, result.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdatePosition()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var positionId = 1;
            var updatedPosition = new Position
            {
                PositionId = positionId,
                Name = "Senior Manager"
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                dbContext.Entry(dbContext.Positions.First()).State = EntityState.Detached;
                // Act
                await positionRepository.UpdateAsync(updatedPosition);
                await dbContext.SaveChangesAsync();

                var result = await positionRepository.GetByIdAsync(positionId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedPosition.PositionId, result.PositionId);
                Assert.Equal(updatedPosition.Name, result.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var positionId = -1;
            var updatedPosition = new Position
            {
                PositionId = positionId,
                Name = "Senior Manager"
            };

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                // Act
                await positionRepository.UpdateAsync(updatedPosition);
                await dbContext.SaveChangesAsync();

                var result = await positionRepository.GetByIdAsync(positionId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeletePosition()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var positionId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                // Act
                var positionBeforeDeletion = await positionRepository.GetByIdAsync(positionId);

                var isDeleted = await positionRepository.DeleteAsync(positionId);
                await dbContext.SaveChangesAsync();

                var positionAfterDeletion = await positionRepository.GetByIdAsync(positionId);

                // Assert
                Assert.NotNull(positionBeforeDeletion);
                Assert.True(isDeleted);
                Assert.Null(positionAfterDeletion);
            }
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldDeletePosition()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var positionId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                // Act
                var positionBeforeDeletion = await positionRepository.GetByIdAsync(positionId);

                var isDeleted = await positionRepository.DeleteAsync(positionId);
                await dbContext.SaveChangesAsync();

                var positionAfterDeletion = await positionRepository.GetByIdAsync(positionId);

                // Assert
                Assert.Null(positionBeforeDeletion);
                Assert.False(isDeleted);
                Assert.Null(positionAfterDeletion);
            }
        }
        [Fact]
        public async Task GetDetailsAsync_WithValidId_ShouldReturnDepartmentWithNavigationalProperties()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var positionId = 1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                // Act
                var result = await positionRepository.GetDetailsAsync(positionId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(positions[0].PositionId, result.PositionId);
                Assert.Equal(positions[0].Name, result.Name);
                Assert.NotNull(result.Workers);
            }
        }

        [Fact]
        public async Task GetDetailsAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            databaseFixture.ResetDatabase();

            var positionId = -1;

            using (var dbContext = databaseFixture.CreateContext())
            {
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();

                var positionRepository = new PositionRepository(dbContext);

                // Act
                var result = await positionRepository.GetDetailsAsync(positionId);

                // Assert
                Assert.Null(result);
            }
        }
    }
}
