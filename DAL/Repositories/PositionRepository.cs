using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        public PositionRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }
        public override async Task<Position?> GetDetailsAsync(int id)
        {
            var positions = dbContext.Positions
                .Where(position => position.PositionId == id);

            if (!positions.Any())
                return null;

            return await positions
                .Include(position => position.Workers)
                .FirstAsync();
        }
    }
}
