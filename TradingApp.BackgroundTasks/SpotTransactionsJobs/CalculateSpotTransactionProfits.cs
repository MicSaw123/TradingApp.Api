using Quartz;
using TradingApp.Application.Services.SpotTransactionService;

namespace TradingApp.BackgroundTasks.SpotTransactionsJobs
{
    internal class CalculateSpotTransactionProfits : IJob
    {
        private readonly ISpotTransactionService _spotTransactionService;

        public CalculateSpotTransactionProfits(ISpotTransactionService spotTransactionService)
        {
            _spotTransactionService = spotTransactionService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            CancellationToken cancellation = default;
            await _spotTransactionService.CalculateSpotTransactionProfit(cancellation);
            return;
        }
    }
}
