using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CodeSolvedTracker.Infrastructure.Repositories;

namespace CodeSolvedTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly ISubmissionRepository _repository;
    
    public StatsController(ISubmissionRepository repository)
    {
        _repository = repository;
    }
    
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out int userId) ? userId : 0;
    }
    
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized();
            
        var data = await _repository.GetDashboardDataAsync(userId);
        return Ok(data);
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategoryStats()
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized();
            
        var stats = await _repository.GetCategoryStatsAsync(userId);
        return Ok(stats);
    }
}
