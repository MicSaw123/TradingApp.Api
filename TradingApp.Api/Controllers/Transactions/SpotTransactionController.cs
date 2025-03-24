using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Services.SpotTransactionService;

namespace TradingApp.Api.Controllers.Transactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotTransactionController : ApiControllerBase
    {
        private readonly ISpotTransactionService _spotTransactionService;

        public SpotTransactionController(ISpotTransactionService spotTransactionService)
        {
            _spotTransactionService = spotTransactionService;
        }

        [HttpPut("EditSpotTransaction")]
        public async Task<IActionResult> EditSpotTransaction(SpotTransactionDto transactionDto,
            CancellationToken cancellation = default)
        {
            return CreateResponse(await _spotTransactionService.EditSpotTransaction(transactionDto, cancellation));
        }

        [HttpGet("GetInactiveSpotTransactionsByPortfolioId")]
        public async Task<IActionResult> GetInactiveSpotTransactionsByPortfolioId(int portfolioId)
        {
            return CreateResponse(await _spotTransactionService
                .GetInactiveSpotTransactionsByPortfolioId(portfolioId));
        }

        [HttpGet("GetActiveSpotTransactionsByPortfolioId")]
        public async Task<IActionResult> GetActiveSpotTransactionsByPortfolioId(int portfolioId)
        {
            return CreateResponse(await _spotTransactionService.GetActiveTransactionsByPortfolioId(portfolioId));
        }
    }
}
