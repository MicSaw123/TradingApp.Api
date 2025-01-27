using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Services.FuturesTransactionsService;

namespace TradingApp.Api.Controllers.FuturesTransactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuturesTransactionController : ApiControllerBase
    {
        private readonly IFuturesTransactionService _futuresTransactionService;

        public FuturesTransactionController(IFuturesTransactionService futuresTransactionService)
        {
            _futuresTransactionService = futuresTransactionService;
        }

        [HttpGet("GetFuturesTransactionsByPortfolioId")]
        public async Task<IActionResult> GetFuturesTransactionsByPortfolioId(int portfolioId)
        {
            return CreateResponse(await _futuresTransactionService.GetFuturesTransactionsByPortfolioId(portfolioId));
        }

        [HttpPut("CloseFuturesTransaction")]
        public async Task<IActionResult> CloseFuturesTransaction(int id, int portfolioId,
            CancellationToken cancellation = default)
        {
            return CreateResponse(await _futuresTransactionService.CloseFuturesTransaction(id, portfolioId, cancellation));
        }

        [HttpPut("EditFuturesTransaction")]
        public async Task<IActionResult> EditFuturesTransaction(FuturesTransactionDto futuresTransactionDto,
            CancellationToken cancellation = default)
        {
            return CreateResponse(await _futuresTransactionService
                .EditFuturesTransaction(futuresTransactionDto, cancellation));
        }
    }
}
