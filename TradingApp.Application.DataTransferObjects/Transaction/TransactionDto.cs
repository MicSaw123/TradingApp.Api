namespace TradingApp.Application.DataTransferObjects.Transaction
{
    public class TransactionDto
    {
        public string CoinSymbol { get; set; } = string.Empty;

        public float BuyingPrice { get; set; }

        public float MoneyInput { get; set; }

        public float TransactionProfit { get; set; }

        public float AmountOfCoin { get; set; }

        public DateOnly ClosingTransactionDate { get; set; }

        public DateOnly OpenTransactionDate { get; set; }

        public DateOnly LastEditTransactionDate { get; set; }
    }
}
