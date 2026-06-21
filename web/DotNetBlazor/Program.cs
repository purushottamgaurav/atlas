using DotNetBlazor.Components;
using DotNetBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Named HttpClient to the WebAPI — bypass dev cert
builder.Services.AddHttpClient("QuizApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7008/");
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
});

// Quiz services — Scoped so each Blazor circuit gets its own instance
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<HubService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
