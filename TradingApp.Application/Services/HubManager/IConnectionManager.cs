namespace TradingApp.Application.Services.ConnectionManager
{
    public interface IConnectionManager
    {
        void AddConnectionIdToList(string connectionId);

        Task<List<string>> GetAllConnections();
    }
}
