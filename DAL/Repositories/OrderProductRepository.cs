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
    }
}
