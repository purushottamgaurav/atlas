using DotNetWebApi.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS allowed all policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();


// MIDDLEWARE USAGE EXAMPLES ( Logging, Authentication, CorrelationId, Stopwatch, RateLimiting, GlobalExceptionHandler, etc. )

// 1- Add the logging middleware to the pipeline
app.Use(async (context, next) =>
{
    // Before the next middleware
    Console.WriteLine($"Request: {context.Request.Path}");

    await next(context); // call next

    // After the next middleware (response phase)
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});

// 2- Class-based middleware
app.UseMiddleware<LoggingMiddleware>();

// 3- Extension method for middleware
app.UseLogging();


app.Run();
