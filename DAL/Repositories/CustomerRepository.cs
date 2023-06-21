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
            var customers = dbContext.Customers
                .Where(customer => customer.CustomerId == id);

            if (!customers.Any())
                return null;

            return await customers
                .Include(customer => customer.Orders)
                .FirstAsync();
        }
    }
}
