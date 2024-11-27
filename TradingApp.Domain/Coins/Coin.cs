namespace TradingApp.Domain.Coins
{
    public class Coin
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string AllTimeHighPrice { get; set; } = string.Empty;

        public string AllTimeLowPrice { get; set; } = string.Empty;
    }
}
