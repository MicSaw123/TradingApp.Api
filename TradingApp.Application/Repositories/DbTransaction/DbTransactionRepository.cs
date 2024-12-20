using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using TradingApp.Application.Services.Interfaces.Database;

namespace TradingApp.Application.Repositories.DbTransactionRepository
{
    public class DbTransactionRepository : IDbTransactionRepository
    {
        private readonly IDbContext _context;

        public DbTransactionRepository(IDbContext context)
        {
            _context = context;
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }
    }
}
