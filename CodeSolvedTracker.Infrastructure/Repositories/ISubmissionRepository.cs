using CodeSolvedTracker.Core.Models;

namespace CodeSolvedTracker.Infrastructure.Repositories;

public interface ISubmissionRepository
{
    Task<List<Submission>> GetAllAsync(int userId);
    Task AddAsync(Submission submission);
    Task AddRangeAsync(List<Submission> submissions);
    Task<DashboardData> GetDashboardDataAsync(int userId);
    Task<Dictionary<string, double>> GetCategoryStatsAsync(int userId);
}
