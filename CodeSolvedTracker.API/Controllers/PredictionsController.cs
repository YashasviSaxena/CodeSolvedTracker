using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CodeSolvedTracker.Infrastructure.Services;

namespace CodeSolvedTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PredictionsController : ControllerBase
{
    private readonly IPredictionService _predictionService;
    
    public PredictionsController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }
    
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out int userId) ? userId : 0;
    }
    
    [HttpGet("skill-gap")]
    public async Task<IActionResult> GetSkillGap()
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized();
            
        var result = await _predictionService.GetSkillGapAsync(userId);
        return Ok(result);
    }
    
    [HttpGet("practice-plan")]
    public async Task<IActionResult> GetPracticePlan()
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized();
            
        var result = await _predictionService.GetPracticePlanAsync(userId);
        return Ok(result);
    }
}
