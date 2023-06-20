using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(WarehouseContext dbContext) : base(dbContext)
        {
                
        }

        public override async Task<Customer?> GetDetailsAsync(int id)
        {
            return await ((DbSet<Customer>)dbContext.Customers.Include(customer => customer.Orders)).FindAsync(id);
        }
    }
}
