using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.Services.FuturesPortfoliosService;

namespace TradingApp.Api.Controllers.FuturesPortfolio
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuturesPortfolioController : ApiControllerBase
    {
        private readonly IFuturesPortfolioService _futuresPortfolioService;

        public FuturesPortfolioController(IFuturesPortfolioService futuresPortfolioService)
        {
            _futuresPortfolioService = futuresPortfolioService;
        }

        [HttpPost("AddBalance")]
        public async Task<RequestResult> AddBalance(int id, float amountToAdd, CancellationToken cancellation)
        {
            return await _futuresPortfolioService.AddBalance(id, amountToAdd, cancellation);
        }

        [HttpPost("SubtractBalance")]
        public async Task<RequestResult> Subtract(int id, float amountToSubtract, CancellationToken cancellation)
        {
            return await _futuresPortfolioService.SubtractBalance(id, amountToSubtract, cancellation);
        }
    }
}
