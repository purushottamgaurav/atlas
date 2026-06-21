using DotNetWebApi.Models;

namespace DotNetWebApi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(QuizDbContext context)
    {
        if (context.Users.Any()) return;

        var admin = new User
        {
            Username = "admin",
            Email = "admin@quiz.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(admin);
        await context.SaveChangesAsync();

        var quizzes = new List<Quiz>
        {
            new()
            {
                Title = "General Knowledge",
                Description = "Test your general knowledge with these fun questions!",
                Category = "General",
                IsPublic = true,
                CreatedByUserId = admin.UserId,
                Questions = new List<Question>
                {
                    new() { Text = "What is the capital of France?", OrderNumber = 1, TimeLimitSeconds = 20, Points = 100,
                        Answers = new List<Answer> {
                            new() { Text = "Paris", IsCorrect = true },
                            new() { Text = "London", IsCorrect = false },
                            new() { Text = "Berlin", IsCorrect = false },
                            new() { Text = "Madrid", IsCorrect = false }
                        }
                    },
                    new() { Text = "Which planet is known as the Red Planet?", OrderNumber = 2, TimeLimitSeconds = 20, Points = 100,
                        Answers = new List<Answer> {
                            new() { Text = "Venus", IsCorrect = false },
                            new() { Text = "Jupiter", IsCorrect = false },
                            new() { Text = "Mars", IsCorrect = true },
                            new() { Text = "Saturn", IsCorrect = false }
                        }
                    },
                    new() { Text = "How many continents are there on Earth?", OrderNumber = 3, TimeLimitSeconds = 15, Points = 100,
                        Answers = new List<Answer> {
                            new() { Text = "5", IsCorrect = false },
                            new() { Text = "6", IsCorrect = false },
                            new() { Text = "7", IsCorrect = true },
                            new() { Text = "8", IsCorrect = false }
                        }
                    },
                    new() { Text = "What is the largest ocean on Earth?", OrderNumber = 4, TimeLimitSeconds = 20, Points = 100,
                        Answers = new List<Answer> {
                            new() { Text = "Atlantic Ocean", IsCorrect = false },
                            new() { Text = "Indian Ocean", IsCorrect = false },
                            new() { Text = "Arctic Ocean", IsCorrect = false },
                            new() { Text = "Pacific Ocean", IsCorrect = true }
                        }
                    },
                    new() { Text = "Who wrote 'Romeo and Juliet'?", OrderNumber = 5, TimeLimitSeconds = 20, Points = 100,
                        Answers = new List<Answer> {
                            new() { Text = "Charles Dickens", IsCorrect = false },
                            new() { Text = "William Shakespeare", IsCorrect = true },
                            new() { Text = "Mark Twain", IsCorrect = false },
                            new() { Text = "Jane Austen", IsCorrect = false }
                        }
                    }
                }
            },
            new()
            {
                Title = "Tech Trivia",
                Description = "How well do you know technology and programming?",
                Category = "Technology",
                IsPublic = true,
                CreatedByUserId = admin.UserId,
                Questions = new List<Question>
                {
                    new() { Text = "What does HTTP stand for?", OrderNumber = 1, TimeLimitSeconds = 25, Points = 100,
                        Answers = new List<Answer> {
                            new() { Text = "HyperText Transfer Protocol", IsCorrect = true },
                            new() { Text = "High Technology Transfer Protocol", IsCorrect = false },
                            new() { Text = "Hyper Terminal Text Protocol", IsCorrect = false },
                            new() { Text = "Hyperlink Text Transmission Protocol", IsCorrect = false }
                        }
                    },
                    new() { Text = "Which company created the C# programming language?", OrderNumber = 2, TimeLimitSeconds = 20, Points = 100,
                        Answers = new List<Answer> {
                            new() { Text = "Google", IsCorrect = false },
                            new() { Text = "Apple", IsCorrect = false },
                            new() { Text = "Microsoft", IsCorrect = true },
                            new() { Text = "Oracle", IsCorrect = false }
                        }
                    },
                    new() { Text = "What does SQL stand for?", OrderNumber = 3, TimeLimitSeconds = 20, Points = 100,
                        Answers = new List<Answer> {
                            new() { Text = "Structured Query Language", IsCorrect = true },
                            new() { Text = "Simple Query Language", IsCorrect = false },
                            new() { Text = "Sequential Query Logic", IsCorrect = false },
                            new() { Text = "Standard Query List", IsCorrect = false }
                        }
                    },
                    new() { Text = "Which data structure uses LIFO (Last In, First Out)?", OrderNumber = 4, TimeLimitSeconds = 25, Points = 150,
                        Answers = new List<Answer> {
                            new() { Text = "Queue", IsCorrect = false },
                            new() { Text = "Stack", IsCorrect = true },
                            new() { Text = "Tree", IsCorrect = false },
                            new() { Text = "Graph", IsCorrect = false }
                        }
                    },
                    new() { Text = "What is the time complexity of binary search?", OrderNumber = 5, TimeLimitSeconds = 30, Points = 150,
                        Answers = new List<Answer> {
                            new() { Text = "O(n)", IsCorrect = false },
                            new() { Text = "O(n²)", IsCorrect = false },
                            new() { Text = "O(log n)", IsCorrect = true },
                            new() { Text = "O(1)", IsCorrect = false }
                        }
                    }
                }
            }
        };

        context.Quizzes.AddRange(quizzes);
        await context.SaveChangesAsync();
    }
}
