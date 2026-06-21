using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DotNetMaui.PageModels;

public partial class RoomPageModel : ObservableObject
{
    private HubService? _hub;
    private readonly SessionStore _session;

    [ObservableProperty] private string roomCode = string.Empty;
    [ObservableProperty] private string quizTitle = string.Empty;
    [ObservableProperty] private ObservableCollection<PlayerDto> players = [];
    [ObservableProperty] private bool isHost;
    [ObservableProperty] private string statusMessage = string.Empty;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private bool isCountingDown;
    [ObservableProperty] private int countdown;

    private IDispatcherTimer? _countdownTimer;

    public RoomPageModel(SessionStore session)
    {
        _session = session;
    }

    public void Initialize(RoomStateDto state, bool isHost, HubService hub)
    {
        _hub = hub;
        RoomCode = state.Code;
        QuizTitle = state.QuizTitle ?? string.Empty;
        IsHost = isHost;
        Players = new ObservableCollection<PlayerDto>(state.Players);
        StatusMessage = "Waiting for players...";

        SubscribeHubEvents();
    }

    private void SubscribeHubEvents()
    {
        if (_hub is null) return;
        _hub.RoomStateReceived += OnRoomStateReceived;
        _hub.PlayerJoined += OnPlayerJoined;
        _hub.PlayerLeft += OnPlayerLeft;
        _hub.GameStarting += OnGameStarting;
        _hub.QuestionStarted += OnQuestionStarted;
        _hub.ErrorReceived += OnErrorReceived;
    }

    public void Unsubscribe()
    {
        if (_hub is null) return;
        _hub.RoomStateReceived -= OnRoomStateReceived;
        _hub.PlayerJoined -= OnPlayerJoined;
        _hub.PlayerLeft -= OnPlayerLeft;
        _hub.GameStarting -= OnGameStarting;
        _hub.QuestionStarted -= OnQuestionStarted;
        _hub.ErrorReceived -= OnErrorReceived;

        _countdownTimer?.Stop();
        _countdownTimer = null;
    }

    private void OnRoomStateReceived(RoomStateDto state)
    {
        Players = new ObservableCollection<PlayerDto>(state.Players);
    }

    private void OnPlayerJoined(PlayerDto player)
    {
        Players.Add(player);
        StatusMessage = $"{player.Username} joined";
    }

    private void OnPlayerLeft(string username)
    {
        var player = Players.FirstOrDefault(p => p.Username == username);
        if (player is not null)
            Players.Remove(player);
        StatusMessage = $"{username} left";
    }

    private void OnGameStarting(int countdownSeconds)
    {
        IsCountingDown = true;
        Countdown = countdownSeconds;
        StatusMessage = "Game starting!";

        _countdownTimer?.Stop();
        _countdownTimer = Application.Current!.Dispatcher.CreateTimer();
        _countdownTimer.Interval = TimeSpan.FromSeconds(1);
        _countdownTimer.Tick += (_, _) =>
        {
            Countdown--;
            if (Countdown <= 0)
            {
                _countdownTimer.Stop();
                IsCountingDown = false;
            }
        };
        _countdownTimer.Start();
    }

    private void OnQuestionStarted(GameQuestionDto question)
    {
        Unsubscribe();
        _session.CurrentQuestion = question;
        MainThread.BeginInvokeOnMainThread(async () =>
            await Shell.Current.GoToAsync("game"));
    }

    private void OnErrorReceived(string message)
    {
        StatusMessage = $"Error: {message}";
    }

    [RelayCommand]
    private async Task StartGameAsync()
    {
        if (_hub is null || !IsHost) return;
        IsBusy = true;
        try
        {
            await _hub.StartGameAsync(RoomCode);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LeaveRoomAsync()
    {
        Unsubscribe();
        if (_hub is not null)
        {
            try { await _hub.LeaveRoomAsync(RoomCode); } catch { }
        }
        await Shell.Current.GoToAsync("//lobby");
    }
}
