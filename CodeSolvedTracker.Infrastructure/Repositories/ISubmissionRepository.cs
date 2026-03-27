using CodeSolvedTracker.Core.Models;

namespace CodeSolvedTracker.Infrastructure.Repositories;

public interface ISubmissionRepository
{
    Task<List<Submission>> GetAllAsync();
    Task AddAsync(Submission submission);
    Task AddRangeAsync(List<Submission> submissions);
    Task<DashboardData> GetDashboardDataAsync();
    Task<Dictionary<string, double>> GetCategoryStatsAsync();
}
