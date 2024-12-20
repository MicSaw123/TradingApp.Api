namespace TradingApp.Application.DataTransferObjects.Transaction
{
    public class SpotTransactionDto : TransactionDto
    {
        public float SellingPrice { get; set; }

        public int SpotPortfolioId { get; set; }
    }
}
