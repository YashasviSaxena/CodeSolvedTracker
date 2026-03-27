using CodeSolvedTracker.Core.Models;

namespace CodeSolvedTracker.Infrastructure.Services;

public interface IPredictionService
{
    Task<SkillGap> GetSkillGapAsync();
    Task<PracticePlan> GetPracticePlanAsync();
}
