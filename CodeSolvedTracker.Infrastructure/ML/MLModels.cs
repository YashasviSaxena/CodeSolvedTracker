using Microsoft.ML.Data;

namespace CodeSolvedTracker.Infrastructure.ML;

public class ProblemPerformanceData
{
    [LoadColumn(0)]
    public string ProblemCategory { get; set; } = string.Empty;
    
    [LoadColumn(1)]
    public float SuccessRate { get; set; }
    
    [LoadColumn(2)]
    public float AvgTimeSpent { get; set; }
    
    [LoadColumn(3)]
    public float AttemptsCount { get; set; }
    
    [LoadColumn(4)]
    public string RecommendedCategory { get; set; } = string.Empty;
}

public class PracticePlanPrediction
{
    [ColumnName("PredictedLabel")]
    public string RecommendedCategory { get; set; } = string.Empty;
    
    public float[] Score { get; set; } = Array.Empty<float>();
}
