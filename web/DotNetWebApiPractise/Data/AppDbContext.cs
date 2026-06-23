using DotNetWebApiPractise.Modals;
using Microsoft.EntityFrameworkCore;

namespace DotNetWebApiPractise.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .ToTable("Employee")
                .HasIndex(u => u.Id)
                .IsUnique();
        }

    }
}
