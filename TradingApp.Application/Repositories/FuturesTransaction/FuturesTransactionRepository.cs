using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.TransactionRepository.FuturesTransactionRepository
{
    public class FuturesTransactionRepository : IFuturesTransactionRepository
    {
        private readonly IDbContext _context;

        public FuturesTransactionRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task UpdateFuturesTransaction(FuturesTransaction transaction, CancellationToken cancellation)
        {
            _context.Set<FuturesTransaction>().Update(transaction);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateFuturesTransactionRange(List<FuturesTransaction> futuresTransactions,
            CancellationToken cancellation)
        {
            _context.Set<FuturesTransaction>().UpdateRange(futuresTransactions);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<IEnumerable<FuturesTransaction>> GetActiveFuturesTransactionsByPortfolioId(int portfolioId)
        {
            var futuresTransactions
                = _context.Set<FuturesTransaction>()
                    .Where(x => x.FuturesPortfolioId == portfolioId && x.IsActive == true);
            return futuresTransactions;
        }

        public async Task<FuturesTransaction> GetActiveFuturesTransactionById(int id)
        {
            var futuresTransaction = await _context.Set<FuturesTransaction>()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            return futuresTransaction;
        }

        public async Task AddFuturesTransaction(FuturesTransaction futuresTransaction,
            CancellationToken cancellation)
        {
            await _context.Set<FuturesTransaction>().AddAsync(futuresTransaction, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<IEnumerable<FuturesTransaction>>
            GetInactiveFuturesTransactionsByPortfolioId(int portfolioId)
        {
            var inactiveFuturesTransactions = _context.Set<FuturesTransaction>()
                .Where(x => x.IsActive == false).AsEnumerable();
            return inactiveFuturesTransactions;
        }

        public async Task<FuturesTransaction> GetFuturesTransactionByCoinSymbol(int portfolioId, string coinSymbol)
        {
            var futuresTransaction = await _context.Set<FuturesTransaction>()
                .FirstOrDefaultAsync(x =>
                    x.FuturesPortfolioId == portfolioId && x.CoinSymbol == coinSymbol);
            return futuresTransaction;
        }
    }
}
