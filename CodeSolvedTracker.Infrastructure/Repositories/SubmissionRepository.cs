using Microsoft.EntityFrameworkCore;
using CodeSolvedTracker.Core.Models;
using CodeSolvedTracker.Infrastructure.Data;

namespace CodeSolvedTracker.Infrastructure.Repositories;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly AppDbContext _context;
    
    public SubmissionRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Submission>> GetAllAsync()
    {
        return await _context.Submissions
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
    }
    
    public async Task AddAsync(Submission submission)
    {
        await _context.Submissions.AddAsync(submission);
        await _context.SaveChangesAsync();
    }
    
    public async Task AddRangeAsync(List<Submission> submissions)
    {
        await _context.Submissions.AddRangeAsync(submissions);
        await _context.SaveChangesAsync();
    }
    
    public async Task<DashboardData> GetDashboardDataAsync()
    {
        var submissions = await _context.Submissions.ToListAsync();
        var accepted = submissions.Where(s => s.Status == "Accepted").ToList();
        
        return new DashboardData
        {
            TotalSolved = accepted.Count,
            SuccessRate = submissions.Any() ? (double)accepted.Count / submissions.Count * 100 : 0,
            ByCategory = accepted
                .GroupBy(s => s.ProblemCategory)
                .Select(g => new CategoryCount 
                { 
                    Category = g.Key, 
                    Count = g.Count() 
                })
                .OrderByDescending(x => x.Count)
                .ToList(),
            RecentActivity = submissions
                .OrderByDescending(s => s.SubmittedAt)
                .Take(10)
                .Select(s => new RecentActivity
                {
                    ProblemTitle = s.ProblemTitle,
                    SolvedAt = s.SubmittedAt,
                    Status = s.Status
                })
                .ToList()
        };
    }
    
    public async Task<Dictionary<string, double>> GetCategoryStatsAsync()
    {
        var submissions = await _context.Submissions.ToListAsync();
        
        return submissions
            .GroupBy(s => s.ProblemCategory)
            .ToDictionary(
                g => g.Key,
                g => g.Count(s => s.Status == "Accepted") / (double)g.Count() * 100
            );
    }
}
