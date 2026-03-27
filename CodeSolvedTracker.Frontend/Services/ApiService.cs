using System.Net.Http.Json;
using CodeSolvedTracker.Core.Models;

namespace CodeSolvedTracker.Frontend.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private const string ApiBaseUrl = "http://localhost:5173";
    
    public ApiService(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<DashboardData> GetDashboardAsync()
    {
        try
        {
            var url = $"{ApiBaseUrl}/api/stats/dashboard";
            return await _http.GetFromJsonAsync<DashboardData>(url) ?? new DashboardData();
        }
        catch
        {
            return new DashboardData();
        }
    }
    
    public async Task<SkillGap> GetSkillGapAsync()
    {
        try
        {
            var url = $"{ApiBaseUrl}/api/predictions/skill-gap";
            return await _http.GetFromJsonAsync<SkillGap>(url) ?? new SkillGap();
        }
        catch
        {
            return new SkillGap();
        }
    }
    
    public async Task<PracticePlan> GetPracticePlanAsync()
    {
        try
        {
            var url = $"{ApiBaseUrl}/api/predictions/practice-plan";
            return await _http.GetFromJsonAsync<PracticePlan>(url) ?? new PracticePlan();
        }
        catch
        {
            return new PracticePlan();
        }
    }
}
