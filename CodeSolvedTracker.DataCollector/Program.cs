using CodeSolvedTracker.Core.Models;
using CodeSolvedTracker.Infrastructure.Data;
using CodeSolvedTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        var repository = serviceProvider.GetRequiredService<ISubmissionRepository>();
        
        var submissions = GenerateMockData();
        await repository.AddRangeAsync(submissions);
        
        Console.WriteLine($"Added {submissions.Count} submissions to database!");
        
        var stats = await repository.GetDashboardDataAsync();
        Console.WriteLine($"\nDashboard Stats:");
        Console.WriteLine($"Total Solved: {stats.TotalSolved}");
        Console.WriteLine($"Success Rate: {stats.SuccessRate:F1}%");
        Console.WriteLine("\nProblems by Category:");
        foreach (var cat in stats.ByCategory)
        {
            Console.WriteLine($"  {cat.Category}: {cat.Count}");
        }
    }
    
    static List<Submission> GenerateMockData()
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
