namespace TradingApp.Application.DataTransferObjects.ConnectionId
{
    public class ConnectionIdDto
    {
        private static ConnectionIdDto _instance;

        string ConnectionId { get; set; } = string.Empty;

        public string SetConnectionId(string connectionId)
        {
            if (_instance == null)
            {
                _instance = new ConnectionIdDto();
                _instance.ConnectionId = connectionId;
            }
            return _instance.ConnectionId;
        }

        public string GetConnectionId()
        {
            if (_instance == null)
            {
                _instance = new ConnectionIdDto();
            }
            return _instance.ConnectionId;
        }
    }
}
