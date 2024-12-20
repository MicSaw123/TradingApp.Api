using Quartz;
using TradingApp.Application.Services.FuturesTransactionsToOpenService;

namespace TradingApp.BackgroundTasks.FuturesTransactionsToOpenBackgroundJob
{
    public class FuturesTransactionsToOpen : IJob
    {
        private readonly IFuturesTransactionsToOpenService _futuresTransactionsToOpenService;

        public FuturesTransactionsToOpen(IFuturesTransactionsToOpenService futuresTransactionsToOpenService)
        {
            _futuresTransactionsToOpenService = futuresTransactionsToOpenService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            CancellationToken cancellation = default;
            await _futuresTransactionsToOpenService.OpenFuturesTransactionToOpen(cancellation);
            return;
        }
    }
}
