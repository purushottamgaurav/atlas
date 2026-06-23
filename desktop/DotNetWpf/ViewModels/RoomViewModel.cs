using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetWpf.Models;
using DotNetWpf.Services;
using System.Collections.ObjectModel;

namespace DotNetWpf.ViewModels;

public partial class RoomViewModel : ObservableObject
{
    private readonly HubService _hub;
    private readonly NavigationService _nav;
    private readonly SessionStore _session;

    [ObservableProperty] private string _roomCode = string.Empty;
    [ObservableProperty] private string _quizTitle = string.Empty;
    [ObservableProperty] private ObservableCollection<PlayerDto> _players = [];
    [ObservableProperty] private bool _isHost;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _isCountingDown;
    [ObservableProperty] private int _countdown;

    public RoomViewModel(HubService hub, NavigationService nav, SessionStore session)
    {
        _hub = hub;
        _nav = nav;
        _session = session;

        _hub.RoomStateReceived += OnRoomState;
        _hub.PlayerJoined += OnPlayerJoined;
        _hub.PlayerLeft += OnPlayerLeft;
        _hub.GameStarting += OnGameStarting;
        _hub.QuestionStarted += OnQuestionStarted;
        _hub.ErrorReceived += msg => StatusMessage = msg;
    }

    public void Initialize(RoomStateDto state, bool isHost)
    {
        RoomCode = state.Code;
        QuizTitle = state.QuizTitle ?? string.Empty;
        IsHost = isHost;
        Players = new ObservableCollection<PlayerDto>(state.Players ?? []);
        StatusMessage = $"Room Code: {RoomCode} — Share this with friends!";
    }

    private void OnRoomState(RoomStateDto state)
    {
        Players = new ObservableCollection<PlayerDto>(state.Players ?? []);
    }

    private void OnPlayerJoined(PlayerDto player)
    {
        if (Players.All(p => p.UserId != player.UserId))
            Players.Add(player);
    }

    private void OnPlayerLeft(string username)
    {
        var player = Players.FirstOrDefault(p => p.Username == username);
        if (player != null) Players.Remove(player);
    }

    private async void OnGameStarting(int countdownSeconds)
    {
        IsCountingDown = true;
        Countdown = countdownSeconds;
        StatusMessage = "Game is starting!";

        for (int i = countdownSeconds; i > 0; i--)
        {
            Countdown = i;
            await Task.Delay(1000);
        }
    }

    private void OnQuestionStarted(GameQuestionDto question)
    {
        IsCountingDown = false;
        var gameVm = _nav.Get<GameViewModel>();
        gameVm.LoadQuestion(question, RoomCode);
        _nav.NavigateTo<GameViewModel>();
        Detach();
    }

    [RelayCommand(CanExecute = nameof(CanStart))]
    private async Task StartGameAsync()
    {
        IsBusy = true;
        StatusMessage = "Starting game...";
        try
        {
            await _hub.StartGameAsync(RoomCode);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            IsBusy = false;
        }
    }

    private bool CanStart() => IsHost && !IsBusy && Players.Count >= 1;

    [RelayCommand]
    private async Task LeaveRoomAsync()
    {
        try { await _hub.LeaveRoomAsync(RoomCode); } catch { /* ignore */ }
        Detach();
        _nav.NavigateTo<LobbyViewModel>();
    }

    private void Detach()
    {
        _hub.RoomStateReceived -= OnRoomState;
        _hub.PlayerJoined -= OnPlayerJoined;
        _hub.PlayerLeft -= OnPlayerLeft;
        _hub.GameStarting -= OnGameStarting;
        _hub.QuestionStarted -= OnQuestionStarted;
    }
}
