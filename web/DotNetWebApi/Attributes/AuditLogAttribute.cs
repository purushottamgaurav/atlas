using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetWebApi.Attributes
{
    public class AuditLogAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path;

            Console.WriteLine($"[AUDIT] : {DateTime.Now}, {method}, {path}");
        }
    }
}
