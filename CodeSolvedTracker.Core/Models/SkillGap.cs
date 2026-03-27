namespace CodeSolvedTracker.Core.Models;

public class SkillGap
{
    public string Category { get; set; } = string.Empty;
    public double GapScore { get; set; }
    public int RecommendedNextProblemsCount { get; set; }
    public string Recommendation { get; set; } = string.Empty;
}
