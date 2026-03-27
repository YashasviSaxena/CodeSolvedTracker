using Microsoft.ML;
using CodeSolvedTracker.Core.Models;
using CodeSolvedTracker.Infrastructure.ML;
using CodeSolvedTracker.Infrastructure.Repositories;

namespace CodeSolvedTracker.Infrastructure.Services;

public class PredictionService : IPredictionService
{
    private readonly ISubmissionRepository _repository;
    private readonly MLContext _mlContext;
    private ITransformer? _model;
    private readonly string _modelPath = Path.Combine(Directory.GetCurrentDirectory(), "model.zip");
    
    public PredictionService(ISubmissionRepository repository)
    {
        _repository = repository;
        _mlContext = new MLContext();
        LoadOrCreateModel();
    }
    
    private void LoadOrCreateModel()
    {
        if (File.Exists(_modelPath))
        {
            _model = _mlContext.Model.Load(_modelPath, out _);
        }
        else
        {
            CreateAndTrainModel();
        }
    }
    
    private void CreateAndTrainModel()
    {
        var trainingData = new List<ProblemPerformanceData>
        {
            new() { ProblemCategory = "Arrays", SuccessRate = 0.85f, AvgTimeSpent = 15f, AttemptsCount = 1.2f, RecommendedCategory = "Strings" },
            new() { ProblemCategory = "Strings", SuccessRate = 0.75f, AvgTimeSpent = 20f, AttemptsCount = 1.5f, RecommendedCategory = "Trees" },
            new() { ProblemCategory = "DP", SuccessRate = 0.45f, AvgTimeSpent = 45f, AttemptsCount = 3.2f, RecommendedCategory = "DP" },
            new() { ProblemCategory = "Graphs", SuccessRate = 0.60f, AvgTimeSpent = 35f, AttemptsCount = 2.1f, RecommendedCategory = "DP" },
            new() { ProblemCategory = "Trees", SuccessRate = 0.70f, AvgTimeSpent = 25f, AttemptsCount = 1.8f, RecommendedCategory = "Graphs" },
            new() { ProblemCategory = "Hash Tables", SuccessRate = 0.90f, AvgTimeSpent = 10f, AttemptsCount = 1.0f, RecommendedCategory = "Arrays" },
            new() { ProblemCategory = "Linked Lists", SuccessRate = 0.80f, AvgTimeSpent = 18f, AttemptsCount = 1.3f, RecommendedCategory = "Trees" },
            new() { ProblemCategory = "Sorting", SuccessRate = 0.88f, AvgTimeSpent = 12f, AttemptsCount = 1.1f, RecommendedCategory = "Arrays" }
        };
        
        var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);
        
        var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ProblemPerformanceData.RecommendedCategory))
            .Append(_mlContext.Transforms.Concatenate("Features",
                nameof(ProblemPerformanceData.SuccessRate),
                nameof(ProblemPerformanceData.AvgTimeSpent),
                nameof(ProblemPerformanceData.AttemptsCount)))
            .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
        
        _model = pipeline.Fit(dataView);
        _mlContext.Model.Save(_model, dataView.Schema, _modelPath);
    }
    
    public async Task<SkillGap> GetSkillGapAsync()
    {
        var categoryStats = await _repository.GetCategoryStatsAsync();
        
        if (!categoryStats.Any())
        {
            return new SkillGap
            {
                Category = "Start Practicing",
                GapScore = 1.0,
                RecommendedNextProblemsCount = 5,
                Recommendation = "Start by solving problems in Arrays category"
            };
        }
        
        var weakestCategory = categoryStats
            .OrderBy(x => x.Value)
            .First();
        
        var input = new ProblemPerformanceData
        {
            ProblemCategory = weakestCategory.Key,
            SuccessRate = (float)(weakestCategory.Value / 100),
            AvgTimeSpent = 25f,
            AttemptsCount = 2f
        };
        
        string recommendation;
        if (_model != null)
        {
            var engine = _mlContext.Model.CreatePredictionEngine<ProblemPerformanceData, PracticePlanPrediction>(_model);
            var prediction = engine.Predict(input);
            recommendation = prediction.RecommendedCategory;
        }
        else
        {
            recommendation = weakestCategory.Key;
        }
        
        return new SkillGap
        {
            Category = weakestCategory.Key,
            GapScore = 1 - (weakestCategory.Value / 100),
            RecommendedNextProblemsCount = 5,
            Recommendation = $"Focus on {recommendation} problems. Your success rate in {weakestCategory.Key} is only {weakestCategory.Value:F1}%."
        };
    }
    
    public async Task<PracticePlan> GetPracticePlanAsync()
    {
        var skillGap = await GetSkillGapAsync();
        
        var suggestedProblems = new List<SuggestedProblem>
        {
            new() { Title = $"Easy {skillGap.Category} Problem", Category = skillGap.Category, Difficulty = "Easy" },
            new() { Title = $"Medium {skillGap.Category} Challenge", Category = skillGap.Category, Difficulty = "Medium" },
            new() { Title = $"Hard {skillGap.Category} Mastery", Category = skillGap.Category, Difficulty = "Hard" }
        };
        
        return new PracticePlan
        {
            Focus = skillGap.Recommendation,
            EstimatedTime = "2-3 hours",
            SuggestedProblems = suggestedProblems
        };
    }
}
