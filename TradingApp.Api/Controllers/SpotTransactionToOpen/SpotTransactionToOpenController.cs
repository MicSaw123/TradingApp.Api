using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.DataTransferObjects.Spot;
using TradingApp.Application.Services.SpotTransactionToOpenService;

namespace TradingApp.Api.Controllers.SpotTransactionToOpen
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotTransactionToOpenController : ApiControllerBase
    {
        private readonly ISpotTransactionToOpenService _spotTransactionToOpenService;

        public SpotTransactionToOpenController(ISpotTransactionToOpenService
            spotTransactionToOpenService)
        {
            _spotTransactionToOpenService = spotTransactionToOpenService;
        }

        [HttpPost("CreateAwaitngSpotTransaction")]
        public async Task<IActionResult> CreateAwaitingSpotTransaction(
            SpotTransactionToOpenDto spotTransactionToOpenDto,
            CancellationToken cancellation = default)
        {
            return CreateResponse(await _spotTransactionToOpenService
                .AddAwaitingTransactionToSpotPortfolio(spotTransactionToOpenDto, cancellation));
        }

        [HttpPut("OpenWaitingSpotTransaction")]
        public async Task<IActionResult> OpenWaitingSpotTransaction
            (CancellationToken cancellation = default)
        {
            return CreateResponse(await _spotTransactionToOpenService
                .OpenWaitingSpotTransaction(cancellation));
        }

        [HttpDelete("CancelAwaitingSpotTransaction")]
        public async Task<IActionResult> CancelAwaitingSpotTransaction(int id, int spotPortfolioId,
            CancellationToken cancellation = default)
        {
            return CreateResponse(await _spotTransactionToOpenService.
                CancelAwaitingSpotTransaction(id, spotPortfolioId, cancellation));
        }

        [HttpPut("EditAwaitingSpotTransaction")]
        public async Task<IActionResult> EditAwaitingSpotTransaction(SpotTransactionToOpenDto spotTransactionToOpenDto,
            CancellationToken cancellation = default)
        {
            return CreateResponse
                (await _spotTransactionToOpenService.EditAwaitingSpotTransaction(spotTransactionToOpenDto, cancellation));
        }
    }
}
