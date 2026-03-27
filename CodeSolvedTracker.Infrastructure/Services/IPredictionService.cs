using CodeSolvedTracker.Core.Models;

namespace CodeSolvedTracker.Infrastructure.Services;

public interface IPredictionService
{
    Task<SkillGap> GetSkillGapAsync(int userId);
    Task<PracticePlan> GetPracticePlanAsync(int userId);
}
