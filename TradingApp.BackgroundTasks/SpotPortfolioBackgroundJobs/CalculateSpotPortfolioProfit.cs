using Quartz;
using TradingApp.Application.Services.SpotTransactionService;

namespace TradingApp.BackgroundTasks.SpotPortfolioBackgroundJobs
{
    internal class CalculateSpotPortfolioProfit : IJob
    {
        private readonly ISpotTransactionService _spotTransactionService;

        public CalculateSpotPortfolioProfit(ISpotTransactionService spotTransactionService)
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
