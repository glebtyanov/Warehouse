using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class StatusRepository : BaseRepository<Status>, IStatusRepository
    {
        public StatusRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }

        public override async Task<Status?> GetDetailsAsync(int id)
        { 
            var statuses = dbContext.Statuses
                .Where(status => status.StatusId == id);

            if (!statuses.Any())
                return null;

            return await statuses
                .Include(status => status.Orders)
                .FirstAsync();
        }
    }
}
