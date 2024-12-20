using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.DataTransferObjects.Futures;
using TradingApp.Application.Services.FuturesTransactionsToOpenService;

namespace TradingApp.Api.Controllers.FuturesTransactionsToOpen
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuturesTransactionsToOpen : ApiControllerBase
    {
        private readonly IFuturesTransactionsToOpenService _futuresTransactionsToOpenService;

        public FuturesTransactionsToOpen(IFuturesTransactionsToOpenService futuresTransactionsToOpenService)
        {
            _futuresTransactionsToOpenService = futuresTransactionsToOpenService;
        }

        [HttpPost("AddFuturesTransactionToOpen")]
        public async Task<RequestResult> AddFuturesTransactionToOpen(FuturesTransactionToOpenDto transactionToOpen,
            CancellationToken cancellation)
        {
            return await _futuresTransactionsToOpenService.AddFuturesTransactionToOpen(transactionToOpen, cancellation);
        }

        [HttpDelete("CancelFuturesTransactionToOpen")]
        public async Task<RequestResult> CancelFuturesTransactionToOpen(int id, int futuresPortfolioId,
            CancellationToken cancellation)
        {
            return await _futuresTransactionsToOpenService
                .CancelFuturesTransactionToOpen(id, futuresPortfolioId, cancellation);
        }

        [HttpPut("EditFuturesTransactionToOpen")]
        public async Task<RequestResult> EditFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation)
        {
            return await _futuresTransactionsToOpenService
                .EditFuturesTransactionToOpen(futuresTransactionToOpenDto, cancellation);
        }
    }
}
