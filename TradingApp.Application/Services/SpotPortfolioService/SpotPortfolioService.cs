using AutoMapper;
using TradingApp.Application.DataTransferObjects.Portfolio;
using TradingApp.Application.Repositories.SpotPortfolioRepository;
using TradingApp.Domain.Errors.Errors.SpotPortfolioErrors;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Services.SpotPortfolioService
{
    public class SpotPortfolioService : ISpotPortfolioService
    {
        private readonly ISpotPortfolioRepository _spotPortfolioRepository;
        private readonly IMapper _mapper;

        public SpotPortfolioService(ISpotPortfolioRepository spotPortfolioRepository, IMapper mapper)
        {
            _spotPortfolioRepository = spotPortfolioRepository;
            _mapper = mapper;
        }

        public async Task<RequestResult> AddBalance(int id, float amountToAdd, CancellationToken cancellation)
        {
            var spotPortfolio = await _spotPortfolioRepository.GetSpotPortfolioById(id);
            spotPortfolio.DisposableBalance += amountToAdd;
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

        public async Task<SpotPortfolio> GetSpotPortfolioById(int portfolioId)
        {
            var spotPortfolio = await _spotPortfolioRepository.GetSpotPortfolioById(portfolioId);
            return spotPortfolio;
        }

        public async Task<RequestResult<SpotPortfolioDto>> GetSpotPortfolioDtoById(int portfolioId)
        {
            var spotPortfolio = await GetSpotPortfolioById(portfolioId);
            var spotPortfolioDto = _mapper.Map<SpotPortfolioDto>(spotPortfolio);
            if (spotPortfolioDto is null)
            {
                return RequestResult<SpotPortfolioDto>.Failure(PortfolioError.ErrorGetPortfolioById);
            }
            return RequestResult<SpotPortfolioDto>.Success(spotPortfolioDto);
        }

        public async Task<RequestResult<List<SpotPortfolio>>> GetSpotPortfolios()
        {
            var result = await _spotPortfolioRepository.GetSpotPortfolios();
            if (result is null)
            {
                return RequestResult<List<SpotPortfolio>>.Failure(PortfolioError.ErrorGetPortfolioById);
            }
            return RequestResult<List<SpotPortfolio>>.Success(result);
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
            spotPortfolio.DisposableBalance -= amountToSubtract;
            if (spotPortfolio.DisposableBalance < 0)
            {
                return RequestResult.Failure(PortfolioError.NonSufficientFunds);
            }
            await _spotPortfolioRepository.UpdateSpotPortfolio(spotPortfolio, cancellation);
            return RequestResult.Success();
        }
    }
}
