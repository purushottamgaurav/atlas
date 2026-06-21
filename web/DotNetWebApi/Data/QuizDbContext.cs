using DotNetWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetWebApi.Data;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<GameRoom> GameRooms { get; set; }
    public DbSet<GameParticipant> GameParticipants { get; set; }
    public DbSet<GameScore> GameScores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<GameRoom>()
            .HasIndex(r => r.Code).IsUnique();

        modelBuilder.Entity<GameParticipant>()
            .HasIndex(p => new { p.GameRoomId, p.UserId }).IsUnique();

        modelBuilder.Entity<Quiz>()
            .HasOne(q => q.CreatedBy)
            .WithMany(u => u.Quizzes)
            .HasForeignKey(q => q.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Quiz)
            .WithMany(qz => qz.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GameRoom>()
            .HasOne(r => r.Quiz)
            .WithMany(q => q.GameRooms)
            .HasForeignKey(r => r.QuizId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GameParticipant>()
            .HasOne(p => p.GameRoom)
            .WithMany(r => r.Participants)
            .HasForeignKey(p => p.GameRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GameScore>()
            .HasOne(s => s.GameRoom)
            .WithMany(r => r.Scores)
            .HasForeignKey(s => s.GameRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
