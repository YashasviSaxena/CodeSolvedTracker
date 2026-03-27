using CodeSolvedTracker.Core.Models;
using CodeSolvedTracker.Infrastructure.Data;
using CodeSolvedTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BCrypt.Net;

namespace CodeSolvedTracker.DataCollector;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Generating mock coding practice data...");
        
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=../CodeSolvedTracker.API/codingtracker.db"));
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();
        
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var repository = serviceProvider.GetRequiredService<ISubmissionRepository>();
        
        // Create default user if not exists
        var defaultUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "demo");
        if (defaultUser == null)
        {
            defaultUser = new User
            {
                Username = "demo",
                Email = "demo@example.com",
                PasswordHash = BCrypt.HashPassword("demo123"),
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(defaultUser);
            await context.SaveChangesAsync();
            Console.WriteLine("Created default user: demo / demo123");
        }
        
        // Check if user already has submissions
        var existingSubmissions = await context.Submissions.CountAsync(s => s.UserId == defaultUser.Id);
        if (existingSubmissions > 0)
        {
            Console.WriteLine($"User already has {existingSubmissions} submissions. Skipping seed.");
            return;
        }
        
        var submissions = GenerateMockData(defaultUser.Id);
        await context.Submissions.AddRangeAsync(submissions);
        await context.SaveChangesAsync();
        
        Console.WriteLine($"Added {submissions.Count} submissions for user {defaultUser.Username}!");
        
        var stats = await repository.GetDashboardDataAsync(defaultUser.Id);
        Console.WriteLine($"\nDashboard Stats:");
        Console.WriteLine($"Total Solved: {stats.TotalSolved}");
        Console.WriteLine($"Success Rate: {stats.SuccessRate:F1}%");
        Console.WriteLine("\nProblems by Category:");
        foreach (var cat in stats.ByCategory)
        {
            Console.WriteLine($"  {cat.Category}: {cat.Count}");
        }
    }
    
    static List<Submission> GenerateMockData(int userId)
    {
        var categories = new[] { "Arrays", "Strings", "DP", "Graphs", "Trees", "Hash Tables", "Linked Lists" };
        var difficulties = new[] { "Easy", "Medium", "Hard" };
        var statuses = new[] { "Accepted", "Accepted", "Accepted", "Wrong Answer", "Time Limit Exceeded" };
        var random = new Random();
        var submissions = new List<Submission>();
        
        for (int i = 0; i < 200; i++)
        {
            var category = categories[random.Next(categories.Length)];
            var isAccepted = true;
            string status;
            
            if (category == "DP" && random.Next(100) < 60)
            {
                status = "Wrong Answer";
                isAccepted = false;
            }
            else
            {
                status = statuses[random.Next(statuses.Length)];
                isAccepted = status == "Accepted";
            }
            
            submissions.Add(new Submission
            {
                UserId = userId,
                Platform = "LeetCode",
                ProblemId = $"problem_{i}",
                ProblemTitle = $"{category} Problem {random.Next(1, 100)}",
                ProblemCategory = category,
                ProblemDifficulty = difficulties[random.Next(3)],
                SubmittedAt = DateTime.Now.AddDays(-random.Next(30)).AddHours(-random.Next(24)),
                Status = status,
                RuntimeMs = isAccepted ? random.Next(50, 500) : null,
                MemoryUsageMb = isAccepted ? random.Next(30, 200) : null
            });
        }
        
        return submissions;
    }
}
