using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotPortfolioRepository
{
    public interface ISpotPortfolioRepository
    {
        Task<List<SpotPortfolio>> GetSpotPortfolios();

        Task UpdateSpotPortfolio(SpotPortfolio spotPortfolio, CancellationToken cancellation);

        Task<SpotPortfolio> GetSpotPortfolioById(int id);

        Task UpdateSpotPortfolios(List<SpotPortfolio> spotPortfolios, CancellationToken cancellation);

        Task AddSpotPortfolio(SpotPortfolio spotportfolio, CancellationToken cancellation);
    }
}
