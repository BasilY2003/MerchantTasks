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

            var problem = LocalizedErrorHelper.Create(ErrorCode.InternalServerError, "InternalServerError");

            context.Result = new ObjectResult(problem)
            {
                StatusCode = (int) problem.ErrorCode
            };
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }

}