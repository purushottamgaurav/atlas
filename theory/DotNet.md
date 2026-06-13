# .NET / .NET Core Interview Q&A

---

1. **.NET vs .NET Core?**
.NET Framework is Windows-only, mature, supports WinForms/WPF/ASP.NET. .NET Core is cross-platform (Windows, Linux, macOS), open-source, modular, and high performance. From .NET 5 onwards, Microsoft unified both under ".NET" (5, 6, 7, 8...), dropping the "Core" branding. .NET Framework still exists but receives no new features.

2. **How do we implement background work?**
Implement IHostedService interface with StartAsync and StopAsync methods, or inherit from BackgroundService and override ExecuteAsync. Register it in Program.cs with builder.Services.AddHostedService<MyService>(). Used for recurring tasks, queue processing, or long-running jobs.
```csharp
public class MyWorker : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken ct) {
        while (!ct.IsCancellationRequested) {
            // do work
            await Task.Delay(1000, ct);
        }
    }
}
```

3. **What are the JSON files in .NET Core?**
appsettings.json — main app configuration (connection strings, app settings). appsettings.{Environment}.json — environment-specific overrides (appsettings.Development.json, appsettings.Production.json). launchSettings.json — local dev launch profiles (ports, env variables), not deployed. global.json — pins the .NET SDK version for the project.

4. **How do we handle concurrency and parallelism in .NET Core?**
Concurrency — async/await for I/O-bound work, Task Parallel Library (TPL) for CPU-bound work. Parallelism — Parallel.For, Parallel.ForEach for data parallelism, Parallel.Invoke for task parallelism. Thread safety — lock, Monitor, SemaphoreSlim, Interlocked, ConcurrentDictionary, ConcurrentQueue. Channels — Channel<T> for producer-consumer pipelines.

5. **Architecture of .NET Core?**
CLR (CoreCLR) — runtime that executes managed code; includes GC, JIT, type system. BCL (Base Class Library) — core APIs like System, Collections, IO, Threading. JIT (RyuJIT) — compiles IL to native code at runtime. ASP.NET Core — web framework built on top. .NET Standard — compatibility spec between .NET Framework, Core, and Xamarin. .NET 5+ unifies all into a single platform.

6. **What are metapackages in .NET?**
A metapackage is a NuGet package that references a group of related packages as a single dependency. Example: Microsoft.AspNetCore.App is a metapackage that pulls in all core ASP.NET packages. Simplifies dependency management — one reference instead of dozens.

7. **What are middleware?**
Middleware are components that form the HTTP request/response pipeline in ASP.NET Core. Each component can process the request, pass it to the next component, or short-circuit the pipeline.
Types — built-in (Authentication, Routing, StaticFiles, CORS, Session, ExceptionHandler) and custom.
Custom middleware — implement via a class with InvokeAsync(HttpContext context) or use app.Use() inline.
```csharp
public class LoggingMiddleware {
    private readonly RequestDelegate _next;
    public LoggingMiddleware(RequestDelegate next) => _next = next;
    public async Task InvokeAsync(HttpContext context) {
        Console.WriteLine($"Request: {context.Request.Path}");
        await _next(context);
    }
}
// Register: app.UseMiddleware<LoggingMiddleware>();
```

8. **What are filters? Types, order, custom?**
Filters run code at specific stages of the action execution pipeline. Acronym ARAER — Authorization, Resource, Action, Exception, Result.
Authorization filter — runs first; checks if user is authorized.
Resource filter — runs after authorization; used for caching or short-circuiting.
Action filter — runs before/after action method executes.
Exception filter — handles unhandled exceptions from actions.
Result filter — runs before/after action result executes.
Order — Authorization → Resource → Action → (Exception if needed) → Result.
Custom — implement IActionFilter or inherit ActionFilterAttribute and override OnActionExecuting/OnActionExecuted.

9. **Dependency Injection — Scoped, Singleton, Transient?**
Transient — new instance every time it is requested. Use for lightweight stateless services.
Scoped — one instance per HTTP request. Use for DbContext, unit-of-work patterns.
Singleton — one instance for the entire application lifetime. Use for caching, configuration, shared state.
```csharp
builder.Services.AddTransient<IMyService, MyService>();
builder.Services.AddScoped<IMyService, MyService>();
builder.Services.AddSingleton<IMyService, MyService>();
```

10. **What is Use(), Map(), and Run() in middleware?**
Use() — adds middleware that can call the next component in the pipeline. Used for most middleware.
Run() — adds terminal middleware; does not call next. Ends the pipeline.
Map() — branches the pipeline based on a URL path prefix. Each branch has its own pipeline.
```csharp
app.Use(async (ctx, next) => { await next(); });
app.Map("/health", branch => branch.Run(async ctx => await ctx.Response.WriteAsync("OK")));
app.Run(async ctx => await ctx.Response.WriteAsync("End"));
```

11. **How to check code coverage?**
Use dotnet test with coverlet: dotnet test --collect:"XPlat Code Coverage". This generates a coverage.xml file. View results in Visual Studio (built-in), or use ReportGenerator to produce an HTML report. Integrate with SonarQube or Azure DevOps pipelines for CI coverage gates.

12. **Why do we mock class objects or services?**
Mocking isolates the unit under test by replacing real dependencies with controlled fakes. This avoids hitting databases, APIs, or external services during unit tests. Use Moq or NSubstitute. Enables fast, reliable, deterministic tests and allows testing edge cases (e.g., simulating exceptions from a service).

13. **What is the use of IApplicationBuilder?**
IApplicationBuilder is used in the Configure method (or middleware setup in Program.cs) to define the HTTP request pipeline. It exposes Use, Map, Run, and UseMiddleware to register and order middleware components.

14. **ConfigureServices() vs Configure() — which is called first?**
ConfigureServices() is called first — it registers services into the DI container. Configure() is called second — it uses those services to build the HTTP pipeline. In .NET 6+ minimal hosting, both are combined in Program.cs using WebApplicationBuilder and WebApplication.

15. **What is the use of IWebHostEnvironment?**
Provides information about the web hosting environment. Used to get the environment name (Development, Production), the content root path (app files), and the web root path (wwwroot — publicly servable static files). Commonly used to conditionally configure services or middleware per environment.

16. **What is the use of IServiceCollection?**
IServiceCollection is the container used to register services for dependency injection. Accessed in ConfigureServices() or Program.cs. Services registered here are resolved by the DI container throughout the application lifetime. Extension methods like AddDbContext, AddControllers, AddAuthentication all operate on IServiceCollection.

17. **Middleware vs Filter — which to use when?**
Middleware — use for cross-cutting concerns that apply to all requests regardless of whether they hit a controller. Examples: logging, authentication, CORS, compression, exception handling.
Filter — use for concerns specific to MVC/API actions. Examples: authorization checks on specific actions, model validation, action-level caching, response formatting.
Rule of thumb — if it's pipeline-wide, use middleware. If it's controller/action-specific, use a filter.

18. **CQRS design pattern in microservices?**
CQRS (Command Query Responsibility Segregation) separates read (Query) and write (Command) operations into distinct models. Commands change state and return no data (or just a result). Queries return data and don't change state. Benefits: independent scaling of reads/writes, simpler models, easier auditing. Typically paired with MediatR in .NET for dispatching commands and queries without direct coupling.
```csharp
// Command
public record CreateOrderCommand(string Item) : IRequest<int>;
// Query
public record GetOrderQuery(int Id) : IRequest<OrderDto>;
```

19. **Is order important for middleware? What is the preferred order?**
Yes, order is critical. Middleware runs in the order it is registered for requests and in reverse order for responses. Recommended order in ASP.NET Core:
ExceptionHandler → HSTS → HTTPS Redirection → Static Files → Routing → CORS → Authentication → Authorization → Custom Middleware → Endpoints.
Getting order wrong can cause issues (e.g., placing Authorization before Authentication means the user is never authenticated first).

20. **How to manage dependencies in a .NET application?**
Use the built-in DI container — register services in Program.cs with AddTransient, AddScoped, AddSingleton. For complex scenarios use third-party containers like Autofac. Manage NuGet package dependencies via .csproj and dotnet restore. Use IOptions<T> pattern for configuration dependencies. Avoid service locator anti-pattern — always prefer constructor injection.

21. **SonarQube vs SonarLint?**
SonarLint is an IDE plugin (Visual Studio, VS Code, Rider) that gives real-time code quality and security feedback as you type — runs locally. SonarQube is a server-based platform for continuous code quality analysis across the entire codebase — integrated in CI/CD pipelines. SonarLint is for the developer in the editor; SonarQube is for the team and the build pipeline. Both use the same rule sets.

22. **How to upgrade from .NET 6 to .NET 8?**
Update the target framework in .csproj: change net6.0 to net8.0. Update all Microsoft.* and System.* NuGet packages to their .NET 8 versions via dotnet outdated or NuGet Package Manager. Update global.json SDK version if present. Review breaking changes in the official .NET 8 migration guide. Test thoroughly — pay attention to changes in minimal APIs, nullable reference types, and any deprecated APIs. Run dotnet build and fix any warnings or errors.

37. **Database First vs Code First approach in Entity Framework?**
Database First — the database already exists. You reverse-engineer it into EF models using Scaffold-DbContext. The database is the source of truth. Good when working with an existing legacy database.
Code First — you write C# model classes first and EF generates the database from them using migrations. The code is the source of truth. Preferred for greenfield projects — keeps schema in version control via migrations.
38. **What is scaffolding in EF?**
Scaffolding reverse-engineers an existing database into EF model classes and a DbContext. Run via CLI: dotnet ef dbcontext scaffold "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer -o Models. Generates entity classes and Fluent API configuration based on the existing schema. Also used in MVC to auto-generate controller and view files from a model class.
39. **Difference between Single, SingleOrDefault, First, FirstOrDefault?**
First() — returns the first matching element. Throws InvalidOperationException if no match found.
FirstOrDefault() — returns the first matching element or default (null/0) if none found. Does not throw.
Single() — returns the only matching element. Throws if no match or more than one match found.
SingleOrDefault() — returns the only match or default if none. Throws only if more than one match found.
Use FirstOrDefault when you expect zero or more results and want the first. Use SingleOrDefault when exactly zero or one result is expected (e.g., lookup by unique ID).
40. **How to do pagination through LINQ?**
Use Skip and Take. Skip((pageNumber - 1) * pageSize) skips the records of previous pages. Take(pageSize) fetches only the current page's records.
```csharp
int pageNumber = 2, pageSize = 10;
var results = dbContext.Employees
    .OrderBy(e => e.Id)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```
Always use OrderBy before Skip/Take — SQL has no guaranteed row order without it.
 
27. **AsTracking vs AsNoTracking?**
AsTracking (default) — EF tracks changes to retrieved entities. When you call SaveChanges(), it detects and persists changes. Slight memory and performance overhead.
AsNoTracking — EF does not track the entities. Faster and uses less memory. Use for read-only queries where you don't need to update the data.
```csharp
var employees = dbContext.Employees.AsNoTracking().ToList();
```
 
28. **Eager Loading vs Lazy Loading vs Explicit Loading in EF?**
Eager Loading — loads related data immediately with the main query using Include(). One query with a JOIN. Best when you know you need related data.
```csharp
dbContext.Orders.Include(o => o.Customer).Include(o => o.Items).ToList();
```
Lazy Loading — related data is loaded automatically when the navigation property is first accessed. Requires virtual keyword on navigation properties and UseLazyLoadingProxies(). Can cause N+1 query problem.
Explicit Loading — related data is loaded manually on demand using Entry().Collection().Load() or Entry().Reference().Load(). Good when you conditionally need related data.
```csharp
var order = dbContext.Orders.First();
dbContext.Entry(order).Reference(o => o.Customer).Load();
```
 
29. **OnModelCreating vs OnModelBinding. How to create rules in EF?**
OnModelCreating — override in DbContext. Used to configure the model using Fluent API — define table names, column constraints, relationships, indexes, and keys. Runs once when the model is first built.
OnModelBinding does not exist in EF — this may refer to model binding in MVC (how HTTP request data maps to action parameters).
Fluent API rules in OnModelCreating:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Employee>()
        .HasKey(e => e.Id);
    modelBuilder.Entity<Employee>()
        .Property(e => e.Name).IsRequired().HasMaxLength(100);
    modelBuilder.Entity<Employee>()
        .HasOne(e => e.Department)
        .WithMany(d => d.Employees)
        .HasForeignKey(e => e.DeptId);
}
```
 
30. **DbContext vs DbSet?**
DbContext — the main class that manages the database connection, tracks changes, and coordinates querying and saving. One DbContext per unit of work (scoped lifetime in DI). Contains DbSet properties for each entity.
DbSet<T> — represents a table in the database. Exposes LINQ query methods (Where, Find, Add, Remove, etc.). Each DbSet corresponds to one entity/table.
```csharp
public class AppDbContext : DbContext {
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
}
```
 
31. **How to set a foreign key and use a stored procedure in EF?**
Foreign key — configure with Fluent API in OnModelCreating or use Data Annotations ([ForeignKey]).
```csharp
// Fluent API
modelBuilder.Entity<Order>()
    .HasOne(o => o.Customer)
    .WithMany(c => c.Orders)
    .HasForeignKey(o => o.CustomerId);
 
// Data Annotation
[ForeignKey("CustomerId")]
public Customer Customer { get; set; }
```
Stored Procedure — call via FromSqlRaw for SELECT or ExecuteSqlRaw for non-query operations.
```csharp
// Query SP
var result = dbContext.Employees.FromSqlRaw("EXEC GetEmployeesByDept @DeptId", param).ToList();
 
// Non-query SP
dbContext.Database.ExecuteSqlRaw("EXEC UpdateSalary @Id, @Amount", idParam, amountParam);
```
 
32. **What are DataTables in ADO.NET?**
DataTable is an in-memory representation of a database table — has rows (DataRow) and columns (DataColumn). Part of DataSet (a collection of DataTables). Used in disconnected data access — fill it from the database using SqlDataAdapter, manipulate in memory, then optionally push changes back. Still used in legacy apps and bulk operations but largely replaced by EF and typed models in modern .NET.
```csharp
var dt = new DataTable();
using var adapter = new SqlDataAdapter("SELECT * FROM Employees", connectionString);
adapter.Fill(dt);
foreach (DataRow row in dt.Rows) { Console.WriteLine(row["Name"]); }
```
 
33. **How to insert a collection of data in one go in EF?**
Use AddRange to add all entities at once and call SaveChanges once. Avoid calling SaveChanges inside a loop.
```csharp
var employees = new List<Employee> {
    new Employee { Name = "Alice" },
    new Employee { Name = "Bob" }
};
dbContext.Employees.AddRange(employees);
await dbContext.SaveChangesAsync();
```
For very large datasets use EF Core Bulk Extensions (third-party) or SqlBulkCopy (ADO.NET) for maximum performance — EF AddRange still generates individual INSERT statements under the hood.