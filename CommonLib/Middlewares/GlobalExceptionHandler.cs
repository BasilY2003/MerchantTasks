using CommonLib.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CommonLib.Middlewares
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly LoggingService _logger;

        public GlobalExceptionFilter(LoggingService logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;
            _logger.LogError(ex,
                "Exception caught in MVC filter for {Method} {Path} Query={QueryString}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                context.HttpContext.Request.QueryString);

            var InternalErrorMessage = LocalizedMessage.GetMessage("InternalServerError");
            var errorMessage = new ErrorResponse
            {
                StatusCode = ErrorCode.InternalServerError,
                ResponseMessage = InternalErrorMessage,
                Details = ex.Message,
            };
            context.Result = new ObjectResult(errorMessage)
            {
                StatusCode = (int) errorMessage.StatusCode
            };
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}