namespace TradingApp.Application.DataTransferObjects.Spot
{
    public class SpotTransactionToOpenDto
    {
        public int Id { get; set; }

        public float BuyingPrice { get; set; }

        public float MoneyInput { get; set; }

        public string CoinSymbol { get; set; } = string.Empty;

        public float SellingPrice { get; set; }

        public int SpotPortfolioId { get; set; }
    }
}
