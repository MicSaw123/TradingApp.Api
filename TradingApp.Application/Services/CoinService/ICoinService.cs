using TradingApp.Application.DataTransferObjects.Coin;

namespace TradingApp.Application.Services.CoinService
{
    public interface ICoinService
    {
        public Task<RequestResult<IEnumerable<CoinDto>>> GetCoins();

        public Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsBySymbol(List<string> symbols);

        public Task<RequestResult<string>> GetCoinNameById(int coinId);

        public Task<RequestResult> SeedCoins(CancellationToken cancellation);

        public Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsPerPage(int pageSize,
            int page);
    }
}
