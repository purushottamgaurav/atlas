using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetWpf.Models;
using DotNetWpf.Services;
using System.Collections.ObjectModel;

namespace DotNetWpf.ViewModels;

public partial class ResultsViewModel : ObservableObject
{
    private readonly NavigationService _nav;
    private readonly HubService _hub;
    private readonly SessionStore _session;

    [ObservableProperty] private ObservableCollection<LeaderboardEntryDto> _leaderboard = [];
    [ObservableProperty] private int _myRank;
    [ObservableProperty] private int _myScore;
    [ObservableProperty] private string _myUsername = string.Empty;
    [ObservableProperty] private string _winnerName = string.Empty;

    public ResultsViewModel(NavigationService nav, HubService hub, SessionStore session)
    {
        _nav = nav;
        _hub = hub;
        _session = session;
    }

    public void Initialize(GameResultDto result)
    {
        Leaderboard = new ObservableCollection<LeaderboardEntryDto>(result.Leaderboard ?? []);
        MyUsername = _session.Username;

        var me = result.Leaderboard?.FirstOrDefault(e => e.UserId == _session.UserId);
        if (me != null)
        {
            MyRank  = me.Rank;
            MyScore = me.TotalScore;
        }

        WinnerName = result.Leaderboard?.FirstOrDefault()?.Username ?? string.Empty;
    }

    [RelayCommand]
    private async Task BackToLobbyAsync()
    {
        try { await _hub.DisconnectAsync(); } catch { /* ignore */ }

        var lobbyVm = _nav.Get<LobbyViewModel>();
        _nav.NavigateTo<LobbyViewModel>();
        _ = lobbyVm.InitializeAsync();
    }
}
