using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IWorkerRepository : IBaseRepository<Worker>
    {
        public Task<Worker?> FindByEmailAsync(string email);
    }
}
