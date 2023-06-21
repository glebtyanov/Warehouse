using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IDepartmentWorkerRepository : IBaseRepository<DepartmentWorker>
    {
        public bool Exists(DepartmentWorker departmentWorker);
    }
}
