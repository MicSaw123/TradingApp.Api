namespace TradingApp.Domain.Errors.Responses
{
    public class SuccessResponse
    {
        public bool IsSuccessful { get; set; }

        public DateTime CompletedAt { get; set; }

        public static SuccessResponse CreateResponse()
        {
            return new SuccessResponse
            {
                IsSuccessful = true,
                CompletedAt = DateTime.Now
            };
        }
    }
    public class SuccessResponse<T> : SuccessResponse
    {
        public T Result { get; set; }

        public static SuccessResponse<T> CreateResponse(T result)
        {
            return new SuccessResponse<T>
            {
                Result = result,
                CompletedAt = DateTime.Now,
                IsSuccessful = true
            };
        }
    }
}
