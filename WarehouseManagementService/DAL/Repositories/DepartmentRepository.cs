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
            var departments = dbContext.Departments
                .Where(department => department.DepartmentId == id);

            if (!departments.Any())
                return null;

            return await departments
                .Include(department => department.Workers)
                .FirstAsync();
        }
    }
}
