# Web API Interview Q&A

---

1. **RESTful (REST) vs RESTless (SOAP)?**
REST is an architectural style using HTTP verbs (GET, POST, PUT, DELETE) with lightweight JSON/XML payloads. It is stateless, scalable, and easy to consume. SOAP is a protocol with strict XML-based messaging, built-in error handling (faults), and WS-Security standards. REST is preferred for public APIs and microservices. SOAP is used in enterprise/legacy systems where formal contracts and reliability guarantees (WS-* standards) are required.

2. **What are REST principles?**
Uniform Interface — consistent resource-based URLs and HTTP verbs. Statelessness — each request contains all information needed; server stores no client state. Client-Server Separation — client and server evolve independently. Caching — responses can be cached to improve performance (Cache-Control headers). Layered Architecture — client doesn't know if it's talking to the actual server or a load balancer/proxy. Code on Demand (optional) — server can send executable code to client (e.g., JavaScript).

3. **Caching types?**
Output Caching — caches the full HTTP response for a resource. In .NET 7+ use app.UseOutputCache() and [OutputCache] attribute.
Data Caching — caches data objects (query results, computed values).
Fragment Caching — caches parts of a response (partial views).
In-Memory Cache — IMemoryCache, fast, lives in the process, lost on restart.
Distributed Cache — IDistributedCache backed by Redis or SQL Server, shared across multiple instances/servers. Use in load-balanced environments.
Client-Side Caching — browser caches responses using Cache-Control, ETag, and Expires headers.
Server-Side Caching — in-memory or distributed cache on the server.

4. **Error handling in Web API?**
Exception Filter — implement IExceptionFilter or inherit ExceptionFilterAttribute; applies at action or controller level. Register globally in AddControllers(o => o.Filters.Add<MyExceptionFilter>()).
Middleware — UseExceptionHandler for global unhandled exceptions; returns a consistent error response.
ProblemDetails — standard RFC 7807 error response format. Use AddProblemDetails() in .NET 7+.
HttpResponseException — throw directly from action to return a specific status code.
Scope — [HandleError] on action, controller, or globally via filters.
```csharp
app.UseExceptionHandler(err => err.Run(async ctx => {
    ctx.Response.StatusCode = 500;
    await ctx.Response.WriteAsJsonAsync(new { error = "Something went wrong" });
}));
```

5. **What is content negotiation in Web API?**
Content negotiation is the mechanism by which the client and server agree on the format of the response. The client sends an Accept header (e.g., Accept: application/json or Accept: application/xml) and the server returns the response in the requested format if supported. ASP.NET Core supports this via output formatters. Add XML support with AddXmlSerializerFormatters().

6. **Anti-forgery token?**
Prevents Cross-Site Request Forgery (CSRF) attacks. The server generates a unique token per request embedded in forms. On POST, the server validates the token — if missing or invalid, the request is rejected. Prevents malicious sites from submitting forms on behalf of authenticated users. In ASP.NET Core use [ValidateAntiForgeryToken] on POST actions and @Html.AntiForgeryToken() in Razor forms. APIs using JWT/bearer tokens are generally not vulnerable to CSRF since cookies are not used.

7. **Cookies in ASP.NET?**
Cookies are small key-value data stored in the browser and sent with every request to the same domain. Use Response.Cookies.Append() to set and Request.Cookies[] to read. Set HttpOnly to prevent JS access, Secure to send only over HTTPS, and SameSite to restrict cross-site sending. Used for authentication (auth cookies), session tracking, and user preferences.
```csharp
Response.Cookies.Append("key", "value", new CookieOptions {
    HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict,
    Expires = DateTimeOffset.UtcNow.AddDays(7)
});
```

8. **State and session management in ASP.NET?**
Server-side — Application State (shared across all users, app lifetime), Session State (per-user, per-browser session, stored in memory/Redis/SQL), Profile Properties (persisted user data), Cache (temporary server-side data store).
Client-side — Cookies (small key-value, sent with requests), QueryString (data in URL), ViewState (Web Forms only, stores page state in hidden field), Hidden Fields, Local Storage / Session Storage (browser-based, JS-accessible), State management libraries like Redux (for SPAs).

9. **Authentication — Types, JWT, OAuth2, OpenID, Identity?**
Authentication verifies who the user is.
JWT (JSON Web Token) — stateless token containing claims, signed with a secret or certificate. Sent in Authorization: Bearer header.
OAuth2 — authorization framework for delegated access (e.g., login with Google). Issues access tokens to third-party apps.
OpenID Connect — identity layer on top of OAuth2. Returns an ID token with user identity info.
ASP.NET Core Identity — full membership system with user store, password hashing, roles, claims. Backed by a database.
Cookie Authentication — server issues an encrypted cookie after login; validated on each request.

10. **How do we create and validate a JWT token?**
Create — generate a JwtSecurityToken with claims, expiry, signing credentials, and serialize it. Validate — middleware reads the Authorization header, verifies the signature, checks expiry and issuer.
```csharp
// Create
var token = new JwtSecurityToken(
    issuer: "myapp", audience: "myapp",
    claims: new[] { new Claim(ClaimTypes.Name, "user1") },
    expires: DateTime.UtcNow.AddHours(1),
    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

// Validate (in Program.cs)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new() {
        ValidateIssuer = true, ValidateAudience = true,
        ValidateLifetime = true, ValidateIssuerSigningKey = true,
        IssuerSigningKey = key
    });
```

11. **Authorization — ways?**
Role-based — [Authorize(Roles = "Admin")]. User must be in the specified role.
Policy-based — define policies with requirements; [Authorize(Policy = "MinAge")]. Flexible and testable.
Claims-based — authorize based on claim values in the token.
Resource-based — check authorization against a specific resource using IAuthorizationService.
Attribute-based — [Authorize] on controller or action. [AllowAnonymous] to opt out.

12. **Bundling and minification in .NET Core?**
Not natively built into .NET Core (unlike .NET Framework's System.Web.Optimization). Use Gulp or Webpack to bundle and minify JS/CSS as part of the build pipeline. For simple cases use LibMan (Library Manager) for client-side assets. BundlerMinifier is a Visual Studio extension for basic bundling. Production builds with React/Angular/Vue handle this automatically.

13. **What are HTTP verbs?**
GET — retrieve a resource; safe and idempotent.
POST — create a new resource; not idempotent.
PUT — replace an entire resource; idempotent.
PATCH — partial update of a resource; idempotent.
DELETE — remove a resource; idempotent.
HEAD — same as GET but returns only headers, no body.
OPTIONS — returns supported HTTP methods for a resource (used in CORS preflight).

14. **How to make REST APIs more secure?**
Use HTTPS always — enforce with UseHttpsRedirection and HSTS.
Authentication and Authorization — JWT, OAuth2, or API keys.
Rate Limiting — prevent abuse with AspNetCoreRateLimit or built-in .NET 7 rate limiting middleware.
Input Validation — validate all inputs; use model validation attributes and FluentValidation.
CORS — restrict allowed origins with a strict CORS policy.
Avoid exposing sensitive data — don't return stack traces or internal details in error responses.
Use HTTPS-only cookies — HttpOnly, Secure, SameSite flags.
Secrets management — use Azure Key Vault, AWS Secrets Manager, or environment variables. Never hardcode secrets.
OWASP Top 10 — protect against injection, broken auth, excessive data exposure, etc.

15. **What are WebSockets?**
WebSockets provide a full-duplex, persistent connection between client and server over a single TCP connection. Unlike HTTP which is request-response, WebSockets allow both sides to send messages at any time without a new request. Used for real-time features like chat, live notifications, dashboards, and multiplayer games. In ASP.NET Core use app.UseWebSockets() and SignalR (which abstracts WebSockets with fallbacks). Handshake starts as an HTTP request and upgrades to WebSocket protocol via the Upgrade header.