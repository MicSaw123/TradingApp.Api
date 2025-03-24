using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.PaginationDto;

namespace TradingApp.Application.Services.CoinService
{
    public interface ICoinService
    {
        public Task<RequestResult<List<CoinDto>>> GetCoins();

        public Task<RequestResult<CoinDto>> GetCoinBySymbol(string symbol);

        public Task<RequestResult> SeedCoins(CancellationToken cancellation);

        public Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsPerPage(PaginationDto paginationDto);

        public Task<RequestResult> UpdateAllTimeValues(CancellationToken cancellation);
    }
}
