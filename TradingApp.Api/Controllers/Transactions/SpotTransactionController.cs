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
    }
}
