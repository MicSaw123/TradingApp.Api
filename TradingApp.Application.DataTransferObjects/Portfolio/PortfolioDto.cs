namespace TradingApp.Application.DataTransferObjects.Portfolio
{
    public class PortfolioDto
    {
        public string UserId { get; set; } = string.Empty;

        public float Balance { get; set; }

        public float DailyProfit { get; set; }

        public float WeeklyProfit { get; set; }

        public int FuturesPortfolioId { get; set; }

        public int SpotPortfolioId { get; set; }
    }
}
