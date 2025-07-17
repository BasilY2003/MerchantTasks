using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CommonLib.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LoggingService _logger;

        public LoggingMiddleware(RequestDelegate next, LoggingService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)

        {

            var sw = Stopwatch.StartNew();
            var request = context.Request;
            var requestPath = $"{request.Method} {request.Path}";

            _logger.LogRequest("--> Starting request: {RequestPath}", requestPath);

            await _next(context);

            sw.Stop();

            _logger.LogRequest("==> Completed request: {RequestPath} in {ElapsedMilliseconds}ms, Status: {StatusCode}",
                requestPath, sw.ElapsedMilliseconds, context.Response.StatusCode);
        }








    }
}

