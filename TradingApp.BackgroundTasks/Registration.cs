using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TradingApp.BackgroundTasks.CoinBackgroundJobs;
using TradingApp.BackgroundTasks.FuturesTransactionsToOpenBackgroundJob;
using TradingApp.BackgroundTasks.SpotTransactionsJobs;

namespace TradingApp.BackgroundTasks
{
    public static class Registration
    {
        public static void AddBackgroundTasks(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                var openAwaitingSpotTransactions = JobKey.Create(nameof(OpenAwaitingSpotTransactions));
                var closeSpotTransactions = JobKey.Create(nameof(CloseSpotTransactions));
                var calculateSpotPortfolioProfit = JobKey.Create(nameof(CalculateSpotTransactionProfits));
                var openFuturesTransactionsToOpen = JobKey.Create(nameof(FuturesTransactionsToOpen));
                var updateAllTimeCoinValues = JobKey.Create(nameof(UpdateAllTimeCoinValues));

                options.AddJob<OpenAwaitingSpotTransactions>(openAwaitingSpotTransactions)
                .AddTrigger(trigger => trigger.ForJob(openAwaitingSpotTransactions).
                WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(20).RepeatForever()));

                options.AddJob<CloseSpotTransactions>(closeSpotTransactions).AddTrigger(trigger =>
                trigger.ForJob(closeSpotTransactions).
                WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(20).RepeatForever()));

                options.AddJob<CalculateSpotTransactionProfits>(calculateSpotPortfolioProfit)
                .AddTrigger(trigger => trigger.ForJob(calculateSpotPortfolioProfit).
                WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(20).RepeatForever()));

                options.AddJob<FuturesTransactionsToOpen>(openFuturesTransactionsToOpen).AddTrigger(trigger =>
                trigger.ForJob(openFuturesTransactionsToOpen).WithSimpleSchedule(schedule =>
                schedule.WithIntervalInSeconds(20).RepeatForever()));

                options.AddJob<UpdateAllTimeCoinValues>(updateAllTimeCoinValues).AddTrigger(trigger =>
                trigger.ForJob(updateAllTimeCoinValues).WithSimpleSchedule(schedule =>
                schedule.WithIntervalInSeconds(30).RepeatForever()));
            });

            services.AddQuartzHostedService();
        }
    }
}
