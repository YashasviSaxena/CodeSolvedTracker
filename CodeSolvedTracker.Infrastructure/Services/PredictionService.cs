using CodeSolvedTracker.Core.Models;
using CodeSolvedTracker.Infrastructure.Repositories;

namespace CodeSolvedTracker.Infrastructure.Services;

public class PredictionService : IPredictionService
{
    private readonly ISubmissionRepository _repository;
    
    public PredictionService(ISubmissionRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<SkillGap> GetSkillGapAsync(int userId)
    {
        var categoryStats = await _repository.GetCategoryStatsAsync(userId);
        
        if (!categoryStats.Any())
        {
            return new SkillGap
            {
                Category = "Arrays",
                GapScore = 0.5,
                RecommendedNextProblemsCount = 5,
                Recommendation = "Start practicing with Arrays category problems"
            };
        }
        
        var weakestCategory = categoryStats.OrderBy(x => x.Value).First();
        double gapScore = 1 - (weakestCategory.Value / 100);
        
        string recommendation = $"Your success rate in {weakestCategory.Key} is only {weakestCategory.Value:F1}%. ";
        
        if (weakestCategory.Key == "DP")
            recommendation += "Focus on understanding dynamic programming patterns: Fibonacci, Knapsack, and Longest Common Subsequence.";
        else if (weakestCategory.Key == "Graphs")
            recommendation += "Practice BFS, DFS, and shortest path algorithms.";
        else
            recommendation += "Solve more problems in this category to build confidence.";
        
        return new SkillGap
        {
            Category = weakestCategory.Key,
            GapScore = gapScore,
            RecommendedNextProblemsCount = 5,
            Recommendation = recommendation
        };
    }
    
    public async Task<PracticePlan> GetPracticePlanAsync(int userId)
    {
        var skillGap = await GetSkillGapAsync(userId);
        
        var problems = new List<SuggestedProblem>();
        
        if (skillGap.Category == "DP")
        {
            problems.Add(new SuggestedProblem { Title = "Fibonacci Number", Category = "DP", Difficulty = "Easy" });
            problems.Add(new SuggestedProblem { Title = "Climbing Stairs", Category = "DP", Difficulty = "Easy" });
            problems.Add(new SuggestedProblem { Title = "House Robber", Category = "DP", Difficulty = "Medium" });
            problems.Add(new SuggestedProblem { Title = "Longest Increasing Subsequence", Category = "DP", Difficulty = "Medium" });
            problems.Add(new SuggestedProblem { Title = "Coin Change", Category = "DP", Difficulty = "Medium" });
        }
        else if (skillGap.Category == "Graphs")
        {
            problems.Add(new SuggestedProblem { Title = "Number of Islands", Category = "Graphs", Difficulty = "Medium" });
            problems.Add(new SuggestedProblem { Title = "Course Schedule", Category = "Graphs", Difficulty = "Medium" });
            problems.Add(new SuggestedProblem { Title = "Clone Graph", Category = "Graphs", Difficulty = "Medium" });
        }
        else
        {
            problems.Add(new SuggestedProblem { Title = $"Easy {skillGap.Category} Problem", Category = skillGap.Category, Difficulty = "Easy" });
            problems.Add(new SuggestedProblem { Title = $"Medium {skillGap.Category} Challenge", Category = skillGap.Category, Difficulty = "Medium" });
            problems.Add(new SuggestedProblem { Title = $"Hard {skillGap.Category} Mastery", Category = skillGap.Category, Difficulty = "Hard" });
        }
        
        return new PracticePlan
        {
            Focus = $"Master {skillGap.Category} - Your weakest area",
            EstimatedTime = problems.Count == 5 ? "3-4 hours" : "2-3 hours",
            SuggestedProblems = problems
        };
    }
}
