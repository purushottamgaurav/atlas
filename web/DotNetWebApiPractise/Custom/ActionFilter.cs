using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetWebApiPractise.Custom
{
    public class ActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Before Action");
        }
    }
}
