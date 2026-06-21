using System.Net.Http.Json;
using System.Text.Json;

namespace DotNetMaui.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly SessionStore _session;

    public ApiService(SessionStore session)
    {
        _session = session;
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };
        _http = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7008/")
        };
    }

    private void SetAuthHeader()
    {
        _http.DefaultRequestHeaders.Authorization =
            string.IsNullOrEmpty(_session.Token)
                ? null
                : new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _session.Token);
    }

    private async Task<T> ReadAsync<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string body = await response.Content.ReadAsStringAsync();
            string msg = body;
            try
            {
                var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("message", out var m))
                    msg = m.GetString() ?? body;
                else if (doc.RootElement.TryGetProperty("title", out var t))
                    msg = t.GetString() ?? body;
            }
            catch { }
            throw new InvalidOperationException(msg);
        }
        return (await response.Content.ReadFromJsonAsync<T>())!;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);
        return await ReadAsync<AuthResponse>(response);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", request);
        return await ReadAsync<AuthResponse>(response);
    }

    public async Task<List<QuizSummaryDto>> GetQuizzesAsync()
    {
        SetAuthHeader();
        var response = await _http.GetAsync("api/quizzes");
        return await ReadAsync<List<QuizSummaryDto>>(response);
    }

    public async Task<RoomStateDto> CreateRoomAsync(CreateRoomRequest request)
    {
        SetAuthHeader();
        var response = await _http.PostAsJsonAsync("api/rooms", request);
        return await ReadAsync<RoomStateDto>(response);
    }

    public async Task<List<RoomDto>> GetActiveRoomsAsync()
    {
        SetAuthHeader();
        var response = await _http.GetAsync("api/rooms");
        return await ReadAsync<List<RoomDto>>(response);
    }

    public async Task<RoomStateDto> GetRoomAsync(string code)
    {
        SetAuthHeader();
        var response = await _http.GetAsync($"api/rooms/{code}");
        return await ReadAsync<RoomStateDto>(response);
    }
}
