using TradingApp.Application.Repositories.SpotPortfolioRepository;
using TradingApp.Domain.Errors.Errors.SpotPortfolioErrors;
using TradingApp.Domain.Spot;

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
            var spotPortfolio = await _spotPortfolioRepository.GetSpotPortfolioById(id);
            spotPortfolio.Balance += amountToAdd;
            if (spotPortfolio is null)
            {
                return RequestResult.Failure(PortfolioError.ErrorAddFunds);
            }
            await _spotPortfolioRepository.UpdateSpotPortfolio(spotPortfolio, cancellation);
            return RequestResult.Success();
        }

        public async Task<RequestResult> AddSpotPortfolio(SpotPortfolio spotPortfolio, CancellationToken cancellation)
        {
            await _spotPortfolioRepository.AddSpotPortfolio(spotPortfolio, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return RequestResult.Failure(PortfolioError.ErrorAddPortfolio);
            }
            return RequestResult.Success();
        }

        public async Task EditSpotPortfolio(SpotPortfolio spotPortfolio, CancellationToken cancellation)
        {
            await _spotPortfolioRepository.UpdateSpotPortfolio(spotPortfolio, cancellation);
        }

        public async Task<RequestResult> EditSpotPortfolios(List<SpotPortfolio> spotPortfolios, CancellationToken cancellation)
        {
            await _spotPortfolioRepository.UpdateSpotPortfolios(spotPortfolios, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return RequestResult.Failure(PortfolioError.ErrorUpdatePortfolio);
            }
            return RequestResult.Success();
        }

        public Task<SpotPortfolio> GetSpotPortfolioById(int portfolioId)
        {
            var spotPortfolio = _spotPortfolioRepository.GetSpotPortfolioById(portfolioId);
            return spotPortfolio;
        }

        public async Task<RequestResult<SpotPortfolio>> GetSpotPortfolioByUserId(string userId)
        {
            var spotPortfolio = await _spotPortfolioRepository.GetSpotPortfolioByUserId(userId);
            if (spotPortfolio is null)
            {
                return RequestResult<SpotPortfolio>.Failure(PortfolioError.ErrorGetPortfolioByUserId);
            }
            return RequestResult<SpotPortfolio>.Success(spotPortfolio);
        }

        public async Task<RequestResult<IEnumerable<SpotPortfolio>>> GetSpotPortfolios()
        {
            var result = await _spotPortfolioRepository.GetSpotPortfolios();
            if (result is null)
            {
                return RequestResult<IEnumerable<SpotPortfolio>>.Failure(PortfolioError.ErrorGetPortfolioById);
            }
            return RequestResult<IEnumerable<SpotPortfolio>>.Success(result);
        }

        public async Task<RequestResult> RemoveMonthlyProfitFromPortfolio(int portfolioId, float amountToRemove, CancellationToken cancellation)
        {
            var spotPortfolio = await _spotPortfolioRepository.GetSpotPortfolioById(portfolioId);
            try
            {
                spotPortfolio.MonthlyProfit -= amountToRemove;
                await _spotPortfolioRepository.UpdateSpotPortfolio(spotPortfolio, cancellation);
            }
            catch (Exception ex)
            {
                return RequestResult.Failure(PortfolioError.ErrorRemoveProfits);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> RemoveWeeklyProfitFromPortfolio(int portfolioId, float amountToRemove, CancellationToken cancellation)
        {
            var spotPortfolio = await _spotPortfolioRepository.GetSpotPortfolioById(portfolioId);
            try
            {
                spotPortfolio.WeeklyProfit -= amountToRemove;
                await _spotPortfolioRepository.UpdateSpotPortfolio(spotPortfolio, cancellation);
            }
            catch (Exception ex)
            {
                return RequestResult.Failure(PortfolioError.ErrorRemoveProfits);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> SubtractBalance(int id, float amountToSubtract,
            CancellationToken cancellation)
        {
            var spotPortfolio = await _spotPortfolioRepository.GetSpotPortfolioById(id);
            spotPortfolio.Balance -= amountToSubtract;
            if (spotPortfolio.Balance < 0)
            {
                return RequestResult.Failure(PortfolioError.NonSufficientFunds);
            }
            await _spotPortfolioRepository.UpdateSpotPortfolio(spotPortfolio, cancellation);
            return RequestResult.Success();
        }
    }
}
