namespace ApiLib.Controllers
{
    using CommonLib.Localization;
    using DataLib.Resources;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public BaseController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        protected IActionResult LocalizedNotFound(string entity, object id)
        {
            var message = _localizer["NotFound", entity, id];
            Console.WriteLine($"Localized message: {message}");

            var response = new ErrorResponse
            {
                ErrorCode = ErrorCode.NotFound,
                ErrorMessage = message
            };
            return NotFound(response);
        }

        protected IActionResult LocalizedBadRequest(string entity)
        {
            var message = _localizer["BadRequest", entity];
            var response = new ErrorResponse
            {
                ErrorCode = ErrorCode.InvalidRequest,
                ErrorMessage = message
            };
            return BadRequest(response);
        }

    }






}
