namespace TradingApp.Application.DataTransferObjects.Transaction
{
    public class TransactionDto
    {
        public string CoinSymbol { get; set; } = string.Empty;

        public float BuyingPrice { get; set; }

        public float MoneyInput { get; set; }

        public bool isActive { get; set; }

        public float TransactionProfit { get; set; }
    }
}
