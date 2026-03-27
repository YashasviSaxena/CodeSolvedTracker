using Microsoft.AspNetCore.Mvc;
using CodeSolvedTracker.Infrastructure.Services;

namespace CodeSolvedTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PredictionsController : ControllerBase
{
    private readonly IPredictionService _predictionService;
    
    public PredictionsController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }
    
    [HttpGet("skill-gap")]
    public async Task<IActionResult> GetSkillGap()
    {
        var result = await _predictionService.GetSkillGapAsync();
        return Ok(result);
    }
    
    [HttpGet("practice-plan")]
    public async Task<IActionResult> GetPracticePlan()
    {
        var result = await _predictionService.GetPracticePlanAsync();
        return Ok(result);
    }
}
