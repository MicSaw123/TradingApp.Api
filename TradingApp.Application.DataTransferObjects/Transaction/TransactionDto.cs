namespace TradingApp.Application.DataTransferObjects.Transaction
{
    public class TransactionDto
    {
        public float BuyingPrice { get; set; }

        public int CoinId { get; set; }

        public float SellingPrice { get; set; }

        public bool isActive { get; set; }

        public float TransactionProfit { get; set; }
    }
}
