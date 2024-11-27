using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.ConnectionId;
using TradingApp.Application.DataTransferObjects.PaginationDto;

namespace TradingApp.Application.Realtime
{
    public class CoinListHub : Hub<ICoinListHub>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ConnectionIdDto _connectionId;

        public CoinListHub(IMemoryCache memoryCache, ConnectionIdDto connectionId)
        {
            _memoryCache = memoryCache;
            _connectionId = connectionId;
        }

        public async Task GetPaginationParameters(int pageSize, int page)
        {
            _memoryCache.Set(_connectionId.GetConnectionId(), (pageSize, page), TimeSpan.FromSeconds(10));
        }

        public async Task GetCoinsPerPage(List<CoinDto> coins)
        {
            return;
        }

        public override async Task OnConnectedAsync()
        {
            _connectionId.SetConnectionId(Context.ConnectionId);
            _memoryCache.Set(_connectionId.GetConnectionId(), new PaginationDto
            {
                Page = 1,
                PageSize = 15,
            }, TimeSpan.FromSeconds(10));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _memoryCache.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
