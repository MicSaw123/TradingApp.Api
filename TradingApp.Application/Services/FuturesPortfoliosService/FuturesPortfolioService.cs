using AutoMapper;
using TradingApp.Application.DataTransferObjects.Futures;
using TradingApp.Application.Repositories.FuturesPortfolios;
using TradingApp.Domain.Errors.Errors.SpotPortfolioErrors;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Services.FuturesPortfoliosService
{
    public class FuturesPortfolioService : IFuturesPortfolioService
    {
        private readonly IFuturesPortfolioRepository _futuresPortfolioRepository;
        private readonly IMapper _mapper;

        public FuturesPortfolioService(IFuturesPortfolioRepository futuresPortfolioRepository, IMapper mapper)
        {
            _futuresPortfolioRepository = futuresPortfolioRepository;
            _mapper = mapper;
        }

        public async Task<RequestResult> AddBalance(int portfolioId, float balanceToAdd, CancellationToken cancellation)
        {
            var portfolio = await _futuresPortfolioRepository.GetFuturesPortfolioById(portfolioId);
            try
            {
                portfolio.DisposableBalance += balanceToAdd;
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

        public async Task<RequestResult<FuturesPortfolioDto>> GetFuturesPortfolioDtoById(int portfolioId)
        {
            var futuresPortfolio = await _futuresPortfolioRepository
                .GetFuturesPortfolioById(portfolioId);
            var futuresPortfolioDto = _mapper.Map<FuturesPortfolioDto>(futuresPortfolio);
            if (futuresPortfolioDto is null)
            {
                return RequestResult<FuturesPortfolioDto>.Failure(PortfolioError.ErrorGetPortfolioById);
            }

            return RequestResult<FuturesPortfolioDto>.Success(futuresPortfolioDto);
        }

        public async Task<FuturesPortfolio> GetFuturesPortfolioById(int portfolioId)
        {
            return await _futuresPortfolioRepository.GetFuturesPortfolioById(portfolioId);
        }

        public async Task<List<FuturesPortfolio>> GetFuturesPortfolios()
        {
            return await _futuresPortfolioRepository.GetFuturesPortfolios();
        }

        public async Task UpdateFuturesPortfolios(List<FuturesPortfolio> futuresPortfolios, CancellationToken cancellation)
        {
            await _futuresPortfolioRepository.UpdateFuturesPortfolios(futuresPortfolios, cancellation);
        }

        public async Task UpdateFuturesPortfolio(FuturesPortfolio futuresPortfolio, CancellationToken cancellation)
        {
            await _futuresPortfolioRepository.UpdateFuturesPortfolio(futuresPortfolio, cancellation);
        }

        public async Task<RequestResult> SubtractBalance(int portfolioId, float balanceToSubtract,
            CancellationToken cancellation)
        {
            var portfolio = await _futuresPortfolioRepository.GetFuturesPortfolioById(portfolioId);
            try
            {
                portfolio.DisposableBalance -= balanceToSubtract;
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
