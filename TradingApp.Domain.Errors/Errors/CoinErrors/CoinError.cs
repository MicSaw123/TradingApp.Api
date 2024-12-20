using System.Net;

namespace TradingApp.Domain.Errors.Errors.CoinErrors
{
    public class CoinError
    {
        public static readonly Error ErrorFetchCoins = new FetchCoinException();


        private sealed class FetchCoinException : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public FetchCoinException() : base(nameof(ErrorFetchCoins), 801)
            {
                Message = "There was an error while fetching coins!";
            }
        }
    }
}
