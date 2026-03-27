namespace CodeSolvedTracker.Core.Models;

public class DashboardData
{
    public int TotalSolved { get; set; }
    public double SuccessRate { get; set; }
    public List<CategoryCount> ByCategory { get; set; } = new();
    public List<RecentActivity> RecentActivity { get; set; } = new();
}

public class CategoryCount
{
    public string Category { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class RecentActivity
{
    public string ProblemTitle { get; set; } = string.Empty;
    public DateTime SolvedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
