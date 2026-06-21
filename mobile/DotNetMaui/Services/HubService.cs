using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace DotNetMaui.Services;

public class HubService
{
    private readonly SessionStore _session;
    private HubConnection? _connection;

    public event Action<RoomStateDto>? RoomStateReceived;
    public event Action<PlayerDto>? PlayerJoined;
    public event Action<string>? PlayerLeft;
    public event Action<int>? GameStarting;
    public event Action<GameQuestionDto>? QuestionStarted;
    public event Action<AnswerResultDto>? AnswerReceived;
    public event Action<string>? PlayerAnswered;
    public event Action<QuestionResultDto>? QuestionEnded;
    public event Action<GameResultDto>? GameEnded;
    public event Action<string>? ErrorReceived;

    public HubService(SessionStore session)
    {
        _session = session;
    }

    public async Task ConnectAsync()
    {
        if (_connection is not null && _connection.State != HubConnectionState.Disconnected)
            return;

        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7008/hubs/quiz", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(_session.Token);
                options.HttpMessageHandlerFactory = _ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                };
            })
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers();
        await _connection.StartAsync();
    }

    private void RegisterHandlers()
    {
        if (_connection is null) return;

        _connection.On<RoomStateDto>("RoomState", state =>
            MainThread.BeginInvokeOnMainThread(() => RoomStateReceived?.Invoke(state)));

        _connection.On<PlayerDto>("PlayerJoined", player =>
            MainThread.BeginInvokeOnMainThread(() => PlayerJoined?.Invoke(player)));

        _connection.On<string>("PlayerLeft", username =>
            MainThread.BeginInvokeOnMainThread(() => PlayerLeft?.Invoke(username)));

        _connection.On<JsonElement>("GameStarting", element =>
        {
            int countdown = 0;
            if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("countdown", out var c))
                countdown = c.GetInt32();
            else if (element.ValueKind == JsonValueKind.Number)
                countdown = element.GetInt32();
            MainThread.BeginInvokeOnMainThread(() => GameStarting?.Invoke(countdown));
        });

        _connection.On<GameQuestionDto>("QuestionStarted", question =>
            MainThread.BeginInvokeOnMainThread(() => QuestionStarted?.Invoke(question)));

        _connection.On<JsonElement>("AnswerResult", element =>
        {
            var result = JsonSerializer.Deserialize<AnswerResultDto>(element.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AnswerResultDto();
            MainThread.BeginInvokeOnMainThread(() => AnswerReceived?.Invoke(result));
        });

        _connection.On<string>("PlayerAnswered", username =>
            MainThread.BeginInvokeOnMainThread(() => PlayerAnswered?.Invoke(username)));

        _connection.On<QuestionResultDto>("QuestionEnded", result =>
            MainThread.BeginInvokeOnMainThread(() => QuestionEnded?.Invoke(result)));

        _connection.On<GameResultDto>("GameEnded", result =>
            MainThread.BeginInvokeOnMainThread(() => GameEnded?.Invoke(result)));

        _connection.On<string>("Error", message =>
            MainThread.BeginInvokeOnMainThread(() => ErrorReceived?.Invoke(message)));
    }

    public async Task JoinRoomAsync(string roomCode)
    {
        if (_connection is null) throw new InvalidOperationException("Not connected");
        await _connection.InvokeAsync("JoinRoom", roomCode);
    }

    public async Task LeaveRoomAsync(string roomCode)
    {
        if (_connection is null) return;
        try { await _connection.InvokeAsync("LeaveRoom", roomCode); } catch { }
    }

    public async Task StartGameAsync(string roomCode)
    {
        if (_connection is null) throw new InvalidOperationException("Not connected");
        await _connection.InvokeAsync("StartGame", roomCode);
    }

    public async Task SubmitAnswerAsync(string roomCode, int questionId, int answerId)
    {
        if (_connection is null) throw new InvalidOperationException("Not connected");
        await _connection.InvokeAsync("SubmitAnswer", roomCode, questionId, answerId);
    }

    public async Task DisconnectAsync()
    {
        if (_connection is not null)
        {
            try { await _connection.StopAsync(); } catch { }
            await _connection.DisposeAsync();
            _connection = null;
        }
    }
}
