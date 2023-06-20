using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }

        public override async Task<Product?> GetDetailsAsync(int id)
        {
            return await ((DbSet<Product>)dbContext.Products
                .Include(product => product.Orders)
                ).FindAsync(id);
        }
    }
}
