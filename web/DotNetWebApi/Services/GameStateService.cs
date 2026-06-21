using DotNetWebApi.Models;
using System.Collections.Concurrent;

namespace DotNetWebApi.Services;

public class PlayerAnswer
{
    public int AnswerId { get; set; }
    public bool IsCorrect { get; set; }
    public int PointsEarned { get; set; }
    public DateTime AnsweredAt { get; set; }
}

public class ActiveGame
{
    public string RoomCode { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public List<Question> Questions { get; set; } = new();
    public int CurrentQuestionIndex { get; set; } = -1;
    public DateTime QuestionStartedAt { get; set; }
    public ConcurrentDictionary<int, PlayerAnswer> Answers { get; set; } = new();
    public Dictionary<int, int> PlayerScores { get; set; } = new();
    public Dictionary<int, string> PlayerNames { get; set; } = new();
    public CancellationTokenSource? QuestionCts { get; set; }
    public bool IsRunning { get; set; }

    public Question? CurrentQuestion =>
        CurrentQuestionIndex >= 0 && CurrentQuestionIndex < Questions.Count
            ? Questions[CurrentQuestionIndex]
            : null;
}

public class GameStateService
{
    private readonly ConcurrentDictionary<string, ActiveGame> _games = new();

    public ActiveGame CreateGame(string roomCode, int quizId, string quizTitle, List<Question> questions, Dictionary<int, string> players)
    {
        var game = new ActiveGame
        {
            RoomCode = roomCode,
            QuizId = quizId,
            QuizTitle = quizTitle,
            Questions = questions.OrderBy(q => q.OrderNumber).ToList(),
            PlayerNames = new Dictionary<int, string>(players),
            PlayerScores = players.Keys.ToDictionary(id => id, _ => 0),
            IsRunning = true
        };
        _games[roomCode] = game;
        return game;
    }

    public ActiveGame? GetGame(string roomCode) =>
        _games.TryGetValue(roomCode, out var game) ? game : null;

    public bool IsGameActive(string roomCode) => _games.ContainsKey(roomCode);

    public PlayerAnswer? RecordAnswer(string roomCode, int userId, int answerId)
    {
        var game = GetGame(roomCode);
        if (game == null || game.CurrentQuestion == null) return null;
        if (game.Answers.ContainsKey(userId)) return null; // already answered

        var question = game.CurrentQuestion;
        var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
        var isCorrect = correctAnswer?.AnswerId == answerId;

        int points = 0;
        if (isCorrect)
        {
            var elapsed = (DateTime.UtcNow - game.QuestionStartedAt).TotalSeconds;
            var remaining = Math.Max(0, question.TimeLimitSeconds - elapsed);
            var ratio = remaining / question.TimeLimitSeconds;
            points = (int)(question.Points * Math.Max(0.3, ratio));
            game.PlayerScores[userId] = game.PlayerScores.GetValueOrDefault(userId, 0) + points;
        }

        var answer = new PlayerAnswer
        {
            AnswerId = answerId,
            IsCorrect = isCorrect,
            PointsEarned = points,
            AnsweredAt = DateTime.UtcNow
        };

        game.Answers[userId] = answer;

        // Cancel the timer if all players have answered
        if (game.Answers.Count >= game.PlayerNames.Count)
            game.QuestionCts?.Cancel();

        return answer;
    }

    public void AddPlayer(string roomCode, int userId, string username)
    {
        var game = GetGame(roomCode);
        if (game == null) return;
        game.PlayerNames[userId] = username;
        if (!game.PlayerScores.ContainsKey(userId))
            game.PlayerScores[userId] = 0;
    }

    public void RemoveGame(string roomCode) => _games.TryRemove(roomCode, out _);
}
