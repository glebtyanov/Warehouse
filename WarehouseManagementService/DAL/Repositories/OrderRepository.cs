using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }

        public override async Task<Order?> GetDetailsAsync(int id)
        {
            var orders = dbContext.Orders
                .Where(order => order.OrderId == id);

            if (!orders.Any())
                return null;

            return await orders
                .Include(order => order.Transaction)
                .Include(order => order.Worker)
                .Include(order => order.Status)
                .Include(order => order.Products)
                .Include(order => order.Customer)
                .FirstAsync();
        }
    }
}
