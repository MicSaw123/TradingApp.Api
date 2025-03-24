using Quartz;
using TradingApp.Application.Services.SpotTransactionService;

namespace TradingApp.BackgroundTasks.SpotTransactionsJobs
{
    internal class CloseSpotTransactions : IJob
    {
        private readonly ISpotTransactionService _spotTransactionService;

        public CloseSpotTransactions(ISpotTransactionService spotTransactionService)
        {
            _spotTransactionService = spotTransactionService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                CancellationToken cancellation = default;
                await _spotTransactionService.CloseExistingSpotTransactions(cancellation);
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
