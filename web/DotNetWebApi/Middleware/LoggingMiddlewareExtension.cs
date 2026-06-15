namespace DotNetWebApi.Middleware
{
    public static class LoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
            => app.UseMiddleware<LoggingMiddleware>();
    }
}
