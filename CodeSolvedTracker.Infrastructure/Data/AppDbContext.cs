using Microsoft.EntityFrameworkCore;
using CodeSolvedTracker.Core.Models;

namespace CodeSolvedTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Submission>()
            .HasIndex(s => s.SubmittedAt);
            
        modelBuilder.Entity<Submission>()
            .HasIndex(s => s.ProblemCategory);
            
        // Configure User-Submission relationship
        modelBuilder.Entity<Submission>()
            .HasOne(s => s.User)
            .WithMany(u => u.Submissions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Unique constraint on Username and Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
            
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
