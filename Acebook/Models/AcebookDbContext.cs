namespace acebook.Models;
using Microsoft.EntityFrameworkCore;

public class AcebookDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    // Do not delete this line of code it does something! 
    public DbSet<User> User { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<FriendRequest> FriendRequests {get; set;}
    public DbSet<Like>? Likes { get; set; }
    public string? DbPath { get; }

    public string? GetDatabaseName() {
      string? DatabaseNameArg = Environment.GetEnvironmentVariable("DATABASE_NAME");

      if( DatabaseNameArg == null)
      {
        System.Console.WriteLine(
          "DATABASE_NAME is null. Defaulting to test database."
        );
        return "acebook_csharp_test";
      }
      else
      {
        System.Console.WriteLine(
          "Connecting to " + DatabaseNameArg
        );
        return DatabaseNameArg;
      }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=1234;Database=" + GetDatabaseName());
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
          .Navigation(post => post.User)
          .AutoInclude();

          modelBuilder.Entity<User>()
.HasMany(p => p.Posts)
.WithOne(u => u.User)
.HasForeignKey(p => p.UserId);
    }
}
