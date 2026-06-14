# .NET / .NET Core Interview Questions & Answers

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
// Custom middleware class
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

// Extension method for clean registration
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
// Custom action filter
public class LogActionFilter : IActionFilter {
    public void OnActionExecuting(ActionExecutingContext context)
        => Console.WriteLine($"Action starting: {context.ActionDescriptor.DisplayName}");

    public void OnActionExecuted(ActionExecutedContext context)
        => Console.WriteLine("Action finished");
}

// Register globally
builder.Services.AddControllers(options => {
    options.Filters.Add<LogActionFilter>();
});

// Or on a controller/action
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
// Strongly-typed config class
public class EmailOptions {
    public string SmtpHost { get; set; }
    public int Port { get; set; }
}

// Register
builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));

// Inject and use
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

// Factory
public class PaymentGatewayFactory {
    private readonly IServiceProvider _sp;
    public PaymentGatewayFactory(IServiceProvider sp) => _sp = sp;

    public IPaymentGateway Create(string type) => type switch {
        "stripe" => _sp.GetRequiredService<StripeGateway>(),
        "paypal" => _sp.GetRequiredService<PayPalGateway>(),
        _ => throw new ArgumentException("Unknown gateway")
    };
}

// Register
services.AddTransient<StripeGateway>();
services.AddTransient<PayPalGateway>();
services.AddSingleton<PaymentGatewayFactory>();
```

---

**Q17. What is the Decorator Pattern with DI?**

Wraps an existing service to add behavior (logging, caching, retry) **without modifying** the original.

```csharp
public interface IOrderService { Task<Order> GetOrderAsync(int id); }

// Real implementation
public class OrderService : IOrderService {
    public async Task<Order> GetOrderAsync(int id) => await db.Orders.FindAsync(id);
}

// Decorator — adds caching
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

// Register with Scrutor or manually
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

// Command
public record CreateOrderCommand(string Product, int Qty) : IRequest<int>;

// Handler
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int> {
    public async Task<int> Handle(CreateOrderCommand cmd, CancellationToken ct) {
        // create order logic
        return newOrderId;
    }
}

// Register
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Use in controller
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
// Command — write side
public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;

// Query — read side (different model, can use optimized SQL)
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

// Register
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
// ArrayPool — rent and return a byte array
var pool = ArrayPool<byte>.Shared;
byte[] buffer = pool.Rent(4096);
try { await stream.ReadAsync(buffer); }
finally { pool.Return(buffer); } // return to pool

// stackalloc — for tiny, short-lived buffers
Span<int> nums = stackalloc int[10];
nums[0] = 42;

// MemoryPool — for async-compatible slices
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

// Run
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
// Custom health check
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

// Register
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<ExternalApiHealthCheck>("external-api");

// Expose endpoint
app.MapHealthChecks("/health");
```

---

**Q30. How to implement a custom logging provider?**

```csharp
// Custom logger
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

// Provider
public class FileLoggerProvider : ILoggerProvider {
    public ILogger CreateLogger(string category) => new FileLogger("app.log");
    public void Dispose() {}
}

// Register
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
// Default — EF tracks the entity
var product = await db.Products.FirstAsync(p => p.Id == 1);
product.Price = 99; // EF detects this change
await db.SaveChangesAsync(); // UPDATE runs

// No tracking — faster, read-only
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
// Eager — one query with JOIN
var order = await db.Orders
    .Include(o => o.Customer)
    .Include(o => o.Items)
        .ThenInclude(i => i.Product)
    .FirstAsync(o => o.Id == 1);

// Lazy — loads Customer when first accessed (requires virtual + proxies)
var order = await db.Orders.FindAsync(1);
Console.WriteLine(order.Customer.Name); // query fires here

// Explicit — you control when to load
var order = await db.Orders.FindAsync(1);
await db.Entry(order).Reference(o => o.Customer).LoadAsync();
await db.Entry(order).Collection(o => o.Items).LoadAsync();
```

---

**Q35. What is the N+1 problem in EF Core? How to prevent it?**

When you loop over a collection and trigger a separate query **for each item** — 1 query + N queries = N+1.

```csharp
// ❌ N+1 — 1 query for orders + 1 per order for customer
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
    .Include(o => o.Customer)               // Order → Customer
        .ThenInclude(c => c.Address)        // Customer → Address
    .Include(o => o.Items)                  // Order → Items
        .ThenInclude(i => i.Product)        // Items → Product
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
product.Price = 50;                           // Modified (auto-detected)
await db.SaveChangesAsync();                  // UPDATE runs

// Check state manually
var state = db.Entry(product).State; // EntityState.Modified

// Manually set state (for disconnected scenarios)
db.Entry(product).State = EntityState.Modified;
await db.SaveChangesAsync();
```

---

**Q38. `DbContext` vs `DbSet`?**

- `DbContext` — represents a **database session** (connection, transactions, change tracking).
- `DbSet<T>` — represents a **table** and lets you query/add/remove entities.

```csharp
public class AppDbContext : DbContext {       // DbContext = the session
    public DbSet<Product> Products { get; set; } // DbSet = the Products table
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
// Data Annotations — on the model
public class Product {
    [Key] public int Id { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Price { get; set; }
}

// Fluent API — in OnModelCreating (preferred for complex rules)
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
// Add concurrency token to model
public class Product {
    public int Id { get; set; }
    public decimal Price { get; set; }
    [Timestamp] public byte[] RowVersion { get; set; } // SQL Server rowversion
}

// Handle the exception
try {
    await db.SaveChangesAsync();
} catch (DbUpdateConcurrencyException ex) {
    var entry = ex.Entries.Single();
    var dbValues = await entry.GetDatabaseValuesAsync();

    if (dbValues == null) {
        // Record was deleted by another user
        Console.WriteLine("Record no longer exists.");
    } else {
        // Refresh with DB values and retry, or show conflict to user
        entry.OriginalValues.SetValues(dbValues);
        await db.SaveChangesAsync(); // retry with fresh values
    }
}
```

---

**Q41. How to insert a collection of data efficiently in EF Core?**

```csharp
// AddRange — single roundtrip to DB
var products = new List<Product> {
    new() { Name = "A", Price = 10 },
    new() { Name = "B", Price = 20 },
};
await db.Products.AddRangeAsync(products);
await db.SaveChangesAsync();

// For bulk operations (thousands of rows), use EF Core Bulk Extensions
// dotnet add package EFCore.BulkExtensions
await db.BulkInsertAsync(products);     // much faster for large data
await db.BulkUpdateAsync(products);
```

---

**Q42. How to set a foreign key and call a stored procedure in EF Core?**

```csharp
// Foreign key via Fluent API
modelBuilder.Entity<Order>()
    .HasOne(o => o.Customer)
    .WithMany(c => c.Orders)
    .HasForeignKey(o => o.CustomerId)
    .OnDelete(DeleteBehavior.Cascade);

// Call a stored procedure
var orders = await db.Orders
    .FromSqlRaw("EXEC GetOrdersByCustomer @CustomerId = {0}", customerId)
    .ToListAsync();

// With parameters (safer)
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
adapter.Fill(table);  // fills table without keeping connection open

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
    // Arrange — set up inputs and dependencies
    var service = new OrderService();
    var items = new List<OrderItem> { new() { Price = 100, Qty = 2 } };

    // Act — call the method under test
    var total = service.CalculateTotal(items, discountPercent: 10);

    // Assert — verify the outcome
    Assert.Equal(180, total); // 200 - 10%
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

// MemberData for complex objects
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
// Using Moq
var mockRepo = new Mock<IOrderRepository>();

// Stub — control return value
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
// Without mocking: OrderService needs a real DB running
// With mocking: replace DB with a fake

var mockDb = new Mock<IOrderRepository>();
mockDb.Setup(r => r.GetByIdAsync(99)).ThrowsAsync(new NotFoundException());

var service = new OrderService(mockDb.Object);
await Assert.ThrowsAsync<NotFoundException>(() => service.GetAsync(99));
// ✅ No DB needed — tested error path cleanly
```

---

**Q49. How to check code coverage in .NET?**

```bash
# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report with ReportGenerator
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

# Open coverage-report/index.html in browser
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

They complement each other — SonarLint catches issues early, SonarQube enforces them at the team level.

---

*End of 50 .NET / .NET Core Interview Questions — 5 Years Experience*