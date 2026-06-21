using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotNetWebApi.Data;

public class QuizDbContextFactory : IDesignTimeDbContextFactory<QuizDbContext>
{
    public QuizDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<QuizDbContext>()
            .UseSqlite("Data Source=quiz.db")
            .Options;
        return new QuizDbContext(options);
    }
}
