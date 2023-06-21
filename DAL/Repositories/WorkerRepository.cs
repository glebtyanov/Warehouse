using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class WorkerRepository : BaseRepository<Worker>, IWorkerRepository
    {
        public WorkerRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }

        public override async Task<Worker?> GetDetailsAsync(int id)
        {
            var workers = dbContext.Workers
                .Where(worker => worker.WorkerId == id);

            if (!workers.Any())
                return null;

            return await workers
                .Include(worker => worker.Orders)
                .Include(worker => worker.Position)
                .Include(worker => worker.Departments)
                .FirstAsync();
        }

        public async Task<Worker?> FindByEmailAsync(string email)
        {
            var workersWithGivenEmail = dbContext.Workers.Where(worker => worker.Email == email);

            if (!workersWithGivenEmail.Any())
            {
                return null;
            }

            return await workersWithGivenEmail.FirstAsync();
        }
    }
}
