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
                ErrorMessage = LocalizedMessage.GetMessage("NotFound", resource, id),
                ErrorCode = ErrorCode.NotFound
            });

        protected ActionResult BadRequestResponse(string message, object? details = null) =>
            BadRequest(new ErrorResponse
            {
                ErrorMessage = LocalizedMessage.GetMessage("BadRequest"),
                ErrorCode = ErrorCode.InvalidRequest
            });

        protected string L(string key, params object[] args) =>
            LocalizedMessage.GetMessage(key, args);

    }
}
