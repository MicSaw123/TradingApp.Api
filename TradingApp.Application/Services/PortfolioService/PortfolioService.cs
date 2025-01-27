using AutoMapper;
using TradingApp.Application.DataTransferObjects.Portfolio;
using TradingApp.Application.Repositories.SummaryPortfolio;
using TradingApp.Domain.Errors.Errors.SpotPortfolioErrors;
using TradingApp.Domain.SummaryPortfolio;

namespace TradingApp.Application.Services.PortfolioService
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IMapper _mapper;

        public PortfolioService(IPortfolioRepository portfolioRepository, IMapper mapper)
        {
            _portfolioRepository = portfolioRepository;
            _mapper = mapper;
        }

        public async Task<RequestResult> AddPortfolio(Portfolio portfolio, CancellationToken cancellation)
        {
            if (portfolio is null)
            {
                return RequestResult.Failure(PortfolioError.ErrorAddPortfolio);
            }
            await _portfolioRepository.AddPortfolio(portfolio, cancellation);
            return RequestResult.Success();
        }

        public async Task<RequestResult<PortfolioDto>> GetPortfolioByUserId(string userId)
        {
            var portfolio = await _portfolioRepository.GetPortfolioByUserId(userId);
            if (portfolio is null)
            {
                return RequestResult<PortfolioDto>.Failure(PortfolioError.ErrorGetPortfolioByUserId);
            }
            var portfolioDto = _mapper.Map<PortfolioDto>(portfolio);
            return RequestResult<PortfolioDto>.Success(portfolioDto);
        }
    }
}
