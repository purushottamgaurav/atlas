namespace DotNetBlazor.Services;

using DotNetBlazor.Models;
using Microsoft.AspNetCore.SignalR.Client;

public class HubService(SessionService session) : IAsyncDisposable
{
    private HubConnection? _conn;

    // Events — subscribers (Blazor components) must call InvokeAsync(StateHasChanged)
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

    public HubConnectionState State => _conn?.State ?? HubConnectionState.Disconnected;

    public async Task ConnectAsync()
    {
        if (_conn is not null) await _conn.DisposeAsync();

        _conn = new HubConnectionBuilder()
            .WithUrl("https://localhost:7008/hubs/quiz", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(session.Token);
                options.HttpMessageHandlerFactory = _ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                };
            })
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers();
        await _conn.StartAsync();
    }

    private void RegisterHandlers()
    {
        var c = _conn!;
        c.On<RoomStateDto>("RoomState", dto => RoomStateReceived?.Invoke(dto));
        c.On<PlayerDto>("PlayerJoined", dto => PlayerJoined?.Invoke(dto));
        c.On<string>("PlayerLeft", name => PlayerLeft?.Invoke(name));
        c.On<System.Text.Json.JsonElement>("GameStarting",
            data => GameStarting?.Invoke(data.TryGetProperty("countdown", out var v) ? v.GetInt32() : 3));
        c.On<GameQuestionDto>("QuestionStarted", dto => QuestionStarted?.Invoke(dto));
        c.On<System.Text.Json.JsonElement>("AnswerReceived",
            data => AnswerReceived?.Invoke(new AnswerResultDto(
                data.TryGetProperty("success", out var s) && s.GetBoolean(),
                data.TryGetProperty("pointsEarned", out var p) ? p.GetInt32() : 0,
                data.TryGetProperty("message", out var m) ? m.GetString() : null)));
        c.On<string>("PlayerAnsweredNotification", name => PlayerAnswered?.Invoke(name));
        c.On<QuestionResultDto>("QuestionEnded", dto => QuestionEnded?.Invoke(dto));
        c.On<GameResultDto>("GameEnded", dto => GameEnded?.Invoke(dto));
        c.On<string>("Error", msg => ErrorReceived?.Invoke(msg));
    }

    public Task JoinRoomAsync(string code) => _conn!.InvokeAsync("JoinRoom", code);
    public Task LeaveRoomAsync(string code) => _conn!.InvokeAsync("LeaveRoom", code);
    public Task StartGameAsync(string code) => _conn!.InvokeAsync("StartGame", code);
    public Task SubmitAnswerAsync(string code, int questionId, int answerId) =>
        _conn!.InvokeAsync("SubmitAnswer", code, questionId, answerId);

    public async ValueTask DisposeAsync()
    {
        if (_conn is not null)
        {
            await _conn.StopAsync();
            await _conn.DisposeAsync();
        }
    }
}
