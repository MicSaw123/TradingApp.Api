using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Repositories.FuturesTransactionToOpenRepository;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.FuturesTransactionsToOpen
{
    public class FuturesTransactionToOpenRepository : IFuturesTransactionToOpenRepository
    {
        private readonly IDbContext _context;

        public FuturesTransactionToOpenRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task AddFuturesTransactionToOpen(FuturesTransactionToOpen futuresTransactionToOpen,
            CancellationToken cancellation)
        {
            await _context.Set<FuturesTransactionToOpen>().AddAsync(futuresTransactionToOpen);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task RemoveFuturesTransactionToOpen(FuturesTransactionToOpen futuresTransactionToOpen, CancellationToken cancellation)
        {
            _context.Set<FuturesTransactionToOpen>().Remove(futuresTransactionToOpen);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task EditFuturesTransactionToOpen(FuturesTransactionToOpen futuresTransactionToOpen, CancellationToken cancellation)
        {
            _context.Set<FuturesTransactionToOpen>().Update(futuresTransactionToOpen);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<IEnumerable<FuturesTransactionToOpen>> GetFuturesTransactionsToOpenByPortfolioId(int portfolioId)
        {
            var futuresTransactionsToOpen = _context.Set<FuturesTransactionToOpen>().Where(x => x.FuturesPortfolioId == portfolioId);
            return futuresTransactionsToOpen;
        }

        public async Task<FuturesTransactionToOpen> GetFuturesTransactionToOpenById(int id)
        {
            var futuresTransactionToOpen = await _context.Set<FuturesTransactionToOpen>().FirstOrDefaultAsync(x => x.Id == id);
            return futuresTransactionToOpen;
        }

        public async Task<IEnumerable<FuturesTransactionToOpen>> GetFuturesTransactionsToOpen()
        {
            var transactions = _context.Set<FuturesTransactionToOpen>().AsEnumerable();
            return transactions;
        }

    }
}
