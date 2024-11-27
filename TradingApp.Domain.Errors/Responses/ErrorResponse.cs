using System.Net;

namespace TradingApp.Domain.Errors.Responses
{
    public class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public static ErrorResponse CreateResponse(string message, HttpStatusCode statusCode)
        {
            return new ErrorResponse { StatusCode = statusCode, Message = message };
        }
    }
}
