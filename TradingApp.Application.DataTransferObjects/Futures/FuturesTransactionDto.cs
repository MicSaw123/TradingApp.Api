namespace TradingApp.Application.DataTransferObjects.Transaction
{
    public class FuturesTransactionDto
    {
        public int FuturesPortfolioId { get; set; }

        public int Leverage { get; set; }

        public float ClosingPrice { get; set; }
    }
}
