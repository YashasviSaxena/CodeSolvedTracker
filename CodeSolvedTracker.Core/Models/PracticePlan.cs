namespace CodeSolvedTracker.Core.Models;

public class PracticePlan
{
    public string Focus { get; set; } = string.Empty;
    public string EstimatedTime { get; set; } = string.Empty;
    public List<SuggestedProblem> SuggestedProblems { get; set; } = new();
}

public class SuggestedProblem
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
}
