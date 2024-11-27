using TradingApp.Application.DataTransferObjects.Portfolio;
using TradingApp.Application.Repositories.SpotPortfolioRepository;

namespace TradingApp.Application.Services.SpotPortfolioService
{
    public class SpotPortfolioService : ISpotPortfolioService
    {
        private readonly ISpotPortfolioRepository _spotPortfolioRepository;

        public SpotPortfolioService(ISpotPortfolioRepository spotPortfolioRepository)
        {
            _spotPortfolioRepository = spotPortfolioRepository;
        }

        public async Task<RequestResult> AddBalance(int id, float amountToAdd, CancellationToken cancellation)
        {
            var result = await _spotPortfolioRepository.
                AddBalance(id, amountToAdd, cancellation);
            return result;
        }

        public async Task<RequestResult<SpotPortfolioDto>> GetSpotPortfolioByUserId(string userId)
        {
            var result = await _spotPortfolioRepository.GetSpotPortfolioByUserId(userId);
            return result;
        }

        public async Task<RequestResult> SubtractBalance(int id, float amountToSubtract,
            CancellationToken cancellation)
        {
            var result = await _spotPortfolioRepository.SubtractBalance(id,
                amountToSubtract, cancellation);
            return result;
        }
    }
}
