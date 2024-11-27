using Microsoft.AspNetCore.Mvc;
using System.Net;
using TradingApp.Domain.Errors;
using TradingApp.Domain.Errors.Responses;

namespace TradingApp.Api.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult CreateResponse(RequestResult requestResult)
        {
            if (requestResult is null)
            {
                return CreateCriticalFailure();
            }
            if (requestResult.IsFailure)
            {
                return CreateFailure(requestResult);
            }
            return StatusCode((int)requestResult.Error.HttpStatusCode, SuccessResponse.CreateResponse());
        }

        protected IActionResult CreateResponse<T>(RequestResult<T> requestResult)
        {
            if (requestResult is null)
            {
                return CreateCriticalFailure();
            }
            if (requestResult.IsFailure)
            {
                return CreateFailure(requestResult);
            }
            return StatusCode((int)requestResult.Error.HttpStatusCode, SuccessResponse<T>.CreateResponse(requestResult.Result));
        }

        private IActionResult CreateFailure(BaseResult requestResult)
        {
            return StatusCode((int)requestResult.Error.HttpStatusCode, ErrorResponse.CreateResponse(requestResult.Error.Message, requestResult.Error.HttpStatusCode));
        }

        private IActionResult CreateCriticalFailure()
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ErrorResponse.CreateResponse(Error.ErrorUnknown.Message, Error.ErrorUnknown.HttpStatusCode));
        }
    }
}
