using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Repositories.SpotTransactionToAdd;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotTransactionsToOpen
{
    public class SpotTransactionToOpenRepository : ISpotTransactionToOpenRepository
    {
        private readonly IDbContext _context;

        public SpotTransactionToOpenRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task AddSpotTransactionToOpen(SpotTransactionToOpen spotTransactionToOpen, CancellationToken cancellation)
        {
            await _context.Set<SpotTransactionToOpen>().AddAsync(spotTransactionToOpen);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task RemoveSpotTransactionToOpen(SpotTransactionToOpen spotTransactionToOpen,
            CancellationToken cancellation)
        {
            _context.Set<SpotTransactionToOpen>().Remove(spotTransactionToOpen);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task EditSpotTransactionToOpen(SpotTransactionToOpen spotTransactionToOpen,
            CancellationToken cancellation)
        {
            _context.Set<SpotTransactionToOpen>().Update(spotTransactionToOpen);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<IEnumerable<SpotTransactionToOpen>> GetSpotTransactionsToOpenByPortfolioId(int portfolioId)
        {
            var spotTransactionsToOpen = _context.Set<SpotTransactionToOpen>()
                .Where(x => x.SpotPortfolioId == portfolioId);
            return spotTransactionsToOpen;
        }

        public async Task<SpotTransactionToOpen> GetSpotTransactionToOpenById(int id)
        {
            var spotTransactionToOpen = await _context.Set<SpotTransactionToOpen>().FirstOrDefaultAsync(x => x.Id == id);
            return spotTransactionToOpen;
        }

        public async Task<IEnumerable<SpotTransactionToOpen>> GetSpotTransactionsToOpen()
        {
            var transactions = _context.Set<SpotTransactionToOpen>().ToList();
            return transactions;
        }

    }
}
