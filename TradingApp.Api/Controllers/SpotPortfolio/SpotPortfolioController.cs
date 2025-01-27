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

        [HttpGet("GetSpotPortfolioByUserId")]
        public async Task<IActionResult> GetSpotPortfolioByUserId(string userId)
        {
            var result = await _spotPortfolioService.GetSpotPortfolioByUserId(userId);
            return CreateResponse(result);
        }

        [HttpPut("SubtractBalance")]
        public async Task<IActionResult> SubtractBalance(int id, float amountToSubtract,
            CancellationToken cancellation)
        {
            var result = await _spotPortfolioService.SubtractBalance(id,
                amountToSubtract, cancellation);
            return CreateResponse(result);
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
