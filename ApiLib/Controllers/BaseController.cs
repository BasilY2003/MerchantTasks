namespace ApiLib.Controllers
{
    using CommonLib.Localization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult NotFoundResponse(string resource, object id) =>
            NotFound(new ErrorResponse
            {
                ResponseMessage = LocalizedMessage.GetMessage("NotFound", resource, id),
                StatusCode = ErrorCode.NotFound
            });

        protected ActionResult AlreadyUsedUsername(string message, object? details = null) =>
             Conflict(new ErrorResponse
             {
                 ResponseMessage = LocalizedMessage.GetMessage("TakenUserName"),
                 StatusCode = ErrorCode.Confict,
             });

        protected ActionResult UnauthorizedLoginResponse() =>
         Unauthorized(new ErrorResponse
         {
             ResponseMessage = LocalizedMessage.GetMessage("LoginCredentials"),
             StatusCode = ErrorCode.InvalidRequest
         });

        protected ActionResult SuccessResponse() =>
         Unauthorized(new ErrorResponse
         {
             ResponseMessage = LocalizedMessage.GetMessage("Success"),
             StatusCode = ErrorCode.InvalidRequest
         });

        protected string L(string key, params object[] args) =>
            LocalizedMessage.GetMessage(key, args);
    }
}
