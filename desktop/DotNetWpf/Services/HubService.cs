using DotNetWpf.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace DotNetWpf.Services;

// Wraps the SignalR connection to the WebAPI /hubs/quiz endpoint.
// Exposes strongly-typed C# events that ViewModels subscribe to.
// All event callbacks are dispatched to the UI thread so ViewModels
// can safely update ObservableCollections and bound properties.
public class HubService
{
    private HubConnection? _connection;
    private readonly SessionStore _session;

    // ── Server → Client events ────────────────────────────────────────────────
    public event Action<RoomStateDto>?    RoomStateReceived;
    public event Action<PlayerDto>?       PlayerJoined;
    public event Action<string>?          PlayerLeft;
    public event Action<int>?             GameStarting;       // payload = countdown seconds
    public event Action<GameQuestionDto>? QuestionStarted;
    public event Action<AnswerResultDto>? AnswerReceived;
    public event Action<string>?          PlayerAnswered;     // username who answered
    public event Action<QuestionResultDto>? QuestionEnded;
    public event Action<GameResultDto>?   GameEnded;
    public event Action<string>?          ErrorReceived;
    public event Action?                  Reconnecting;
    public event Action?                  Reconnected;

    public HubConnectionState State =>
        _connection?.State ?? HubConnectionState.Disconnected;

    public HubService(SessionStore session) => _session = session;

    public async Task ConnectAsync()
    {
        if (_connection != null)
            await _connection.DisposeAsync();

        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7008/hubs/quiz", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(_session.Token);
                // Bypass dev cert — remove for production
                options.HttpMessageHandlerFactory = _ => new System.Net.Http.HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                };
            })
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers();

        _connection.Reconnecting  += _ => { Dispatch(() => Reconnecting?.Invoke()); return Task.CompletedTask; };
        _connection.Reconnected   += _ => { Dispatch(() => Reconnected?.Invoke()); return Task.CompletedTask; };

        await _connection.StartAsync();
    }

    private void RegisterHandlers()
    {
        var conn = _connection!;
        conn.On<RoomStateDto>("RoomState",
            dto => Dispatch(() => RoomStateReceived?.Invoke(dto)));

        conn.On<PlayerDto>("PlayerJoined",
            dto => Dispatch(() => PlayerJoined?.Invoke(dto)));

        conn.On<string>("PlayerLeft",
            name => Dispatch(() => PlayerLeft?.Invoke(name)));

        // Server sends anonymous { countdown: 3 }; extract via JsonElement
        conn.On<System.Text.Json.JsonElement>("GameStarting",
            data =>
            {
                var cd = data.TryGetProperty("countdown", out var v) ? v.GetInt32() : 3;
                Dispatch(() => GameStarting?.Invoke(cd));
            });

        conn.On<GameQuestionDto>("QuestionStarted",
            dto => Dispatch(() => QuestionStarted?.Invoke(dto)));

        conn.On<System.Text.Json.JsonElement>("AnswerReceived",
            data =>
            {
                var dto = new AnswerResultDto
                {
                    Success      = data.TryGetProperty("success",      out var s) && s.GetBoolean(),
                    PointsEarned = data.TryGetProperty("pointsEarned", out var p) ? p.GetInt32() : 0,
                    Message      = data.TryGetProperty("message",      out var m) ? m.GetString() : null,
                };
                Dispatch(() => AnswerReceived?.Invoke(dto));
            });

        conn.On<string>("PlayerAnsweredNotification",
            name => Dispatch(() => PlayerAnswered?.Invoke(name)));

        conn.On<QuestionResultDto>("QuestionEnded",
            dto => Dispatch(() => QuestionEnded?.Invoke(dto)));

        conn.On<GameResultDto>("GameEnded",
            dto => Dispatch(() => GameEnded?.Invoke(dto)));

        conn.On<string>("Error",
            msg => Dispatch(() => ErrorReceived?.Invoke(msg)));
    }

    // ── Client → Server methods ───────────────────────────────────────────────

    public Task JoinRoomAsync(string code) =>
        _connection!.InvokeAsync("JoinRoom", code);

    public Task LeaveRoomAsync(string code) =>
        _connection!.InvokeAsync("LeaveRoom", code);

    public Task StartGameAsync(string code) =>
        _connection!.InvokeAsync("StartGame", code);

    public Task SubmitAnswerAsync(string code, int questionId, int answerId) =>
        _connection!.InvokeAsync("SubmitAnswer", code, questionId, answerId);

    public async Task DisconnectAsync()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }
    }

    private static void Dispatch(Action action) =>
        System.Windows.Application.Current.Dispatcher.Invoke(action);
}
