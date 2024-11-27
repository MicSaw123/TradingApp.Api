using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TradingApp.BackgroundTasks.SpotPortfolioBackgroundJobs;
using TradingApp.BackgroundTasks.SpotTransactionsJobs;

namespace TradingApp.BackgroundTasks
{
    public static class Registration
    {
        public static void AddBackgroundTasks(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
                var openAwaitingSpotTransactions = JobKey.Create(nameof(OpenAwaitingSpotTransactions));
                var closeSpotTransactions = JobKey.Create(nameof(CloseSpotTransactions));
                var calculateSpotTransactionProfits = JobKey.Create(nameof(CalculateSpotPortfolioProfit));

                options.AddJob<OpenAwaitingSpotTransactions>(openAwaitingSpotTransactions)
                .AddTrigger(trigger => trigger.ForJob(openAwaitingSpotTransactions).
                WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(20).RepeatForever()));

                options.AddJob<CloseSpotTransactions>(closeSpotTransactions).AddTrigger(trigger =>
                trigger.ForJob(closeSpotTransactions).
                WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(20).RepeatForever()));

                options.AddJob<CalculateSpotPortfolioProfit>(calculateSpotTransactionProfits)
                .AddTrigger(trigger => trigger.ForJob(calculateSpotTransactionProfits).
                WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(20).RepeatForever()));

            });

            services.AddQuartzHostedService();
        }
    }
}
