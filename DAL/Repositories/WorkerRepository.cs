using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class WorkerRepository : BaseRepository<Worker>, IWorkerRepository
    {
        public WorkerRepository(WarehouseContext dbContext) : base(dbContext)
        {
                
        }

        public override async Task<Worker?> GetDetailsAsync(int id)
        {
            return await ((DbSet<Worker>)dbContext.Workers
                .Include(worker => worker.Orders)
                .Include(worker => worker.Position)
                .Include(worker => worker.Departments)
                ).FindAsync(id);
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
