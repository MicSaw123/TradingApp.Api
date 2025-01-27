using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotPortfolioRepository
{
    public interface ISpotPortfolioRepository
    {
        Task<IEnumerable<SpotPortfolio>> GetSpotPortfolios();

        Task UpdateSpotPortfolio(SpotPortfolio spotPortfolio, CancellationToken cancellation);

        Task<SpotPortfolio> GetSpotPortfolioById(int id);

        Task UpdateSpotPortfolios(List<SpotPortfolio> spotPortfolios, CancellationToken cancellation);

        Task<SpotPortfolio> GetSpotPortfolioByUserId(string userId);

        Task AddSpotPortfolio(SpotPortfolio spotportfolio, CancellationToken cancellation);
    }
}
