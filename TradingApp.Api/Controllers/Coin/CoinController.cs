using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.DataTransferObjects.PaginationDto;
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

        [HttpPost("GetCoinsPerPage")]
        public async Task<IActionResult> GetCoinsPerPage([FromBody] PaginationDto paginationDto)
        {
            return CreateResponse(await _coinService.GetCoinsPerPage(paginationDto));
        }
    }
}
