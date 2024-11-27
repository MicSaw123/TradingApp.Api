using TradingApp.Application.DataTransferObjects.Coin;

namespace TradingApp.Application.Realtime
{
    public interface ICoinListHub
    {
        Task GetCoinsPerPage(List<CoinDto> coins);

        Task GetPaginationParameters(int pageSize, int page);
    }
}
