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
            var transactions = dbContext.Transactions
                .Where(transaction => transaction.TransactionId == id);

            if (!transactions.Any())
                return null;

            return await transactions
                .Include(transaction => transaction.Order)
                .FirstAsync();
        }
    }
}
