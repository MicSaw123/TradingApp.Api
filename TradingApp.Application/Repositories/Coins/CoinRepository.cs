using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Coins;

namespace TradingApp.Application.Repositories.Coins
{
    public class CoinRepository : ICoinRepository
    {
        private readonly IDbContext _context;

        public CoinRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task AddCoins(List<Coin> coins, CancellationToken cancellation)
        {
            await _context.Set<Coin>().AddRangeAsync(coins);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task EditCoins(List<Coin> coins, CancellationToken cancellation)
        {
            _context.Set<Coin>().UpdateRange(coins);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<List<Coin>> GetCoins()
        {
            var coins = _context.Set<Coin>().ToList();
            return coins;
        }


    }
}
