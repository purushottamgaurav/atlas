namespace DotNetWebApiPractise.Custom
{
    public class LoggingMiddleware 
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<LoggingMiddleware> _logger;
        public  LoggingMiddleware(RequestDelegate requestDelegate, ILogger<LoggingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            _logger.LogInformation("INside the Custome MiddleWare");
           await _requestDelegate(httpContext);
        }
    }
}
