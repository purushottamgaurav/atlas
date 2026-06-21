using DotNetWpf.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace DotNetWpf.Services;

// Thin wrapper around HttpClient — handles auth header injection,
// JSON deserialization, and converts HTTP errors into readable exceptions.
public class ApiService
{
    private readonly HttpClient _http;
    private readonly SessionStore _session;
    private readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public ApiService(SessionStore session)
    {
        _session = session;

        // Bypass dev cert validation — remove this for production
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        _http = new HttpClient(handler) { BaseAddress = new Uri("https://localhost:7008/") };
    }

    // ── Auth ─────────────────────────────────────────────────────────────────

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/login",
            new LoginRequest { Email = email, Password = password });
        return await ReadAsync<AuthResponse>(resp);
    }

    public async Task<AuthResponse> RegisterAsync(string username, string email, string password)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/register",
            new RegisterRequest { Username = username, Email = email, Password = password });
        return await ReadAsync<AuthResponse>(resp);
    }

    // ── Quizzes ───────────────────────────────────────────────────────────────

    public async Task<List<QuizSummaryDto>> GetQuizzesAsync()
    {
        SetAuthHeader();
        var resp = await _http.GetAsync("api/quizzes");
        return await ReadAsync<List<QuizSummaryDto>>(resp);
    }

    // ── Rooms ─────────────────────────────────────────────────────────────────

    public async Task<RoomStateDto> CreateRoomAsync(int quizId, int maxPlayers = 8)
    {
        SetAuthHeader();
        var resp = await _http.PostAsJsonAsync("api/rooms",
            new CreateRoomRequest { QuizId = quizId, MaxPlayers = maxPlayers });
        return await ReadAsync<RoomStateDto>(resp);
    }

    public async Task<List<RoomDto>> GetActiveRoomsAsync()
    {
        SetAuthHeader();
        var resp = await _http.GetAsync("api/rooms/active");
        return await ReadAsync<List<RoomDto>>(resp);
    }

    public async Task<RoomStateDto> GetRoomAsync(string code)
    {
        SetAuthHeader();
        var resp = await _http.GetAsync($"api/rooms/{code}");
        return await ReadAsync<RoomStateDto>(resp);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void SetAuthHeader()
    {
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _session.Token);
    }

    private async Task<T> ReadAsync<T>(HttpResponseMessage resp)
    {
        if (!resp.IsSuccessStatusCode)
        {
            string detail;
            try
            {
                var err = await resp.Content.ReadFromJsonAsync<JsonElement>(_json);
                detail = err.TryGetProperty("error", out var e) ? e.GetString()! : resp.ReasonPhrase!;
            }
            catch
            {
                detail = resp.ReasonPhrase ?? "Unknown error";
            }
            throw new InvalidOperationException(detail);
        }

        return (await resp.Content.ReadFromJsonAsync<T>(_json))!;
    }
}
