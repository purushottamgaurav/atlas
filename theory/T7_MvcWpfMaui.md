# .NET MVC, WPF & MAUI Interview Q&A 

> Covers ASP.NET Core MVC, WPF, and .NET MAUI across fundamentals, patterns, and scenario-based questions.

---

## Section 1: ASP.NET Core MVC Fundamentals

---

**Q1. What is the lifecycle of an ASP.NET Core MVC request?**

When a request hits the server, it flows through these stages in order:

1. **Middleware Pipeline** — authentication, logging, CORS, session, etc.
2. **Routing** — the router matches the URL to a controller and action.
3. **Controller Instantiation** — DI creates the controller with its dependencies.
4. **Authorization Filters** — check if the user is allowed to access this action.
5. **Model Binding** — values from route, query string, form, and body are bound to action parameters.
6. **Model Validation** — `[Required]`, `[Range]`, etc. are evaluated.
7. **Action Filters (OnActionExecuting)** — run before the action method.
8. **Action Method Executes** — your controller code runs.
9. **Action Filters (OnActionExecuted)** — run after the action method.
10. **Result Execution** — the `IActionResult` (View, JSON, redirect) is executed.
11. **Result Filters** — run after the result is executed.
12. **Exception Filters** — catch unhandled exceptions at any stage above.
13. **Response sent** to client.

---

**Q2. What are the types of routing in MVC and how do you implement them?**

There are two types: **Conventional** and **Attribute** routing.

**Conventional routing** — defined in `Program.cs` as a pattern applied to all controllers.

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

URL `/Products/Details/5` maps to `ProductsController.Details(5)`.

**Attribute routing** — defined directly on the controller or action using attributes.

```csharp
[Route("api/products")]
public class ProductsController : Controller
{
    [HttpGet("{id}")]
    public IActionResult Details(int id) => View();
}
```

Use conventional routing for standard MVC pages. Use attribute routing for APIs and when you need full control over URLs. You can mix both in the same app.

---

**Q3. Can you overload a controller action method?**

Not directly with the same HTTP verb. MVC can't distinguish between two `Get()` methods with different parameters from a URL alone. You handle this by:

- Using different routes via attribute routing.
- Using different HTTP verbs (`[HttpGet]` and `[HttpPost]`).
- Using optional parameters in a single action.

```csharp
// This works - different HTTP verbs
[HttpGet]
public IActionResult Edit(int id) => View();

[HttpPost]
public IActionResult Edit(ProductViewModel model) => RedirectToAction("Index");
```

---

**Q4. What are ActionResult types in MVC?**

`IActionResult` is the base interface. Common implementations:

- `ViewResult` — renders a Razor view (`View()`).
- `PartialViewResult` — renders a partial view (`PartialView()`).
- `JsonResult` — returns JSON data (`Json()`).
- `RedirectResult` — redirects to a URL (`Redirect()`).
- `RedirectToActionResult` — redirects to an action (`RedirectToAction()`).
- `ContentResult` — returns a plain string (`Content()`).
- `FileResult` — returns a file download (`File()`).
- `NotFoundResult` — returns 404 (`NotFound()`).
- `BadRequestResult` — returns 400 (`BadRequest()`).
- `StatusCodeResult` — returns any HTTP status code (`StatusCode()`).
- `EmptyResult` — returns nothing.

---

**Q5. Can a Web API return a View?**

Technically yes — if the controller inherits from `Controller` (not `ControllerBase`) and you return `View()`. But this is bad practice. Web API controllers should return data (JSON/XML) not HTML. Views are for MVC controllers. Keep them separate for maintainability and testability.

---

**Q6. What are the four types of Action Filters and what do they do?**

Filters run at specific points in the pipeline around action execution.

- **Authorization Filter** — runs first, before everything else. Checks if the user can access the action. If denied, short-circuits the pipeline.
- **Action Filter** — runs before (`OnActionExecuting`) and after (`OnActionExecuted`) the action method. Good for logging, input validation, or modifying action arguments.
- **Result Filter** — runs before (`OnResultExecuting`) and after (`OnResultExecuted`) the result is written to the response. Good for modifying response headers.
- **Exception Filter** — catches unhandled exceptions thrown during action or result execution. Good for consistent error responses.

```csharp
public class LogActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
        => Console.WriteLine($"Executing: {context.ActionDescriptor.DisplayName}");

    public void OnActionExecuted(ActionExecutedContext context)
        => Console.WriteLine($"Executed: {context.ActionDescriptor.DisplayName}");
}
```

---

**Q7. What is the difference between RenderBody, RenderSection, and RenderPage?**

These are used in **Layout views** (`_Layout.cshtml`).

- **`@RenderBody()`** — required in a layout. This is where the content of child views (the actual page) is injected. Each layout has exactly one `RenderBody`.
- **`@RenderSection("Scripts", required: false)`** — renders a named section that child views can optionally fill. Common for scripts or sidebar content that only some pages need.
- **`@RenderPage("~/Views/Shared/_Banner.cshtml")`** — renders another Razor file inline. Less common; used to embed a fixed partial that isn't data-driven.

```html
<!-- _Layout.cshtml -->
<div class="container">
    @RenderBody()
</div>
<footer>
    @RenderSection("Footer", required: false)
</footer>
```

```html
<!-- Index.cshtml -->
@section Footer {
    <p>Custom footer for this page</p>
}
```

---

**Q8. What are Tag Helpers in MVC?**

Tag Helpers let you write server-side logic using HTML-like syntax in Razor views. They replace `Html.BeginForm()` and similar methods with cleaner markup.

```html
<!-- Old Html Helper way -->
@Html.ActionLink("Click here", "Index", "Home")

<!-- Tag Helper way - looks like HTML, easier to read -->
<a asp-controller="Home" asp-action="Index">Click here</a>

<!-- Form tag helper -->
<form asp-action="Login" asp-controller="Account" method="post">
    <input asp-for="Email" class="form-control" />
    <span asp-validation-for="Email" class="text-danger"></span>
    <button type="submit">Login</button>
</form>
```

Tag helpers are processed on the server and output standard HTML. They're IntelliSense-friendly and feel natural to front-end developers.

---

**Q9. What are @Model, @Html, and @Url in Razor Views?**

- **`@Model`** — the strongly-typed data object passed from the controller to the view. Defined at the top with `@model ProductViewModel`. Gives you compile-time checking and IntelliSense.
- **`@Html`** — an `IHtmlHelper` instance for generating HTML: forms, inputs, display text, validation messages, etc. (`Html.DisplayFor()`, `Html.EditorFor()`).
- **`@Url`** — an `IUrlHelper` instance for generating URLs. Better than hardcoding paths because it respects routing configuration.

```html
@model ProductViewModel

<h2>@Model.Name</h2>
<p>Price: @Html.DisplayFor(m => m.Price)</p>
<a href="@Url.Action("Edit", "Products", new { id = Model.Id })">Edit</a>
```

---

**Q10. How do you implement data validation in MVC?**

Use **Data Annotations** on your model and enable client-side validation with jQuery Validate.

```csharp
public class RegisterViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = string.Empty;
}
```

```csharp
// Controller
[HttpPost]
public IActionResult Register(RegisterViewModel model)
{
    if (!ModelState.IsValid)
        return View(model); // return form with validation errors

    // proceed
}
```

```html
<!-- View - shows validation error next to the field -->
<span asp-validation-for="Email" class="text-danger"></span>
```

For complex rules, use **FluentValidation** instead of data annotations.

---

**Q11. What is the difference between ViewData, ViewBag, TempData, and ViewModel?**

| Feature | ViewData | ViewBag | TempData | ViewModel |
|---|---|---|---|---|
| Type | `ViewDataDictionary` | Dynamic wrapper over ViewData | Cookie/Session based | Strongly typed class |
| Type safety | No | No | No | Yes |
| Lifetime | Current request | Current request | Survives one redirect | Current request |
| IntelliSense | No | No | No | Yes |

**ViewModel** is always the best option. The others are escape hatches for passing small pieces of data without creating a full ViewModel.

```csharp
// ViewData
ViewData["Title"] = "Home Page";

// ViewBag (same underlying store as ViewData)
ViewBag.Title = "Home Page";

// TempData (survives redirect)
TempData["SuccessMessage"] = "Saved!";
return RedirectToAction("Index");
```

---

**Q12. What are TempData.Keep() and TempData.Peek()?**

By default, TempData is deleted after it's read once. `Keep()` and `Peek()` change this behavior.

- **`TempData.Peek("key")`** — reads the value **without** marking it for deletion. The value is still available on the next request.
- **`TempData.Keep("key")`** — after you've already read it, call `Keep` to tell TempData to retain it for one more request instead of deleting it.

```csharp
// Read without deleting
var msg = TempData.Peek("SuccessMessage");

// Read (marks for deletion), then decide to keep it
var msg = TempData["SuccessMessage"];
TempData.Keep("SuccessMessage"); // retain it
```

---

**Q13. How does TempData store data internally?**

By default in ASP.NET Core, TempData uses the **cookie-based TempData provider**. The data is serialized and stored in a cookie. When the next request reads it, the cookie is cleared.

Alternatively, you can use the **Session-based provider**, which stores data server-side in the session store:

```csharp
builder.Services.AddSession();
builder.Services.AddMvc().AddSessionStateTempDataProvider();
```

Cookie-based is the default because it doesn't require a session store. Use session-based when TempData values are large (cookies have a ~4KB limit).

---

**Q14. What are Areas in MVC and why are they useful?**

Areas let you split a large MVC application into smaller functional sections, each with its own controllers, views, and models. This is useful for:

- Separating `Admin`, `Customer`, and `Public` sections of a site.
- Organizing large codebases by feature domain.
- Different routing per area.

```csharp
// Register area routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
```

```csharp
// Controller in Admin area
[Area("Admin")]
public class DashboardController : Controller
{
    public IActionResult Index() => View();
}
```

Views for the Admin area live under `Areas/Admin/Views/Dashboard/Index.cshtml`.

---

**Q15. What is the difference between a Partial View and a Layout View?**

- **Partial View** — a reusable chunk of HTML/Razor for a part of a page (e.g., a product card, a comment widget). It has no layout of its own. Rendered inside a parent view.
- **Layout View** — defines the outer shell of the page (header, footer, nav, `<html>`, `<body>`). Child views are injected into it via `@RenderBody()`.

```html
<!-- Rendering a partial view -->
@await Html.PartialAsync("_ProductCard", product)

<!-- Or with Tag Helper -->
<partial name="_ProductCard" model="product" />
```

A page uses a layout for structure and partials for reusable components within that structure.

---

**Q16. Is it mandatory to use HTTP verbs on actions in MVC or Web API?**

In **Web API**, yes — you should always decorate actions with `[HttpGet]`, `[HttpPost]`, etc. because the API controller doesn't use conventional routing by default.

In **MVC**, it's not strictly required — conventional routing uses the action name to map requests. However, it's a best practice to explicitly mark POST actions with `[HttpPost]` to prevent GET requests from triggering form submissions.

```csharp
// MVC - works without verb but best practice to add it
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(ProductViewModel model) { }
```

---

**Q17. What is an Anti-Forgery Token and how do you implement it in MVC?**

An anti-forgery token (CSRF token) prevents **Cross-Site Request Forgery** attacks where a malicious website tricks a logged-in user's browser into submitting a form to your site.

A hidden token is embedded in your form. When the form is submitted, the token is validated on the server. If it doesn't match, the request is rejected.

```html
<!-- In Razor view - form tag helper adds token automatically -->
<form asp-action="Create" method="post">
    <!-- token is auto-injected by the tag helper -->
    <input asp-for="Name" />
    <button type="submit">Save</button>
</form>
```

```csharp
// Controller action
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(ProductViewModel model) { }
```

If you use `<form asp-action="">` (tag helper), the token is added automatically. If you use plain HTML `<form>`, add `@Html.AntiForgeryToken()` inside the form manually.

---

**Q18. What are the types of Views in MVC?**

- **Regular View** — a full Razor page rendered by a controller action. Lives in `Views/ControllerName/ActionName.cshtml`.
- **Partial View** — a reusable fragment of HTML rendered inside another view. Name starts with `_` by convention (e.g., `_ProductCard.cshtml`).
- **Layout View** — defines the page shell (header, footer, scripts). Usually `_Layout.cshtml` in `Views/Shared`.
- **View Components** — like partial views but have their own logic/dependency injection (replacement for `Child Actions` from older MVC).
- **Razor Pages** — a page-based model where the view and handler are in the same `.cshtml` file (alternative to MVC).
- **Display/Editor Templates** — placed in `DisplayTemplates` or `EditorTemplates` folders, auto-applied for specific types.

---

**Q19. How do you call a Web API from an MVC controller?**

Use `HttpClient` injected via DI. Register it as a typed client.

```csharp
// Register in Program.cs
builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.myapp.com/");
});

// Typed client
public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _client;
    public ProductApiClient(HttpClient client) => _client = client;

    public async Task<List<Product>> GetProductsAsync()
    {
        var response = await _client.GetAsync("api/products");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Product>>() ?? [];
    }
}

// MVC Controller
public class ShopController : Controller
{
    private readonly IProductApiClient _api;
    public ShopController(IProductApiClient api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var products = await _api.GetProductsAsync();
        return View(products);
    }
}
```

---

**Q20. How do you access cookies in a Razor View?**

Cookies are part of the HTTP request context. Access them via `HttpContext` which is available in views through the injected `IHttpContextAccessor` or directly from the view context.

```html
<!-- Access cookie in Razor view -->
@{
    var userId = Context.Request.Cookies["UserId"];
}
<p>User: @userId</p>
```

For setting cookies, do it from the controller (not the view):

```csharp
// Set cookie in controller
Response.Cookies.Append("UserId", "12345", new CookieOptions
{
    HttpOnly = true,
    Secure = true,
    Expires = DateTimeOffset.UtcNow.AddDays(7)
});
```

---

**Q21. What is View Component in ASP.NET Core MVC?**

A View Component is like a mini-controller with a view. It has its own logic and DI but renders only a part of the page. It replaces the old `ChildActionResult` from ASP.NET MVC 5.

```csharp
public class CartSummaryViewComponent : ViewComponent
{
    private readonly ICartService _cart;
    public CartSummaryViewComponent(ICartService cart) => _cart = cart;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var count = await _cart.GetItemCountAsync(User.Identity!.Name!);
        return View(count);
    }
}
```

```html
<!-- Invoke in a view -->
@await Component.InvokeAsync("CartSummary")
<!-- or with tag helper -->
<vc:cart-summary />
```

View lives at `Views/Shared/Components/CartSummary/Default.cshtml`.

---

**Q22. What is bundling and minification in ASP.NET Core?**

Bundling combines multiple CSS/JS files into one file to reduce HTTP requests. Minification removes whitespace, comments, and shortens variable names to reduce file size.

In ASP.NET Core, use the **LibMan** tool or **WebOptimizer** NuGet package:

```csharp
// WebOptimizer
builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/bundle.css", "css/site.css", "css/custom.css");
    pipeline.AddJavaScriptBundle("/js/bundle.js", "js/site.js", "js/app.js");
});
```

In views, reference the bundle path. The runtime handles the rest.

---

**Q23. What is Glimpse and how do you check performance in MVC?**

Glimpse was a popular diagnostics tool for ASP.NET MVC that showed request timelines, executed SQL, routes matched, and model binding details in a browser toolbar. It is **no longer actively maintained** for ASP.NET Core.

Modern replacements:

- **Application Insights** — full production monitoring with request tracing and performance counters.
- **MiniProfiler** — lightweight profiler showing SQL queries and timings in the browser corner during development.
- **dotnet-trace / dotnet-counters** — CLI tools for profiling.
- **Visual Studio Diagnostic Tools** — built-in performance profiling.

```csharp
// MiniProfiler setup
builder.Services.AddMiniProfiler().AddEntityFramework();
app.UseMiniProfiler();
```

---

**Q24. What is the difference between `Html.Partial` and `Html.RenderPartial`?**

Both render a partial view. The difference is how they write output:

- **`Html.Partial()`** — returns an `IHtmlContent` string. You use it with `@` in the view. Slightly more memory usage because it builds the full string first.
- **`Html.RenderPartial()`** — writes directly to the response stream. Slightly faster. Must be used inside a code block `@{ Html.RenderPartial("_Name"); }`.

In practice, prefer `@await Html.PartialAsync("_Name")` in ASP.NET Core as it's async-safe.

---

**Q25. How does model binding work in MVC?**

Model binding automatically maps HTTP request data (route values, query strings, form fields, JSON body) to action method parameters and model properties. The binder checks sources in this order: route data → query string → form data → request body.

```csharp
// All three sources automatically bound
[HttpPost("{id}")]
public IActionResult Update(
    int id,                          // from route
    [FromQuery] string format,       // from query string
    [FromBody] ProductViewModel model) // from JSON body
{
}
```

You can customize binding with `[FromRoute]`, `[FromQuery]`, `[FromBody]`, `[FromHeader]`, `[FromForm]` attributes.

---

**Q26. What is the difference between `[Bind]` and `[BindNever]`?**

These control which properties get bound during model binding.

- **`[Bind("Name,Price")]`** — only bind these named properties. Everything else is ignored. Prevents over-posting attacks where a client sends fields they shouldn't be allowed to set (like `IsAdmin`).
- **`[BindNever]`** — marks a property to always be excluded from binding. Applied on the model property itself.

```csharp
// Only bind Name and Price from the request
[HttpPost]
public IActionResult Create([Bind("Name,Price")] Product product) { }

// Or on the model itself
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    [BindNever]
    public bool IsActive { get; set; } // never bound from request
}
```

---

**Q27. How do you implement custom validation in MVC?**

Create a class that inherits from `ValidationAttribute`:

```csharp
public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is DateTime date && date <= DateTime.Today)
            return new ValidationResult("Date must be in the future.");
        return ValidationResult.Success;
    }
}

// Use on model
public class EventViewModel
{
    [Required]
    public string Name { get; set; } = "";

    [FutureDate]
    public DateTime EventDate { get; set; }
}
```

---

**Q28. What is `IActionResult` vs `ActionResult<T>` in MVC?**

`IActionResult` is untyped — Swagger doesn't know what the response body looks like.

`ActionResult<T>` is typed — Swagger infers the response schema. It also allows implicit conversion, so you can return `T` directly and it wraps it in a 200 OK automatically.

```csharp
// Untyped
public IActionResult Get(int id) => Ok(product);

// Typed - better for documentation and code clarity
public ActionResult<ProductViewModel> Get(int id)
{
    var product = _service.Get(id);
    if (product == null) return NotFound();
    return product; // implicit 200 OK
}
```

---

**Q29. What is a ViewModel and why should you use it instead of domain models in views?**

A ViewModel is a class designed specifically for a view. It contains only the data the view needs — no more, no less.

Reasons to use ViewModels over domain models:
- Domain models may have fields you don't want to expose (passwords, internal flags).
- Views often need data combined from multiple domain entities.
- Validation rules for a view form may differ from DB model constraints.
- Prevents over-posting attacks.

```csharp
// Domain model (also maps to DB)
public class User { public int Id; public string Email; public string PasswordHash; }

// ViewModel (safe to expose to view)
public class UserProfileViewModel { public string Email; public string DisplayName; }
```

---

**Q30. How do you pass data from a controller to a view?**

Four ways, in order of preference:

1. **ViewModel** (best) — pass a strongly typed model to `View(model)`.
2. **ViewData** — `ViewData["Key"] = value` — accessed in view as `@ViewData["Key"]`.
3. **ViewBag** — `ViewBag.Key = value` — dynamic, accessed as `@ViewBag.Key`.
4. **TempData** — for data that survives a redirect.

```csharp
public IActionResult Index()
{
    var model = new HomeViewModel { Title = "Welcome", Products = _service.GetAll() };
    return View(model);
}
```

---

## Section 2: WPF (Windows Presentation Foundation)

---

**Q31. What is WPF and how does it differ from WinForms?**

WPF (Windows Presentation Foundation) is a UI framework for Windows desktop apps using XAML for UI definition and DirectX for rendering. It supports data binding, styles, templates, animations, and vector graphics natively.

| Feature | WPF | WinForms |
|---|---|---|
| UI definition | XAML (markup) | Code-behind or designer |
| Rendering | DirectX (GPU) | GDI+ (CPU) |
| Data binding | Rich, two-way | Limited |
| Styling | Full CSS-like styles | Basic |
| Resolution | DPI-aware, vector | Pixel-based |
| Pattern | MVVM natural | MVC/codebehind |

WPF is the choice for modern, visually rich Windows desktop apps. WinForms is simpler for basic tooling UIs.

---

**Q32. What is XAML?**

XAML (eXtensible Application Markup Language) is an XML-based language for defining UI in WPF, UWP, and MAUI. It declaratively describes the UI tree — what controls exist, how they look, and how they bind to data.

```xml
<Window x:Class="MyApp.MainWindow"
        Title="My App" Width="400" Height="300">
    <Grid>
        <TextBlock Text="{Binding WelcomeMessage}" FontSize="24" />
        <Button Content="Click Me" Command="{Binding ClickCommand}" />
    </Grid>
</Window>
```

The code-behind (`MainWindow.xaml.cs`) sets the `DataContext` and handles any non-MVVM logic.

---

**Q33. What is the MVVM pattern in WPF?**

MVVM (Model-View-ViewModel) separates concerns into three layers:

- **Model** — business data and logic (e.g., `Order` class, database calls).
- **View** — the XAML UI. It knows nothing about business logic. Binds to ViewModel.
- **ViewModel** — exposes data and commands for the View. Implements `INotifyPropertyChanged` to update the UI when data changes.

This separation makes the UI fully testable without running the actual window.

```csharp
public class MainViewModel : INotifyPropertyChanged
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public ICommand SaveCommand { get; } = new RelayCommand(Save);

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```

---

**Q34. What is `INotifyPropertyChanged` and why is it needed?**

`INotifyPropertyChanged` is an interface with one event: `PropertyChanged`. When a ViewModel property value changes, raising this event tells the WPF binding engine to update the UI automatically.

Without it, the View only reads properties once at binding time and never updates when the ViewModel data changes.

```csharp
public class ProductViewModel : INotifyPropertyChanged
{
    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            if (_price == value) return;
            _price = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
```

---

**Q35. What are Dependency Properties in WPF?**

Dependency Properties are a special property system built into WPF that supports data binding, animation, styling, and inheritance. They replace standard CLR properties when you need these features.

```csharp
public class MyControl : Control
{
    // Dependency Property declaration
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(MyControl),
            new PropertyMetadata("Default Title"));

    // CLR wrapper
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}
```

The WPF binding system works through Dependency Properties — regular CLR properties can't be bound, animated, or styled by WPF.

---

**Q36. What is data binding in WPF and what are its modes?**

Data binding connects a UI element property to a ViewModel property. The binding engine syncs them automatically.

Modes:
- `OneWay` — ViewModel → View only. View doesn't update the ViewModel.
- `TwoWay` — ViewModel ↔ View. Changes in either direction sync both. Used with text boxes, checkboxes.
- `OneTime` — binds once at startup. No live updates.
- `OneWayToSource` — View → ViewModel only.

```xml
<!-- TwoWay binding - TextBox updates ViewModel when user types -->
<TextBox Text="{Binding Name, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<!-- OneWay - just display -->
<TextBlock Text="{Binding TotalPrice, StringFormat=C}" />
```

---

**Q37. What are Commands in WPF and why use them instead of click events?**

Commands decouple UI actions from the code that handles them. Instead of wiring button clicks in code-behind, you bind to an `ICommand` property on the ViewModel. This keeps the View and ViewModel separated and makes the ViewModel testable.

```csharp
// RelayCommand (common implementation)
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? param) => _canExecute?.Invoke() ?? true;
    public void Execute(object? param) => _execute();
    public event EventHandler? CanExecuteChanged;
}

// In ViewModel
public ICommand SaveCommand { get; }

public MainViewModel()
{
    SaveCommand = new RelayCommand(Save, () => !string.IsNullOrEmpty(Name));
}
```

```xml
<Button Content="Save" Command="{Binding SaveCommand}" />
```

---

**Q38. What are Styles and Control Templates in WPF?**

**Styles** let you define a set of property values applied to controls of a specific type — similar to CSS classes.

```xml
<Style TargetType="Button">
    <Setter Property="Background" Value="#007bff" />
    <Setter Property="Foreground" Value="White" />
    <Setter Property="Padding" Value="10,5" />
</Style>
```

**Control Templates** let you completely replace the visual structure of a control while keeping its behavior. For example, making a Button look like a rounded pill.

```xml
<ControlTemplate TargetType="Button">
    <Border Background="{TemplateBinding Background}" CornerRadius="15">
        <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center" />
    </Border>
</ControlTemplate>
```

---

**Q39. What is the difference between StaticResource and DynamicResource?**

Both reference resources (styles, brushes, templates) defined in a `ResourceDictionary`.

- **`StaticResource`** — resolved once at load time. Faster. Use when the resource never changes.
- **`DynamicResource`** — resolved at runtime and updates the UI if the resource changes. Slower but supports theme switching.

```xml
<Button Background="{StaticResource PrimaryBrush}" />  <!-- fast, no runtime update -->
<Button Background="{DynamicResource ThemeAccentBrush}" /> <!-- updates if theme changes -->
```

---

**Q40. What are WPF layout panels?**

WPF provides several panels for arranging child controls:

- **`Grid`** — row/column based layout. Most versatile and commonly used.
- **`StackPanel`** — stacks children vertically or horizontally.
- **`WrapPanel`** — like StackPanel but wraps to next line when full.
- **`DockPanel`** — docks children to edges (Top, Bottom, Left, Right).
- **`Canvas`** — absolute positioning with X/Y coordinates.
- **`UniformGrid`** — grid where all cells are equal size.

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>
    <TextBlock Grid.Row="0" Text="Header" />
    <ListBox Grid.Row="1" ItemsSource="{Binding Items}" />
</Grid>
```

---

**Q41. What is an ObservableCollection in WPF?**

`ObservableCollection<T>` is a generic list that raises `CollectionChanged` events when items are added, removed, or moved. WPF's `ItemsControl` (ListBox, DataGrid, etc.) listens to these events and auto-updates the UI.

A plain `List<T>` doesn't notify the UI of changes. Always use `ObservableCollection` for lists bound to WPF controls.

```csharp
public class MainViewModel
{
    public ObservableCollection<Product> Products { get; } = new();

    public void AddProduct(Product p)
    {
        Products.Add(p); // UI updates automatically
    }
}
```

---

**Q42. What is the difference between UserControl and CustomControl in WPF?**

- **UserControl** — a composition of existing controls put together in XAML. Has a XAML file and code-behind. Easy to create. Best for app-specific composite UI (e.g., a user profile card).
- **CustomControl** — extends an existing control by overriding its `ControlTemplate`. Defined in code with a default style in `Themes/Generic.xaml`. Best for reusable, themeable, lookless controls (e.g., a star rating control for a library).

---

**Q43. How do you navigate between views/pages in WPF?**

WPF has built-in `NavigationWindow` and `Frame` for page navigation. For MVVM apps, most teams use a ViewModel-first navigation approach or a library like **Prism**.

```csharp
// Simple NavigationService approach
public interface INavigationService
{
    void NavigateTo<TViewModel>();
}

public class NavigationService : INavigationService
{
    private readonly MainViewModel _main;
    public void NavigateTo<TViewModel>()
    {
        _main.CurrentView = _container.Resolve<TViewModel>();
    }
}
```

```xml
<!-- Shell window shows current page -->
<ContentControl Content="{Binding CurrentView}" />
```

---

**Q44. What is value converter in WPF?**

A Value Converter transforms data during binding — from ViewModel type to View type (or back). Implement `IValueConverter`.

```csharp
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (bool)value ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => (Visibility)value == Visibility.Visible;
}
```

```xml
<Window.Resources>
    <local:BoolToVisibilityConverter x:Key="BoolToVis" />
</Window.Resources>

<ProgressBar Visibility="{Binding IsLoading, Converter={StaticResource BoolToVis}}" />
```

---

**Q45. What is routed events in WPF?**

Routed events travel through the visual tree — either up (bubbling) or down (tunneling) or staying at the element (direct).

- **Bubbling** — starts at the source element and travels up to the root. Example: `Button.Click` bubbles up so a parent `Grid` can also handle it.
- **Tunneling** — starts at the root and travels down to the source. Preview events are tunneled (`PreviewMouseDown`).
- **Direct** — only raised on the element itself.

```xml
<!-- Handling a bubbled click on the parent Grid -->
<Grid Button.Click="Grid_ButtonClick">
    <Button Content="Save" />
    <Button Content="Cancel" />
</Grid>
```

---

**Q46. How do you handle long-running tasks in WPF without freezing the UI?**

Use `async/await` to keep the UI thread free while waiting for I/O or CPU-bound work.

```csharp
// WPF UI runs on one thread - block it and the window freezes
private async void LoadButton_Click(object sender, RoutedEventArgs e)
{
    IsLoading = true; // updates a spinner via binding
    var data = await _service.GetDataAsync(); // doesn't block UI thread
    Items = new ObservableCollection<Item>(data);
    IsLoading = false;
}
```

For CPU-bound work:

```csharp
var result = await Task.Run(() => HeavyComputation(data));
```

If you need to update UI from a background thread, use `Dispatcher`:

```csharp
Application.Current.Dispatcher.Invoke(() => Items.Add(newItem));
```

---

**Q47. What is resource dictionary in WPF?**

A `ResourceDictionary` is a shared dictionary of reusable resources — styles, brushes, templates, strings. Defined in XAML files and merged into the app.

```xml
<!-- Colors.xaml -->
<ResourceDictionary>
    <SolidColorBrush x:Key="PrimaryBrush" Color="#007bff" />
    <SolidColorBrush x:Key="DangerBrush" Color="#dc3545" />
</ResourceDictionary>
```

```xml
<!-- App.xaml - merge all resource dictionaries -->
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Resources/Colors.xaml" />
            <ResourceDictionary Source="Resources/Styles.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

---

**Q48. What is DataTemplate in WPF?**

A `DataTemplate` defines how a data object is visually displayed in controls like `ListBox`, `ComboBox`, or `ContentControl`. It's the bridge between your data and how it looks.

```xml
<ListBox ItemsSource="{Binding Products}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="{Binding Name}" FontWeight="Bold" Width="150" />
                <TextBlock Text="{Binding Price, StringFormat=C}" Foreground="Green" />
            </StackPanel>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

---

**Q49. What is the difference between `x:Name` and `Name` in WPF XAML?**

Both give an element a name you can reference in code-behind. The difference is subtle:

- `x:Name` — XAML directive processed by the XAML compiler. Works on any XAML element.
- `Name` — the WPF `FrameworkElement.Name` dependency property. Only works on `FrameworkElement` subclasses.

In practice, `x:Name` is more universal. Both result in a field in the generated code-behind. Prefer `x:Name`.

---

**Q50. What are Triggers in WPF?**

Triggers let you change property values or play animations in response to conditions — without code-behind.

- **Property Trigger** — fires when a property has a specific value.
- **Data Trigger** — fires when a bound data value meets a condition.
- **Event Trigger** — fires when a routed event is raised (often used for animations).

```xml
<Style TargetType="Button">
    <Style.Triggers>
        <!-- Change color when mouse is over -->
        <Trigger Property="IsMouseOver" Value="True">
            <Setter Property="Background" Value="DodgerBlue" />
        </Trigger>
        <!-- Data trigger - dim button when IsEnabled binding is false -->
        <DataTrigger Binding="{Binding IsSaving}" Value="True">
            <Setter Property="Opacity" Value="0.5" />
        </DataTrigger>
    </Style.Triggers>
</Style>
```

---

## Section 3: .NET MAUI

---

**Q51. What is .NET MAUI?**

.NET MAUI (Multi-platform App UI) is Microsoft's framework for building native cross-platform apps for iOS, Android, macOS, and Windows from a single C# and XAML codebase. It is the evolution of Xamarin.Forms with a unified project structure and tighter .NET integration.

---

**Q52. How does .NET MAUI differ from Xamarin.Forms?**

| Feature | Xamarin.Forms | .NET MAUI |
|---|---|---|
| Project structure | Separate head projects per platform | Single project |
| .NET version | Older Xamarin/.NET | .NET 6+ unified |
| Performance | Slower startup | Faster, optimized renderers |
| Handlers | Renderers (slower) | Handlers (faster, leaner) |
| Desktop | Limited | Windows and macOS native |
| Styling | Basic | More flexible with CSS support |

MAUI is the strategic successor. Xamarin.Forms reached end of support in May 2024.

---

**Q53. What is the project structure of a .NET MAUI app?**

A MAUI app is a single project targeting multiple platforms:

```
MyApp/
├── Platforms/
│   ├── Android/       ← Android-specific code and manifest
│   ├── iOS/           ← iOS-specific code and Info.plist
│   ├── MacCatalyst/   ← macOS-specific code
│   └── Windows/       ← Windows-specific code
├── Resources/
│   ├── Fonts/
│   ├── Images/        ← single image, auto-resized per platform
│   └── Styles/
├── App.xaml           ← app-level resources
├── AppShell.xaml      ← navigation structure
├── MainPage.xaml      ← first page
└── MauiProgram.cs     ← startup and DI registration
```

---

**Q54. What is Shell in .NET MAUI?**

Shell is the high-level navigation and app structure container. It provides:

- A URI-based navigation system.
- Bottom tabs, flyout menu, and top tabs out of the box.
- Consistent navigation hierarchy across platforms.

```xml
<!-- AppShell.xaml -->
<Shell>
    <TabBar>
        <ShellContent Title="Home" Icon="home.png" ContentTemplate="{DataTemplate views:HomePage}" />
        <ShellContent Title="Profile" Icon="profile.png" ContentTemplate="{DataTemplate views:ProfilePage}" />
    </TabBar>
</Shell>
```

```csharp
// Navigate by URI
await Shell.Current.GoToAsync("//Profile");
await Shell.Current.GoToAsync($"ProductDetail?id={productId}");
```

---

**Q55. How do you register services and use dependency injection in .NET MAUI?**

In `MauiProgram.cs`, use the `MauiAppBuilder` to register services:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Register services
        builder.Services.AddSingleton<IProductService, ProductService>();
        builder.Services.AddTransient<ProductDetailViewModel>();
        builder.Services.AddTransient<ProductDetailPage>();

        return builder.Build();
    }
}
```

Pages and ViewModels are resolved automatically when navigated to via Shell.

---

**Q56. How does data binding work in .NET MAUI?**

Just like WPF, MAUI uses XAML data binding. Set `BindingContext` to the ViewModel and bind control properties to ViewModel properties.

```csharp
public class ProductViewModel : INotifyPropertyChanged
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}
```

```xml
<ContentPage BindingContext="{Binding Source={x:Static vm:ProductViewModel}}">
    <Label Text="{Binding Name}" />
    <Entry Text="{Binding Name, Mode=TwoWay}" />
</ContentPage>
```

---

**Q57. What is the MVVM Community Toolkit and how does it simplify MAUI development?**

The **CommunityToolkit.Mvvm** NuGet package reduces MVVM boilerplate significantly using source generators.

```csharp
// Without toolkit: 15+ lines per property
// With toolkit: 2 lines
public partial class ProductViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = "";  // auto-generates Name property with INotifyPropertyChanged

    [RelayCommand]
    private async Task SaveAsync()  // auto-generates SaveCommand ICommand
    {
        await _service.SaveAsync(Name);
    }
}
```

The `[ObservableProperty]` attribute generates the full property with change notification. `[RelayCommand]` generates the `ICommand` implementation automatically.

---

**Q58. What are the main layout controls in .NET MAUI?**

- **`VerticalStackLayout`** — stacks children top to bottom.
- **`HorizontalStackLayout`** — stacks children left to right.
- **`Grid`** — row/column layout (same as WPF Grid).
- **`FlexLayout`** — CSS Flexbox-inspired layout for responsive UIs.
- **`AbsoluteLayout`** — positions children at absolute or proportional coordinates.
- **`ScrollView`** — wraps content in a scrollable container.

```xml
<Grid RowDefinitions="Auto,*,60" ColumnDefinitions="*,*">
    <Label Grid.Row="0" Grid.ColumnSpan="2" Text="Header" />
    <CollectionView Grid.Row="1" Grid.ColumnSpan="2" ItemsSource="{Binding Items}" />
    <Button Grid.Row="2" Grid.Column="0" Text="Cancel" />
    <Button Grid.Row="2" Grid.Column="1" Text="Save" />
</Grid>
```

---

**Q59. What is CollectionView in .NET MAUI and how is it different from ListView?**

`CollectionView` is the modern, high-performance replacement for `ListView`. Key differences:

- Supports vertical and horizontal scrolling.
- No built-in cells — use `DataTemplate` freely.
- Supports multi-select, grouping, and empty state template.
- Better performance with virtualization.
- No `ViewCell` wrapper needed.

```xml
<CollectionView ItemsSource="{Binding Products}"
                SelectionMode="Single"
                SelectedItem="{Binding SelectedProduct}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Product">
            <Grid Padding="10">
                <Label Text="{Binding Name}" FontSize="16" />
                <Label Grid.Row="1" Text="{Binding Price, StringFormat='{0:C}'}" />
            </Grid>
        </DataTemplate>
    </CollectionView.ItemTemplate>
    <CollectionView.EmptyView>
        <Label Text="No products found." HorizontalOptions="Center" />
    </CollectionView.EmptyView>
</CollectionView>
```

---

**Q60. How do you handle platform-specific code in .NET MAUI?**

Three approaches:

1. **Partial classes** — create `MyService.cs` and platform-specific `MyService.Android.cs`, `MyService.iOS.cs`. MAUI compiles the correct one per platform.

2. **`#if` preprocessor** — for small platform differences in a single file.

```csharp
public string GetDeviceInfo()
{
#if ANDROID
    return "Running on Android";
#elif IOS
    return "Running on iOS";
#else
    return "Running on Windows/Mac";
#endif
}
```

3. **Platform folders** — put code in `Platforms/Android/`, `Platforms/iOS/` etc. Only compiled for that platform.

---

**Q61. How do you make HTTP calls in .NET MAUI?**

Use `HttpClient` registered as a singleton (creating many HttpClients causes socket exhaustion).

```csharp
// Register in MauiProgram.cs
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<IProductService, ProductService>();

// Service
public class ProductService : IProductService
{
    private readonly HttpClient _client;
    public ProductService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://api.myapp.com/");
    }

    public async Task<List<Product>> GetProductsAsync()
        => await _client.GetFromJsonAsync<List<Product>>("products") ?? [];
}
```

On Android, also add network permission in `AndroidManifest.xml`.

---

**Q62. How do you persist data locally in .NET MAUI?**

Three common options:

- **`Preferences`** — key-value store for simple settings (wraps SharedPreferences on Android, NSUserDefaults on iOS).
- **SQLite** — local relational database using `sqlite-net-pcl`.
- **`SecureStorage`** — encrypted key-value for sensitive data like tokens.

```csharp
// Preferences
Preferences.Set("UserId", "12345");
var userId = Preferences.Get("UserId", defaultValue: "");

// SecureStorage
await SecureStorage.SetAsync("AuthToken", token);
var token = await SecureStorage.GetAsync("AuthToken");

// SQLite
var db = new SQLiteAsyncConnection(dbPath);
await db.CreateTableAsync<Product>();
await db.InsertAsync(new Product { Name = "Widget" });
var products = await db.Table<Product>().ToListAsync();
```

---

**Q63. What is Shell navigation with query parameters in MAUI?**

You can pass data between pages via URI query parameters.

```csharp
// Navigate with parameter
await Shell.Current.GoToAsync($"ProductDetail?productId={product.Id}");
```

```csharp
// Receive on destination page ViewModel
[QueryProperty(nameof(ProductId), "productId")]
public partial class ProductDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private int _productId;

    partial void OnProductIdChanged(int value)
    {
        // load product when Id is set
        LoadProductAsync(value);
    }
}
```

---

**Q64. What are MAUI Handlers and how do they differ from Xamarin Renderers?**

In Xamarin.Forms, each control had a **Renderer** — a class that wrapped and translated to a native control. Renderers were heavy, used reflection, and were tightly coupled.

In MAUI, **Handlers** replace renderers. They use a property mapper — a dictionary of delegates — to map MAUI control properties to native control properties. Handlers are lighter, faster, and easier to customize.

```csharp
// Customize all Entries globally (remove underline on Android)
Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
{
#if ANDROID
    handler.PlatformView.BackgroundTintList =
        Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
});
```

---

**Q65. How do you implement push notifications in .NET MAUI?**

Use **Azure Notification Hubs** or **Firebase Cloud Messaging (FCM)** for Android and **APNs** for iOS.

The typical flow:
1. App registers with the push provider (FCM/APNs) and gets a device token.
2. App sends the token to your backend API.
3. Backend stores the token.
4. When you want to push, backend sends a message to Notification Hub, which fans out to the registered devices.

The `Plugin.Firebase.CloudMessaging` or `CommunityToolkit.Maui` libraries simplify the setup.

---

**Q66. How do you handle permissions in .NET MAUI?**

Use the `Permissions` class to check and request permissions at runtime.

```csharp
public async Task<bool> RequestCameraAsync()
{
    var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

    if (status != PermissionStatus.Granted)
        status = await Permissions.RequestAsync<Permissions.Camera>();

    return status == PermissionStatus.Granted;
}
```

You still need to declare permissions in the platform manifests (`AndroidManifest.xml` for Android, `Info.plist` for iOS).

---

**Q67. What is the AppShell flyout in .NET MAUI?**

The flyout is a side drawer navigation menu — like a hamburger menu. Define it in `AppShell.xaml`.

```xml
<Shell FlyoutBehavior="Flyout">
    <FlyoutItem Title="Home" Icon="home.png">
        <ShellContent ContentTemplate="{DataTemplate views:HomePage}" />
    </FlyoutItem>
    <FlyoutItem Title="Settings" Icon="settings.png">
        <ShellContent ContentTemplate="{DataTemplate views:SettingsPage}" />
    </FlyoutItem>
    <MenuItem Text="Logout" Command="{Binding LogoutCommand}" />
</Shell>
```

---

**Q68. How do you implement offline support in a .NET MAUI app?**

- Use **SQLite** to cache data locally when online.
- Check connectivity with `Connectivity.NetworkAccess` before making API calls.
- If offline, read from local SQLite DB.
- Queue write operations locally and sync when connectivity is restored.

```csharp
public async Task<List<Product>> GetProductsAsync()
{
    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
    {
        var products = await _apiService.GetProductsAsync();
        await _localDb.SaveProductsAsync(products); // cache
        return products;
    }
    return await _localDb.GetProductsAsync(); // offline fallback
}
```

---

**Q69. What is the difference between `ContentPage`, `NavigationPage`, and `TabbedPage` in MAUI?**

- **`ContentPage`** — the basic single-page container. Has a title, toolbar items, and one content area.
- **`NavigationPage`** — wraps a ContentPage to provide a back-stack navigation with a navigation bar. Push/pop pages onto the stack.
- **`TabbedPage`** — a page with tab bar for switching between child pages. In modern MAUI, you should prefer Shell tabs over `TabbedPage`.

In Shell apps, you don't use NavigationPage or TabbedPage directly — Shell handles navigation and tabs for you.

---

**Q70. How do you implement MVVM navigation in MAUI without tightly coupling View and ViewModel?**

Register pages and ViewModels in DI, and use Shell's URI navigation. The ViewModel doesn't reference the View at all.

```csharp
// MauiProgram.cs
builder.Services.AddTransient<ProductDetailPage>();
builder.Services.AddTransient<ProductDetailViewModel>();

// AppShell.xaml.cs - register route
Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));

// From any ViewModel
await Shell.Current.GoToAsync(nameof(ProductDetailPage), new Dictionary<string, object>
{
    { "Product", selectedProduct }
});
```

The ViewModel receives the parameter via `[QueryProperty]`. No direct ViewModel-to-ViewModel reference needed.

---

## Section 4: Advanced & Cross-Cutting

---

**Q71. Compare MVC, MVVM, and MVP patterns.**

| Pattern | View knows ViewModel? | Testability | Used in |
|---|---|---|---|
| MVC | Yes (via controller) | Good | ASP.NET Core MVC |
| MVVM | No (bindings) | Excellent | WPF, MAUI, Angular |
| MVP | Via interface | Excellent | WinForms, Android |

- **MVC** — Controller handles input, updates Model, selects View.
- **MVVM** — ViewModel exposes data; View binds to it. No direct coupling.
- **MVP** — Presenter updates View via an interface. View is very thin.

---

**Q72. What is Blazor and how does it compare to MVC?**

Blazor is a framework for building interactive web UIs with C# instead of JavaScript.

- **Blazor Server** — C# runs on the server, UI updates via SignalR. Small download, needs persistent connection.
- **Blazor WebAssembly** — C# runs in the browser via WebAssembly. No server needed after download. Larger initial load.

Compared to MVC: MVC returns full HTML pages from the server. Blazor renders components and handles interactivity in C# — more like React/Angular but in .NET.

---

**Q73. What is the difference between `async void` and `async Task` and why does it matter in WPF/MAUI?**

`async Task` — the caller can await it, catch exceptions, and track completion. This is what you should always use.

`async void` — fire and forget. Exceptions are unhandled and crash the app. The caller can't await it.

The **only** acceptable use of `async void` is event handlers (WPF click handlers, MAUI `Clicked` events) because the event signature requires `void`.

```csharp
// BAD - exception will crash app silently
private async void LoadData() { await _service.GetAsync(); }

// GOOD
private async Task LoadDataAsync() { await _service.GetAsync(); }

// OK - event handler, must be void
private async void Button_Clicked(object sender, EventArgs e)
{
    await LoadDataAsync(); // wrap the async work in an awaitable Task method
}
```

---

**Q74. What is the Mediator pattern and how is it used with MediatR in .NET?**

The Mediator pattern decouples senders and receivers — instead of object A calling object B directly, both communicate through a mediator. MediatR is the popular .NET implementation.

Requests (commands/queries) are sent to the mediator, which finds the right handler.

```csharp
// Query
public record GetProductQuery(int Id) : IRequest<ProductDto>;

// Handler
public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken ct)
        => await _db.Products.FindAsync(request.Id);
}

// Controller
var product = await _mediator.Send(new GetProductQuery(id));
```

---

**Q75. What is `IDisposable` and when should you implement it?**

`IDisposable` signals that a class holds unmanaged resources (file handles, DB connections, HTTP clients, etc.) that need to be released deterministically. Implement it when your class:

- Wraps a stream, database connection, or socket.
- Subscribes to events (to unsubscribe on dispose).
- Holds references to other `IDisposable` objects.

```csharp
public class ReportService : IDisposable
{
    private readonly SqlConnection _conn;
    private bool _disposed;

    public ReportService(string connectionString)
        => _conn = new SqlConnection(connectionString);

    public void Dispose()
    {
        if (!_disposed)
        {
            _conn.Dispose();
            _disposed = true;
        }
    }
}
```

Always use `using` when working with `IDisposable` objects.

---

**Q76. How do you implement localization in ASP.NET Core MVC?**

Use `.resx` resource files per culture and the built-in localization middleware.

```csharp
// Program.cs
builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("fr-FR") }
});
```

```csharp
// In controller or service
public class HomeController : Controller
{
    private readonly IStringLocalizer<HomeController> _localizer;
    public HomeController(IStringLocalizer<HomeController> localizer) => _localizer = localizer;

    public IActionResult Index() => View(model: _localizer["WelcomeMessage"].Value);
}
```

---

**Q77. How do you implement caching in an MVC controller?**

Use the `[ResponseCache]` attribute for output caching or inject `IMemoryCache` for programmatic caching.

```csharp
// Response caching - browser and proxies cache the response
[ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "page" })]
public IActionResult Index(int page = 1) => View(GetProducts(page));

// In-memory caching for service data
public class ProductService
{
    private readonly IMemoryCache _cache;

    public async Task<List<Product>> GetAllAsync()
    {
        return await _cache.GetOrCreateAsync("all_products", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await _db.Products.ToListAsync();
        }) ?? [];
    }
}
```

---

**Q78. What is SignalR and how do you use it in an MVC app?**

SignalR is a library for real-time communication between server and clients over WebSockets (with fallback to long polling). Clients subscribe to a **Hub** and the server can push messages to connected clients.

```csharp
// Hub
public class NotificationHub : Hub
{
    public async Task SendMessage(string user, string message)
        => await Clients.All.SendAsync("ReceiveMessage", user, message);
}

// Register in Program.cs
builder.Services.AddSignalR();
app.MapHub<NotificationHub>("/notificationHub");
```

```javascript
// Client (JavaScript in MVC view)
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub").build();

connection.on("ReceiveMessage", (user, message) => {
    console.log(`${user}: ${message}`);
});
await connection.start();
```

---

**Q79. How do you implement global exception handling in ASP.NET Core MVC?**

Use `UseExceptionHandler` middleware for a central error page:

```csharp
// Program.cs
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/Home/Error");
```

```csharp
// Error controller
[AllowAnonymous]
public class HomeController : Controller
{
    [Route("Home/Error")]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        // log feature.Error
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id });
    }
}
```

For API-style error responses, use `UseExceptionHandler` with a JSON response (see the Web API Q&A).

---

**Q80. What is the role of `_ViewStart.cshtml` and `_ViewImports.cshtml`?**

- **`_ViewStart.cshtml`** — runs before every view and sets common configurations. Most commonly used to specify the default layout:

```csharp
// _ViewStart.cshtml
@{ Layout = "_Layout"; }
```

- **`_ViewImports.cshtml`** — defines common `@using` statements, tag helper registrations, and `@inject` directives shared across all views in the same folder and subfolders. Reduces boilerplate.

```csharp
// _ViewImports.cshtml
@using MyApp.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@inject ICurrentUser CurrentUser
```

---

**Q81. How do you use Razor Pages vs MVC? When would you choose one over the other?**

**Razor Pages** co-locate the view and its handler in a single `.cshtml` file with a `PageModel` class. Each page is self-contained — no need for a separate controller.

**MVC** separates concerns into Controller, View, and Model. Better for complex routing, APIs, and situations where one controller handles many actions.

Choose Razor Pages for page-centric scenarios (CRUD pages, forms). Choose MVC when you have complex routing, RESTful endpoints, or need the full controller lifecycle.

```csharp
// Razor Page handler (Products/Index.cshtml.cs)
public class IndexModel : PageModel
{
    public List<Product> Products { get; set; } = [];

    public async Task OnGetAsync()
        => Products = await _service.GetAllAsync();

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToPage();
    }
}
```

---

**Q82. What is the `[FromServices]` attribute in MVC?**

`[FromServices]` injects a service directly into an action method parameter without constructor injection. Useful when you need a service in just one action, not the whole controller.

```csharp
[HttpGet]
public async Task<IActionResult> Report(
    [FromServices] IReportService reportService)
{
    var data = await reportService.GenerateAsync();
    return View(data);
}
```

---

**Q83. What is Endpoint Routing in ASP.NET Core?**

Endpoint Routing is the modern routing system in ASP.NET Core 3+. Routes are matched once by the routing middleware and the endpoint is stored in `HttpContext`. Authorization, CORS, and other middleware can then inspect the endpoint before it runs.

This is more efficient than older routing because routing happens once rather than per-middleware.

```csharp
app.UseRouting();         // match routes, store endpoint in context
app.UseAuthentication();  // can check endpoint's [Authorize] requirements
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
// Or the shorter form in .NET 6+:
app.MapControllers();
```

---

**Q84. How would you test a WPF ViewModel?**

Since ViewModels contain no UI code, they're plain C# classes — test them like any other class with xUnit and Moq.

```csharp
public class ProductViewModelTests
{
    [Fact]
    public async Task LoadProducts_SetsProductsCollection()
    {
        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.GetAllAsync())
                   .ReturnsAsync(new List<Product> { new() { Name = "Widget" } });

        var vm = new ProductListViewModel(mockService.Object);
        await vm.LoadProductsCommand.ExecuteAsync(null);

        Assert.Single(vm.Products);
        Assert.Equal("Widget", vm.Products[0].Name);
    }
}
```

Test: property changes fire `PropertyChanged`, commands can execute/cannot execute correctly, and business logic produces right outputs.

---

**Q85. What is Prism Framework for WPF/MAUI?**

Prism is a popular framework for building loosely coupled, maintainable MVVM apps. It provides:

- **Module system** — break the app into independently loaded modules.
- **Region navigation** — swap ViewModels/Views into named regions without coupling.
- **Event Aggregator** — publish/subscribe events between ViewModels without direct references.
- **Dialog Service** — show dialogs from ViewModels.

```csharp
// Event Aggregator - publish from one ViewModel
_eventAggregator.GetEvent<ProductSavedEvent>().Publish(savedProduct);

// Subscribe in another ViewModel
_eventAggregator.GetEvent<ProductSavedEvent>().Subscribe(OnProductSaved);
```

---

**Q86. How do you implement a master-detail layout in WPF?**

```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="250" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>

    <!-- Master list -->
    <ListBox Grid.Column="0"
             ItemsSource="{Binding Products}"
             SelectedItem="{Binding SelectedProduct}" />

    <!-- Detail panel - switches content based on selection -->
    <ContentControl Grid.Column="1"
                    Content="{Binding SelectedProduct}">
        <ContentControl.ContentTemplate>
            <DataTemplate>
                <StackPanel Margin="20">
                    <TextBlock Text="{Binding Name}" FontSize="24" />
                    <TextBlock Text="{Binding Description}" />
                </StackPanel>
            </DataTemplate>
        </ContentControl.ContentTemplate>
    </ContentControl>
</Grid>
```

---

**Q87. How do you implement theming (dark/light mode) in WPF?**

Use a `ResourceDictionary` per theme and swap it at runtime.

```csharp
public void SetTheme(string themeName)
{
    var dict = new ResourceDictionary
    {
        Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative)
    };

    var existing = Application.Current.Resources.MergedDictionaries
        .FirstOrDefault(d => d.Source?.OriginalString.Contains("Themes/") == true);

    if (existing != null)
        Application.Current.Resources.MergedDictionaries.Remove(existing);

    Application.Current.Resources.MergedDictionaries.Add(dict);
}
```

Controls using `DynamicResource` for colors and brushes will update automatically.

---

**Q88. How do you implement theming in .NET MAUI?**

MAUI supports AppThemeBinding to automatically respond to device dark/light mode.

```xml
<Label Text="Hello"
       TextColor="{AppThemeBinding Light=Black, Dark=White}" />

<ContentPage Background="{AppThemeBinding Light=#ffffff, Dark=#1a1a1a}">
```

For manual theme switching:

```csharp
Application.Current!.UserAppTheme = AppTheme.Dark;
```

Centralize theme colors in `App.xaml` resources using `AppThemeBinding` on `Color` resources.

---

**Q89. How do you implement logging in an ASP.NET Core MVC app?**

ASP.NET Core has a built-in `ILogger<T>` system. Inject it and call it.

```csharp
public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    public ProductsController(ILogger<ProductsController> logger) => _logger = logger;

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Loading products at {Time}", DateTime.UtcNow);
        try
        {
            var products = await _service.GetAllAsync();
            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load products");
            return StatusCode(500);
        }
    }
}
```

For production, use **Serilog** as the logging provider with sinks for Application Insights, file, or ELK.

---

**Q90. What is output caching in ASP.NET Core 7+?**

Output caching stores the complete response (including headers) and serves it for subsequent matching requests without running the controller again. It's more powerful than `ResponseCache` because it works server-side.

```csharp
// Register
builder.Services.AddOutputCache();
app.UseOutputCache();

// Apply to action
[OutputCache(Duration = 60)]
public IActionResult Index() => View(GetExpensiveData());

// Or with policies
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("ProductsPage", builder =>
        builder.Expire(TimeSpan.FromMinutes(5))
               .VaryByQuery("page", "category"));
});
```

---

**Q91. What are minimal APIs in .NET 6+ and how do they compare to MVC controllers?**

Minimal APIs define routes and handlers directly in `Program.cs` with minimal ceremony. Great for microservices and small APIs.

```csharp
var app = builder.Build();

app.MapGet("/products", async (IProductService svc) =>
    await svc.GetAllAsync());

app.MapGet("/products/{id}", async (int id, IProductService svc) =>
{
    var p = await svc.GetByIdAsync(id);
    return p is null ? Results.NotFound() : Results.Ok(p);
});

app.MapPost("/products", async (ProductDto dto, IProductService svc) =>
{
    var created = await svc.CreateAsync(dto);
    return Results.Created($"/products/{created.Id}", created);
});
```

MVC controllers are better for large apps with many actions, filters, and conventions. Minimal APIs are better for focused microservices.

---

**Q92. What is the difference between `Redirect` and `RedirectToAction` in MVC?**

- **`Redirect(url)`** — redirects to an absolute or relative URL string. No type safety, no routing awareness.
- **`RedirectToAction("ActionName", "ControllerName", routeValues)`** — generates the URL from routing tables. Safer — if you rename a controller, it still works because it uses routing, not hardcoded strings.
- **`RedirectToRoute`** — similar but uses a named route.

```csharp
// Fragile - hardcoded URL
return Redirect("/Products/Index");

// Better - uses routing
return RedirectToAction("Index", "Products");

// After POST - always redirect to prevent form resubmission on F5
[HttpPost]
public IActionResult Create(ProductViewModel model)
{
    _service.Save(model);
    return RedirectToAction("Index");
}
```

---

**Q93. What is the PRG pattern (Post-Redirect-Get) in MVC?**

After a successful form POST, always redirect to a GET instead of returning a view directly. This prevents the browser from resubmitting the form when the user refreshes the page.

Without PRG: user submits an order → refresh → order submitted again.
With PRG: user submits an order → redirect to confirmation page → refresh just reloads the confirmation.

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult PlaceOrder(OrderViewModel model)
{
    if (!ModelState.IsValid) return View(model);
    _service.PlaceOrder(model);
    TempData["Success"] = "Order placed!";
    return RedirectToAction("Confirmation"); // ← GET
}

[HttpGet]
public IActionResult Confirmation() => View();
```

---

**Q94. How do you implement pagination in an MVC view?**

Pass page metadata in the ViewModel and render prev/next links.

```csharp
public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int Page { get; set; }
    public int TotalPages { get; set; }
}
```

```html
<!-- _Pagination.cshtml partial -->
@model PagedResult<dynamic>
<nav>
    @if (Model.Page > 1)
    {
        <a asp-action="Index" asp-route-page="@(Model.Page - 1)">← Prev</a>
    }
    <span>Page @Model.Page of @Model.TotalPages</span>
    @if (Model.Page < Model.TotalPages)
    {
        <a asp-action="Index" asp-route-page="@(Model.Page + 1)">Next →</a>
    }
</nav>
```

---

**Q95. What is a Display Template and Editor Template in MVC?**

Display and Editor Templates are partial views auto-applied to specific types when using `Html.DisplayFor()` and `Html.EditorFor()`.

Place them in `Views/Shared/DisplayTemplates/` and `Views/Shared/EditorTemplates/`. Name them after the type or `UIHint`.

```html
<!-- Views/Shared/DisplayTemplates/Currency.cshtml -->
@model decimal
<span class="currency">@Model.ToString("C")</span>
```

```csharp
// Model property
[UIHint("Currency")]
public decimal Price { get; set; }
```

```html
<!-- View - automatically uses the Currency template -->
@Html.DisplayFor(m => m.Price)
```

---

**Q96. How do you handle file uploads in ASP.NET Core MVC?**

```csharp
// ViewModel
public class UploadViewModel
{
    [Required]
    public IFormFile File { get; set; } = null!;
}

// Controller
[HttpPost]
public async Task<IActionResult> Upload(UploadViewModel model)
{
    if (!ModelState.IsValid) return View(model);

    var uploads = Path.Combine(_env.WebRootPath, "uploads");
    Directory.CreateDirectory(uploads);

    var filePath = Path.Combine(uploads, Path.GetRandomFileName()
        + Path.GetExtension(model.File.FileName));

    using var stream = new FileStream(filePath, FileMode.Create);
    await model.File.CopyToAsync(stream);

    return RedirectToAction("Index");
}
```

```html
<!-- View - must use enctype multipart -->
<form asp-action="Upload" method="post" enctype="multipart/form-data">
    <input asp-for="File" type="file" />
    <button type="submit">Upload</button>
</form>
```

---

**Q97. What are the animation capabilities in WPF?**

WPF has a rich animation system built in. You can animate any dependency property over time.

```xml
<Button Content="Click Me">
    <Button.Triggers>
        <EventTrigger RoutedEvent="Button.Click">
            <BeginStoryboard>
                <Storyboard>
                    <!-- Fade out over 1 second -->
                    <DoubleAnimation
                        Storyboard.TargetProperty="Opacity"
                        From="1.0" To="0.0" Duration="0:0:1" />
                    <!-- Grow width -->
                    <DoubleAnimation
                        Storyboard.TargetProperty="Width"
                        To="200" Duration="0:0:0.5" />
                </Storyboard>
            </BeginStoryboard>
        </EventTrigger>
    </Button.Triggers>
</Button>
```

In MAUI, use `Animation`, `ViewExtensions` (e.g., `FadeTo`, `TranslateTo`, `ScaleTo`) or the Community Toolkit animations.

---

**Q98. What is the difference between `ContentView` and `Frame` in .NET MAUI?**

- **`ContentView`** — a base class for creating reusable custom controls in MAUI. It wraps a single piece of content and is the building block for custom views.
- **`Frame`** — a container that adds a border, rounded corners, shadow, and padding around its content. Used for card-style UI elements.

```xml
<!-- Frame - card UI -->
<Frame CornerRadius="12" HasShadow="True" Padding="16">
    <VerticalStackLayout>
        <Label Text="{Binding Name}" FontSize="18" FontAttributes="Bold" />
        <Label Text="{Binding Description}" />
    </VerticalStackLayout>
</Frame>
```

In newer MAUI versions, `Border` is preferred over `Frame` as it's more flexible.

---

**Q99. How do you implement deep linking in .NET MAUI?**

Deep linking lets an external URL open a specific page in your app. MAUI Shell supports this natively.

Register the URL scheme in platform manifests, then handle it in code:

```csharp
// Register route
Routing.RegisterRoute("product/detail", typeof(ProductDetailPage));

// Handle incoming deep link
protected override async void OnAppLinkRequestReceived(Uri uri)
{
    base.OnAppLinkRequestReceived(uri);
    // uri: myapp://product/detail?id=42
    var id = HttpUtility.ParseQueryString(uri.Query)["id"];
    await Shell.Current.GoToAsync($"product/detail?id={id}");
}
```

On iOS, configure `CFBundleURLSchemes` in `Info.plist`. On Android, configure an intent filter in `AndroidManifest.xml`.

---

**Q100. What are the key performance optimizations for a .NET MAUI app?**

- **Use `CollectionView` over `ListView`** — better virtualization.
- **Compiled bindings** (`x:DataType`) — resolves bindings at compile time instead of runtime reflection.
- **Avoid deep view hierarchies** — flatten layouts where possible.
- **Lazy load images** — use `CachedImage` from FFImageLoading or built-in async image loading.
- **Minimize work on the main thread** — always use async for I/O and `Task.Run` for CPU work.
- **Use `Shell` for navigation** — avoids keeping all pages in memory.
- **Reduce app startup time** — defer non-critical initialization using `Task.Run` after the app loads.
- **Profile with dotnet-trace** or Visual Studio's built-in MAUI profiler.

```xml
<!-- Compiled bindings - faster than reflection at runtime -->
<CollectionView ItemsSource="{Binding Products}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Product">
            <Label Text="{Binding Name}" /> <!-- resolved at compile time -->
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

---

*End of Q&A — 100 Questions covering ASP.NET Core MVC, WPF, and .NET MAUI*