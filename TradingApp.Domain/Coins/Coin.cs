namespace TradingApp.Domain.Coins
{
    public class Coin
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public float AllTimeHighPrice { get; set; }

        public float AllTimeLowPrice { get; set; }
    }
}
