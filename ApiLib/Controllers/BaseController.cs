using CommonLib.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult NotFoundResponse(string resource, object id) =>
            NotFound(new ResponseMessage
            {
                Message = LocalizedMessage.GetMessage("NotFound", resource, id),
                StatusCode = ResponseCode.NotFound
            });

        protected ActionResult AlreadyUsedUsername(string message, object? details = null) =>
             Conflict(new ResponseMessage
             {
                 Message = LocalizedMessage.GetMessage("TakenUserName"),
                 StatusCode = ResponseCode.Confict,
             });

        protected ActionResult UnauthorizedLoginResponse() =>
         Unauthorized(new ResponseMessage
         {
             Message = LocalizedMessage.GetMessage("LoginCredentials"),
             StatusCode = ResponseCode.InvalidRequest
         });

        protected ActionResult SuccessResponse() =>
         Unauthorized(new ResponseMessage
         {
             Message = LocalizedMessage.GetMessage("Success"),
             StatusCode = ResponseCode.InvalidRequest
         });

        protected string L(string key, params object[] args) =>
            LocalizedMessage.GetMessage(key, args);
    }
}
