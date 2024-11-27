namespace TradingApp.Domain.Base
{
    public class BaseTransactionToOpen
    {
        public int Id { get; set; }

        public float BuyingPrice { get; set; }

        public float MoneyInput { get; set; }

        public string CoinSymbol { get; set; } = string.Empty;
    }
}
