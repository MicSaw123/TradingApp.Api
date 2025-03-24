using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.Services.SpotPortfolioService;

namespace TradingApp.Api.Controllers.SpotPortfolio
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotPortfolioController : ApiControllerBase
    {
        private readonly ISpotPortfolioService _spotPortfolioService;

        public SpotPortfolioController(ISpotPortfolioService spotPortfolioService)
        {
            _spotPortfolioService = spotPortfolioService;
        }

        [HttpPut("SubtractBalance")]
        public async Task<IActionResult> SubtractBalance(int id, float amountToSubtract,
            CancellationToken cancellation)
        {
            var result = await _spotPortfolioService.SubtractBalance(id,
                amountToSubtract, cancellation);
            return CreateResponse(result);
        }

        [HttpGet("GetSpotPortfolioById")]
        public async Task<IActionResult> GetSpotPortfolioById(int portfolioId)
        {
            return CreateResponse(await _spotPortfolioService.GetSpotPortfolioDtoById(portfolioId));
        }

        [HttpPut("AddBalance")]
        public async Task<IActionResult> AddBalance(int id, float amountToAdd,
            CancellationToken cancellation)
        {
            var result = await _spotPortfolioService.AddBalance(id, amountToAdd, cancellation);
            return CreateResponse(result);
        }
    }
}
