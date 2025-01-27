using TradingApp.Domain.Spot;

namespace TradingApp.Application.Services.SpotPortfolioService
{
    public interface ISpotPortfolioService
    {
        Task<RequestResult> SubtractBalance(int id, float amountToSubtract,
            CancellationToken cancellation);

        Task<RequestResult> AddBalance(int id, float amountToAdd,
            CancellationToken cancellation);

        Task<RequestResult> RemoveMonthlyProfitFromPortfolio(int portfolioId, float amountToRemove, CancellationToken cancellation);

        Task<RequestResult> RemoveWeeklyProfitFromPortfolio(int portfolioId, float amountToRemove, CancellationToken cancellation);

        Task<RequestResult<IEnumerable<SpotPortfolio>>> GetSpotPortfolios();

        Task<RequestResult> EditSpotPortfolios(List<SpotPortfolio> spotPortfolio, CancellationToken cancellation);

        Task<SpotPortfolio> GetSpotPortfolioById(int portfolioId);

        Task EditSpotPortfolio(SpotPortfolio spotPortfolio, CancellationToken cancellation);

        Task<RequestResult<SpotPortfolio>> GetSpotPortfolioByUserId(string userId);

        Task<RequestResult> AddSpotPortfolio(SpotPortfolio spotPortfolio, CancellationToken cancellation);
    }
}
