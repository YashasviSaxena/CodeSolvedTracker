namespace CodeSolvedTracker.Core.Models;

public class Submission
{
    public int Id { get; set; }
    public string Platform { get; set; } = string.Empty;
    public string ProblemId { get; set; } = string.Empty;
    public string ProblemTitle { get; set; } = string.Empty;
    public string ProblemCategory { get; set; } = string.Empty;
    public string ProblemDifficulty { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? RuntimeMs { get; set; }
    public int? MemoryUsageMb { get; set; }
}
