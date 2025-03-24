namespace TradingApp.Application.DataTransferObjects.Coin
{
    public class CoinDto
    {
        public string Symbol { get; set; } = string.Empty;

        public float Price { get; set; }

        public float AllTimeHighPrice { get; set; }

        public float AllTimeLowPrice { get; set; }
    }
}
