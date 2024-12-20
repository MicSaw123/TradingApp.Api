
using TradingApp.Application.Repositories.FuturesPortfolios;

namespace TradingApp.Application.Services.FuturesPortfoliosService
{
    public class FuturesPortfolioService : IFuturesPortfolioService
    {
        private readonly IFuturesPortfolioRepository _futuresPortfolioRepository;

        public FuturesPortfolioService(IFuturesPortfolioRepository futuresPortfolioRepository)
        {
            _futuresPortfolioRepository = futuresPortfolioRepository;
        }

        public async Task<RequestResult> AddBalance(int portfolioId, float balanceToAdd, CancellationToken cancellation)
        {
            var result = await _futuresPortfolioRepository.AddBalance(portfolioId, balanceToAdd, cancellation);
            return result;
        }

        public async Task<RequestResult> SubtractBalance(int portfolioId, float balanceToSubtract,
            CancellationToken cancellation)
        {
            var result = await _futuresPortfolioRepository.SubtractBalance(portfolioId, balanceToSubtract, cancellation);
            return result;
        }
    }
}
