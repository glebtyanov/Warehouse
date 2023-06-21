using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly WarehouseContext dbContext;

        public BaseRepository(WarehouseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        virtual public async Task<List<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync(CancellationToken.None);
        }

        virtual public async Task<T?> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        virtual public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        virtual public async Task<T?> UpdateAsync(T entity)
        {
            if (!dbContext.Set<T>().Contains(entity))
                return null;

            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return entity;
        }

        virtual public async Task<bool> DeleteAsync(int id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return true;
        }

        virtual public async Task<T?> GetDetailsAsync(int id)
        {
            return await GetByIdAsync(id);
        }
    }
}