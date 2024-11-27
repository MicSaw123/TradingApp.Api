namespace TradingApp.Application.DataTransferObjects.PaginationDto
{
    public class PaginationDto
    {
        public string ConnectionId { get; set; } = string.Empty;

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
