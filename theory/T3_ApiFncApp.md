# .NET Web API Interview Q&A

> Covers Web API, Azure Function Apps, Scenario-Based, and System Design questions.

---

## Section 1: Web API Fundamentals

---

**Q1. What is REST and what are its core principles?**

REST (Representational State Transfer) is an architectural style for building APIs over HTTP. The core principles are:

- **Stateless** — Every request must contain all information needed to process it. The server stores no session state between calls.
- **Client-Server** — The UI and data storage are separated, so they can evolve independently.
- **Uniform Interface** — Resources are identified by URIs, manipulated via representations (JSON/XML), and responses are self-descriptive.
- **Cacheable** — Responses should declare whether they can be cached to improve performance.
- **Layered System** — A client doesn't know if it's talking directly to the server or a load balancer/proxy in between.
- **Code on Demand** (optional) — Servers can send executable code to clients (e.g., JavaScript).

---

**Q2. What is the difference between REST and SOAP?**

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

**Q3. What are HTTP verbs and when do you use each?**

- `GET` — Read a resource. Safe and idempotent. No request body.
- `POST` — Create a resource. Not idempotent (calling it twice creates two records).
- `PUT` — Replace a resource entirely. Idempotent.
- `PATCH` — Partially update a resource. Idempotent if designed properly.
- `DELETE` — Remove a resource. Idempotent.
- `HEAD` — Same as GET but returns only headers, no body. Useful for checking if a resource exists.
- `OPTIONS` — Returns the allowed HTTP methods on a resource. Used in CORS preflight.

---

**Q4. What are common HTTP status codes?**

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

**Q5. What is content negotiation in Web API?**

Content negotiation is the mechanism by which a client and server agree on the format of data exchanged. The client sends an `Accept` header specifying what it wants (e.g., `application/json` or `application/xml`), and the server responds in that format if it supports it. In ASP.NET Core, this is handled automatically by output formatters.

```csharp
builder.Services.AddControllers()
    .AddXmlSerializerFormatters(); // enables XML in addition to JSON
```

If the server can't match the requested format, it returns `406 Not Acceptable`.

---

**Q6. What is CORS and how do you enable it in Web API?**

CORS (Cross-Origin Resource Sharing) is a browser security policy that blocks JavaScript from making requests to a different domain than the one that served the page. You enable it on the server to explicitly allow specific origins.

```csharp
// Program.cs
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

**Q7. How do you handle errors in Web API?**

There are four main approaches, used together in a real app:

1. **try-catch in controller** — for specific known errors.
2. **Exception filters** — `IExceptionFilter` or `[ExceptionFilter]` attribute for reusable logic.
3. **Global middleware** — the recommended approach in .NET 6+.
4. **ProblemDetails** — standard RFC 7807 error response format.

```csharp
// Global error middleware
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

**Q8. What is an anti-forgery token and when is it needed?**

An anti-forgery token (CSRF token) protects against Cross-Site Request Forgery attacks — where a malicious site tricks a logged-in user's browser into making requests to your site. It works by embedding a hidden token in forms that the server validates.

In Web API with JWT (stateless), CSRF is generally not a concern since you're not using cookies for auth. You need it when your API uses cookie-based authentication.

```csharp
// In controller
[ValidateAntiForgeryToken]
[HttpPost]
public IActionResult Submit() { ... }
```

---

**Q9. How does session and state management work in ASP.NET Core?**

ASP.NET Core is stateless by design. Options for managing state:

- **Cookies** — small pieces of data stored on the client browser.
- **Session** — server-side storage per user, identified by a session cookie. Needs distributed cache in multi-server setups.
- **Distributed Cache** — Redis or SQL Server to share state across servers.
- **Hidden fields / query strings** — for simple UI state.
- **JWT tokens** — carry claims/state on the client side.

```csharp
// Enable session
builder.Services.AddSession();
app.UseSession();

// Use in controller
HttpContext.Session.SetString("UserId", userId);
var id = HttpContext.Session.GetString("UserId");
```

---

**Q10. What is API versioning and how do you implement it?**

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

**Q11. What is rate limiting and how do you implement it?**

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

For more advanced needs (per user, per IP), you can use the `RateLimiterOptions.OnRejected` callback to return a `429` response.

---

**Q12. How do you make a REST API more secure?**

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

**Q13. What are WebSockets and when would you use them over REST?**

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

**Q14. Why use Web API when WCF also supports REST?**

WCF was designed for SOAP and complex enterprise messaging. While it supports REST through `WebHttpBinding`, it's heavyweight, complex to configure, and designed primarily for the Windows ecosystem. Web API (ASP.NET Core) is:

- Lightweight and fast.
- Cross-platform (runs on Linux/Mac).
- Easier to test and maintain.
- Cloud-native friendly.
- Has first-class support for JSON.

WCF is not supported in .NET Core. Microsoft recommends migrating to gRPC or REST APIs.

---

**Q15. What is gRPC and how does it compare to REST?**

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

## Section 2: Authentication & Authorization

---

**Q16. What are the types of authentication in ASP.NET Core?**

- **Cookie authentication** — stores session in a cookie; used in MVC web apps.
- **JWT (JSON Web Token)** — stateless token-based auth; standard for APIs.
- **OAuth2** — authorization framework that lets users grant access to third-party apps.
- **OpenID Connect (OIDC)** — identity layer on top of OAuth2; handles authentication (who are you?).
- **API Keys** — simple, static keys sent in headers; used for server-to-server calls.
- **Windows Authentication** — for internal enterprise apps on Windows.
- **ASP.NET Core Identity** — full user management system (registration, login, roles, claims).

---

**Q17. How do you create and validate a JWT token?**

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

**Q18. What is the difference between OAuth2 and OpenID Connect?**

OAuth2 is an **authorization** framework — it answers "what can this app do on your behalf?". It doesn't tell you who the user is.

OpenID Connect (OIDC) is built on top of OAuth2 and adds **authentication** — it answers "who is this user?". OIDC provides an `id_token` (JWT) with user identity claims like name, email, sub (subject ID).

In short: OAuth2 = access to resources. OIDC = identity of user.

---

**Q19. What are the ways to implement authorization in ASP.NET Core?**

- **Role-based** — `[Authorize(Roles = "Admin")]`
- **Claims-based** — authorize based on specific claim values.
- **Policy-based** — define named policies with complex requirements.
- **Resource-based** — check if a user can access a specific resource instance (e.g., "can this user edit this post?").
- **Attribute-based** — custom attributes that implement `IAuthorizationFilter`.

```csharp
// Policy-based
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SeniorEmployee", policy =>
        policy.RequireClaim("EmploymentYears", "5", "6", "7", "8"));
});

[Authorize(Policy = "SeniorEmployee")]
public IActionResult GetSensitiveReport() => Ok();
```

---

**Q20. How do you implement refresh tokens with JWT?**

Access tokens are short-lived (15 min–1 hour). When they expire, the client uses a refresh token (long-lived, stored securely) to get a new access token without logging in again.

Flow:
1. Login → server returns access token + refresh token.
2. Access token expires → client calls `/auth/refresh` with refresh token.
3. Server validates refresh token from DB, issues new access token.
4. Refresh token rotation: issue a new refresh token and invalidate the old one.

Store refresh tokens in a DB, not JWT, so you can revoke them.

---

## Section 3: Caching

---

**Q21. What are the caching strategies in Web API?**

- **In-memory caching** — cache in the server process memory using `IMemoryCache`. Fast but not shared across multiple servers.
- **Distributed caching** — cache in Redis or SQL Server using `IDistributedCache`. Works across multiple server instances.
- **Response caching** — cache the entire HTTP response at the HTTP layer using `[ResponseCache]` attribute or middleware.
- **Cache-aside** — application checks cache first; on miss, loads from DB and populates cache.
- **Write-through** — write to cache and DB at the same time.
- **CDN caching** — cache static assets at edge servers closer to users.

```csharp
// In-memory cache
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

**Q22. What is the difference between IMemoryCache and IDistributedCache?**

`IMemoryCache` stores data in the application process memory. It's fast but is local to one server — if you have multiple instances, each has its own cache. It's lost on app restart.

`IDistributedCache` stores data in an external store like Redis. It's shared across all server instances, survives restarts, and supports serialization. It's slightly slower due to network overhead but is the right choice for production multi-instance deployments.

---

**Q23. What are cache-control headers?**

Cache-control headers tell browsers and CDNs how to cache responses.

- `Cache-Control: no-store` — never cache this.
- `Cache-Control: no-cache` — can cache but must revalidate with server before using.
- `Cache-Control: max-age=3600` — cache for 1 hour.
- `Cache-Control: public` — can be cached by CDN/proxies.
- `Cache-Control: private` — only cached by the browser, not proxies.
- `ETag` — a hash of the response. Client sends it back; server returns `304 Not Modified` if unchanged.

---

## Section 4: Performance & Scalability

---

**Q24. How do you build a high-traffic Web API?**

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

**Q25. What is the difference between horizontal and vertical scaling?**

**Vertical scaling** means adding more CPU/RAM to a single server. It has a ceiling and creates a single point of failure.

**Horizontal scaling** means adding more server instances and distributing load. It's more resilient, can scale nearly infinitely, and is the cloud-native approach. It requires the app to be stateless (no local session state).

---

**Q26. What are message queues and when should you use them? (RabbitMQ, Kafka, Azure Service Bus)**

A message queue decouples the component that produces work from the component that processes it. Instead of doing work synchronously in the API response cycle, you push a message to a queue and return immediately, then a background worker processes it.

Use when:
- The task takes too long for a synchronous response (email, PDF generation, video processing).
- You need guaranteed delivery.
- You need to smooth out traffic spikes (buffer requests).

**RabbitMQ** — traditional message broker, supports complex routing. Good for task queues within a single org.

**Kafka** — event streaming platform. High throughput, ordered events, replay support. Good for event sourcing and real-time pipelines.

**Azure Service Bus** — managed cloud queue with topics, sessions, dead-lettering. Best choice for Azure-hosted apps.

---

**Q27. How do you implement retry policies in Web API?**

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

**Q28. What is an API Gateway and Azure API Management?**

An **API Gateway** is a single entry point for all clients. It handles cross-cutting concerns like authentication, rate limiting, routing, logging, and SSL termination, so individual microservices don't have to.

**Azure API Management (APIM)** is Microsoft's managed API gateway. Features include:
- Developer portal with API documentation.
- Policies for request/response transformation.
- Rate limiting and quotas per subscription.
- Caching.
- JWT validation without touching service code.
- Analytics and monitoring.

---

## Section 5: System Design

---

**Q29. Design a notification system for email sending.**

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

**Q30. Design a real-time collaboration tool like Google Docs.**

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

**Q31. How do you ensure data consistency in a distributed system?**

- **Eventual consistency** — accept that data will be consistent "eventually" after all nodes sync. Acceptable for non-critical data.
- **Distributed transactions (Saga pattern)** — coordinate across services using a series of local transactions and compensating transactions on failure.
- **Idempotency** — design operations so calling them multiple times has the same effect as once. Use idempotency keys on write operations.
- **Optimistic concurrency** — use a `RowVersion` or ETag. Reject updates if the version has changed since the client read it.
- **Outbox pattern** — write to DB and outbox table in one transaction, then publish events from the outbox to ensure no lost events.

---

**Q32. How would you migrate a database in production safely?**

1. Use **EF Core migrations** or **Flyway** to version-control schema changes.
2. Test migrations on a staging environment that mirrors production.
3. Apply **backward-compatible migrations** first (add column before removing old one).
4. For large tables, use online migrations to avoid table locks.
5. Always take a **backup before running migrations**.
6. Have a **rollback script** ready — either a Down migration or a backup restore plan.
7. Use **blue-green deployments** — deploy new version alongside old, migrate DB, then switch traffic.

```bash
# EF Core
dotnet ef database update --connection "ProductionConnectionString"

# Verify migration history
dotnet ef migrations list
```

---

**Q33. Explain the Saga pattern for distributed transactions.**

A Saga is a sequence of local transactions. Each service does its own transaction and publishes an event. If a step fails, compensating transactions undo the previous steps.

**Example — Order placement:**
1. Order Service: create order → publish `OrderCreated`
2. Payment Service: charge card → publish `PaymentCompleted`
3. Inventory Service: reserve stock → publish `StockReserved`

If payment fails → Order Service runs compensation: cancel order.

Two implementations:
- **Choreography** — each service reacts to events. Loose coupling but hard to trace.
- **Orchestration** — a central saga orchestrator tells each service what to do. Easier to trace.

---

**Q34. What is the Outbox Pattern?**

The Outbox pattern solves the problem of "write to DB and publish event atomically". Without it, if the service writes to DB then crashes before publishing, the event is lost.

Solution: write the domain record AND the outbound event to the same DB in a single transaction. A separate background poller reads from the outbox table and publishes events to the message bus, then marks them as sent.

```
[Service] → writes [Order] + [OutboxEvent] in one DB transaction
[Outbox Publisher] → polls OutboxEvent table → publishes to queue → marks sent
```

---

**Q35. What is blue-green deployment?**

Blue-green deployment runs two identical production environments. "Blue" is live; "Green" is the new version. You deploy and test Green while Blue serves traffic. Once Green is verified, you switch the load balancer to point to Green. If something goes wrong, you switch back to Blue instantly. Zero downtime.

Azure equivalents: **Deployment Slots** in Azure App Service.

---

## Section 6: Azure Function Apps

---

**Q36. What is an Azure Function App?**

An Azure Function App is a serverless compute service that runs small pieces of code (functions) in response to triggers without managing infrastructure. You pay only for execution time. It's ideal for event-driven, background processing, and integration tasks.

---

**Q37. What are the different trigger types in Azure Functions?**

- **HTTP Trigger** — invoked by an HTTP request. Used to build serverless APIs.
- **Timer Trigger** — runs on a CRON schedule (e.g., every hour).
- **Blob Trigger** — fires when a file is added or updated in Azure Blob Storage.
- **Queue Trigger** — processes messages from Azure Storage Queue.
- **Service Bus Trigger** — processes messages from Azure Service Bus.
- **Event Hub Trigger** — processes events from Azure Event Hubs (high-throughput streaming).
- **Cosmos DB Trigger** — fires on document changes via change feed.
- **Event Grid Trigger** — reacts to events from Azure Event Grid.

---

**Q38. What is the difference between Azure Functions Consumption Plan, Premium Plan, and Dedicated Plan?**

- **Consumption Plan** — scales to zero, pay per execution. Cold start latency on first invocation. Best for intermittent workloads.
- **Premium Plan** — pre-warmed instances (no cold start), VNet integration, more powerful instances. Best for latency-sensitive production workloads.
- **Dedicated (App Service) Plan** — runs on always-on VMs you already pay for. Best if you have an existing App Service plan or need long-running functions.

---

**Q39. How do you write a Timer Trigger function in .NET?**

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

**Q40. How do you write a Service Bus Trigger function?**

```csharp
public class OrderProcessor
{
    [FunctionName("ProcessOrder")]
    public void Run(
        [ServiceBusTrigger("orders-queue", Connection = "ServiceBusConnection")]
        string messageBody,
        ILogger log)
    {
        var order = JsonSerializer.Deserialize<Order>(messageBody);
        log.LogInformation($"Processing order {order.Id}");
        // process the order
    }
}
```

The connection string is stored in application settings, not in code.

---

**Q41. How do you write an HTTP Trigger function?**

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

**Q42. What is Durable Functions?**

Durable Functions is an extension of Azure Functions that allows writing stateful workflows as code. It manages state, checkpointing, and retries automatically. Useful for:

- Long-running workflows (fan-out/fan-in, chaining).
- Human interaction workflows (waiting for approval).
- Aggregating events over time.

```csharp
// Orchestrator
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

**Q43. How do you inject dependencies into Azure Functions?**

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

**Q44. How do you handle errors and retries in Azure Functions?**

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

**Q45. What is the difference between Azure Storage Queue and Azure Service Bus?**

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

## Section 7: Troubleshooting & Diagnostics

---

**Q46. API is using 80% CPU and 90% memory. How do you troubleshoot?**

1. **Check Application Insights** — identify which endpoints are slow or high CPU.
2. **Profile the application** — use dotnet-trace or Visual Studio Profiler to find hot code paths.
3. **Analyze database** — look for missing indexes, N+1 queries, slow query logs.
4. **Check for synchronous blocking** — `.Result` or `.Wait()` on async methods causes thread starvation.
5. **Review caching** — are frequently called endpoints hitting the DB on every request?
6. **Check background jobs** — is a timer job or scheduled task doing heavy work?
7. **Enable logging** — log request duration and identify the slowest operations.
8. **Scale out** — add more instances as a temporary relief while you investigate.

---

**Q47. An endpoint is throwing OutOfMemoryException. What could be wrong?**

- Loading a very large dataset into memory at once (e.g., `ToList()` on millions of rows).
- Unbounded in-memory caching growing over time.
- Large file being read entirely into memory instead of using streaming.
- Memory leak — objects being held in static variables or event handlers.
- Large object heap (LOH) fragmentation in .NET.

**Fix:** Use pagination, streaming (`IAsyncEnumerable`), chunked reads, and check for memory leaks with dotMemory or heap snapshots.

```csharp
// Bad - loads everything into memory
var all = await _db.Orders.ToListAsync();

// Good - stream with IAsyncEnumerable
await foreach (var order in _db.Orders.AsAsyncEnumerable())
{
    await ProcessOrder(order);
}
```

---

**Q48. Memory usage spikes every hour. What could be the cause?**

The hourly pattern points to a **scheduled job or timer**. Check:

- Timer Triggers or Hangfire jobs running hourly that process large datasets.
- IMemoryCache growing because cache entries aren't expiring.
- In-memory collections being rebuilt hourly without releasing the old ones.
- Log files or diagnostics data accumulating in memory.
- Connection pools not being returned properly.

Use `dotnet-counters monitor` to watch GC pressure and heap size in real time.

---

**Q49. Production server went down. How do you troubleshoot?**

1. **Check monitoring/alerting** — Application Insights, Azure Monitor, or health check endpoint.
2. **Check logs** — IIS logs, Windows Event Viewer, or structured logs in Log Analytics.
3. **Check resource usage** — CPU, memory, disk, network at the time of the crash.
4. **Check recent deployments** — did a deployment happen before the crash?
5. **Check dependencies** — is the database, Redis, or an external API down?
6. **Restart the service** if needed as an immediate mitigation while investigating.
7. **Post-mortem** — document timeline, root cause, and preventive measures.

---

**Q50. Users can't log in. How do you troubleshoot?**

1. Check if the `/auth/login` endpoint returns an error in logs.
2. Check if the JWT secret or certificate has changed/expired.
3. Check if the Identity database (AspNetUsers table) is reachable.
4. Check if external identity provider (Azure AD, Google) is having an outage.
5. Check if a recent deployment changed the authentication configuration.
6. Reproduce locally with the same credentials.
7. Check if the issue affects all users or specific ones (specific domain, tenant).
8. Check browser developer tools for the exact error response from the API.

---

**Q51. There is a memory leak causing daily restarts. How do you troubleshoot?**

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

**Q52. Deployment failed midway. What is your rollback strategy?**

- Use **Azure App Service Deployment Slots** — swap back to previous slot instantly.
- Use **blue-green deployment** — switch the load balancer back to old environment.
- For Kubernetes: use `kubectl rollout undo deployment/myapp`.
- For DB migrations: have a Down script for every Up script. Or apply additive-only changes so the old code still works.
- For containers: tag images with version numbers, not just `latest`. Redeploy the previous tag.

Always test rollback procedures in staging before a production release.

---

## Section 8: Scenario-Based Questions

---

**Q53. A client reports that API responses are slow. Walk me through your investigation.**

1. Reproduce the slowness — is it all endpoints or specific ones?
2. Check Application Insights traces for slow requests.
3. Check database query execution time — look for missing indexes or full table scans.
4. Check if N+1 queries are happening (loading navigation properties in a loop).
5. Check if there's no caching on frequently called endpoints.
6. Check external HTTP calls — are they timing out?
7. Check if the app is under load — is latency normal under low traffic?
8. Add timing logs around suspect code to isolate the bottleneck.

---

**Q54. You need to add a breaking API change. How do you do it without affecting existing clients?**

1. **Never modify the existing endpoint.** Create a new versioned endpoint: `/api/v2/orders`.
2. Deprecate the old version — add a `Deprecation` response header and document the sunset date.
3. Notify API consumers well in advance (3–6 months minimum).
4. Keep both versions running until all clients have migrated.
5. After the sunset date, return `410 Gone` from the old version.

---

**Q55. You need to integrate with a third-party payment API that sometimes goes down. How do you handle this?**

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

**Q56. A new endpoint needs to handle 10,000 requests per second. How do you design it?**

- Make the endpoint **stateless** and async.
- Cache the response in Redis — most reads at high scale are repeated queries.
- **Pre-compute** or pre-warm the response in the background.
- Use a **CDN** if the response is the same for all users.
- Enable **response compression**.
- Use **horizontal scaling** — deploy multiple instances behind a load balancer.
- Profile the endpoint under load with k6 or JMeter before going live.
- Avoid any DB write on the hot path — use async fire-and-forget or a queue.

---

**Q57. How would you implement a background job in ASP.NET Core?**

Options in order of complexity:

1. **IHostedService / BackgroundService** — simple background work in the same process.
2. **Hangfire** — persistent job scheduler with a dashboard, retry, and scheduling.
3. **Azure Functions (Timer Trigger)** — fully decoupled, serverless.
4. **Azure WebJobs** — runs alongside an App Service.

```csharp
// BackgroundService example
public class EmailQueueProcessor : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessPendingEmailsAsync();
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
```

---

**Q58. How do you implement health checks in ASP.NET Core?**

```csharp
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "database")
    .AddRedis(redisConnectionString, name: "redis")
    .AddUrlGroup(new Uri("https://api.external.com/ping"), name: "external-api");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
```

Health checks are polled by load balancers and orchestrators (Kubernetes). A failing DB check causes the instance to be taken out of the load balancer rotation.

---

**Q59. How would you secure sensitive configuration values (connection strings, API keys)?**

- Never store secrets in `appsettings.json` or source code.
- In development: use **User Secrets** (`dotnet user-secrets`).
- In production: use **Azure Key Vault** and reference secrets through the configuration system.
- Use **Managed Identity** so the app authenticates to Key Vault without storing any credentials at all.

```csharp
// Add Key Vault to configuration
builder.Configuration.AddAzureKeyVault(
    new Uri("https://myvault.vault.azure.net/"),
    new DefaultAzureCredential()); // uses Managed Identity in Azure
```

---

**Q60. How do you implement idempotency in a REST API?**

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

**Q61. How would you implement pagination in a Web API?**

Use offset/cursor-based pagination. Return metadata so clients know how to navigate.

```csharp
// Controller
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

**Q62. How do you log requests and responses in Web API?**

Use middleware to capture all requests and responses before they reach controllers.

```csharp
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await _next(context);
        sw.Stop();

        _logger.LogInformation("{Method} {Path} responded {StatusCode} in {Elapsed}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds);
    }
}
```

Use **Serilog** with structured logging sinks to write to Application Insights, Seq, or ELK.

---

**Q63. How do you unit test a Web API controller?**

Use `xUnit` + `Moq`. Don't test HTTP plumbing; test business logic.

```csharp
public class ProductsControllerTests
{
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Product?)null);

        var controller = new ProductsController(mockService.Object);
        var result = await controller.GetById(99);

        Assert.IsType<NotFoundResult>(result);
    }
}
```

For integration tests, use `WebApplicationFactory<Program>` to spin up the full app in memory.

---

**Q64. How do you implement the Repository pattern in a Web API?**

The repository pattern abstracts data access logic behind an interface. Controllers depend on the interface, not EF Core directly, making the code testable and swappable.

```csharp
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
    Task SaveChangesAsync();
}

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;
    public OrderRepository(AppDbContext db) => _db = db;

    public Task<Order?> GetByIdAsync(int id) => _db.Orders.FindAsync(id).AsTask();
    public Task<IEnumerable<Order>> GetAllAsync() => Task.FromResult(_db.Orders.AsEnumerable());
    public async Task AddAsync(Order order) => await _db.Orders.AddAsync(order);
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
```

---

**Q65. What is the difference between synchronous and asynchronous controllers and when should you use async?**

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

**Q66. How do you implement file upload and download in Web API?**

```csharp
// Upload
[HttpPost("upload")]
public async Task<IActionResult> Upload(IFormFile file)
{
    if (file.Length == 0) return BadRequest("Empty file");
    var blobClient = _blobContainerClient.GetBlobClient(file.FileName);
    await blobClient.UploadAsync(file.OpenReadStream(), overwrite: true);
    return Ok(new { FileName = file.FileName });
}

// Download (stream, never load into memory)
[HttpGet("download/{filename}")]
public async Task<IActionResult> Download(string filename)
{
    var blobClient = _blobContainerClient.GetBlobClient(filename);
    if (!await blobClient.ExistsAsync()) return NotFound();
    var download = await blobClient.DownloadStreamingAsync();
    return File(download.Value.Content, "application/octet-stream", filename);
}
```

---

**Q67. How would you expose an API to external third-party developers?**

- Publish an **OpenAPI/Swagger** document with clear descriptions and examples.
- Use **API versioning** so changes don't break integrations.
- Issue **API keys** or OAuth2 client credentials per consumer for tracking and rate limiting.
- Use **Azure API Management** for throttling, analytics, and developer portal.
- Provide an **SDK** for popular languages (NuGet, npm) to reduce integration friction.
- Version your API contract and communicate deprecations with ample notice.

---

**Q68. What is the difference between IActionResult and ActionResult<T>?**

`IActionResult` is untyped — it can return any result. It works for Swagger but the return type isn't inferred.

`ActionResult<T>` is typed. Swagger automatically generates the response schema from `T`. It's the preferred approach.

```csharp
// IActionResult - Swagger doesn't know the response type
public IActionResult GetProduct(int id) => Ok(product);

// ActionResult<T> - Swagger generates the Product schema automatically
public ActionResult<Product> GetProduct(int id) => Ok(product);
```

---

**Q69. How does minimal API differ from controller-based API in .NET 6+?**

Minimal APIs define endpoints directly in `Program.cs` without controllers or attributes. They're simpler, faster to bootstrap, and have less overhead, making them good for microservices.

```csharp
// Minimal API
app.MapGet("/products/{id}", async (int id, IProductService svc) =>
{
    var product = await svc.GetByIdAsync(id);
    return product is null ? Results.NotFound() : Results.Ok(product);
});
```

Controller-based APIs offer more features out of the box: model binding, filters, conventions, and attribute routing. For large apps, controllers are still preferred for organisation.

---

**Q70. How do you implement model validation in Web API?**

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

## Section 9: Advanced & Architecture

---

**Q71. What is the difference between Middleware and Filters in ASP.NET Core?**

**Middleware** operates at the HTTP pipeline level — it runs for every request before routing. Good for logging, auth, compression, CORS.

**Filters** operate within the MVC framework level — they run around controller action execution. Types: Authorization, Resource, Action, Exception, Result filters.

Use middleware for cross-cutting infrastructure concerns. Use filters for concerns specific to MVC like input formatting, validation, or exception handling per controller/action.

---

**Q72. What is the Options pattern in ASP.NET Core?**

The Options pattern provides strongly typed access to configuration values. Instead of reading strings from `IConfiguration` directly, you bind configuration sections to classes.

```csharp
// appsettings.json
// { "EmailSettings": { "SmtpHost": "smtp.gmail.com", "Port": 587 } }

public class EmailSettings
{
    public string SmtpHost { get; set; } = string.Empty;
    public int Port { get; set; }
}

// Register
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Use
public class EmailService
{
    private readonly EmailSettings _settings;
    public EmailService(IOptions<EmailSettings> options) => _settings = options.Value;
}
```

---

**Q73. What is the difference between Scoped, Transient, and Singleton lifetimes?**

- **Transient** — a new instance every time it's requested. Use for stateless, lightweight services.
- **Scoped** — one instance per HTTP request. Use for services that hold request-level state, like `DbContext`.
- **Singleton** — one instance for the entire application lifetime. Use for thread-safe, expensive-to-create services like configuration, logging, HTTP clients.

Never inject a scoped service into a singleton — the scoped service becomes effectively singleton (captured dependency).

---

**Q74. What is the CQRS pattern?**

CQRS (Command Query Responsibility Segregation) separates read (Query) and write (Command) operations into different models.

- **Commands** — change state. Return no data or just an ID.
- **Queries** — return data. Don't change state.

This allows you to optimize reads and writes independently, scale them separately, and keep the codebase focused. Often implemented with MediatR.

```csharp
// Command
public record CreateOrderCommand(string CustomerId, int Quantity) : IRequest<int>;

// Query
public record GetOrderQuery(int OrderId) : IRequest<OrderDto>;

// In controller
var orderId = await _mediator.Send(new CreateOrderCommand("C001", 5));
var order = await _mediator.Send(new GetOrderQuery(orderId));
```

---

**Q75. What is an API throttling strategy for multi-tenant SaaS?**

In a multi-tenant SaaS, different customers may have different quotas:

- Identify the tenant from the JWT claim or API key.
- Apply per-tenant rate limits using Redis (tenant ID as key, sliding window counter).
- Return `429 Too Many Requests` with a `Retry-After` header.
- Offer different rate limit tiers per subscription plan.

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy<string>("per-tenant", context =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: context.User.FindFirst("tenant_id")?.Value ?? "anonymous",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 100,
                SegmentsPerWindow = 6
            }));
});
```

---

**Q76. How do you implement a circuit breaker pattern?**

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

**Q77. What is Event Sourcing?**

Instead of storing the current state of a record, Event Sourcing stores the full history of events that led to that state. To get current state, you replay the events.

Example: instead of storing `Order { Status: "Shipped" }`, you store:
1. `OrderCreated`
2. `PaymentReceived`
3. `OrderShipped`

Benefits: full audit trail, ability to rebuild state at any point in time, event replay for projections. Often used with CQRS.

---

**Q78. How do you implement soft delete in EF Core?**

Soft delete marks a record as deleted instead of removing it from the DB. Use a `IsDeleted` column and a global query filter.

```csharp
// Entity
public class Order
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}

// DbContext - filters out deleted in all queries automatically
protected override void OnModelCreating(ModelBuilder builder)
{
    builder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
}

// To delete
order.IsDeleted = true;
await _db.SaveChangesAsync();

// To query including deleted (admin)
var allOrders = await _db.Orders.IgnoreQueryFilters().ToListAsync();
```

---

**Q79. What are the best practices for EF Core in a high-traffic API?**

- Use **async** methods (`FindAsync`, `ToListAsync`).
- Always use **pagination** — never `ToList()` on unbounded sets.
- Use **AsNoTracking()** for read-only queries.
- Select only the columns you need with **projections** (`.Select(o => new { o.Id, o.Name })`).
- Avoid **N+1 queries** — use `.Include()` or split queries.
- Use **compiled queries** for frequently executed queries.
- Don't use DbContext as a singleton — keep it scoped.
- Index columns used in WHERE clauses.

---

**Q80. How do you handle long-running operations in an API?**

For operations that take longer than a few seconds, use an async job pattern:

1. Client sends a request. API validates input, enqueues a job, returns `202 Accepted` with a `Job-Id`.
2. Background worker processes the job.
3. Client polls `GET /jobs/{id}` to check status (or use SignalR to push completion).

```csharp
[HttpPost("reports")]
public async Task<IActionResult> GenerateReport(ReportRequest request)
{
    var jobId = Guid.NewGuid();
    await _queue.EnqueueAsync(new ReportJob(jobId, request));
    return Accepted(new { JobId = jobId, StatusUrl = $"/api/jobs/{jobId}" });
}
```

---

## Section 10: More Scenario & Design Questions

---

**Q81. How would you design a URL shortener service (like bit.ly)?**

**Write path:** Client sends long URL → generate 6-character random code → store mapping in Redis + DB → return short URL.

**Read path:** Client hits short URL → lookup code in Redis cache → 301 redirect to long URL.

**Scale considerations:**
- Redis for sub-millisecond lookups.
- Base62 encoding for short codes.
- Use consistent hashing if distributing storage.
- Analytics (click counts) updated asynchronously via a queue.

---

**Q82. Design an API for a multi-tenant SaaS application.**

- Identify tenants by subdomain, JWT claim, or header.
- Use **row-level tenancy** (all tenants share one DB with a `TenantId` column) or **database-per-tenant**.
- Add a global EF Core query filter for `TenantId` so cross-tenant data leaks are impossible.
- Isolate tenant configuration (feature flags, limits) in a `Tenants` table.
- Rate limit per tenant.
- Log and trace all requests with `TenantId` as a structured property.

---

**Q83. How would you implement an audit trail for all data changes?**

Options:
1. **EF Core SaveChanges interceptor** — intercept all changes and write to an `AuditLogs` table.
2. **Database triggers** — DB-level audit without application code changes.
3. **Event sourcing** — store events as the primary source of truth.

```csharp
public override Task<int> SaveChangesAsync(CancellationToken ct = default)
{
    foreach (var entry in ChangeTracker.Entries()
        .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
    {
        AuditLogs.Add(new AuditLog
        {
            EntityName = entry.Metadata.Name,
            Action = entry.State.ToString(),
            ChangedAt = DateTime.UtcNow,
            ChangedBy = _currentUser.Id,
            OldValues = entry.State != EntityState.Added ? 
                JsonSerializer.Serialize(entry.OriginalValues.ToObject()) : null,
            NewValues = entry.State != EntityState.Deleted ? 
                JsonSerializer.Serialize(entry.CurrentValues.ToObject()) : null
        });
    }
    return base.SaveChangesAsync(ct);
}
```

---

**Q84. How do you design an API that handles file processing asynchronously?**

1. Client uploads file to API.
2. API stores raw file in Azure Blob Storage.
3. API enqueues a processing job with the blob reference.
4. API returns `202 Accepted` with a job ID.
5. Azure Function (Blob Trigger or Queue Trigger) processes the file.
6. Worker stores result and updates job status in DB.
7. Client polls job status endpoint or receives a webhook callback when done.

---

**Q85. How would you implement a webhook system in your API?**

Webhooks let your API push events to subscriber URLs instead of subscribers polling.

1. Expose `POST /webhooks` to let clients register a callback URL and list of events.
2. Store subscriptions in DB: `{ url, event_type, secret }`.
3. When an event occurs, enqueue a delivery job.
4. Worker sends `POST` to the registered URL with event payload.
5. Sign the payload with HMAC-SHA256 using the subscriber's secret so they can verify authenticity.
6. Retry with backoff on failure; deactivate subscription after too many consecutive failures.

---

**Q86. How would you implement search functionality in a Web API?**

- For **simple filtering**: use EF Core LINQ with dynamic predicates.
- For **full-text search**: use Azure Cognitive Search or Elasticsearch.
- For **autocomplete**: use a trie structure or search index with prefix queries.

```csharp
[HttpGet]
public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] string? category)
{
    var query = _db.Products.AsQueryable();

    if (!string.IsNullOrEmpty(q))
        query = query.Where(p => p.Name.Contains(q) || p.Description.Contains(q));

    if (!string.IsNullOrEmpty(category))
        query = query.Where(p => p.Category == category);

    return Ok(await query.Take(50).ToListAsync());
}
```

For production at scale, push to an Elasticsearch index on every write and query Elasticsearch for search.

---

**Q87. What is an Outbox + Inbox pattern?**

The **Outbox** ensures events are published reliably — store events in an outbox table in the same DB transaction as domain writes, then publish them in a background poller.

The **Inbox** ensures events are processed exactly once on the receiving side — before processing a message, check if its message ID exists in an inbox table. If yes, skip (already processed). If no, process and insert the ID.

Together, they give you **at-least-once delivery with idempotent processing** — effectively exactly-once semantics.

---

**Q88. How do you test Azure Functions locally?**

- Use **Azure Functions Core Tools** (`func start`) to run functions locally.
- Use **Azurite** (Azure Storage Emulator) for local Blob/Queue/Table storage.
- Use a local **Service Bus namespace** or Azurite for queue triggers.
- Write unit tests by injecting mock dependencies (same as regular .NET — Functions support DI).
- Write integration tests with `WebApplicationFactory` for HTTP Trigger functions.

```bash
# Install and run locally
npm install -g azure-functions-core-tools@4
func start
```

---

**Q89. How would you implement feature flags in a Web API?**

Feature flags let you toggle functionality without deploying code.

- Use **Azure App Configuration** with feature management.
- Toggle features per environment, per user, or on a percentage rollout.

```csharp
// Program.cs
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
           .UseFeatureFlags();
});
builder.Services.AddFeatureManagement();

// Usage
public class ProductsController : ControllerBase
{
    private readonly IFeatureManager _features;

    [HttpGet("new-layout")]
    public async Task<IActionResult> Get()
    {
        if (await _features.IsEnabledAsync("NewProductLayout"))
            return Ok(/* new response */);
        return Ok(/* old response */);
    }
}
```

---

**Q90. What is the difference between optimistic and pessimistic concurrency?**

**Optimistic concurrency** assumes conflicts are rare. It doesn't lock records. Instead, it checks at save time if the row was modified since it was read (using a `RowVersion`/`ETag`). If it was, it throws a `DbUpdateConcurrencyException`.

**Pessimistic concurrency** locks the row when it's read so no one else can modify it until the transaction is complete. Implemented with `SELECT ... FOR UPDATE` or explicit DB locks.

Optimistic is better for most web APIs (high read, low conflict). Pessimistic is needed when you can't tolerate lost updates (e.g., booking seats, inventory deduction).

---

**Q91. How do you implement distributed tracing in a microservices system?**

Use **OpenTelemetry** to propagate trace context across service boundaries.

- Each service creates a **span** for each operation.
- Spans carry a `TraceId` that links all spans across services into one trace.
- Export spans to **Jaeger**, **Zipkin**, or **Azure Monitor**.

```csharp
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
        tracing.AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
               .AddSqlClientInstrumentation()
               .AddAzureMonitorTraceExporter(o =>
                   o.ConnectionString = appInsightsConnectionString));
```

This means you can see a single request flow through Order API → Payment API → Notification Service all in one trace.

---

**Q92. Explain the strangler fig pattern for migrating a monolith.**

The Strangler Fig pattern incrementally migrates a monolith to microservices without a big-bang rewrite.

1. Put an API Gateway or proxy in front of the monolith.
2. Identify a bounded context (e.g., Orders module) and extract it as a microservice.
3. Route `/api/orders` traffic from the gateway to the new service.
4. Repeat for other modules over time.
5. Once all traffic is migrated, decommission the monolith.

The monolith "strangled" by the new services, just like a strangler fig vine wraps around a tree.

---

**Q93. How do you handle database connection resiliency in EF Core?**

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

**Q94. How would you implement multi-level caching?**

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
        _memoryCache.Set($"product_{id}", product, TimeSpan.FromMinutes(1)); // populate L1
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

**Q95. How would you design an API that supports both synchronous and asynchronous consumers?**

Use the **async API pattern** with polling or webhooks:

- Return `202 Accepted` with a `Location` header pointing to a job status endpoint.
- Synchronous clients: poll `GET /jobs/{id}` until status is `Completed`.
- Async clients: register a webhook URL when submitting the job; receive a callback on completion.

This single API design supports both patterns without requiring two different endpoints.

---

**Q96. What is the Competing Consumers pattern?**

Multiple worker instances all read from the same queue, each processing a different message. This provides natural parallelism and fault tolerance — if one worker crashes, the message is requeued and another worker picks it up.

Azure Service Bus and RabbitMQ both support this. You can scale the number of workers up or down based on queue depth.

---

**Q97. How do you implement an API that needs to process CSV file uploads with millions of rows?**

- Accept the file upload, store it in Blob Storage, enqueue a job, return `202`.
- Worker reads the CSV using streaming (`CsvHelper` with `IAsyncEnumerable`) — never load the whole file into memory.
- Process in batches of ~1,000 rows.
- Use `SqlBulkCopy` or EF Core `BulkInsert` for DB writes.
- Track progress and errors per row in a job status table.
- Report results (rows processed, rows failed) in the job status endpoint.

---

**Q98. How do you implement a dead-letter queue handler?**

A dead-letter queue (DLQ) holds messages that failed all retries. You need to monitor and handle them.

```csharp
[FunctionName("DeadLetterProcessor")]
public async Task Run(
    [ServiceBusTrigger("orders-queue/$DeadLetterQueue", 
        Connection = "ServiceBusConnection")]
    ServiceBusReceivedMessage message,
    ILogger log)
{
    log.LogError("DLQ message: {MessageId}, DeadLetterReason: {Reason}",
        message.MessageId, message.DeadLetterReason);

    // Alert team, store for manual review, or fix and requeue
    await _alertService.NotifyAsync($"DLQ message: {message.MessageId}");
    await _dlqRepository.StoreAsync(message);
}
```

---

**Q99. How do you handle breaking changes in an Event Schema for a message queue?**

- **Add fields instead of removing** — consumers that don't need the new field ignore it.
- **Version your events** — include a `version` field in the payload.
- **Schema registry** — use a schema registry (like Azure Schema Registry) to enforce compatibility.
- Use **forward and backward compatible** schema changes.
- Keep old consumers running until they're updated to understand the new schema.
- For major breaking changes, use a new topic/queue alongside the old one during the migration period.

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

