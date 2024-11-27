using TradingApp.Application.DataTransferObjects.Portfolio;

namespace TradingApp.Application.Repositories.SpotPortfolioRepository
{
    public interface ISpotPortfolioRepository
    {
        Task<RequestResult<SpotPortfolioDto>> GetSpotPortfolioByUserId(string userId);
        Task<RequestResult> SubtractBalance(int id, float amountToSubtract,
            CancellationToken cancellation);

        Task<RequestResult> AddBalance(int id, float amountToAdd,
            CancellationToken cancellation);

        Task<RequestResult<SpotPortfolioDto>> GetSpotPortfolioById(int id);

        Task<RequestResult> CalculateProfits(CancellationToken cancellation);
    }
}
