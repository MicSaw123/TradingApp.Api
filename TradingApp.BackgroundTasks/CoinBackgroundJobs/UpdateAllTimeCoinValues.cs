using Quartz;
using TradingApp.Application.Services.CoinService;

namespace TradingApp.BackgroundTasks.CoinBackgroundJobs
{
    internal class UpdateAllTimeCoinValues : IJob
    {
        private readonly ICoinService _coinService;

        public UpdateAllTimeCoinValues(ICoinService coinService)
        {
            _coinService = coinService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            CancellationToken cancellation = default;
            await _coinService.UpdateAllTimeValues(cancellation);
            return;
        }
    }
}
