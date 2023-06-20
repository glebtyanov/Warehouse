using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{

    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(WarehouseContext dbContext) : base(dbContext)
        {

        }

        public override async Task<Transaction?> GetDetailsAsync(int id)
        {
            return await ((DbSet<Transaction>)dbContext.Transactions
                .Include(transaction => transaction.Order)
                ).FindAsync(id);
        }
    }
}
