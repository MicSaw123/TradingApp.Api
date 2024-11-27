using Quartz;
using TradingApp.Application.Services.SpotTransactionToOpenService;

namespace TradingApp.BackgroundTasks.SpotTransactionsJobs
{
    public class OpenAwaitingSpotTransactions : IJob
    {
        private readonly ISpotTransactionToOpenService _spotTransactionToOpenService;

        public OpenAwaitingSpotTransactions(ISpotTransactionToOpenService spotTransactionToOpenService)
        {
            _spotTransactionToOpenService = spotTransactionToOpenService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            CancellationToken cancellation = default;
            await _spotTransactionToOpenService.OpenWaitingSpotTransaction(cancellation);
            return;
        }
    }
}
