using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.Services.CoinService;

namespace TradingApp.Api.Controllers.Coin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinController : ApiControllerBase
    {
        private readonly ICoinService _coinService;

        public CoinController(ICoinService coinService)
        {
            _coinService = coinService;
        }

        [HttpGet("GetCoins")]
        public async Task<IActionResult> GetCoins()
        {
            return CreateResponse(await _coinService.GetCoins());
        }

        [HttpGet("GetCoinNameById")]
        public async Task<IActionResult> GetCoinNameById(int coinId)
        {
            return CreateResponse(await _coinService.GetCoinNameById(coinId));
        }

        [HttpPost("GetCoinsBySymbol")]
        public async Task<IActionResult> GetCoinsBySymbol([FromBody] List<string> symbols)
        {
            return CreateResponse(await _coinService.GetCoinsBySymbol(symbols));
        }

        [HttpPost("SeedCoins")]
        public async Task<IActionResult> SeedCoins(CancellationToken cancellation)
        {
            return CreateResponse(await _coinService.SeedCoins(cancellation));
        }

        [HttpGet("GetCoinsPerPage")]
        public async Task<IActionResult> GetCoinsPerPage(int pageSize, int page)
        {
            return CreateResponse(await _coinService.GetCoinsPerPage(pageSize, page));
        }
    }
}
