# .NET Front-End Technologies — Interview Q&A (Lookup)

> Fundamental questions covering ASP.NET Core MVC, WPF, WinForms (overview), Web Forms (overview), .NET MAUI, Blazor Server, and Blazor WebAssembly. Beginner → experienced developer level.

---

## Section 1: ASP.NET Core MVC (50 Questions)

---

**Q1. What is ASP.NET Core MVC?**

ASP.NET Core MVC is a lightweight, open-source, cross-platform framework for building web apps and APIs using the Model–View–Controller pattern.

- Runs on Windows, Linux, macOS.
- Built on .NET Core / .NET 6+.
- Supports DI, routing, filters, and Razor views out of the box.

---

**Q2. Explain the MVC architecture.**

MVC separates an application into three responsibilities:

| Layer | Responsibility |
|---|---|
| **Model** | Business data, validation rules, DB entities |
| **View** | UI rendering (Razor/HTML) |
| **Controller** | Receives requests, calls model, returns view/result |

This separation improves testability, maintainability, and parallel development.

---

**Q3. What is a Controller?**

A Controller is a C# class (suffix `Controller`) that handles incoming HTTP requests.

- Inherits from `Controller` (MVC) or `ControllerBase` (API).
- Contains **action methods** that return `IActionResult`.
- Resolves dependencies via constructor injection.

```csharp
public class HomeController : Controller {
    public IActionResult Index() => View();
}
```

---

**Q4. What is a Model in MVC?**

A Model represents the data and business logic of the application.

- Holds entity data (e.g., `Product`, `Customer`).
- Defines validation rules via data annotations.
- Can be a domain class, DTO, or ViewModel.

---

**Q5. What is a View in MVC?**

A View is a `.cshtml` Razor file that generates the HTML output sent to the browser.

- Uses Razor syntax (`@` for code blocks).
- Strongly typed via `@model`.
- Stored under `Views/<Controller>/<Action>.cshtml`.

---

**Q6. What is the Razor View Engine?**

Razor is the templating engine in ASP.NET Core that mixes C# with HTML using the `@` symbol.

- Compiles `.cshtml` files into C# classes.
- Supports IntelliSense and compile-time errors.
- Used in MVC, Razor Pages, and Blazor.

---

**Q7. What is the MVC request lifecycle?**

| Step | Stage |
|---|---|
| 1 | Middleware pipeline |
| 2 | Routing |
| 3 | Controller instantiation (DI) |
| 4 | Authorization filters |
| 5 | Model binding & validation |
| 6 | Action filters → Action method |
| 7 | Result filters → Result execution |
| 8 | Response sent |

---

**Q8. What are the types of routing in MVC?**

Two types:

- **Conventional Routing** — pattern defined in `Program.cs` (`{controller}/{action}/{id?}`).
- **Attribute Routing** — `[Route]`, `[HttpGet]` attributes on controller/action.

You can mix both in the same app.

---

**Q9. What is Conventional Routing?**

Routing pattern set centrally in `Program.cs`. Applies to all controllers automatically.

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

URL `/Products/Details/5` → `ProductsController.Details(5)`.

---

**Q10. What is Attribute Routing?**

Routes defined directly on controller or action using attributes. Best for APIs and custom URL patterns.

```csharp
[Route("api/products")]
public class ProductsController : Controller {
    [HttpGet("{id}")]
    public IActionResult Get(int id) => Ok();
}
```

---

**Q11. What is an Action Method?**

A public method inside a controller that handles an HTTP request and returns an `IActionResult`.

- Decorated with HTTP verbs (`[HttpGet]`, `[HttpPost]`).
- Parameters are bound from route/query/body.
- Must return a result (View, JSON, Redirect, etc.).

---

**Q12. What are the common ActionResult types?**

| Type | Returns |
|---|---|
| `ViewResult` | Renders a Razor view |
| `PartialViewResult` | Renders a partial view |
| `JsonResult` | JSON data |
| `RedirectResult` | Redirect to URL |
| `RedirectToActionResult` | Redirect to action |
| `ContentResult` | Plain text |
| `FileResult` | File download |
| `NotFoundResult` | HTTP 404 |
| `BadRequestResult` | HTTP 400 |

---

**Q13. Can you overload action methods?**

Not directly with the same HTTP verb — MVC cannot distinguish overloads from a URL.

Workarounds:
- Different HTTP verbs (`[HttpGet]` + `[HttpPost]`).
- Different attribute routes.
- `[ActionName]` to give a different name.

---

**Q14. What are Action Filters?**

Filters are code that runs **before** or **after** specific stages of the request pipeline.

- Used for cross-cutting concerns (logging, auth, caching).
- Applied as attributes on actions, controllers, or globally.

---

**Q15. What are the types of Filters in MVC?**

| Filter | Runs |
|---|---|
| **Authorization** | First — checks user access |
| **Resource** | Around model binding |
| **Action** | Before/after action method |
| **Result** | Before/after result execution |
| **Exception** | When unhandled exception thrown |

---

**Q16. What is an Authorization Filter?**

Runs **first** in the filter pipeline. Decides if the user is allowed to access the resource.

- `[Authorize]`, `[AllowAnonymous]` are common examples.
- Short-circuits the pipeline if access is denied.

---

**Q17. What is an Action Filter?**

Runs **before** (`OnActionExecuting`) and **after** (`OnActionExecuted`) the action method.

- Useful for logging, validating input, modifying arguments.
- Implement `IActionFilter` or `IAsyncActionFilter`.

---

**Q18. What is a Result Filter?**

Runs **before** and **after** the IActionResult is executed.

- Useful for modifying the response (e.g., add headers).
- Implement `IResultFilter` or `IAsyncResultFilter`.

---

**Q19. What is an Exception Filter?**

Catches unhandled exceptions thrown during action or result execution.

- Lets you return a consistent error response.
- Implement `IExceptionFilter` or extend `ExceptionFilterAttribute`.

---

**Q20. What is RenderBody, RenderSection, and RenderPage?**

Used inside a layout view (`_Layout.cshtml`):

| Method | Purpose |
|---|---|
| `@RenderBody()` | Required — injects child view content |
| `@RenderSection("name", required:false)` | Optional named section from child view |
| `@RenderPage("path")` | Embeds another `.cshtml` file inline |

---

**Q21. What are Tag Helpers?**

Server-side components that look like standard HTML tags but generate dynamic HTML.

```html
<a asp-controller="Home" asp-action="Index">Home</a>
<form asp-action="Login" method="post"></form>
```

- IntelliSense-friendly.
- Replace older `Html.ActionLink`-style helpers.

---

**Q22. What are HTML Helpers?**

C# methods that return HTML strings from Razor views.

```csharp
@Html.TextBoxFor(m => m.Name)
@Html.ActionLink("Edit", "Edit", new { id = 1 })
```

- Older syntax — Tag Helpers are now preferred.

---

**Q23. What is ViewData?**

Dictionary-based way to pass data from controller to view.

```csharp
ViewData["Title"] = "Home";
```

- Type: `ViewDataDictionary` (string keys, object values).
- Requires casting.
- Lifetime: current request only.

---

**Q24. What is ViewBag?**

Dynamic wrapper around `ViewData`.

```csharp
ViewBag.Title = "Home";
```

- Same underlying store as `ViewData`.
- No IntelliSense or compile-time check.
- Lifetime: current request only.

---

**Q25. What is TempData?**

Stores data that survives **one redirect** (controller → redirect → next action).

```csharp
TempData["Msg"] = "Saved!";
return RedirectToAction("Index");
```

- Backed by cookies or session.
- Cleared after read (unless `Keep`/`Peek` used).

---

**Q26. What is a ViewModel?**

A class designed specifically for a view — contains exactly the data the view needs.

- Strongly typed and IntelliSense-friendly.
- Prevents exposing domain model details (over-posting).
- Best option for view-controller data transfer.

---

**Q27. Difference between ViewData, ViewBag, TempData, and ViewModel.**

| Feature | ViewData | ViewBag | TempData | ViewModel |
|---|---|---|---|---|
| Type-safe | No | No | No | Yes |
| Lifetime | One request | One request | One redirect | One request |
| Storage | Dictionary | Dynamic | Cookie/Session | Class instance |
| IntelliSense | No | No | No | Yes |

---

**Q28. What is Model Binding?**

Automatic mapping of HTTP request data (route, query, form, body) to action parameters and model properties.

- Sources searched in order: route → query → form.
- Customize with `[FromBody]`, `[FromQuery]`, `[FromRoute]`, `[FromHeader]`, `[FromForm]`.

---

**Q29. How is Validation done in MVC?**

Use **Data Annotations** on model properties.

```csharp
[Required] public string Name { get; set; }
[EmailAddress] public string Email { get; set; }
[Range(1,100)] public int Age { get; set; }
```

- Server-side: `ModelState.IsValid`.
- Client-side: jQuery Validate (automatic with tag helpers).

---

**Q30. How do you create Custom Validation?**

Inherit from `ValidationAttribute` and override `IsValid`.

```csharp
public class FutureDateAttribute : ValidationAttribute {
    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        => ((DateTime)value!) > DateTime.Today
            ? ValidationResult.Success
            : new ValidationResult("Must be future date");
}
```

---

**Q31. What are Areas in MVC?**

Areas divide a large MVC app into smaller functional modules (e.g., `Admin`, `Customer`).

- Each Area has its own Controllers, Views, Models.
- Folder: `Areas/<AreaName>/Controllers`.
- Controller is decorated with `[Area("Admin")]`.

---

**Q32. What is a Partial View?**

A reusable Razor fragment rendered inside another view (e.g., comment card, login widget).

- File named with `_` prefix (e.g., `_ProductCard.cshtml`).
- Rendered with `<partial name="_Card" model="m"/>` or `@await Html.PartialAsync(...)`.

---

**Q33. What is a Layout View?**

Defines the outer shell of pages (header, footer, nav, common CSS/JS).

- Usually `_Layout.cshtml` in `Views/Shared`.
- Child views are injected via `@RenderBody()`.
- Default set in `_ViewStart.cshtml`.

---

**Q34. What is a View Component?**

A reusable mini-controller with its own logic, DI, and view — returns a piece of UI (not a full page).

```csharp
public class CartSummaryViewComponent : ViewComponent {
    public IViewComponentResult Invoke() => View(model);
}
```

Invoked via `<vc:cart-summary />`. Replaces old child actions.

---

**Q35. What is Anti-Forgery Token (CSRF protection)?**

A hidden token in forms that prevents Cross-Site Request Forgery attacks.

- Tag-helper `<form>` adds it automatically.
- Validated server-side using `[ValidateAntiForgeryToken]`.
- Manual injection: `@Html.AntiForgeryToken()`.

---

**Q36. What is _ViewStart.cshtml?**

A special Razor file that runs before every view in its folder/subfolders.

- Most common use: set the default Layout.

```csharp
@{ Layout = "_Layout"; }
```

---

**Q37. What is _ViewImports.cshtml?**

A file that defines shared directives for all views in a folder.

- Common contents: `@using`, `@addTagHelper`, `@inject`.
- Reduces boilerplate at the top of every view.

---

**Q38. What is Razor Syntax?**

A markup language mixing C# code with HTML using `@`.

```csharp
@{ var x = 5; }
<p>Value: @x</p>
@if (x > 0) { <p>Positive</p> }
@foreach (var i in items) { <li>@i</li> }
```

---

**Q39. Why use HTTP verbs in action methods?**

Verbs (`[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]`) specify which HTTP method the action handles.

- Required in Web API.
- Best practice in MVC for POST actions.
- Allows action overloading by verb.

---

**Q40. Difference between Redirect and RedirectToAction.**

| Method | Argument | Type-safe |
|---|---|---|
| `Redirect("/path")` | Hardcoded URL | No |
| `RedirectToAction("Action","Controller")` | Action name | Yes — uses routing |
| `RedirectToRoute("name")` | Named route | Yes |

Prefer `RedirectToAction` for maintainability.

---

**Q41. What is the PRG (Post-Redirect-Get) pattern?**

After a successful POST, redirect to a GET. Prevents duplicate form submission on browser refresh.

```csharp
[HttpPost] public IActionResult Save(Model m) {
    _svc.Save(m);
    return RedirectToAction("Index"); // GET
}
```

---

**Q42. Difference between IActionResult and ActionResult<T>.**

| Type | Strongly typed? | Use case |
|---|---|---|
| `IActionResult` | No | Generic actions |
| `ActionResult<T>` | Yes — wraps `T` | API actions returning DTO |

`ActionResult<T>` supports implicit conversion (`return product;`).

---

**Q43. What is the [FromServices] attribute?**

Injects a service directly into an action parameter (instead of constructor).

```csharp
public IActionResult Report([FromServices] IReportSvc svc) {
    return View(svc.Generate());
}
```

Use when service is needed only by one action.

---

**Q44. What are [FromBody], [FromQuery], [FromRoute], [FromForm], [FromHeader]?**

Attributes that explicitly tell the model binder where to read data from.

| Attribute | Source |
|---|---|
| `[FromBody]` | Request body (JSON) |
| `[FromQuery]` | Query string |
| `[FromRoute]` | Route values |
| `[FromForm]` | Form fields |
| `[FromHeader]` | HTTP header |

---

**Q45. What is Bundling and Minification?**

- **Bundling** — combines multiple CSS/JS files into one to reduce HTTP requests.
- **Minification** — removes whitespace/comments to reduce file size.

In ASP.NET Core, use **WebOptimizer** or **LibMan** with build tools.

---

**Q46. What is a strongly typed view?**

A view that declares its model type with `@model`. Provides IntelliSense and compile-time checking.

```csharp
@model ProductViewModel
<h2>@Model.Name</h2>
```

Without it, you must use `ViewData`/`ViewBag`.

---

**Q47. What are Sections in views?**

Optional named blocks a child view can fill in the layout.

Layout:
```csharp
@RenderSection("Scripts", required: false)
```

Child:
```csharp
@section Scripts { <script>...</script> }
```

---

**Q48. How does Dependency Injection work in MVC?**

Services are registered in `Program.cs` and resolved automatically via constructor.

```csharp
builder.Services.AddScoped<IProductService, ProductService>();

public class HomeController : Controller {
    public HomeController(IProductService svc) { ... }
}
```

Lifetimes: Singleton, Scoped, Transient.

---

**Q49. What is Middleware in ASP.NET Core?**

Components that form a pipeline to process HTTP requests/responses.

```csharp
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(...);
```

Each middleware can short-circuit or pass to the next.

---

**Q50. Razor Pages vs MVC.**

| Feature | MVC | Razor Pages |
|---|---|---|
| Structure | Controller + Views | Page + PageModel (one file pair) |
| Best for | APIs, complex routing | Page-centric CRUD apps |
| Routing | Convention/attribute | Folder-based |
| Coupling | Looser | View tightly tied to handler |

---

## Section 2: WPF (15 Questions)

---

**Q51. What is WPF?**

Windows Presentation Foundation — a UI framework for building desktop apps on Windows using XAML and C#.

- Uses DirectX for rendering (GPU accelerated).
- Supports rich data binding, styling, templates, and animations.
- Vector-based UI, DPI-aware.

---

**Q52. What is XAML?**

eXtensible Application Markup Language — an XML-based language to define UI declaratively.

```xml
<Window>
    <Grid>
        <Button Content="Click Me"/>
    </Grid>
</Window>
```

Used in WPF, UWP, MAUI, and Silverlight.

---

**Q53. WPF vs WinForms.**

| Feature | WPF | WinForms |
|---|---|---|
| UI | XAML | Designer/code |
| Rendering | DirectX (GPU) | GDI+ (CPU) |
| Data binding | Rich, two-way | Limited |
| Styling | CSS-like | Basic |
| Pattern | MVVM natural | Code-behind |
| Vector graphics | Yes | No |

---

**Q54. What is a Dependency Property?**

A special property type that supports data binding, animation, styling, and value inheritance.

- Required for WPF binding to work.
- Stored centrally (not as fields).
- Registered with `DependencyProperty.Register`.

---

**Q55. What are the Data Binding modes in WPF?**

| Mode | Direction |
|---|---|
| `OneWay` | Source → Target |
| `TwoWay` | Source ↔ Target |
| `OneTime` | Source → Target (only at init) |
| `OneWayToSource` | Target → Source |
| `Default` | Depends on the control |

---

**Q56. What is INotifyPropertyChanged?**

An interface that lets ViewModels notify the UI when a property changes (so bindings update).

```csharp
public event PropertyChangedEventHandler PropertyChanged;
PropertyChanged?.Invoke(this, new(nameof(Name)));
```

Without it, the UI shows the initial value only.

---

**Q57. What are Commands (ICommand) in WPF?**

A way to bind UI actions (buttons, menus) to ViewModel methods — instead of code-behind event handlers.

- Implements `ICommand` (Execute + CanExecute).
- Enables MVVM by removing UI dependencies.
- Common implementation: `RelayCommand`.

---

**Q58. What is an ObservableCollection?**

A generic list that raises `CollectionChanged` events when items are added/removed.

- WPF lists/grids auto-update when bound to it.
- Plain `List<T>` does **not** notify UI.
- Lives in `System.Collections.ObjectModel`.

---

**Q59. What is a Resource Dictionary?**

A reusable XAML dictionary of resources — styles, brushes, templates, converters.

```xml
<ResourceDictionary>
    <SolidColorBrush x:Key="PrimaryBrush" Color="Blue"/>
</ResourceDictionary>
```

Merged at app or window level via `MergedDictionaries`.

---

**Q60. What are Styles in WPF?**

A set of property values applied to all controls of a type (like CSS classes).

```xml
<Style TargetType="Button">
    <Setter Property="Background" Value="Blue"/>
    <Setter Property="Foreground" Value="White"/>
</Style>
```

Supports inheritance via `BasedOn`.

---

**Q61. What is a Control Template?**

Defines the visual structure of a control — lets you completely redesign its appearance while keeping its behavior.

```xml
<ControlTemplate TargetType="Button">
    <Border CornerRadius="15" Background="{TemplateBinding Background}">
        <ContentPresenter/>
    </Border>
</ControlTemplate>
```

---

**Q62. What is a Data Template?**

Defines how a data object is rendered in lists/controls.

```xml
<DataTemplate DataType="{x:Type local:Product}">
    <StackPanel>
        <TextBlock Text="{Binding Name}"/>
        <TextBlock Text="{Binding Price}"/>
    </StackPanel>
</DataTemplate>
```

---

**Q63. What are Triggers in WPF?**

Change property values or play animations based on conditions, without code-behind.

| Type | Fires on |
|---|---|
| `Trigger` | Control property value |
| `DataTrigger` | Bound data value |
| `EventTrigger` | Routed event raised |
| `MultiTrigger` | Multiple conditions met |

---

**Q64. What are the WPF Layout Panels?**

| Panel | Use |
|---|---|
| `Grid` | Rows/columns (most common) |
| `StackPanel` | Vertical/horizontal stacking |
| `WrapPanel` | Wraps to next line |
| `DockPanel` | Dock to edges |
| `Canvas` | Absolute X/Y positioning |
| `UniformGrid` | Equal-sized cells |

---

**Q65. What is a Value Converter?**

Transforms data during binding — between ViewModel type and View type.

```csharp
public class BoolToVisConverter : IValueConverter {
    public object Convert(...) => (bool)value ? Visibility.Visible : Visibility.Collapsed;
    public object ConvertBack(...) => ...;
}
```

Used via `Converter={StaticResource BoolToVis}`.

---

## Section 3: WinForms (Overview — 3 Questions)

---

**Q66. What is WinForms?**

Windows Forms — the original .NET desktop UI framework, drag-and-drop based, built on GDI+.

- Event-driven, code-behind model.
- Simple and fast to develop.
- Still supported on .NET 6+.
- Good for internal tools and small utilities.

---

**Q67. Key features of WinForms.**

- Drag-and-drop designer in Visual Studio.
- Rich set of controls (Button, TextBox, DataGridView, etc.).
- Event-driven programming model.
- Easy database binding via DataSet/DataTable.
- Limited styling and no native vector graphics.

---

**Q68. WinForms vs WPF — quick comparison.**

| Feature | WinForms | WPF |
|---|---|---|
| Rendering | GDI+ (CPU) | DirectX (GPU) |
| UI definition | Designer code | XAML |
| Styling | Basic | Rich (themes, templates) |
| Data binding | Limited | Rich, two-way |
| Best for | Simple tools | Modern, rich UIs |

---

## Section 4: ASP.NET Web Forms (Overview — 3 Questions)

---

**Q69. What is ASP.NET Web Forms?**

A legacy web framework using event-driven, control-based UI similar to WinForms but for the web.

- Uses `.aspx` pages + code-behind.
- View state preserves data across postbacks.
- Server controls render HTML automatically.
- Replaced by MVC and Razor Pages for new apps.

---

**Q70. What is the Web Forms Page Lifecycle?**

| Stage | Event |
|---|---|
| 1 | `PreInit` |
| 2 | `Init` |
| 3 | `Load` |
| 4 | Postback events (e.g., Button_Click) |
| 5 | `PreRender` |
| 6 | `SaveStateComplete` |
| 7 | `Render` |
| 8 | `Unload` |

---

**Q71. Web Forms vs MVC.**

| Feature | Web Forms | MVC |
|---|---|---|
| Pattern | Event-driven | Model–View–Controller |
| HTML control | Auto-generated | Full control |
| State | ViewState | Stateless |
| Testability | Low | High |
| URL | `.aspx` files | Routing-based |
| Modern? | Legacy | Current |

---

## Section 5: .NET MAUI (9 Questions)

---

**Q72. What is .NET MAUI?**

Multi-platform App UI — Microsoft's framework for building **cross-platform** native apps (iOS, Android, macOS, Windows) from a single C#/XAML codebase.

- Successor to Xamarin.Forms.
- Single project, multiple targets.
- Built on .NET 6+.

---

**Q73. MAUI project structure.**

```
MyApp/
├── Platforms/        (Android, iOS, MacCatalyst, Windows)
├── Resources/        (Fonts, Images, Styles)
├── App.xaml
├── AppShell.xaml     (navigation)
├── MainPage.xaml
└── MauiProgram.cs    (startup, DI)
```

Single project replaces Xamarin's multi-head project model.

---

**Q74. What is Shell in MAUI?**

The top-level navigation container — provides tabs, flyout menu, and URI-based navigation.

```csharp
await Shell.Current.GoToAsync("//Profile");
await Shell.Current.GoToAsync($"Detail?id={id}");
```

Reduces boilerplate vs NavigationPage/TabbedPage.

---

**Q75. MAUI vs Xamarin.Forms.**

| Feature | Xamarin.Forms | MAUI |
|---|---|---|
| Project | Multi-project | Single project |
| Native handlers | Renderers | Handlers (lighter) |
| Performance | Slower | Faster |
| Desktop | Limited | Windows + macOS |
| Status | EOL (2024) | Active |

---

**Q76. What are MAUI Layout Controls?**

| Layout | Purpose |
|---|---|
| `VerticalStackLayout` | Top-to-bottom stack |
| `HorizontalStackLayout` | Left-to-right stack |
| `Grid` | Rows + columns |
| `FlexLayout` | CSS flexbox style |
| `AbsoluteLayout` | X/Y positioning |
| `ScrollView` | Scrollable container |

---

**Q77. What is CollectionView in MAUI?**

Modern, virtualized list control — replacement for the older `ListView`.

- Better performance.
- Supports horizontal/vertical scroll, grouping, multi-select.
- Free `DataTemplate` (no required ViewCell wrapper).

---

**Q78. How does Data Binding work in MAUI?**

Same as WPF — set `BindingContext` to ViewModel, bind control properties.

```xml
<Label Text="{Binding Name}"/>
<Entry Text="{Binding Name, Mode=TwoWay}"/>
```

ViewModel must implement `INotifyPropertyChanged` (or use Community Toolkit's `[ObservableProperty]`).

---

**Q79. Local storage options in MAUI.**

| Option | Use |
|---|---|
| `Preferences` | Key-value settings |
| `SecureStorage` | Encrypted (tokens, passwords) |
| **SQLite** | Local relational DB |
| `FileSystem.AppDataDirectory` | File-based storage |

---

**Q80. How to add Platform-Specific Code in MAUI?**

Three approaches:

1. **Partial classes** — `MyService.Android.cs`, `MyService.iOS.cs`.
2. **`#if` preprocessor** — `#if ANDROID ... #endif`.
3. **Platform folders** — code under `Platforms/Android/`, etc. compiled only for that target.

---

## Section 6: Blazor Server (10 Questions)

---

**Q81. What is Blazor?**

A .NET framework for building interactive web UIs using C# and Razor components — no JavaScript required.

- Two hosting models: **Server** and **WebAssembly**.
- Component-based (like React/Angular).
- Uses Razor syntax (`.razor` files).

---

**Q82. What is Blazor Server?**

A Blazor hosting model where the UI logic runs **on the server**. The browser only receives DOM diffs over a SignalR connection.

- Small download size.
- Full .NET runtime on server.
- Requires persistent connection.

---

**Q83. How does Blazor Server work?**

1. Browser requests a page.
2. Server renders initial HTML.
3. Browser opens a **SignalR** (WebSocket) connection.
4. UI events (clicks, inputs) sent to server.
5. Server runs C# code → sends DOM diffs back.
6. Browser updates UI.

---

**Q84. Pros and Cons of Blazor Server.**

| Pros | Cons |
|---|---|
| Small download | Requires WebSocket connection |
| Full .NET on server | Latency over network |
| Fast initial load | Doesn't work offline |
| Code stays secure | Scales harder (state per user) |

---

**Q85. What is a Razor Component?**

A reusable UI piece (`.razor` file) with markup, C# code, and parameters.

```razor
@code { [Parameter] public string Title { get; set; } }
<h1>@Title</h1>
```

Components compose to build entire pages.

---

**Q86. What are the Blazor Component Lifecycle methods?**

| Method | When |
|---|---|
| `OnInitialized` / `Async` | Once when component is created |
| `OnParametersSet` / `Async` | When parameters update |
| `OnAfterRender(firstRender)` / `Async` | After DOM rendered |
| `ShouldRender` | Decide whether to re-render |
| `Dispose` | Cleanup (if `IDisposable`) |

---

**Q87. How do you pass data to a component (Parameters)?**

Use the `[Parameter]` attribute.

```razor
<ChildCard Title="Hello" Count="5" />

@code {
    [Parameter] public string Title { get; set; }
    [Parameter] public int Count { get; set; }
}
```

For cascading data: `[CascadingParameter]`.

---

**Q88. How is Event Handling done in Blazor?**

Use `@on<event>` directives — wires DOM events to C# methods.

```razor
<button @onclick="Save">Save</button>

@code {
    void Save() => Console.WriteLine("Saved!");
}
```

Supports async handlers and event arguments (`MouseEventArgs`, etc.).

---

**Q89. How does Routing work in Blazor?**

Components use the `@page` directive to declare a route.

```razor
@page "/products/{id:int}"

@code { [Parameter] public int Id { get; set; } }
```

Navigation via `NavigationManager.NavigateTo("/path")`.

---

**Q90. How is State Management done in Blazor Server?**

| Scope | Approach |
|---|---|
| Component | Local fields/properties |
| Cross-component | Scoped DI service |
| User session | Scoped service (per circuit) |
| Persistent | DB, browser storage via JS interop |

`AppState` services using DI are the standard pattern.

---

## Section 7: Blazor WebAssembly (10 Questions)

---

**Q91. What is Blazor WebAssembly?**

A Blazor hosting model where C# code runs **directly in the browser** via WebAssembly — no server required after download.

- Fully client-side (like SPA).
- Works offline.
- Larger initial download (.NET runtime).

---

**Q92. Blazor WebAssembly vs Blazor Server.**

| Feature | Blazor Server | Blazor WASM |
|---|---|---|
| Runs on | Server | Browser |
| Initial load | Small | Large (runtime) |
| Connection | Persistent SignalR | Not required |
| Offline | No | Yes |
| Code security | Hidden on server | Sent to browser |
| Latency | Higher | Lower |

---

**Q93. How does Blazor WebAssembly work?**

1. Browser loads a small `index.html` and `blazor.webassembly.js`.
2. The Blazor runtime (Mono on WASM) is downloaded.
3. App DLLs are downloaded.
4. .NET code runs in the browser sandbox.
5. UI rendered directly via DOM updates.

---

**Q94. What are the Blazor Hosting Models?**

| Model | Where C# runs |
|---|---|
| **Blazor Server** | On the server, UI over SignalR |
| **Blazor WebAssembly** | In the browser (WASM) |
| **Blazor Hybrid** | In a native app (MAUI/WPF/WinForms) |
| **Blazor United** (.NET 8+) | Mix per-page (server + WASM) |

---

**Q95. Pros and Cons of Blazor WebAssembly.**

| Pros | Cons |
|---|---|
| Runs offline | Larger initial download |
| No server load | Code visible to client |
| Reduced server cost | Limited to browser sandbox |
| Static hosting OK | Slower startup than JS |

---

**Q96. How does Dependency Injection work in Blazor?**

Services registered in `Program.cs` and injected via `@inject` or `[Inject]`.

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

```razor
@inject IProductService Svc
```

Lifetimes: `Singleton`, `Scoped`, `Transient` (Scoped = per circuit/session).

---

**Q97. How do Forms and Validation work in Blazor?**

Use `EditForm` with a model and data annotations.

```razor
<EditForm Model="user" OnValidSubmit="Save">
    <DataAnnotationsValidator/>
    <ValidationSummary/>
    <InputText @bind-Value="user.Name"/>
    <button type="submit">Save</button>
</EditForm>
```

Input components: `InputText`, `InputNumber`, `InputDate`, `InputCheckbox`, `InputSelect`.

---

**Q98. What is JS Interop in Blazor?**

Calling JavaScript from C# (or vice versa) using `IJSRuntime`.

```csharp
@inject IJSRuntime JS
await JS.InvokeVoidAsync("alert", "Hello");
var width = await JS.InvokeAsync<int>("getWidth");
```

Used to call browser APIs not directly available in .NET.

---

**Q99. How do you call APIs (HttpClient) in Blazor WebAssembly?**

`HttpClient` is preconfigured for the app's base address.

```csharp
@inject HttpClient Http
var products = await Http.GetFromJsonAsync<List<Product>>("api/products");
```

Use `System.Net.Http.Json` for `GetFromJsonAsync` / `PostAsJsonAsync` helpers.

---

**Q100. How is Authentication done in Blazor WebAssembly?**

Use `Microsoft.AspNetCore.Components.WebAssembly.Authentication`.

- Common providers: **OIDC**, **Azure AD**, **IdentityServer**.
- Use `<AuthorizeView>` to conditionally render UI.
- Protect routes with `[Authorize]`.
- Tokens stored in browser, attached to HTTP requests via `AuthorizationMessageHandler`.

---

