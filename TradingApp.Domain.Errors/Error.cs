using System.Net;

namespace TradingApp.Domain.Errors
{
    public abstract class Error(string Name, int StatusCode, string? Description = null)
    {

        public static readonly Error ErrorUnknown = new UnknownError();

        public static readonly Error ErrorNone = new NoError();

        public abstract HttpStatusCode HttpStatusCode { get; }

        public string Message { get; protected set; }

        public static implicit operator RequestResult(Error error) => RequestResult.Failure(error);

        private sealed class NoError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.OK;
            public NoError() : base(nameof(NoError), 1000)
            {
                Message = string.Empty;
            }
        }

        private sealed class UnknownError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public UnknownError() : base(nameof(UnknownError), 1001)
            {
                Message = "An unknown error occurred";
            }
        }

    }
}
