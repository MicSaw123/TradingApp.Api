namespace TradingApp.Domain.Base
{
    public class BasePortfolio
    {
        public int Id { get; set; }

        public float DisposableBalance { get; set; }

        public float AllocatedBalance { get; set; }

        public float DailyProfit { get; set; }

        public float WeeklyProfit { get; set; }

        public float MonthlyProfit { get; set; }
    }
}
