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
            return await ((DbSet<Status>)dbContext.Statuses
                .Include(status => status.Orders)
                ).FindAsync(id);
        }
    }
}
