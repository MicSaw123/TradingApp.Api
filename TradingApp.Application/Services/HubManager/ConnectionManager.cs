
namespace TradingApp.Application.Services.ConnectionManager
{
    public class ConnectionManager : IConnectionManager
    {
        public List<string> ConnectionIds { get; set; } = new List<string>();

        public void AddConnectionIdToList(string connectionId)
        {
            ConnectionIds.Add(connectionId);
        }

        public Task<List<string>> GetAllConnections()
        {
            var connectionList = ConnectionIds;
            return Task.FromResult(connectionList);
        }

        public void RemoveConnectionById(string connectionId)
        {
            ConnectionIds.Remove(connectionId);
        }
    }
}
