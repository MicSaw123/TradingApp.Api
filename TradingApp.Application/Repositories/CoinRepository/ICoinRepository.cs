using TradingApp.Application.DataTransferObjects.Coin;

namespace TradingApp.Application.Repositories.CoinRepository
{
    public interface ICoinRepository
    {
        Task<RequestResult<IEnumerable<CoinDto>>> GetAllCoins();

        Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsBySymbol(List<string> symbols);

        Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsPerPage(int pageSize, int page);

        Task<RequestResult<string>> GetCoinNameById(int coinId);
    }
}
