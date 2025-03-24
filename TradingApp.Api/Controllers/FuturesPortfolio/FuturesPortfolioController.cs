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
        public async Task<IActionResult> AddBalance(int id, float amountToAdd, CancellationToken cancellation)
        {
            return CreateResponse(await _futuresPortfolioService.AddBalance(id, amountToAdd, cancellation));
        }

        [HttpGet("GetFuturesPortfolioById")]
        public async Task<IActionResult> GetFuturesPortfolioById(int portfolioId)
        {
            return CreateResponse(await _futuresPortfolioService.GetFuturesPortfolioDtoById(portfolioId));
        }

        [HttpPost("SubtractBalance")]
        public async Task<IActionResult> Subtract(int id, float amountToSubtract, CancellationToken cancellation)
        {
            return CreateResponse
                (await _futuresPortfolioService.SubtractBalance(id, amountToSubtract, cancellation));
        }
    }
}
