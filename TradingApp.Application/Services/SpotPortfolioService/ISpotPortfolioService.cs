using TradingApp.Application.DataTransferObjects.Portfolio;

namespace TradingApp.Application.Services.SpotPortfolioService
{
    public interface ISpotPortfolioService
    {
        Task<RequestResult<SpotPortfolioDto>> GetSpotPortfolioByUserId(string userId);

        Task<RequestResult> SubtractBalance(int id, float amountToSubtract,
            CancellationToken cancellation);

        Task<RequestResult> AddBalance(int id, float amountToAdd,
            CancellationToken cancellation);
    }
}
