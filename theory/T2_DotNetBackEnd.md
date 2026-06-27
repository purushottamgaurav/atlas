# .NET Backend Developer Interview Questions & Answers

---

## 🔷 Section 1: .NET Core Architecture & Fundamentals

---

**Q1. What is the difference between .NET and .NET Core?**

| | .NET Framework | .NET Core / .NET 5+ |
|--|--|--|
| Platform | Windows only | Cross-platform (Win, Mac, Linux) |
| Open source | No | Yes |
| Performance | Slower | Faster, optimized |
| Deployment | Machine-wide install | Self-contained possible |
| Future | Legacy, maintenance only | Active development (.NET 6/7/8) |

.NET Core was the rewrite. From .NET 5 onwards, Microsoft unified them under just ".NET".

---

**Q2. What is the architecture of .NET Core?**

.NET Core has three main layers:

- **CLI (dotnet CLI):** Command-line tooling to build, run, and publish apps.
- **BCL (Base Class Library):** Core APIs — collections, I/O, threading, etc.
- **Runtime (CoreCLR):** Executes code — handles JIT, GC, and type system.

```
Your App Code
    ↓
Base Class Library (BCL)
    ↓
CoreCLR Runtime (JIT + GC)
    ↓
OS (Windows / Linux / macOS)
```

Also includes **Kestrel** (web server), **DI container**, **Middleware pipeline**, and **Configuration system** as built-in.

---

**Q3. What are the important JSON config files in .NET Core?**

| File | Purpose |
|--|--|
| `appsettings.json` | Default app configuration |
| `appsettings.{Environment}.json` | Overrides per environment (Development, Production) |
| `launchSettings.json` | Dev-time settings — ports, env vars (not deployed) |
| `global.json` | Specifies the .NET SDK version to use |

```json
// appsettings.json
{
  "ConnectionStrings": { "Default": "Server=.;Database=MyDb;" },
  "Logging": { "LogLevel": { "Default": "Information" } },
  "FeatureFlags": { "EnableNewUI": true }
}
```

Values in `appsettings.Development.json` **override** `appsettings.json` in the dev environment.

---

**Q4. What are metapackages in .NET?**

A metapackage is a NuGet package that **bundles many packages together** — you install one package and get everything.

- `Microsoft.AspNetCore.App` — all ASP.NET Core libraries.
- `Microsoft.NETCore.App` — base .NET runtime libraries.

In modern .NET (5+), these are referenced automatically via the SDK — you rarely reference them manually.

---

**Q5. What is `IApplicationBuilder`? What are `Use()`, `Map()`, and `Run()`?**

`IApplicationBuilder` is used in `Configure()` to build the **middleware pipeline**.

| Method | Behavior |
|--|--|
| `Use()` | Adds middleware that **calls the next** middleware |
| `Run()` | Terminal middleware — **does not call next** |
| `Map()` | Branches the pipeline based on the **URL path** |

```csharp
app.Use(async (context, next) => {
    Console.WriteLine("Before");
    await next();              // call next middleware
    Console.WriteLine("After");
});

app.Map("/health", branch => {
    branch.Run(async ctx => await ctx.Response.WriteAsync("Healthy"));
});

app.Run(async context => {
    await context.Response.WriteAsync("Final response");
});
```

---

**Q6. What is `IWebHostEnvironment`?**

Provides info about the hosting environment — used to check if you're in Development, Staging, or Production.

```csharp
public class Startup {
    private readonly IWebHostEnvironment _env;
    public Startup(IWebHostEnvironment env) => _env = env;

    public void Configure(IApplicationBuilder app) {
        if (_env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        } else {
            app.UseExceptionHandler("/Error");
        }
    }
}
```

---

**Q7. What is `IServiceCollection`?**

The container where you **register all dependencies** in `ConfigureServices()` or `Program.cs`. The runtime uses this to resolve services via DI.

```csharp
builder.Services.AddSingleton<IConfig, AppConfig>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
```

---

**Q8. `ConfigureServices()` vs `Configure()` — which is called first?**

`ConfigureServices()` is **always called first** — it registers all services into the DI container. Then `Configure()` is called to build the middleware pipeline, which can use those registered services.

In .NET 6+, both are merged into `Program.cs`:

```csharp
// ConfigureServices equivalent
builder.Services.AddControllers();
builder.Services.AddScoped<IMyService, MyService>();

var app = builder.Build(); // DI container is built here

// Configure equivalent
app.UseRouting();
app.MapControllers();
app.Run();
```

---

**Q9. Dependency Injection — Singleton vs Scoped vs Transient?**

| Lifetime | Created | Same instance for |
|--|--|--|
| `Singleton` | Once ever | All requests, all users |
| `Scoped` | Per HTTP request | Duration of one request |
| `Transient` | Every time injected | Nothing — always new |

```csharp
services.AddSingleton<ICacheService, MemoryCacheService>();   // one instance forever
services.AddScoped<IOrderService, OrderService>();            // one per request
services.AddTransient<IEmailBuilder, EmailBuilder>();         // new each time

// ⚠️ Never inject Scoped into Singleton — Scoped lives shorter, causes bugs
```

---

**Q10. What is middleware? How do you create custom middleware?**

Middleware is a component in the **request pipeline** — each piece processes the request, optionally calls the next, and processes the response.

```csharp
public class RequestTimingMiddleware {
    private readonly RequestDelegate _next;

    public RequestTimingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context) {
        var sw = Stopwatch.StartNew();
        await _next(context);         // call next middleware
        sw.Stop();
        Console.WriteLine($"Request took {sw.ElapsedMilliseconds}ms");
    }
}

public static class MiddlewareExtensions {
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder app)
        => app.UseMiddleware<RequestTimingMiddleware>();
}

// Register in Program.cs
app.UseRequestTiming();
```

---

**Q11. Is the order of middleware important? What is the preferred order?**

Yes — middleware runs **in registration order** for requests and **reverse order** for responses. Wrong order causes bugs (e.g., auth before routing).

```csharp
// ✅ Recommended order
app.UseExceptionHandler();    // 1. Catch all errors first
app.UseHsts();                // 2. Security headers
app.UseHttpsRedirection();    // 3. Redirect HTTP → HTTPS
app.UseStaticFiles();         // 4. Serve static files (no auth needed)
app.UseRouting();             // 5. Match routes
app.UseAuthentication();      // 6. Who are you?
app.UseAuthorization();       // 7. What can you do?
app.UseMiddleware<CustomMiddleware>(); // 8. Your custom logic
app.MapControllers();         // 9. Execute the endpoint
```

---

**Q12. Middleware vs Filters — which to use when?**

| | Middleware | Filters |
|--|--|--|
| Scope | Entire pipeline | MVC/API action level |
| Access to MVC context | ❌ | ✅ (ActionContext, etc.) |
| Use for | Auth, logging, HTTPS, CORS | Validation, exception handling per action |
| Registered in | `Program.cs` | Controller/Action attributes or globally |

Use **middleware** for cross-cutting concerns at the HTTP level. Use **filters** when you need access to MVC action/controller context.

---

**Q13. What are Filters? Types and order?**

Filters run code at specific points in the MVC pipeline around action execution.

| Type | When it runs |
|--|--|
| `Authorization` | First — checks if user is allowed |
| `Resource` | Before/after model binding |
| `Action` | Before/after action method |
| `Exception` | When unhandled exception occurs |
| `Result` | Before/after action result executes |

**Order:** Authorization → Resource → Action → (action runs) → Result → Exception (if error)

```csharp
public class LogActionFilter : IActionFilter {
    public void OnActionExecuting(ActionExecutingContext context)
        => Console.WriteLine($"Action starting: {context.ActionDescriptor.DisplayName}");

    public void OnActionExecuted(ActionExecutedContext context)
        => Console.WriteLine("Action finished");
}

builder.Services.AddControllers(options => {
    options.Filters.Add<LogActionFilter>();
});

[ServiceFilter(typeof(LogActionFilter))]
public class OrdersController : ControllerBase { }
```

---

## 🔷 Section 2: Configuration & Options

---

**Q14. What is the Options Pattern? Why use it for app settings?**

Instead of injecting `IConfiguration` everywhere, the Options Pattern binds config sections to **strongly-typed classes**, making config clean and testable.

```json
// appsettings.json
{ "Email": { "SmtpHost": "smtp.gmail.com", "Port": 587 } }
```

```csharp
public class EmailOptions {
    public string SmtpHost { get; set; }
    public int Port { get; set; }
}

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));

public class EmailService {
    private readonly EmailOptions _opts;
    public EmailService(IOptions<EmailOptions> opts) => _opts = opts.Value;

    public void Send() => Console.WriteLine($"Sending via {_opts.SmtpHost}:{_opts.Port}");
}
```

Use `IOptions<T>` (singleton), `IOptionsSnapshot<T>` (per-request), or `IOptionsMonitor<T>` (live reload).

---

**Q15. How to upgrade from .NET 6 to .NET 8?**

1. Update the `TargetFramework` in `.csproj`:
```xml
<TargetFramework>net8.0</TargetFramework>
```

2. Update all NuGet packages to their .NET 8 compatible versions.
3. Update `global.json` SDK version if pinned.
4. Fix any breaking changes (check Microsoft's migration guide).
5. Run tests to catch regressions.
6. Test in all environments before deploying.

Use `dotnet-upgrade-assistant` tool to automate most steps:
```bash
dotnet tool install -g upgrade-assistant
upgrade-assistant upgrade MyApp.csproj
```

---

## 🔷 Section 3: Dependency Injection & Design Patterns

---

**Q16. What is the Factory Pattern with DI?**

When you need to create different implementations at runtime (based on a condition), use a factory registered in DI.

```csharp
public interface IPaymentGateway { void Pay(decimal amount); }
public class StripeGateway : IPaymentGateway { public void Pay(decimal a) => Console.WriteLine("Stripe"); }
public class PayPalGateway : IPaymentGateway { public void Pay(decimal a) => Console.WriteLine("PayPal"); }

public class PaymentGatewayFactory {
    private readonly IServiceProvider _sp;
    public PaymentGatewayFactory(IServiceProvider sp) => _sp = sp;

    public IPaymentGateway Create(string type) => type switch {
        "stripe" => _sp.GetRequiredService<StripeGateway>(),
        "paypal" => _sp.GetRequiredService<PayPalGateway>(),
        _ => throw new ArgumentException("Unknown gateway")
    };
}

services.AddTransient<StripeGateway>();
services.AddTransient<PayPalGateway>();
services.AddSingleton<PaymentGatewayFactory>();
```

---

**Q17. What is the Decorator Pattern with DI?**

Wraps an existing service to add behavior (logging, caching, retry) **without modifying** the original.

```csharp
public interface IOrderService { Task<Order> GetOrderAsync(int id); }

public class OrderService : IOrderService {
    public async Task<Order> GetOrderAsync(int id) => await db.Orders.FindAsync(id);
}

public class CachedOrderService : IOrderService {
    private readonly IOrderService _inner;
    private readonly IMemoryCache _cache;

    public CachedOrderService(IOrderService inner, IMemoryCache cache) {
        _inner = inner; _cache = cache;
    }

    public async Task<Order> GetOrderAsync(int id) {
        return await _cache.GetOrCreateAsync($"order_{id}",
            _ => _inner.GetOrderAsync(id));
    }
}

services.AddScoped<OrderService>();
services.AddScoped<IOrderService>(sp =>
    new CachedOrderService(sp.GetRequiredService<OrderService>(),
                           sp.GetRequiredService<IMemoryCache>()));
```

---

**Q18. What is the Mediator Pattern? (MediatR)**

Decouples senders from receivers — components don't talk directly, they send messages through a **mediator**.

```csharp
// Install: dotnet add package MediatR

public record CreateOrderCommand(string Product, int Qty) : IRequest<int>;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int> {
    public async Task<int> Handle(CreateOrderCommand cmd, CancellationToken ct) {
        // create order logic
        return newOrderId;
    }
}

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

public class OrdersController : ControllerBase {
    private readonly IMediator _mediator;
    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand cmd)
        => Ok(await _mediator.Send(cmd));
}
```

---

**Q19. What is the CQRS Pattern?**

**Command Query Responsibility Segregation** — separate the model for **reads (queries)** from **writes (commands)**.

- **Command:** Changes state, returns nothing (or minimal data). e.g., `CreateOrderCommand`.
- **Query:** Reads data, changes nothing. e.g., `GetOrderByIdQuery`.

```csharp
public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;
public record GetProductQuery(int Id) : IRequest<ProductDto>;

// Benefits:
// - Read model can be optimized (e.g., flat denormalized views)
// - Write model enforces business rules
// - Scales independently
```

Often combined with **MediatR** and **Event Sourcing**.

---

**Q20. What is the Saga Pattern?**

Used for **long-running distributed transactions** across multiple services. Instead of a single DB transaction, each step succeeds or triggers a **compensating action** to undo previous steps.

| Style | Description |
|--|--|
| **Orchestration** | Central coordinator tells each service what to do |
| **Choreography** | Each service reacts to events — no central control |

```
// Order Saga (Orchestration):
1. Create Order → success
2. Reserve Inventory → success
3. Charge Payment → FAILS
4. Compensate: Release Inventory ← undo step 2
5. Compensate: Cancel Order ← undo step 1
```

Implemented with tools like **MassTransit**, **NServiceBus**, or **Dapr**.

---

**Q21. What is the Repository Pattern? Should you use it with EF Core?**

Repository abstracts data access behind an interface — your service doesn't know or care if it's SQL, Mongo, or a mock.

```csharp
public interface IProductRepository {
    Task<Product> GetByIdAsync(int id);
    Task AddAsync(Product product);
}

public class EfProductRepository : IProductRepository {
    private readonly AppDbContext _db;
    public EfProductRepository(AppDbContext db) => _db = db;

    public Task<Product> GetByIdAsync(int id) => _db.Products.FindAsync(id).AsTask();
    public async Task AddAsync(Product p) { _db.Products.Add(p); await _db.SaveChangesAsync(); }
}
```

**Should you use it with EF Core?** EF Core's `DbSet` already acts as a repository, and `DbContext` acts as Unit of Work. For simple apps, skip the extra layer. Add it when you want to **swap data sources** or **improve testability**.

---

**Q22. What is the Unit of Work Pattern?**

Groups multiple repository operations into **one transaction** — all succeed or all roll back.

```csharp
public interface IUnitOfWork : IDisposable {
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    Task<int> CommitAsync();
}

public class UnitOfWork : IUnitOfWork {
    private readonly AppDbContext _db;
    public IOrderRepository Orders { get; }
    public IProductRepository Products { get; }

    public UnitOfWork(AppDbContext db) {
        _db = db;
        Orders = new OrderRepository(db);
        Products = new ProductRepository(db);
    }

    public Task<int> CommitAsync() => _db.SaveChangesAsync();
    public void Dispose() => _db.Dispose();
}

// Usage
await uow.Orders.AddAsync(order);
await uow.Products.UpdateStockAsync(productId, -1);
await uow.CommitAsync(); // single save — atomic
```

In EF Core, `DbContext.SaveChangesAsync()` is already a Unit of Work.

---

**Q23. What are architectural patterns? Give a brief overview.**

| Pattern | Description |
|--|--|
| **Layered (N-Tier)** | Split into Presentation, Business, Data layers |
| **MVC** | Model-View-Controller separation |
| **MVVM** | Model-View-ViewModel (WPF, Blazor) |
| **Microservices** | Small, independently deployable services |
| **Monolithic** | All in one deployable unit |
| **Repository** | Abstraction over data access |
| **CQRS** | Separate reads from writes |
| **Event-driven** | Components communicate via events/messages |
| **Saga** | Long-running distributed transactions |
| **Event Sourcing** | Store all changes as events, not current state |
| **Serverless** | Stateless functions triggered by events (Azure Functions) |

---

**Q24. Microservices vs Monolithic Architecture?**

| | Monolithic | Microservices |
|--|--|--|
| Deployment | Single unit | Independent per service |
| Scaling | Whole app | Per service |
| Complexity | Simple | High (networking, distributed) |
| Team size | Small teams | Multiple teams |
| Failure isolation | Poor | Good |
| Good for | Simple/early-stage apps | Large, complex systems |

Start monolithic, extract microservices when you hit scaling or team boundaries.

---

## 🔷 Section 4: Background Work & Performance

---

**Q25. How do you implement background work in .NET Core?**

Use `IHostedService` or the simpler `BackgroundService` base class.

```csharp
public class EmailQueueWorker : BackgroundService {
    private readonly ILogger<EmailQueueWorker> _logger;

    public EmailQueueWorker(ILogger<EmailQueueWorker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            _logger.LogInformation("Processing email queue...");
            await ProcessEmailsAsync();
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}

builder.Services.AddHostedService<EmailQueueWorker>();
```

For more complex scheduling, use **Hangfire** or **Quartz.NET**.

---

**Q26. What is ArrayPool vs MemoryPool vs stackalloc?**

All three reduce heap allocations for better performance.

| | `ArrayPool<T>` | `MemoryPool<T>` | `stackalloc` |
|--|--|--|--|
| Where | Heap (rented) | Heap (rented) | Stack |
| Size limit | Large OK | Large OK | Small only (~1KB) |
| Use with async | ✅ | ✅ | ❌ (Span only) |

```csharp
var pool = ArrayPool<byte>.Shared;
byte[] buffer = pool.Rent(4096);
try { await stream.ReadAsync(buffer); }
finally { pool.Return(buffer); }

Span<int> nums = stackalloc int[10];
nums[0] = 42;

using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(1024);
Memory<byte> mem = owner.Memory;
await stream.ReadAsync(mem);
```

---

**Q27. What is profiling and benchmarking in .NET?**

- **Profiling:** Finding where your app is slow or using too much memory at runtime.
- **Benchmarking:** Measuring how fast a specific piece of code runs with precision.

```csharp
// BenchmarkDotNet — install: dotnet add package BenchmarkDotNet
[MemoryDiagnoser]
public class StringBenchmarks {
    [Benchmark]
    public string ConcatWithPlus() {
        string s = "";
        for (int i = 0; i < 100; i++) s += i;
        return s;
    }

    [Benchmark]
    public string ConcatWithBuilder() {
        var sb = new StringBuilder();
        for (int i = 0; i < 100; i++) sb.Append(i);
        return sb.ToString();
    }
}

BenchmarkRunner.Run<StringBenchmarks>();
```

**Profiling tools:** Visual Studio Diagnostic Tools, dotMemory, dotTrace, PerfView.

---

**Q28. What is Event Sourcing?**

Instead of storing the **current state**, store every **change as an event**. The current state is rebuilt by replaying all events.

```
Normal DB: Order { Status: "Shipped", Total: 99.00 }

Event Sourcing:
Event 1: OrderCreated { Total: 99.00 }
Event 2: PaymentReceived { Amount: 99.00 }
Event 3: OrderShipped { TrackingNo: "XYZ" }
```

**Benefits:** Full audit trail, replay history, temporal queries ("what was the state last Tuesday?"), debugging.
**Cost:** More complex reads, eventual consistency.

---

**Q29. How to implement a custom health check?**

```csharp
public class DatabaseHealthCheck : IHealthCheck {
    private readonly AppDbContext _db;
    public DatabaseHealthCheck(AppDbContext db) => _db = db;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken ct = default) {
        try {
            await _db.Database.CanConnectAsync(ct);
            return HealthCheckResult.Healthy("Database is reachable");
        } catch (Exception ex) {
            return HealthCheckResult.Unhealthy("Database unreachable", ex);
        }
    }
}

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<ExternalApiHealthCheck>("external-api");

app.MapHealthChecks("/health");
```

---

**Q30. How to implement a custom logging provider?**

```csharp
public class FileLogger : ILogger {
    private readonly string _filePath;
    public FileLogger(string filePath) => _filePath = filePath;

    public IDisposable BeginScope<T>(T state) => null;
    public bool IsEnabled(LogLevel level) => true;

    public void Log<T>(LogLevel level, EventId id, T state,
        Exception ex, Func<T, Exception, string> formatter) {
        File.AppendAllText(_filePath, $"[{level}] {formatter(state, ex)}\n");
    }
}

public class FileLoggerProvider : ILoggerProvider {
    public ILogger CreateLogger(string category) => new FileLogger("app.log");
    public void Dispose() {}
}

builder.Logging.AddProvider(new FileLoggerProvider());
```

In practice, use **Serilog** or **NLog** — they are battle-tested, configurable, and support structured logging.

---

## 🔷 Section 5: Entity Framework Core

---

**Q31. Database First vs Code First in EF Core?**

| | Code First | Database First |
|--|--|--|
| Start from | C# model classes | Existing database |
| DB created by | EF Migrations | Already exists |
| Control | Full | Limited |
| Best for | New projects | Legacy DBs |

```bash
# Code First — create migration from code
dotnet ef migrations add InitialCreate
dotnet ef database update

# Database First — scaffold from existing DB
dotnet ef dbcontext scaffold "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer
```

---

**Q32. What is scaffolding in EF Core?**

Scaffolding **reverse-engineers** an existing database into C# model classes and a `DbContext` automatically.

```bash
dotnet ef dbcontext scaffold \
  "Server=.;Database=Shop;Trusted_Connection=True;" \
  Microsoft.EntityFrameworkCore.SqlServer \
  --output-dir Models \
  --context ShopDbContext \
  --data-annotations
```

This generates `Product.cs`, `Order.cs`, `ShopDbContext.cs` from your DB tables.

---

**Q33. `AsTracking` vs `AsNoTracking` in EF Core?**

| | `AsTracking` (default) | `AsNoTracking` |
|--|--|--|
| Change detection | Yes — EF watches for changes | No |
| Performance | Slower | Faster |
| Use for | Update/delete operations | Read-only queries |

```csharp
var product = await db.Products.FirstAsync(p => p.Id == 1);
product.Price = 99;
await db.SaveChangesAsync(); // UPDATE runs

var products = await db.Products
    .AsNoTracking()
    .Where(p => p.Price > 10)
    .ToListAsync();
```

---

**Q34. Eager Loading vs Lazy Loading vs Explicit Loading in EF Core?**

| | When data loads | How |
|--|--|--|
| **Eager** | With the main query | `.Include()` |
| **Lazy** | When you access the property | Auto (requires proxies) |
| **Explicit** | When you call `.Load()` | Manual |

```csharp
// Eager
var order = await db.Orders
    .Include(o => o.Customer)
    .Include(o => o.Items)
        .ThenInclude(i => i.Product)
    .FirstAsync(o => o.Id == 1);

// Lazy
var order = await db.Orders.FindAsync(1);
Console.WriteLine(order.Customer.Name); // query fires here

// Explicit
var order = await db.Orders.FindAsync(1);
await db.Entry(order).Reference(o => o.Customer).LoadAsync();
await db.Entry(order).Collection(o => o.Items).LoadAsync();
```

---

**Q35. What is the N+1 problem in EF Core? How to prevent it?**

When you loop over a collection and trigger a separate query **for each item** — 1 query + N queries = N+1.

```csharp
// ❌ N+1
var orders = await db.Orders.ToListAsync();
foreach (var o in orders)
    Console.WriteLine(o.Customer.Name); // fires a query each time!

// ✅ Fix — use Include (single JOIN query)
var orders = await db.Orders.Include(o => o.Customer).ToListAsync();

// ✅ Or use projection
var results = await db.Orders
    .Select(o => new { o.Id, CustomerName = o.Customer.Name })
    .ToListAsync();
```

---

**Q36. What is `Include` vs `ThenInclude` in EF Core?**

- `Include` — loads a **direct navigation** property.
- `ThenInclude` — loads a **nested navigation** (child of included).

```csharp
var orders = await db.Orders
    .Include(o => o.Customer)
        .ThenInclude(c => c.Address)
    .Include(o => o.Items)
        .ThenInclude(i => i.Product)
    .ToListAsync();
```

---

**Q37. What is Change Tracking in EF Core? Entity states?**

EF Core tracks entities loaded from the DB and detects what changed before `SaveChanges`.

| State | Meaning |
|--|--|
| `Detached` | Not tracked by EF |
| `Unchanged` | Loaded, not modified |
| `Added` | New, will be INSERTed |
| `Modified` | Changed, will be UPDATEd |
| `Deleted` | Marked, will be DELETEd |

```csharp
var product = await db.Products.FindAsync(1); // Unchanged
product.Price = 50;                           // Modified
await db.SaveChangesAsync();                  // UPDATE runs

var state = db.Entry(product).State;

db.Entry(product).State = EntityState.Modified;
await db.SaveChangesAsync();
```

---

**Q38. `DbContext` vs `DbSet`?**

- `DbContext` — represents a **database session** (connection, transactions, change tracking).
- `DbSet<T>` — represents a **table** and lets you query/add/remove entities.

```csharp
public class AppDbContext : DbContext {
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder mb) {
        mb.Entity<Product>().HasKey(p => p.Id);
        mb.Entity<Order>().HasOne(o => o.Customer).WithMany(c => c.Orders);
    }
}
```

---

**Q39. Fluent API vs Data Annotations in EF Core?**

| | Data Annotations | Fluent API |
|--|--|--|
| Where | On the model class | In `OnModelCreating` |
| Power | Limited | Full control |
| Separation | Mixed into model | Separate from model |

```csharp
public class Product {
    [Key] public int Id { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Price { get; set; }
}

modelBuilder.Entity<Product>(e => {
    e.HasKey(p => p.Id);
    e.Property(p => p.Name).IsRequired().HasMaxLength(100);
    e.Property(p => p.Price).HasColumnType("decimal(18,2)");
    e.HasIndex(p => p.Name).IsUnique();
});
```

---

**Q40. How to handle `DbUpdateConcurrencyException`?**

Thrown when two users try to update the same record simultaneously. Handle with a **concurrency token**.

```csharp
public class Product {
    public int Id { get; set; }
    public decimal Price { get; set; }
    [Timestamp] public byte[] RowVersion { get; set; }
}

try {
    await db.SaveChangesAsync();
} catch (DbUpdateConcurrencyException ex) {
    var entry = ex.Entries.Single();
    var dbValues = await entry.GetDatabaseValuesAsync();

    if (dbValues == null) {
        Console.WriteLine("Record no longer exists.");
    } else {
        entry.OriginalValues.SetValues(dbValues);
        await db.SaveChangesAsync();
    }
}
```

---

**Q41. How to insert a collection of data efficiently in EF Core?**

```csharp
var products = new List<Product> {
    new() { Name = "A", Price = 10 },
    new() { Name = "B", Price = 20 },
};
await db.Products.AddRangeAsync(products);
await db.SaveChangesAsync();

// For bulk operations (thousands of rows), use EF Core Bulk Extensions
// dotnet add package EFCore.BulkExtensions
await db.BulkInsertAsync(products);
await db.BulkUpdateAsync(products);
```

---

**Q42. How to set a foreign key and call a stored procedure in EF Core?**

```csharp
modelBuilder.Entity<Order>()
    .HasOne(o => o.Customer)
    .WithMany(c => c.Orders)
    .HasForeignKey(o => o.CustomerId)
    .OnDelete(DeleteBehavior.Cascade);

var orders = await db.Orders
    .FromSqlRaw("EXEC GetOrdersByCustomer @CustomerId = {0}", customerId)
    .ToListAsync();

var param = new SqlParameter("@CustomerId", customerId);
var result = await db.Database
    .ExecuteSqlRawAsync("EXEC ArchiveOldOrders @CustomerId", param);
```

---

**Q43. What are DataTables in ADO.NET?**

`DataTable` is an in-memory table — part of ADO.NET's disconnected model. Useful for legacy code or working with raw data before EF was common.

```csharp
using var conn = new SqlConnection(connectionString);
using var adapter = new SqlDataAdapter("SELECT * FROM Products", conn);

var table = new DataTable();
adapter.Fill(table);

foreach (DataRow row in table.Rows) {
    Console.WriteLine(row["Name"]);
}
```

EF Core is preferred today. Use `DataTable` for legacy integrations, SSRS reports, or dynamic column scenarios.

---

## 🔷 Section 6: Testing

---

**Q44. Unit Testing vs Integration Testing vs End-to-End Testing?**

| | Unit | Integration | End-to-End (E2E) |
|--|--|--|--|
| Tests | Single class/method | Multiple components together | Full app flow |
| Speed | Very fast | Medium | Slow |
| Dependencies | Mocked | Real (DB, services) | Real everything |
| Tools | xUnit, Moq | xUnit + TestServer | Playwright, Selenium |

```
Unit:         OrderService.CalculateTotal() works correctly
Integration:  POST /orders creates a record in the test DB
E2E:          User opens browser, places order, sees confirmation page
```

---

**Q45. What is the AAA pattern in testing?**

**Arrange, Act, Assert** — standard structure for a unit test.

```csharp
[Fact]
public void CalculateTotal_WithDiscount_ReturnsDiscountedPrice() {
    // Arrange
    var service = new OrderService();
    var items = new List<OrderItem> { new() { Price = 100, Qty = 2 } };

    // Act
    var total = service.CalculateTotal(items, discountPercent: 10);

    // Assert
    Assert.Equal(180, total);
}
```

---

**Q46. `Fact` vs `Theory` vs `InlineData` vs `MemberData` in xUnit?**

| Attribute | Use |
|--|--|
| `[Fact]` | A single test, no parameters |
| `[Theory]` | Parameterized test — runs multiple times |
| `[InlineData(...)]` | Inline parameter values for Theory |
| `[MemberData(...)]` | Parameters from a method or property |

```csharp
[Fact]
public void Add_TwoPositives_ReturnsSum() => Assert.Equal(5, Calc.Add(2, 3));

[Theory]
[InlineData(2, 3, 5)]
[InlineData(-1, 1, 0)]
[InlineData(0, 0, 0)]
public void Add_ReturnsCorrectSum(int a, int b, int expected)
    => Assert.Equal(expected, Calc.Add(a, b));

public static IEnumerable<object[]> OrderTestData => new[] {
    new object[] { new Order { Total = 100 }, 10m }
};

[Theory, MemberData(nameof(OrderTestData))]
public void CalculateDiscount_ReturnsExpected(Order order, decimal expected) { }
```

---

**Q47. What is mocking and stubbing in unit testing?**

- **Stub:** Returns fake data — you control what a dependency returns.
- **Mock:** Also verifies that a method was **called** a certain way.

```csharp
var mockRepo = new Mock<IOrderRepository>();

// Stub
mockRepo.Setup(r => r.GetByIdAsync(1))
        .ReturnsAsync(new Order { Id = 1, Total = 99 });

var service = new OrderService(mockRepo.Object);
var order = await service.GetOrderAsync(1);
Assert.Equal(99, order.Total);

// Mock — verify it was called
mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
```

---

**Q48. Why do we mock objects in unit tests?**

- Isolates the **unit under test** from real dependencies (DB, HTTP, email).
- Makes tests **fast** — no real I/O.
- Lets you test **edge cases** easily (e.g., simulate DB throwing an exception).
- Tests don't depend on external systems being available.

```csharp
var mockDb = new Mock<IOrderRepository>();
mockDb.Setup(r => r.GetByIdAsync(99)).ThrowsAsync(new NotFoundException());

var service = new OrderService(mockDb.Object);
await Assert.ThrowsAsync<NotFoundException>(() => service.GetAsync(99));
```

---

**Q49. How to check code coverage in .NET?**

```bash
dotnet test --collect:"XPlat Code Coverage"

dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```

Also integrated in **Visual Studio** (Test → Analyze Code Coverage) and **SonarQube**.

---

**Q50. What is SonarQube vs SonarLint?**

| | SonarLint | SonarQube |
|--|--|--|
| Where runs | Your IDE (VS, VS Code) | CI/CD server |
| When | As you type | On every build/PR |
| Scope | Local, developer | Team-wide, enforced |
| Tracks history | No | Yes — trends, gates |

- **SonarLint:** Install as a VS extension — shows code smells, bugs, security issues as you write code.
- **SonarQube:** Run in your pipeline — fails the build if quality gates aren't met (e.g., coverage < 80%, critical bugs > 0).

---

## 🔷 Section 7: Web API Fundamentals

---

**Q51. What is REST and what are its core principles?**

REST (Representational State Transfer) is an architectural style for building APIs over HTTP. The core principles are:

- **Stateless** — Every request must contain all information needed to process it. The server stores no session state between calls.
- **Client-Server** — The UI and data storage are separated, so they can evolve independently.
- **Uniform Interface** — Resources are identified by URIs, manipulated via representations (JSON/XML), and responses are self-descriptive.
- **Cacheable** — Responses should declare whether they can be cached to improve performance.
- **Layered System** — A client doesn't know if it's talking directly to the server or a load balancer/proxy in between.
- **Code on Demand** (optional) — Servers can send executable code to clients (e.g., JavaScript).

---

**Q52. What is the difference between REST and SOAP?**

| Feature | REST | SOAP |
|---|---|---|
| Protocol | HTTP only | HTTP, SMTP, TCP |
| Format | JSON, XML, plain text | XML only |
| Speed | Faster, lightweight | Slower, heavy |
| Standards | Informal | WS-* standards |
| State | Stateless | Can be stateful |
| Error handling | HTTP status codes | Built-in fault element |

REST is preferred for public APIs and web/mobile apps. SOAP is used in enterprise systems where strict security (WS-Security) and reliability (WS-ReliableMessaging) are needed.

---

**Q53. What are HTTP verbs and when do you use each?**

- `GET` — Read a resource. Safe and idempotent. No request body.
- `POST` — Create a resource. Not idempotent (calling it twice creates two records).
- `PUT` — Replace a resource entirely. Idempotent.
- `PATCH` — Partially update a resource. Idempotent if designed properly.
- `DELETE` — Remove a resource. Idempotent.
- `HEAD` — Same as GET but returns only headers, no body. Useful for checking if a resource exists.
- `OPTIONS` — Returns the allowed HTTP methods on a resource. Used in CORS preflight.

---

**Q54. What are common HTTP status codes?**

| Code | Meaning |
|---|---|
| 200 | OK — request succeeded |
| 201 | Created — resource was created |
| 204 | No Content — success but no body (common for DELETE) |
| 400 | Bad Request — invalid input |
| 401 | Unauthorized — not authenticated |
| 403 | Forbidden — authenticated but not allowed |
| 404 | Not Found |
| 409 | Conflict — duplicate or state conflict |
| 422 | Unprocessable Entity — validation failed |
| 429 | Too Many Requests — rate limit hit |
| 500 | Internal Server Error |
| 503 | Service Unavailable |

---

**Q55. What is content negotiation in Web API?**

Content negotiation is the mechanism by which a client and server agree on the format of data exchanged. The client sends an `Accept` header specifying what it wants (e.g., `application/json` or `application/xml`), and the server responds in that format if it supports it. In ASP.NET Core, this is handled automatically by output formatters.

```csharp
builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
```

If the server can't match the requested format, it returns `406 Not Acceptable`.

---

**Q56. What is CORS and how do you enable it in Web API?**

CORS (Cross-Origin Resource Sharing) is a browser security policy that blocks JavaScript from making requests to a different domain than the one that served the page. You enable it on the server to explicitly allow specific origins.

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("https://myfrontend.com")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

app.UseCors("AllowFrontend");
```

For development, you can use `.AllowAnyOrigin()` but never in production.

---

**Q57. How do you handle errors in Web API?**

There are four main approaches, used together in a real app:

1. **try-catch in controller** — for specific known errors.
2. **Exception filters** — `IExceptionFilter` or `[ExceptionFilter]` attribute for reusable logic.
3. **Global middleware** — the recommended approach in .NET 6+.
4. **ProblemDetails** — standard RFC 7807 error response format.

```csharp
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<IExceptionHandlerFeature>();
        await context.Response.WriteAsJsonAsync(new
        {
            StatusCode = 500,
            Message = "An unexpected error occurred.",
            Detail = error?.Error.Message
        });
    });
});
```

---

**Q58. How does session and state management work in ASP.NET Core?**

ASP.NET Core is stateless by design. Options for managing state:

- **Cookies** — small pieces of data stored on the client browser.
- **Session** — server-side storage per user, identified by a session cookie. Needs distributed cache in multi-server setups.
- **Distributed Cache** — Redis or SQL Server to share state across servers.
- **Hidden fields / query strings** — for simple UI state.
- **JWT tokens** — carry claims/state on the client side.

```csharp
builder.Services.AddSession();
app.UseSession();

HttpContext.Session.SetString("UserId", userId);
var id = HttpContext.Session.GetString("UserId");
```

---

**Q59. What is API versioning and how do you implement it?**

API versioning lets you evolve your API without breaking existing clients. Common strategies:

- **URL segment** — `/api/v1/orders`
- **Query string** — `/api/orders?api-version=1.0`
- **Header** — `X-API-Version: 1.0`

```csharp
// Install: Microsoft.AspNetCore.Mvc.Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
public class OrdersV1Controller : ControllerBase { }

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/orders")]
public class OrdersV2Controller : ControllerBase { }
```

---

**Q60. What is rate limiting and how do you implement it?**

Rate limiting controls how many requests a client can make in a time window to prevent abuse and overload. .NET 7+ has built-in rate limiting middleware.

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 100;
        opt.QueueLimit = 0;
    });
});

app.UseRateLimiter();

[EnableRateLimiting("fixed")]
[HttpGet]
public IActionResult Get() => Ok();
```

---

**Q61. How do you make a REST API more secure?**

- Use HTTPS everywhere (no HTTP).
- Authenticate with JWT or OAuth2.
- Validate all inputs (FluentValidation or Data Annotations).
- Use CORS to whitelist trusted origins.
- Enable rate limiting to prevent brute force.
- Use parameterized queries or an ORM to prevent SQL injection.
- Apply the principle of least privilege on DB accounts.
- Never expose stack traces in production errors.
- Use security headers (`X-Content-Type-Options`, `X-Frame-Options`, `Content-Security-Policy`).
- Rotate secrets and store them in Azure Key Vault or environment variables, never in code.

---

**Q62. What are WebSockets and when would you use them over REST?**

WebSockets provide a persistent, bidirectional connection between client and server, unlike HTTP which is request-response. You use WebSockets when you need real-time, low-latency two-way communication:

- Live chat
- Multiplayer games
- Real-time dashboards
- Collaborative editing (like Google Docs)

REST makes a new connection per request; WebSockets keep the connection open. In ASP.NET Core, SignalR is the high-level abstraction over WebSockets.

```csharp
app.UseWebSockets();
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var ws = await context.WebSockets.AcceptWebSocketAsync();
        // handle communication
    }
    else await next();
});
```

---

**Q63. What is gRPC and how does it compare to REST?**

gRPC is a high-performance RPC framework by Google that uses HTTP/2 and Protocol Buffers (protobuf) for serialization. It's significantly faster than REST+JSON for service-to-service communication.

| Feature | REST | gRPC |
|---|---|---|
| Protocol | HTTP/1.1 | HTTP/2 |
| Format | JSON (text) | Protobuf (binary) |
| Speed | Moderate | Very fast |
| Browser support | Full | Limited |
| Streaming | No (HTTP/2 SSE) | Full bidirectional |
| Contract | OpenAPI (optional) | .proto file (required) |

gRPC is ideal for internal microservice communication. REST is better for public APIs and browser clients.

---

**Q64. What is the difference between synchronous and asynchronous controllers and when should you use async?**

A synchronous controller blocks a thread while waiting for I/O (DB query, HTTP call, file read). Under high load, this exhausts the thread pool.

An async controller releases the thread back to the pool while waiting for I/O, allowing the same thread to handle other requests. This dramatically improves throughput under high load.

**Always use async for any I/O-bound operation.** Use sync only for CPU-bound work that is very fast.

```csharp
// BAD - blocks thread
[HttpGet("{id}")]
public IActionResult Get(int id) => Ok(_db.Products.Find(id));

// GOOD - async I/O
[HttpGet("{id}")]
public async Task<IActionResult> Get(int id)
{
    var product = await _db.Products.FindAsync(id);
    return product is null ? NotFound() : Ok(product);
}
```

---

## 🔷 Section 8: Authentication & Authorization

---

**Q65. What are the types of authentication in ASP.NET Core?**

- **Cookie authentication** — stores session in a cookie; used in MVC web apps.
- **JWT (JSON Web Token)** — stateless token-based auth; standard for APIs.
- **OAuth2** — authorization framework that lets users grant access to third-party apps.
- **OpenID Connect (OIDC)** — identity layer on top of OAuth2; handles authentication (who are you?).
- **API Keys** — simple, static keys sent in headers; used for server-to-server calls.
- **Windows Authentication** — for internal enterprise apps on Windows.
- **ASP.NET Core Identity** — full user management system (registration, login, roles, claims).

---

**Q66. How do you create and validate a JWT token?**

A JWT has three parts: Header (algorithm), Payload (claims), Signature. The server creates the token and the client sends it in the `Authorization: Bearer <token>` header on every request.

```csharp
// Generate JWT
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key-here"));
var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

var token = new JwtSecurityToken(
    issuer: "yourapp",
    audience: "yourapp",
    claims: new[] { new Claim(ClaimTypes.Name, "john@example.com") },
    expires: DateTime.UtcNow.AddHours(1),
    signingCredentials: creds
);

var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
```

```csharp
// Validate JWT in Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourapp",
            ValidAudience = "yourapp",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("your-secret-key-here"))
        };
    });
```

**Benefits of JWT:** Stateless (no server-side session), works across microservices, self-contained claims, easy to scale.

---

**Q67. What is the difference between OAuth2 and OpenID Connect?**

OAuth2 is an **authorization** framework — it answers "what can this app do on your behalf?". It doesn't tell you who the user is.

OpenID Connect (OIDC) is built on top of OAuth2 and adds **authentication** — it answers "who is this user?". OIDC provides an `id_token` (JWT) with user identity claims like name, email, sub (subject ID).

In short: OAuth2 = access to resources. OIDC = identity of user.

---

**Q68. What are the ways to implement authorization in ASP.NET Core?**

- **Role-based** — `[Authorize(Roles = "Admin")]`
- **Claims-based** — authorize based on specific claim values.
- **Policy-based** — define named policies with complex requirements.
- **Resource-based** — check if a user can access a specific resource instance (e.g., "can this user edit this post?").
- **Attribute-based** — custom attributes that implement `IAuthorizationFilter`.

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SeniorEmployee", policy =>
        policy.RequireClaim("EmploymentYears", "5", "6", "7", "8"));
});

[Authorize(Policy = "SeniorEmployee")]
public IActionResult GetSensitiveReport() => Ok();
```

---

**Q69. How do you implement refresh tokens with JWT?**

Access tokens are short-lived (15 min–1 hour). When they expire, the client uses a refresh token (long-lived, stored securely) to get a new access token without logging in again.

Flow:
1. Login → server returns access token + refresh token.
2. Access token expires → client calls `/auth/refresh` with refresh token.
3. Server validates refresh token from DB, issues new access token.
4. Refresh token rotation: issue a new refresh token and invalidate the old one.

Store refresh tokens in a DB, not JWT, so you can revoke them.

---

## 🔷 Section 9: Caching

---

**Q70. What are the caching strategies in Web API?**

- **In-memory caching** — cache in the server process memory using `IMemoryCache`. Fast but not shared across multiple servers.
- **Distributed caching** — cache in Redis or SQL Server using `IDistributedCache`. Works across multiple server instances.
- **Response caching** — cache the entire HTTP response at the HTTP layer using `[ResponseCache]` attribute or middleware.
- **Cache-aside** — application checks cache first; on miss, loads from DB and populates cache.
- **Write-through** — write to cache and DB at the same time.
- **CDN caching** — cache static assets at edge servers closer to users.

```csharp
public class ProductService
{
    private readonly IMemoryCache _cache;
    public ProductService(IMemoryCache cache) => _cache = cache;

    public async Task<Product> GetProduct(int id)
    {
        return await _cache.GetOrCreateAsync($"product_{id}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _db.Products.FindAsync(id);
        });
    }
}
```

---

**Q71. What is the difference between IMemoryCache and IDistributedCache?**

`IMemoryCache` stores data in the application process memory. It's fast but is local to one server — if you have multiple instances, each has its own cache. It's lost on app restart.

`IDistributedCache` stores data in an external store like Redis. It's shared across all server instances, survives restarts, and supports serialization. It's slightly slower due to network overhead but is the right choice for production multi-instance deployments.

---

**Q72. How would you implement multi-level caching?**

Multi-level caching uses fast local cache first, then a shared distributed cache, then the DB.

```
Request → L1 (IMemoryCache, ~ms) → L2 (Redis, ~5ms) → DB (~50ms)
```

```csharp
public async Task<Product> GetProduct(int id)
{
    // L1: in-memory
    if (_memoryCache.TryGetValue($"product_{id}", out Product? cached))
        return cached!;

    // L2: Redis
    var redisValue = await _distributedCache.GetStringAsync($"product_{id}");
    if (redisValue != null)
    {
        var product = JsonSerializer.Deserialize<Product>(redisValue)!;
        _memoryCache.Set($"product_{id}", product, TimeSpan.FromMinutes(1));
        return product;
    }

    // DB
    var fromDb = await _db.Products.FindAsync(id);
    await _distributedCache.SetStringAsync($"product_{id}",
        JsonSerializer.Serialize(fromDb), new DistributedCacheEntryOptions
        { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
    _memoryCache.Set($"product_{id}", fromDb, TimeSpan.FromMinutes(1));
    return fromDb!;
}
```

---

## 🔷 Section 10: Performance & Scalability

---

**Q73. How do you build a high-traffic Web API?**

- **Load balancing** — distribute requests across multiple server instances (Nginx, Azure Load Balancer).
- **Horizontal scaling** — add more server instances instead of bigger servers.
- **Caching** — Redis for frequently read data to reduce DB load.
- **Async/await** — use async controllers to avoid blocking threads.
- **Database optimization** — add indexes, use read replicas, avoid N+1 queries.
- **Connection pooling** — reuse DB connections.
- **Message queues** — offload heavy tasks to background workers via RabbitMQ/Azure Service Bus.
- **CDN** — cache static assets at edge.
- **Compression** — enable gzip/brotli response compression.
- **Pagination** — never return unbounded lists.

---

**Q74. What is the difference between horizontal and vertical scaling?**

**Vertical scaling** means adding more CPU/RAM to a single server. It has a ceiling and creates a single point of failure.

**Horizontal scaling** means adding more server instances and distributing load. It's more resilient, can scale nearly infinitely, and is the cloud-native approach. It requires the app to be stateless (no local session state).

---

**Q75. What are message queues and when should you use them? (RabbitMQ, Kafka, Azure Service Bus)**

A message queue decouples the component that produces work from the component that processes it. Instead of doing work synchronously in the API response cycle, you push a message to a queue and return immediately, then a background worker processes it.

Use when:
- The task takes too long for a synchronous response (email, PDF generation, video processing).
- You need guaranteed delivery.
- You need to smooth out traffic spikes (buffer requests).

**RabbitMQ** — traditional message broker, supports complex routing. Good for task queues within a single org.

**Kafka** — event streaming platform. High throughput, ordered events, replay support. Good for event sourcing and real-time pipelines.

**Azure Service Bus** — managed cloud queue with topics, sessions, dead-lettering. Best choice for Azure-hosted apps.

---

**Q76. How do you implement retry policies in Web API?**

Use **Polly**, the .NET resilience library, to automatically retry failed HTTP calls with exponential backoff.

```csharp
// Install: Microsoft.Extensions.Http.Polly
builder.Services.AddHttpClient<IPaymentService, PaymentService>()
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
```

This retries up to 3 times with delays of 2, 4, and 8 seconds. Polly also supports circuit breakers, timeouts, and fallbacks.

---

**Q77. What is an API Gateway and Azure API Management?**

An **API Gateway** is a single entry point for all clients. It handles cross-cutting concerns like authentication, rate limiting, routing, logging, and SSL termination, so individual microservices don't have to.

**Azure API Management (APIM)** is Microsoft's managed API gateway. Features include:
- Developer portal with API documentation.
- Policies for request/response transformation.
- Rate limiting and quotas per subscription.
- Caching.
- JWT validation without touching service code.
- Analytics and monitoring.

---

## 🔷 Section 11: System Design

---

**Q78. Design a notification system for email sending.**

**Architecture:**

1. API receives a request to send an email.
2. API validates input and writes a message to a queue (Azure Service Bus or RabbitMQ).
3. API returns `202 Accepted` immediately.
4. A background worker (Azure Function or Worker Service) reads from the queue, calls the email provider (SendGrid, SMTP).
5. Worker updates a `Notifications` table with the delivery status.
6. Implement retry with dead-letter queue for failed deliveries.

```
[Client] → [Web API] → [Service Bus Queue] → [Email Worker] → [SendGrid]
                                                     ↓
                                          [Notifications DB]
```

**Key design decisions:**
- Use idempotency keys to avoid sending the same email twice on retry.
- Store the email job in DB before enqueuing so no messages are lost.
- Use dead-letter queue for permanently failed messages and alert on-call.

---

**Q79. Design a real-time collaboration tool like Google Docs.**

**Architecture:**

1. Clients connect via **SignalR** (WebSockets under the hood).
2. Each keystroke generates an operation — use **Operational Transformation (OT)** or **CRDTs** to handle concurrent edits without conflicts.
3. Operations are broadcast to all connected clients in the same document room.
4. Periodically snapshot the full document state to a database.
5. Use **Redis** as the SignalR backplane so it works across multiple server instances.
6. Version each operation with a monotonic sequence number so clients can replay missed operations on reconnect.

```
[Client A] ←→ [SignalR Hub] ←→ [Client B]
                   ↓
              [Redis Pub/Sub]
                   ↓
          [Document Store (Cosmos DB)]
```

---

**Q80. How do you ensure data consistency in a distributed system?**

- **Eventual consistency** — accept that data will be consistent "eventually" after all nodes sync. Acceptable for non-critical data.
- **Distributed transactions (Saga pattern)** — coordinate across services using a series of local transactions and compensating transactions on failure.
- **Idempotency** — design operations so calling them multiple times has the same effect as once. Use idempotency keys on write operations.
- **Optimistic concurrency** — use a `RowVersion` or ETag. Reject updates if the version has changed since the client read it.
- **Outbox pattern** — write to DB and outbox table in one transaction, then publish events from the outbox to ensure no lost events.

---

**Q81. How would you migrate a database in production safely?**

1. Use **EF Core migrations** or **Flyway** to version-control schema changes.
2. Test migrations on a staging environment that mirrors production.
3. Apply **backward-compatible migrations** first (add column before removing old one).
4. For large tables, use online migrations to avoid table locks.
5. Always take a **backup before running migrations**.
6. Have a **rollback script** ready — either a Down migration or a backup restore plan.
7. Use **blue-green deployments** — deploy new version alongside old, migrate DB, then switch traffic.

```bash
dotnet ef database update --connection "ProductionConnectionString"
dotnet ef migrations list
```

---

**Q82. What is the Outbox Pattern?**

The Outbox pattern solves the problem of "write to DB and publish event atomically". Without it, if the service writes to DB then crashes before publishing, the event is lost.

Solution: write the domain record AND the outbound event to the same DB in a single transaction. A separate background poller reads from the outbox table and publishes events to the message bus, then marks them as sent.

```
[Service] → writes [Order] + [OutboxEvent] in one DB transaction
[Outbox Publisher] → polls OutboxEvent table → publishes to queue → marks sent
```

---

**Q83. What is blue-green deployment?**

Blue-green deployment runs two identical production environments. "Blue" is live; "Green" is the new version. You deploy and test Green while Blue serves traffic. Once Green is verified, you switch the load balancer to point to Green. If something goes wrong, you switch back to Blue instantly. Zero downtime.

Azure equivalents: **Deployment Slots** in Azure App Service.

---

## 🔷 Section 12: Azure Function Apps

---

**Q84. What is an Azure Function App?**

An Azure Function App is a serverless compute service that runs small pieces of code (functions) in response to triggers without managing infrastructure. You pay only for execution time. It's ideal for event-driven, background processing, and integration tasks.

---

**Q85. What are the different trigger types in Azure Functions?**

- **HTTP Trigger** — invoked by an HTTP request. Used to build serverless APIs.
- **Timer Trigger** — runs on a CRON schedule (e.g., every hour).
- **Blob Trigger** — fires when a file is added or updated in Azure Blob Storage.
- **Queue Trigger** — processes messages from Azure Storage Queue.
- **Service Bus Trigger** — processes messages from Azure Service Bus.
- **Event Hub Trigger** — processes events from Azure Event Hubs (high-throughput streaming).
- **Cosmos DB Trigger** — fires on document changes via change feed.
- **Event Grid Trigger** — reacts to events from Azure Event Grid.

---

**Q86. What is the difference between Azure Functions Consumption Plan, Premium Plan, and Dedicated Plan?**

- **Consumption Plan** — scales to zero, pay per execution. Cold start latency on first invocation. Best for intermittent workloads.
- **Premium Plan** — pre-warmed instances (no cold start), VNet integration, more powerful instances. Best for latency-sensitive production workloads.
- **Dedicated (App Service) Plan** — runs on always-on VMs you already pay for. Best if you have an existing App Service plan or need long-running functions.

---

**Q87. How do you write a Timer Trigger function in .NET?**

```csharp
public class DailyReportFunction
{
    [FunctionName("DailyReport")]
    public void Run(
        [TimerTrigger("0 0 8 * * *")] TimerInfo timer, // runs at 8 AM daily
        ILogger log)
    {
        log.LogInformation($"Daily report triggered at: {DateTime.UtcNow}");
        // generate and send report
    }
}
```

The CRON expression format in Azure Functions is: `{second} {minute} {hour} {day} {month} {day-of-week}`.

---

**Q88. How do you write an HTTP Trigger function?**

```csharp
public class GetProductFunction
{
    [FunctionName("GetProduct")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")]
        HttpRequest req,
        int id,
        ILogger log)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return new NotFoundResult();
        return new OkObjectResult(product);
    }
}
```

`AuthorizationLevel.Function` requires a function key. Use `AuthorizationLevel.Anonymous` for public endpoints.

---

**Q89. What is Durable Functions?**

Durable Functions is an extension of Azure Functions that allows writing stateful workflows as code. It manages state, checkpointing, and retries automatically. Useful for:

- Long-running workflows (fan-out/fan-in, chaining).
- Human interaction workflows (waiting for approval).
- Aggregating events over time.

```csharp
[FunctionName("OrderWorkflow")]
public static async Task RunOrchestrator(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    await context.CallActivityAsync("ValidateOrder", context.GetInput<Order>());
    await context.CallActivityAsync("ChargePayment", context.GetInput<Order>());
    await context.CallActivityAsync("SendConfirmation", context.GetInput<Order>());
}
```

---

**Q90. How do you inject dependencies into Azure Functions?**

Use `Microsoft.Azure.Functions.Extensions` to add a `Startup` class that registers services with the DI container.

```csharp
[assembly: FunctionsStartup(typeof(MyApp.Startup))]
namespace MyApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnection")));
        }
    }
}
```

---

**Q91. How do you handle errors and retries in Azure Functions?**

- For queue/Service Bus triggers, Azure Functions automatically retries on failure. Configure max retry count in `host.json`.
- Messages that fail all retries go to the **dead-letter queue**.
- Use try-catch inside the function to handle known errors gracefully.
- Use Application Insights for monitoring and alerts.

```json
// host.json
{
  "version": "2.0",
  "extensions": {
    "serviceBus": {
      "maxConcurrentCalls": 16,
      "prefetchCount": 100
    }
  },
  "retry": {
    "strategy": "exponentialBackoff",
    "maxRetryCount": 5,
    "minimumInterval": "00:00:10",
    "maximumInterval": "00:00:300"
  }
}
```

---

**Q92. What is the difference between Azure Storage Queue and Azure Service Bus?**

| Feature | Storage Queue | Service Bus |
|---|---|---|
| Max message size | 64 KB | 256 KB (Standard), 100 MB (Premium) |
| Message ordering | FIFO best-effort | FIFO guaranteed with sessions |
| Dead-letter queue | No | Yes |
| Topics/Subscriptions | No | Yes |
| Transactions | No | Yes |
| At-least-once delivery | Yes | Yes |
| Price | Very cheap | More expensive |

Use Storage Queue for simple, cheap, high-volume scenarios. Use Service Bus for enterprise messaging with ordering, dead-lettering, and pub/sub.

---

## 🔷 Section 13: Scenarios, Advanced & Troubleshooting

---

**Q93. There is a memory leak causing daily restarts. How do you troubleshoot?**

1. Take a **heap snapshot** using dotMemory, PerfView, or `dotnet-dump` before the app crashes.
2. Analyze the snapshot — look for objects that should have been GC'd but are still alive.
3. Common culprits:
   - **Event handlers not unsubscribed** — subscribing to static events holds references.
   - **Static collections** — growing unboundedly.
   - **HttpClient created per request** — each one holds a socket connection.
   - **IDisposable not disposed** — DB connections, streams, etc.
4. Use `dotnet-trace` to watch GC collections over time.
5. After finding the leak, add a unit test that verifies the object is collected.

---

**Q94. You need to integrate with a third-party payment API that sometimes goes down. How do you handle this?**

- Wrap all calls in a **Polly circuit breaker** — stop calling a failing service for a period to let it recover.
- Use **retry with exponential backoff** for transient failures.
- Implement a **fallback** response when the circuit is open (e.g., put order in a pending state).
- Store payment intents in the DB before calling the API so you can resume if the app crashes.
- Monitor for circuit breaker open events and alert the team.

```csharp
services.AddHttpClient<IPaymentService, PaymentService>()
    .AddTransientHttpErrorPolicy(p => p.RetryAsync(3))
    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

---

**Q95. How do you implement idempotency in a REST API?**

An idempotent API produces the same result when called multiple times with the same input. For POST endpoints (which aren't naturally idempotent), use an **idempotency key** sent by the client in a header.

```
POST /api/payments
Idempotency-Key: abc-123-xyz
```

On the server:
1. Check if `abc-123-xyz` exists in a cache/DB.
2. If yes, return the stored response.
3. If no, process the request, store the result with the key, return the result.

This ensures retries after network failures don't create duplicate records.

---

**Q96. How would you implement pagination in a Web API?**

Use offset/cursor-based pagination. Return metadata so clients know how to navigate.

```csharp
[HttpGet]
public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
{
    var total = await _db.Orders.CountAsync();
    var orders = await _db.Orders
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(new
    {
        Data = orders,
        Page = page,
        PageSize = pageSize,
        TotalCount = total,
        TotalPages = (int)Math.Ceiling(total / (double)pageSize)
    });
}
```

For large datasets, prefer **cursor-based pagination** (keyset pagination) over offset — it's more efficient as offsets require scanning all previous rows.

---

**Q97. How do you implement model validation in Web API?**

ASP.NET Core validates models automatically when you add `[ApiController]`. It returns `400 Bad Request` with error details if the model is invalid.

```csharp
public class CreateOrderRequest
{
    [Required]
    public string CustomerId { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Quantity { get; set; }

    [Required, EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;
}
```

For complex validation, use **FluentValidation**:

```csharp
public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Quantity).InclusiveBetween(1, 1000);
        RuleFor(x => x.ContactEmail).EmailAddress();
    }
}
```

---

**Q98. How do you implement a circuit breaker pattern?**

A circuit breaker stops calling a failing service to give it time to recover. It has three states: Closed (normal), Open (blocking calls), Half-Open (testing if recovery happened).

```csharp
services.AddHttpClient<IPaymentService, PaymentService>()
    .AddPolicyHandler(Policy
        .Handle<HttpRequestException>()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (ex, ts) => log.LogWarning("Circuit opened for {Duration}", ts),
            onReset: () => log.LogInformation("Circuit closed"),
            onHalfOpen: () => log.LogInformation("Circuit half-open")));
```

---

**Q99. How do you handle database connection resiliency in EF Core?**

Use EF Core's built-in retry-on-failure execution strategy for transient failures (like SQL Azure briefly unavailable).

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));
```

This automatically retries transient SQL errors (deadlocks, connection timeouts) with exponential backoff.

---

**Q100. Describe your approach to designing a microservices API from scratch.**

1. **Define bounded contexts** — each service owns its own data and domain logic.
2. **API Gateway** — single entry point handling auth, routing, and rate limiting.
3. **Service communication** — synchronous (REST/gRPC) for real-time needs; asynchronous (Service Bus) for decoupled workflows.
4. **Authentication** — centralize with an identity provider (Azure AD, IdentityServer). Services validate JWTs independently.
5. **Observability** — distributed tracing (OpenTelemetry), structured logging (Serilog → App Insights), health checks.
6. **Resilience** — Polly for retries and circuit breakers, idempotency on all message handlers.
7. **Data** — each service has its own database. No shared DB across services.
8. **CI/CD** — independent deployment pipelines per service. Use feature flags for safe rollouts.
9. **Documentation** — OpenAPI per service, aggregated via API Management.
10. **Security** — HTTPS everywhere, secrets in Key Vault, Managed Identity, minimal privilege.

---

