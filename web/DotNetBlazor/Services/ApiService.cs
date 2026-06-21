namespace DotNetBlazor.Services;
using DotNetBlazor.Models;
using System.Net.Http.Json;
using System.Text.Json;

public class ApiService(IHttpClientFactory factory, SessionService session)
{
    private static readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true };

    private HttpClient Client()
    {
        var client = factory.CreateClient("QuizApi");
        if (!string.IsNullOrEmpty(session.Token))
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.Token);
        return client;
    }

    public Task<AuthResponse?> LoginAsync(LoginRequest req) =>
        PostAsync<AuthResponse>("api/auth/login", req);

    public Task<AuthResponse?> RegisterAsync(RegisterRequest req) =>
        PostAsync<AuthResponse>("api/auth/register", req);

    public Task<List<QuizSummaryDto>?> GetQuizzesAsync() =>
        GetAsync<List<QuizSummaryDto>>("api/quizzes");

    public Task<List<RoomDto>?> GetActiveRoomsAsync() =>
        GetAsync<List<RoomDto>>("api/rooms/active");

    public Task<RoomStateDto?> GetRoomAsync(string code) =>
        GetAsync<RoomStateDto>($"api/rooms/{code}");

    public Task<RoomStateDto?> CreateRoomAsync(CreateRoomRequest req) =>
        PostAsync<RoomStateDto>("api/rooms", req);

    private async Task<T?> GetAsync<T>(string url)
    {
        var resp = await Client().GetAsync(url);
        if (!resp.IsSuccessStatusCode) return default;
        return await resp.Content.ReadFromJsonAsync<T>(Json);
    }

    private async Task<T?> PostAsync<T>(string url, object body)
    {
        var resp = await Client().PostAsJsonAsync(url, body);
        if (!resp.IsSuccessStatusCode)
        {
            var err = await resp.Content.ReadFromJsonAsync<JsonElement>(Json);
            var msg = err.TryGetProperty("error", out var e) ? e.GetString() : resp.ReasonPhrase;
            throw new InvalidOperationException(msg ?? "Request failed");
        }
        return await resp.Content.ReadFromJsonAsync<T>(Json);
    }
}
