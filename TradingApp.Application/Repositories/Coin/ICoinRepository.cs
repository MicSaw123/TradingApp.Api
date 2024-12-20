using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.PaginationDto;

namespace TradingApp.Application.Repositories.CoinRepository
{
    public interface ICoinRepository
    {
        Task<RequestResult<IEnumerable<CoinDto>>> GetAllCoins();

        Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsBySymbol(List<string> symbols);

        Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsPerPage(PaginationDto pagination);

        Task<RequestResult<CoinDto>> GetCoinBySymbol(string symbol);
    }
}
