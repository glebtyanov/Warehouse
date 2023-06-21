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

        public async override Task<DepartmentWorker?> GetByIdAsync(int id)
        {
            return null;
        }

        public bool Exists(DepartmentWorker departmentWorker)
        {
            return dbContext.DepartmentWorkers
                .Where(dw => dw.DepartmentId == departmentWorker.DepartmentId
                && dw.WorkerId == departmentWorker.WorkerId)
                .Count() > 0;
        }
    }
}
