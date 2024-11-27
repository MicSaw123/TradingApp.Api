namespace TradingApp.Domain.Base
{
    public class BaseTransaction
    {
        public int Id { get; set; }

        public float BuyingPrice { get; set; }

        public float AmountOfCoin { get; set; }

        public float MoneyInput { get; set; }

        public float SellingPrice { get; set; }

        public string CoinSymbol { get; set; } = string.Empty;

        public bool isActive { get; set; }

        public float TransactionProfit { get; set; }
    }
}
