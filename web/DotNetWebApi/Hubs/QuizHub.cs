using DotNetWebApi.Data;
using DotNetWebApi.DTOs.Game;
using DotNetWebApi.DTOs.Question;
using DotNetWebApi.DTOs.Room;
using DotNetWebApi.Models;
using DotNetWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetWebApi.Hubs;

[Authorize]
public class QuizHub : Hub
{
    private readonly GameStateService _gameState;
    private readonly QuizDbContext _context;
    private readonly ILogger<QuizHub> _logger;

    public QuizHub(GameStateService gameState, QuizDbContext context, ILogger<QuizHub> logger)
    {
        _gameState = gameState;
        _context = context;
        _logger = logger;
    }

    private int GetUserId()
    {
        var principal = Context.User ?? throw new HubException("Unauthenticated.");
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue("sub")
            ?? throw new HubException("Unauthenticated.");
        return int.Parse(value);
    }

    private string GetUsername() =>
        Context.User!.FindFirstValue("username") ?? "Unknown";

    public async Task JoinRoom(string roomCode)
    {
        roomCode = roomCode.ToUpper();
        var userId = GetUserId();
        var username = GetUsername();

        var room = await _context.GameRooms
            .Include(r => r.Participants)
            .Include(r => r.Quiz)
            .FirstOrDefaultAsync(r => r.Code == roomCode);

        if (room == null)
        {
            await Clients.Caller.SendAsync("Error", $"Room '{roomCode}' not found.");
            return;
        }

        if (room.Status == RoomStatus.Completed)
        {
            await Clients.Caller.SendAsync("Error", "This game has already ended.");
            return;
        }

        if (room.Status == RoomStatus.Active)
        {
            await Clients.Caller.SendAsync("Error", "Game is already in progress.");
            return;
        }

        if (room.Participants.Count >= room.MaxPlayers && !room.Participants.Any(p => p.UserId == userId))
        {
            await Clients.Caller.SendAsync("Error", "Room is full.");
            return;
        }

        // Add participant to DB if not already there
        if (!room.Participants.Any(p => p.UserId == userId))
        {
            var participant = new GameParticipant
            {
                GameRoomId = room.GameRoomId,
                UserId = userId,
                Username = username
            };
            _context.GameParticipants.Add(participant);
            await _context.SaveChangesAsync();
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

        // Notify others that this player joined
        await Clients.OthersInGroup(roomCode).SendAsync("PlayerJoined", new PlayerDto
        {
            UserId = userId,
            Username = username,
            IsHost = userId == room.HostUserId,
            JoinedAt = DateTime.UtcNow
        });

        // Send current room state to the caller
        var freshRoom = await _context.GameRooms
            .Include(r => r.Quiz)
            .Include(r => r.Participants)
            .FirstAsync(r => r.Code == roomCode);

        var host = await _context.Users.FindAsync(freshRoom.HostUserId);

        await Clients.Caller.SendAsync("RoomState", new RoomStateDto
        {
            Code = freshRoom.Code,
            QuizId = freshRoom.QuizId,
            QuizTitle = freshRoom.Quiz?.Title ?? string.Empty,
            HostUsername = host?.Username ?? string.Empty,
            HostUserId = freshRoom.HostUserId,
            Status = freshRoom.Status,
            PlayerCount = freshRoom.Participants.Count,
            MaxPlayers = freshRoom.MaxPlayers,
            CreatedAt = freshRoom.CreatedAt,
            Players = freshRoom.Participants.Select(p => new PlayerDto
            {
                UserId = p.UserId,
                Username = p.Username,
                IsHost = p.UserId == freshRoom.HostUserId,
                JoinedAt = p.JoinedAt
            }).ToList()
        });
    }

    public async Task LeaveRoom(string roomCode)
    {
        roomCode = roomCode.ToUpper();
        var username = GetUsername();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);
        await Clients.Group(roomCode).SendAsync("PlayerLeft", username);
    }

    public async Task StartGame(string roomCode)
    {
        roomCode = roomCode.ToUpper();
        var userId = GetUserId();

        var room = await _context.GameRooms
            .Include(r => r.Participants)
            .Include(r => r.Quiz)
            .FirstOrDefaultAsync(r => r.Code == roomCode);

        if (room == null)
        {
            await Clients.Caller.SendAsync("Error", "Room not found.");
            return;
        }

        if (room.HostUserId != userId)
        {
            await Clients.Caller.SendAsync("Error", "Only the host can start the game.");
            return;
        }

        if (room.Status != RoomStatus.Waiting)
        {
            await Clients.Caller.SendAsync("Error", "Game is not in waiting state.");
            return;
        }

        if (room.Participants.Count < 1)
        {
            await Clients.Caller.SendAsync("Error", "At least 1 player must join before starting.");
            return;
        }

        var questions = await _context.Questions
            .Include(q => q.Answers)
            .Where(q => q.QuizId == room.QuizId)
            .OrderBy(q => q.OrderNumber)
            .ToListAsync();

        if (questions.Count == 0)
        {
            await Clients.Caller.SendAsync("Error", "This quiz has no questions.");
            return;
        }

        // Update room status
        room.Status = RoomStatus.Active;
        await _context.SaveChangesAsync();

        var players = room.Participants.ToDictionary(p => p.UserId, p => p.Username);
        var game = _gameState.CreateGame(roomCode, room.QuizId, room.Quiz?.Title ?? string.Empty, questions, players);

        _logger.LogInformation("Game started in room {RoomCode} with {PlayerCount} players", roomCode, players.Count);

        // Run the game loop in a background task so we don't block the hub method
        _ = Task.Run(() => RunGameLoopAsync(roomCode, game, room.GameRoomId, questions));
    }

    private async Task RunGameLoopAsync(string roomCode, ActiveGame game, int gameRoomId, List<Question> questions)
    {
        try
        {
            await Clients.Group(roomCode).SendAsync("GameStarting", new { countdown = 3 });
            await Task.Delay(3000);

            for (int i = 0; i < questions.Count; i++)
            {
                game.CurrentQuestionIndex = i;
                game.Answers = new System.Collections.Concurrent.ConcurrentDictionary<int, PlayerAnswer>();
                game.QuestionStartedAt = DateTime.UtcNow;

                var question = questions[i];
                var questionDto = new GameQuestionDto
                {
                    QuestionId = question.QuestionId,
                    Text = question.Text,
                    TimeLimitSeconds = question.TimeLimitSeconds,
                    Points = question.Points,
                    OrderNumber = question.OrderNumber,
                    QuestionNumber = i + 1,
                    TotalQuestions = questions.Count,
                    Answers = question.Answers.Select(a => new AnswerOptionDto
                    {
                        AnswerId = a.AnswerId,
                        Text = a.Text
                    }).ToList()
                };

                await Clients.Group(roomCode).SendAsync("QuestionStarted", questionDto);

                // Wait for timer or until all players answer
                using var cts = new CancellationTokenSource();
                game.QuestionCts = cts;

                try
                {
                    await Task.Delay(question.TimeLimitSeconds * 1000, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    // All players answered early — continue
                }

                // Compute results for this question
                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                var playerScores = game.PlayerNames.Select(kvp =>
                {
                    var pa = game.Answers.TryGetValue(kvp.Key, out var ans) ? ans : null;
                    return new PlayerScoreDto
                    {
                        UserId = kvp.Key,
                        Username = kvp.Value,
                        RoundPoints = pa?.PointsEarned ?? 0,
                        TotalScore = game.PlayerScores.GetValueOrDefault(kvp.Key, 0),
                        AnsweredCorrectly = pa?.IsCorrect ?? false
                    };
                }).OrderByDescending(p => p.TotalScore).ToList();

                var questionResult = new QuestionResultDto
                {
                    QuestionId = question.QuestionId,
                    CorrectAnswerId = correctAnswer?.AnswerId ?? 0,
                    CorrectAnswerText = correctAnswer?.Text ?? string.Empty,
                    PlayerScores = playerScores
                };

                await Clients.Group(roomCode).SendAsync("QuestionEnded", questionResult);

                // Brief pause between questions (skip after the last)
                if (i < questions.Count - 1)
                    await Task.Delay(3000);
            }

            // Persist final scores to DB
            await PersistScoresAsync(gameRoomId, game, questions.Count);

            // Build final leaderboard
            var finalLeaderboard = game.PlayerScores
                .OrderByDescending(kv => kv.Value)
                .Select((kv, index) => new LeaderboardEntryDto
                {
                    Rank = index + 1,
                    UserId = kv.Key,
                    Username = game.PlayerNames.GetValueOrDefault(kv.Key, "Unknown"),
                    TotalScore = kv.Value,
                    CorrectAnswers = game.Answers.Count(a => a.Value.IsCorrect && a.Key == kv.Key),
                    TotalQuestions = questions.Count,
                    QuizTitle = game.QuizTitle,
                    CompletedAt = DateTime.UtcNow
                })
                .ToList();

            var gameResult = new GameResultDto
            {
                RoomCode = roomCode,
                QuizTitle = game.QuizTitle,
                Leaderboard = finalLeaderboard
            };

            await Clients.Group(roomCode).SendAsync("GameEnded", gameResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in game loop for room {RoomCode}", roomCode);
            await Clients.Group(roomCode).SendAsync("Error", "An error occurred during the game.");
        }
        finally
        {
            _gameState.RemoveGame(roomCode);
        }
    }

    private async Task PersistScoresAsync(int gameRoomId, ActiveGame game, int totalQuestions)
    {
        try
        {
            var room = await _context.GameRooms.FindAsync(gameRoomId);
            if (room != null)
            {
                room.Status = RoomStatus.Completed;
            }

            var ranked = game.PlayerScores
                .OrderByDescending(kv => kv.Value)
                .Select((kv, index) => new { kv.Key, kv.Value, Rank = index + 1 })
                .ToList();

            foreach (var entry in ranked)
            {
                var correctCount = 0;
                // Count correct answers from game state (per question)
                // Since Answers dict is per-question (reset each round), approximate via score
                _context.GameScores.Add(new GameScore
                {
                    GameRoomId = gameRoomId,
                    UserId = entry.Key,
                    Username = game.PlayerNames.GetValueOrDefault(entry.Key, "Unknown"),
                    TotalScore = entry.Value,
                    CorrectAnswers = correctCount,
                    TotalQuestions = totalQuestions,
                    Rank = entry.Rank
                });
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist scores for game room {GameRoomId}", gameRoomId);
        }
    }

    public async Task SubmitAnswer(string roomCode, int questionId, int answerId)
    {
        roomCode = roomCode.ToUpper();
        var userId = GetUserId();
        var username = GetUsername();

        var game = _gameState.GetGame(roomCode);
        if (game == null || !game.IsRunning)
        {
            await Clients.Caller.SendAsync("Error", "No active game in this room.");
            return;
        }

        if (game.CurrentQuestion?.QuestionId != questionId)
        {
            await Clients.Caller.SendAsync("Error", "This question is no longer active.");
            return;
        }

        var playerAnswer = _gameState.RecordAnswer(roomCode, userId, answerId);
        if (playerAnswer == null)
        {
            await Clients.Caller.SendAsync("AnswerReceived", new { success = false, message = "Already answered or invalid." });
            return;
        }

        await Clients.Caller.SendAsync("AnswerReceived", new { success = true, pointsEarned = playerAnswer.PointsEarned });
        await Clients.OthersInGroup(roomCode).SendAsync("PlayerAnsweredNotification", username);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = GetUsername();
        _logger.LogInformation("Player {Username} disconnected", username);
        await base.OnDisconnectedAsync(exception);
    }
}
