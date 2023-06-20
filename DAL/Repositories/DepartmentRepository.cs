using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }

        public override async Task<Department?> GetDetailsAsync(int id)
        {
            return await ((DbSet<Department>)dbContext.Departments.Include(department => department.Workers)).FindAsync(id);
        }
    }
}
