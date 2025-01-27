using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotTransactionRepository
{
    public class SpotTransactionRepository : ISpotTransactionRepository
    {
        private readonly IDbContext _context;

        public SpotTransactionRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SpotTransaction>> GetSpotTransactionsWithSellingPrice()
        {
            var transactions = _context.Set<SpotTransaction>().Where(x => x.SellingPrice != 0 && x.IsActive == true)
                .AsEnumerable();
            return transactions;
        }

        public async Task<IEnumerable<SpotTransaction>> GetActiveSpotTransactionsByPortfolioId(int portfolioId)
        {
            var spotTransactions = _context.Set<SpotTransaction>().Where(x => x.SpotPortfolioId == portfolioId && x.IsActive == true)
                .AsEnumerable();
            return spotTransactions;
        }

        public async Task<IEnumerable<SpotTransaction>> GetInactiveSpotTransactionsByPortfolioId(int portfolioId)
        {
            var incativeTransactions = _context.Set<SpotTransaction>().Where(x => x.IsActive == false && x.SpotPortfolioId == portfolioId)
                .AsEnumerable();
            return incativeTransactions;
        }

        public async Task<SpotTransaction> GetSpotTransactionById(int transactionId)
        {
            var spotTransaction = await _context.Set<SpotTransaction>().FirstOrDefaultAsync(x => x.Id == transactionId);
            return spotTransaction;
        }

        public async Task UpdateSpotTransaction(SpotTransaction spotTransaction, CancellationToken cancellation)
        {
            _context.Set<SpotTransaction>().Update(spotTransaction);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateSpotTransactionRange(List<SpotTransaction> spotTransactions, CancellationToken cancellation)
        {
            _context.Set<SpotTransaction>().UpdateRange(spotTransactions);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task AddSpotTransaction(SpotTransaction spotTransaction, CancellationToken cancellation)
        {
            await _context.Set<SpotTransaction>().AddAsync(spotTransaction);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<SpotTransaction> GetExistingSpotTransactionWithSpecifiedCoinSymbol(int portfolioId, string coinSymbol)
        {
            var spotTransaction = _context.Set<SpotTransaction>().FirstOrDefault(x => x.SpotPortfolioId == portfolioId &&
            x.CoinSymbol == coinSymbol);
            return spotTransaction;
        }
    }
}
