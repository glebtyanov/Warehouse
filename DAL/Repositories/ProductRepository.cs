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
            var products = dbContext.Products
                .Where(product => product.ProductId == id);

            if (!products.Any())
                return null;

            return await products
                .Include(product => product.Orders)
                .FirstAsync();
        }
    }
}
