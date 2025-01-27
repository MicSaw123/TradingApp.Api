using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.Services.PortfolioService;

namespace TradingApp.Api.Controllers.Portfolio
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ApiControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPortfolioByUserId(string userId)
        {
            return CreateResponse(await _portfolioService.GetPortfolioByUserId(userId));
        }
    }
}
