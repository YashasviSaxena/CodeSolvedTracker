using Microsoft.EntityFrameworkCore;
using CodeSolvedTracker.Core.Models;

namespace CodeSolvedTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Submission> Submissions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Submission>()
            .HasIndex(s => s.SubmittedAt);
            
        modelBuilder.Entity<Submission>()
            .HasIndex(s => s.ProblemCategory);
    }
}
