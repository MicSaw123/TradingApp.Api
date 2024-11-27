namespace TradingApp.Application.DataTransferObjects.Portfolio
{
    public class BasePortfolioDto
    {
        public float Balance { get; set; }

        public float DailyProfit { get; set; }

        public float WeeklyProfit { get; set; }

        public float MonthlyProfit { get; set; }
    }
}
