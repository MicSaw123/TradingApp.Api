using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.PaginationDto;

namespace TradingApp.Application.Services.CoinService
{
    public interface ICoinService
    {
        public Task<RequestResult<IEnumerable<CoinDto>>> GetCoins();

        public Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsBySymbol(List<string> symbols);

        public Task<RequestResult> SeedCoins(CancellationToken cancellation);

        public Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsPerPage(PaginationDto paginationDto);
    }
}
