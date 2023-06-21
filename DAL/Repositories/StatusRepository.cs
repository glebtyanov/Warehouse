using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class StatusRepository : BaseRepository<Status>, IStatusRepository
    {
        public StatusRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }
    }
}
