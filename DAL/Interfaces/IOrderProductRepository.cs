using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IOrderProductRepository : IBaseRepository<OrderProduct>
    {
        public bool Exists(OrderProduct orderProduct);
    }
}
