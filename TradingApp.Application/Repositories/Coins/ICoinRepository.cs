using TradingApp.Domain.Coins;

namespace TradingApp.Application.Repositories.Coins
{
    public interface ICoinRepository
    {
        Task<List<Coin>> GetCoins();

        Task EditCoins(List<Coin> coins, CancellationToken cancellation);

        Task AddCoins(List<Coin> coins, CancellationToken cancellation);
    }
}
