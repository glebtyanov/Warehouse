using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class OrderProductRepository : BaseRepository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }
        public async override Task<OrderProduct?> GetByIdAsync(int id)
        {
            return null;
        }

        public bool Exists(OrderProduct orderProduct)
        {
            return dbContext.OrderProducts
                .Where(dw => dw.ProductId == orderProduct.ProductId
                && dw.OrderId == orderProduct.OrderId)
                .Count() > 0;
        }
    }
}
