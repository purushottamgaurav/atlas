# MVC Interview Q&A

---

1. **Life cycle of an MVC application?**
Request hits RouteTable → UrlRoutingModule matches the route → MvcRouteHandler creates MvcHandler → MvcHandler activates the Controller via IControllerFactory → Controller executes — runs Authorization filters, then Action filters (OnActionExecuting), then the Action method, then Action filters (OnActionExecuted) → Action returns an ActionResult → Result filters run → ActionResult executes (e.g., ViewResult calls the View Engine) → Result filters (OnResultExecuted) → Response is sent to the client.

2. **MVC routing — types and how to implement?**
Convention-based routing — defined in Program.cs using app.MapControllerRoute(). Pattern like {controller}/{action}/{id?} maps URLs to controllers automatically.
Attribute routing — define routes directly on controllers and actions using [Route], [HttpGet], [HttpPost] etc. More explicit and preferred for APIs.
```csharp
// Convention-based
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Attribute routing
[Route("api/products")]
[HttpGet("{id}")]
public IActionResult Get(int id) { }
```

3. **What is RPC and its uses?**
RPC (Remote Procedure Call) allows a program to execute a function on a remote server as if it were a local call. The caller doesn't need to know the network details. gRPC is the modern .NET implementation — uses HTTP/2 and Protocol Buffers for fast, strongly-typed communication. Used in microservices for internal service-to-service communication where performance matters more than REST's flexibility.

4. **Can we overload a controller action method?**
Not directly — HTTP routing doesn't distinguish overloads by parameter type. Solution: use the [ActionName] attribute to give the overloaded method a different route name, or use different HTTP verb attributes ([HttpGet], [HttpPost]) to differentiate methods with the same name.
```csharp
[ActionName("GetById")]
public IActionResult Get(int id) { }
public IActionResult Get(string name) { }
```

5. **Why is Web API required even if WCF supports RESTful services?**
WCF REST is cumbersome — requires heavy configuration (XML, bindings, endpoints). Web API is lightweight, convention-based, and built specifically for HTTP. Web API natively supports content negotiation, model binding, attribute routing, and JSON. It integrates seamlessly with ASP.NET Core middleware, DI, and filters. WCF is designed for SOAP and enterprise messaging; Web API is designed for the web.

6. **What are HTTP status codes?**
1xx — Informational (100 Continue). 2xx — Success (200 OK, 201 Created, 204 No Content). 3xx — Redirection (301 Moved Permanently, 302 Found). 4xx — Client Errors (400 Bad Request, 401 Unauthorized, 403 Forbidden, 404 Not Found, 409 Conflict, 422 Unprocessable Entity). 5xx — Server Errors (500 Internal Server Error, 502 Bad Gateway, 503 Service Unavailable).

7. **Can a Web API return a View?**
No. Web API controllers return data (JSON/XML), not HTML views. However, you can return HTML content using ContentResult with content type text/html. If you need to return a rendered view from an API, switch to a regular MVC controller.
```csharp
return Content("<h1>Hello</h1>", "text/html");
```

8. **Four types of action filters — Authorization, Action, Result, Exception?**
Authorization filter — runs first before anything else; checks if the user is allowed to execute the action. Implement IAuthorizationFilter.
Action filter — runs before and after the action method executes. Use for logging, model validation. Implement IActionFilter.
Result filter — runs before and after the ActionResult is executed. Use for modifying the response. Implement IResultFilter.
Exception filter — handles unhandled exceptions thrown by action methods or other filters. Implement IExceptionFilter.

9. **RenderPage vs RenderBody vs RenderSection?**
RenderBody() — used in a layout page as a placeholder where the content of the child view is injected. Only one RenderBody per layout.
RenderSection("name", required: false) — defines named sections in a layout. Child views fill them with @section name { }. Can have multiple sections; the boolean second parameter controls whether the section is required.
RenderPage("~/Views/Shared/_Widget.cshtml") — renders another page/partial at the specified path inline. Less common; use Partial or ViewComponent instead.

10. **What are Tag Helpers in MVC?**
Tag Helpers enable server-side C# code to render HTML elements in Razor views using HTML-like syntax. They replace the older Html.Helper() methods with cleaner markup. Examples: asp-for, asp-action, asp-controller, asp-validation-for. Custom tag helpers inherit from TagHelper and override ProcessAsync.
```html
<form asp-action="Login" asp-controller="Account" method="post">
    <input asp-for="Email" class="form-control" />
    <span asp-validation-for="Email"></span>
</form>
```

11. **How to implement data validation in MVC?**
Use Data Annotation attributes on model properties. Common attributes: [Required], [Range(1, 100)], [StringLength(50)], [DataType(DataType.Email)], [RegularExpression("pattern")], [Compare("OtherProperty")].
Server-side: check ModelState.IsValid in the action. Client-side: include jQuery Validation and Unobtrusive Validation scripts — validation runs in the browser automatically from the same attributes.
```csharp
public class User {
    [Required] [StringLength(50)] public string Name { get; set; }
    [Range(18, 99)] public int Age { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; }
}
```

12. **Different types of return types in MVC controller?**
ViewResult — returns a rendered Razor view. PartialViewResult — returns a partial view. RedirectResult — redirects to a URL. RedirectToActionResult — redirects to another action. JsonResult — returns JSON data. ContentResult — returns raw string/HTML. FileResult — returns a file download. StatusCodeResult — returns a specific HTTP status code. EmptyResult — returns nothing (void equivalent). IActionResult — base interface; use when return type can vary.

13. **ViewData vs ViewBag vs TempData vs ViewModel. Keep() vs Peek()?**
ViewData — dictionary (ViewData["key"]) — passes data from controller to view. Requires casting. Lives for current request only.
ViewBag — dynamic wrapper over ViewData (ViewBag.Key = value). No casting needed. Same lifetime.
TempData — survives across one redirect (stored in cookies or session). Deleted after read. Use Keep() to retain data for another request without reading it. Use Peek() to read data without marking it for deletion.
ViewModel — a strongly-typed class passed to the view via View(model). The recommended and type-safe approach for complex data.

14. **What are Areas and their benefits?**
Areas allow you to partition a large MVC application into smaller functional groups, each with its own controllers, views, and models. Benefits: better code organization, separation of concerns, multiple teams can work independently. Example: Admin area, Customer area, API area — each with their own folder structure.
```csharp
[Area("Admin")]
public class DashboardController : Controller { }
// Route: /Admin/Dashboard/Index
```

15. **Difference between Partial View and Razor View?**
Razor View (.cshtml) — a full view file rendered by a controller action. Has a layout, represents a complete page or response.
Partial View — renders a portion of HTML without a layout. Reused across multiple views. Called using @Html.Partial("_MyPartial") or <partial name="_MyPartial" /> tag helper. Used for reusable UI components like headers, widgets, or list items.

16. **What is Glimpse in MVC? How to check performance?**
Glimpse was a diagnostics and performance tool for ASP.NET MVC — showed request timelines, SQL queries, route data, and model binding info directly in the browser. It is now largely deprecated. Modern alternatives: Application Insights (Azure), MiniProfiler, built-in .NET diagnostics, and dotnet-trace / dotnet-counters CLI tools.

17. **Is it mandatory to use HTTP verbs for actions in Web API or MVC?**
No, it is not mandatory in either. MVC can use action name conventions without verb attributes. Web API will default GET/POST based on action name prefix (Get, Post, etc.) if using convention-based routing. However, using explicit verb attributes ([HttpGet], [HttpPost], etc.) is strongly preferred for readability, clarity, and to avoid routing ambiguity — especially in APIs.

18. **How to configure and call Web API from MVC?**
Register HttpClient in DI using builder.Services.AddHttpClient(). Inject IHttpClientFactory into the MVC controller. Use it to create a client and call the API.
```csharp
// Register
builder.Services.AddHttpClient("MyApi", c => c.BaseAddress = new Uri("https://api.example.com/"));

// Use
var client = _httpClientFactory.CreateClient("MyApi");
var result = await client.GetFromJsonAsync<List<Product>>("api/products");
```

19. **How does TempData store internally?**
By default TempData uses cookies to store data across redirects (CookieTempDataProvider). It serializes the data to JSON and stores it in a cookie. You can switch to session-based storage using services.AddControllersWithViews().AddSessionStateTempDataProvider() and enabling session middleware. Cookie-based is default in ASP.NET Core; session-based was the default in older ASP.NET MVC.

20. **How to access a cookie object in a View?**
Cookies are part of the HTTP context. In a Razor view access them via the Context object.
```csharp
// In View (.cshtml)
var value = Context.Request.Cookies["MyCookie"];

// Or inject IHttpContextAccessor if needed in a service
```
Avoid putting business logic in views. Prefer passing cookie-derived data through ViewData, ViewBag, or ViewModel from the controller.