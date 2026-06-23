using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetWpf.Models;
using DotNetWpf.Services;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace DotNetWpf.ViewModels;

public enum AnswerState { Available, Selected, Correct, Wrong }

public partial class AnswerButtonViewModel : ObservableObject
{
    [ObservableProperty] private AnswerOptionDto _answer = null!;
    [ObservableProperty] private AnswerState _state = AnswerState.Available;
}

public partial class GameViewModel : ObservableObject
{
    private readonly HubService _hub;
    private readonly NavigationService _nav;

    private string _roomCode = string.Empty;
    private int? _selectedAnswerId;
    private readonly DispatcherTimer _timer;

    [ObservableProperty] private string _questionText = string.Empty;
    [ObservableProperty] private int _questionNumber;
    [ObservableProperty] private int _totalQuestions;
    [ObservableProperty] private int _timeLeft;
    [ObservableProperty] private int _timeLimitSeconds;
    [ObservableProperty] private int _myScore;
    [ObservableProperty] private int _pointsEarned;
    [ObservableProperty] private bool _showPointsEarned;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private ObservableCollection<AnswerButtonViewModel> _answerButtons = [];
    [ObservableProperty] private ObservableCollection<PlayerScoreDto> _roundScores = [];
    [ObservableProperty] private bool _showRoundResults;
    [ObservableProperty] private bool _inputEnabled = true;

    private int _currentQuestionId;

    public GameViewModel(HubService hub, NavigationService nav)
    {
        _hub = hub;
        _nav = nav;

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += OnTimerTick;

        _hub.AnswerReceived += OnAnswerReceived;
        _hub.QuestionEnded += OnQuestionEnded;
        _hub.QuestionStarted += OnQuestionStarted;
        _hub.GameEnded += OnGameEnded;
        _hub.PlayerAnswered += name => StatusMessage = $"{name} answered!";
        _hub.ErrorReceived += msg => StatusMessage = msg;
    }

    public void LoadQuestion(GameQuestionDto question, string roomCode)
    {
        _roomCode = roomCode;
        _currentQuestionId = question.QuestionId;
        _selectedAnswerId = null;

        QuestionText = question.Text;
        QuestionNumber = question.QuestionNumber;
        TotalQuestions = question.TotalQuestions;
        TimeLimitSeconds = question.TimeLimitSeconds;
        TimeLeft = question.TimeLimitSeconds;
        StatusMessage = string.Empty;
        ShowRoundResults = false;
        ShowPointsEarned = false;
        InputEnabled = true;

        AnswerButtons = new ObservableCollection<AnswerButtonViewModel>(
            question.Answers.Select(a => new AnswerButtonViewModel { Answer = a }));

        _timer.Stop();
        _timer.Start();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        TimeLeft--;
        if (TimeLeft <= 0)
        {
            _timer.Stop();
            InputEnabled = false;
            StatusMessage = "Time's up!";
        }
    }

    [RelayCommand]
    private async Task SelectAnswerAsync(AnswerButtonViewModel button)
    {
        if (!InputEnabled || _selectedAnswerId.HasValue) return;

        _selectedAnswerId = button.Answer.AnswerId;
        InputEnabled = false;
        _timer.Stop();

        button.State = AnswerState.Selected;

        try
        {
            await _hub.SubmitAnswerAsync(_roomCode, _currentQuestionId, button.Answer.AnswerId);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void OnAnswerReceived(AnswerResultDto result)
    {
        PointsEarned = result.PointsEarned;
        ShowPointsEarned = result.PointsEarned > 0;
        StatusMessage = result.Message ?? (result.Success ? "Correct!" : "Wrong answer");
    }

    private void OnQuestionStarted(GameQuestionDto question)
    {
        LoadQuestion(question, _roomCode);
    }

    private void OnQuestionEnded(QuestionResultDto result)
    {
        _timer.Stop();
        InputEnabled = false;

        // Reveal correct / wrong state on buttons
        foreach (var btn in AnswerButtons)
        {
            if (btn.Answer.AnswerId == result.CorrectAnswerId)
                btn.State = AnswerState.Correct;
            else if (btn.State == AnswerState.Selected)
                btn.State = AnswerState.Wrong;
        }

        RoundScores = new ObservableCollection<PlayerScoreDto>(result.PlayerScores ?? []);
        ShowRoundResults = true;

        // Update my running score
        var me = result.PlayerScores?.FirstOrDefault(p => p.Username == _hub.GetType().Name);
        if (me != null) MyScore = me.TotalScore;
    }

    private void OnGameEnded(GameResultDto result)
    {
        _timer.Stop();
        Detach();

        var resultsVm = _nav.Get<ResultsViewModel>();
        resultsVm.Initialize(result);
        _nav.NavigateTo<ResultsViewModel>();
    }

    private void Detach()
    {
        _hub.AnswerReceived -= OnAnswerReceived;
        _hub.QuestionEnded -= OnQuestionEnded;
        _hub.QuestionStarted -= OnQuestionStarted;
        _hub.GameEnded -= OnGameEnded;
    }
}
