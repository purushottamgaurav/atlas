using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DotNetMaui.PageModels;

public partial class ResultsPageModel : ObservableObject
{
    private readonly HubService _hub;
    private readonly SessionStore _session;
    private readonly LobbyPageModel _lobby;

    [ObservableProperty] private ObservableCollection<LeaderboardEntryDto> leaderboard = [];
    [ObservableProperty] private int myRank;
    [ObservableProperty] private int myScore;
    [ObservableProperty] private string winnerName = string.Empty;

    public ResultsPageModel(HubService hub, SessionStore session, LobbyPageModel lobby)
    {
        _hub = hub;
        _session = session;
        _lobby = lobby;
    }

    public void Initialize()
    {
        if (_session.LastGameResult is null) return;
        InitializeFromResult(_session.LastGameResult);
    }

    private void InitializeFromResult(GameResultDto result)
    {
        if (result.Leaderboard is null) return;

        Leaderboard = new ObservableCollection<LeaderboardEntryDto>(result.Leaderboard);

        var me = result.Leaderboard.FirstOrDefault(e => e.UserId == _session.UserId);
        if (me is not null)
        {
            MyRank = me.Rank;
            MyScore = me.TotalScore;
        }

        var winner = result.Leaderboard.FirstOrDefault(e => e.Rank == 1);
        WinnerName = winner?.Username ?? string.Empty;
    }

    [RelayCommand]
    private async Task BackToLobbyAsync()
    {
        try { await _hub.DisconnectAsync(); } catch { }
        await Shell.Current.GoToAsync("//lobby");
        await _lobby.InitializeAsync();
    }
}
