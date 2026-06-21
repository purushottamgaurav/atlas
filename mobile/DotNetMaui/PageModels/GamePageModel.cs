using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DotNetMaui.PageModels;

public enum AnswerState { Available, Selected, Correct, Wrong }

public partial class AnswerButtonModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BackgroundColor))]
    [NotifyPropertyChangedFor(nameof(BorderColor))]
    private AnswerState state = AnswerState.Available;

    [ObservableProperty] private AnswerOptionDto answer = new();

    public Color BackgroundColor => State switch
    {
        AnswerState.Selected => Color.FromArgb("#2D2D80"),
        AnswerState.Correct  => Color.FromArgb("#1A4A2E"),
        AnswerState.Wrong    => Color.FromArgb("#4A1A1A"),
        _                    => Color.FromArgb("#1E1E38")
    };

    public Color BorderColor => State switch
    {
        AnswerState.Selected => Color.FromArgb("#6C63FF"),
        AnswerState.Correct  => Color.FromArgb("#2ECC71"),
        AnswerState.Wrong    => Color.FromArgb("#FF4757"),
        _                    => Color.FromArgb("#2D2D55")
    };
}

public partial class GamePageModel : ObservableObject
{
    private readonly HubService _hub;
    private readonly SessionStore _session;

    [ObservableProperty] private string questionText = string.Empty;

    [ObservableProperty] private int questionNumber;
    [ObservableProperty] private int totalQuestions;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TimerProgress))]
    private int timeLeft;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TimerProgress))]
    private int timeLimitSeconds;

    public double TimerProgress => TimeLimitSeconds > 0 ? (double)TimeLeft / TimeLimitSeconds : 0;
    [ObservableProperty] private int myScore;
    [ObservableProperty] private int pointsEarned;
    [ObservableProperty] private bool showPointsEarned;
    [ObservableProperty] private string statusMessage = string.Empty;
    [ObservableProperty] private ObservableCollection<AnswerButtonModel> answerButtons = [];
    [ObservableProperty] private ObservableCollection<PlayerScoreDto> roundScores = [];
    [ObservableProperty] private bool showRoundResults;
    [ObservableProperty] private bool inputEnabled;

    private IDispatcherTimer? _timer;
    private string _roomCode = string.Empty;
    private int _selectedAnswerId = -1;

    public GamePageModel(HubService hub, SessionStore session)
    {
        _hub = hub;
        _session = session;
    }

    public void Attach()
    {
        _hub.QuestionStarted += OnQuestionStarted;
        _hub.AnswerReceived += OnAnswerReceived;
        _hub.QuestionEnded += OnQuestionEnded;
        _hub.GameEnded += OnGameEnded;
        _hub.PlayerAnswered += OnPlayerAnswered;
        _hub.ErrorReceived += OnErrorReceived;

        // Load current question from session if available
        if (_session.CurrentQuestion is not null)
        {
            _roomCode = _session.CurrentRoomCode;
            LoadQuestion(_session.CurrentQuestion, _roomCode);
        }
    }

    public void Detach()
    {
        _hub.QuestionStarted -= OnQuestionStarted;
        _hub.AnswerReceived -= OnAnswerReceived;
        _hub.QuestionEnded -= OnQuestionEnded;
        _hub.GameEnded -= OnGameEnded;
        _hub.PlayerAnswered -= OnPlayerAnswered;
        _hub.ErrorReceived -= OnErrorReceived;

        _timer?.Stop();
        _timer = null;
    }

    private void LoadQuestion(GameQuestionDto question, string roomCode)
    {
        _roomCode = roomCode;
        _selectedAnswerId = -1;
        ShowRoundResults = false;
        ShowPointsEarned = false;
        InputEnabled = true;

        QuestionText = question.Text;
        QuestionNumber = question.QuestionNumber;
        TotalQuestions = question.TotalQuestions;
        TimeLimitSeconds = question.TimeLimitSeconds;
        TimeLeft = question.TimeLimitSeconds;

        AnswerButtons = new ObservableCollection<AnswerButtonModel>(
            question.Answers.Select(a => new AnswerButtonModel { Answer = a }));

        _timer?.Stop();
        _timer = Application.Current!.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (_, _) =>
        {
            if (TimeLeft > 0)
                TimeLeft--;
            else
                _timer.Stop();
        };
        _timer.Start();
    }

    private void OnQuestionStarted(GameQuestionDto question)
    {
        _session.CurrentQuestion = question;
        LoadQuestion(question, _session.CurrentRoomCode);
    }

    private void OnAnswerReceived(AnswerResultDto result)
    {
        PointsEarned = result.PointsEarned;
        ShowPointsEarned = true;
        StatusMessage = result.Message ?? string.Empty;
    }

    private void OnQuestionEnded(QuestionResultDto result)
    {
        _timer?.Stop();
        InputEnabled = false;

        // Reveal correct / wrong answers
        foreach (var btn in AnswerButtons)
        {
            if (btn.Answer.AnswerId == result.CorrectAnswerId)
                btn.State = AnswerState.Correct;
            else if (btn.Answer.AnswerId == _selectedAnswerId && btn.State == AnswerState.Selected)
                btn.State = AnswerState.Wrong;
        }

        if (result.PlayerScores is not null)
        {
            RoundScores = new ObservableCollection<PlayerScoreDto>(result.PlayerScores);
            var me = result.PlayerScores.FirstOrDefault(p => p.UserId == _session.UserId);
            if (me is not null) MyScore = me.TotalScore;
        }

        ShowRoundResults = true;
    }

    private void OnGameEnded(GameResultDto result)
    {
        _timer?.Stop();
        _session.LastGameResult = result;
        MainThread.BeginInvokeOnMainThread(async () =>
            await Shell.Current.GoToAsync("results"));
    }

    private void OnPlayerAnswered(string username)
    {
        StatusMessage = $"{username} answered!";
    }

    private void OnErrorReceived(string message)
    {
        StatusMessage = $"Error: {message}";
    }

    [RelayCommand]
    private async Task SelectAnswerAsync(AnswerButtonModel button)
    {
        if (!InputEnabled || _selectedAnswerId != -1) return;

        _selectedAnswerId = button.Answer.AnswerId;
        button.State = AnswerState.Selected;
        InputEnabled = false;

        try
        {
            await _hub.SubmitAnswerAsync(_roomCode, _session.CurrentQuestion!.QuestionId, button.Answer.AnswerId);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }
}
