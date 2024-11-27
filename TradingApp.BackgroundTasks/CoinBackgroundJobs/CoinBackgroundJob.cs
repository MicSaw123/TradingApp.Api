using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingApp.Application.DataTransferObjects.ConnectionId;
using TradingApp.Application.DataTransferObjects.PaginationDto;
using TradingApp.Application.Realtime;
using TradingApp.Application.Services.CoinService;

namespace TradingApp.BackgroundTasks.CoinBackgroundJobs
{
    public class CoinBackgroundJob : BackgroundService
    {
        private readonly IHubContext<CoinListHub, ICoinListHub> _hubContext;
        private readonly IMemoryCache _memoryCache;
        private readonly ConnectionIdDto _connectionIdDto;
        private readonly IServiceProvider _serviceProvider;

        public CoinBackgroundJob(IHubContext<CoinListHub, ICoinListHub> hubContext,
            IMemoryCache memoryCache, ConnectionIdDto connectionIdDto, IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _memoryCache = memoryCache;
            _connectionIdDto = connectionIdDto;
            _serviceProvider = serviceProvider;
        }

        public override async Task StopAsync(CancellationToken cancellation)
        {
            await base.StopAsync(cancellation);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var connectionId = _connectionIdDto.GetConnectionId();
                var pageInfo = (PaginationDto)_memoryCache.Get(connectionId)!;

                if (pageInfo != null)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var coinService = scope.ServiceProvider.GetService<ICoinService>();
                        var coins = await coinService!.GetCoinsPerPage(pageInfo.PageSize, pageInfo.Page);
                        var coinList = coins.Result.ToList();
                        await _hubContext.Clients.All.GetCoinsPerPage(coinList);
                    }
                }
                await Task.Delay(20000);
            }
        }
    }
}
