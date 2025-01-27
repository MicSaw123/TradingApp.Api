using TradingApp.Application.Repositories.FuturesPortfolios;
using TradingApp.Domain.Errors.Errors.SpotPortfolioErrors;
using TradingApp.Domain.Futures;

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
            var portfolio = await _futuresPortfolioRepository.GetFuturesPortfolioById(portfolioId);
            try
            {
                portfolio.Balance += balanceToAdd;
                await _futuresPortfolioRepository.UpdateFuturesPortfolio(portfolio, cancellation);
                return RequestResult.Success();

            }
            catch (Exception ex)
            {
                return RequestResult.Failure(PortfolioError.ErrorGetPortfolioById);

            }
        }

        public async Task<RequestResult> AddFuturesPortfolio(FuturesPortfolio portfolio, CancellationToken cancellation)
        {
            await _futuresPortfolioRepository.AddFuturesPortfolio(portfolio, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return RequestResult.Failure(PortfolioError.ErrorAddPortfolio);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> SubtractBalance(int portfolioId, float balanceToSubtract,
            CancellationToken cancellation)
        {
            var portfolio = await _futuresPortfolioRepository.GetFuturesPortfolioById(portfolioId);
            try
            {
                portfolio.Balance -= balanceToSubtract;
                await _futuresPortfolioRepository.UpdateFuturesPortfolio(portfolio, cancellation);
                return RequestResult.Success();
            }
            catch (Exception ex)
            {
                return RequestResult.Failure(PortfolioError.NonSufficientFunds);
            }
        }
    }
}
