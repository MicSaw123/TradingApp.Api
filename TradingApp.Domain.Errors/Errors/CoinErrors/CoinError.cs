using System.Net;

namespace TradingApp.Domain.Errors.Errors.CoinErrors
{
    public class CoinError
    {
        public static readonly Error ErrorFetchCoins = new FetchCoinException();

        public static readonly Error ErrorUpdateCoins = new UpdateCoinsError();

        public static readonly Error ErrorSeedCoins = new SeedCoinsError();

        public static readonly Error ErrorGetCoinsPerPage = new GetCoinsPerPageError();



        private sealed class FetchCoinException : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public FetchCoinException() : base(nameof(ErrorFetchCoins), 801)
            {
                Message = "There was an error while fetching coins!";
            }
        }

        private sealed class UpdateCoinsError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public UpdateCoinsError() : base(nameof(ErrorUpdateCoins), 802)
            {
                Message = "There was an error while updating coins!";
            }
        }

        private sealed class SeedCoinsError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public SeedCoinsError() : base(nameof(ErrorSeedCoins), 803)
            {
                Message = "There was an error while seeding the coins!";
            }
        }

        private sealed class GetCoinsPerPageError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public GetCoinsPerPageError() : base(nameof(ErrorGetCoinsPerPage), 804)
            {
                Message = "There was an error while fetching coins for page!";
            }
        }
    }
}
