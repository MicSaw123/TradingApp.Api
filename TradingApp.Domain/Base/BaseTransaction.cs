namespace TradingApp.Domain.Base
{
    public class BaseTransaction
    {
        public int Id { get; set; }

        public float BuyingPrice { get; set; }

        public float AmountOfCoin { get; set; }

        public float MoneyInput { get; set; }

        public string CoinSymbol { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public float TransactionProfit { get; set; }

        public float TodaysProfit { get; set; }

        public DateOnly ClosingTransactionDate { get; set; }
    }
}
