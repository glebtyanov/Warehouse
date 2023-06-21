using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(WarehouseContext dbContext) : base(dbContext)
        {
                
        }
    }
}
