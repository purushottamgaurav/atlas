namespace DotNetWebApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        public LoggingMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Incoming: {context.Request.Method} {context.Request.Path}");

            await requestDelegate(context); // pass to next middleware

            Console.WriteLine($"Outgoing: {context.Response.StatusCode}");
        }
    }
}
