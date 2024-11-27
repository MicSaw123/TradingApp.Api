namespace TradingApp.Application.DataTransferObjects.Portfolio
{
    public class PortfolioDto
    {
        public string TradingAppUserId { get; set; } = string.Empty;

        public float Balance { get; set; }

        public float DailyProfit { get; set; }

        public float WeeklyProfit { get; set; }

        public int FuturesPortfolioId { get; set; }

        public SpotPortfolioDto SpotPortfolio { get; set; }
    }
}
