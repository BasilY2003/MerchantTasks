using Microsoft.Extensions.Logging;

namespace CommonLib.Middlewares
{
    public class LoggingService 
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogRequest(string message, params object[] args)
        {
            using (Serilog.Context.LogContext.PushProperty("LogType", "Request"))
            {
                _logger.LogInformation(message, args);
            }
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            using (Serilog.Context.LogContext.PushProperty("LogType", "Error"))
            {
                _logger.LogError(ex, message, args);
            }
        }
    }
}
