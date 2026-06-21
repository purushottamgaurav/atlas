using DotNetFunctions.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Observability
builder.Services.AddOpenTelemetry()
    .UseFunctionsWorkerDefaults();

// Application services
builder.Services.AddSingleton<IOrderService, OrderService>();

builder.Build().Run();
