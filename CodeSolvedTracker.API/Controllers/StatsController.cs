using Microsoft.AspNetCore.Mvc;
using CodeSolvedTracker.Infrastructure.Repositories;

namespace CodeSolvedTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly ISubmissionRepository _repository;
    
    public StatsController(ISubmissionRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var data = await _repository.GetDashboardDataAsync();
        return Ok(data);
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategoryStats()
    {
        var stats = await _repository.GetCategoryStatsAsync();
        return Ok(stats);
    }
}
