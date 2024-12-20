
namespace TradingApp.Application.Services.ConnectionManager
{
    public class ConnectionManager : IConnectionManager
    {
        public List<string> ConncetionIds { get; set; } = new List<string>();

        public void AddConnectionIdToList(string connectionId)
        {

            ConncetionIds.Add(connectionId);
        }

        public Task<List<string>> GetAllConnections()
        {
            var connectionList = ConncetionIds;
            return Task.FromResult(connectionList);
        }
    }
}
