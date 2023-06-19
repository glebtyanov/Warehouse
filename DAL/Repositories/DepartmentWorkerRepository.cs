using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class DepartmentWorkerRepository : BaseRepository<DepartmentWorker>, IDepartmentWorkerRepository
    {
        public DepartmentWorkerRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }
    }
}
