using System.Net.Http.Json;
using CodeSolvedTracker.Core.Models;

namespace CodeSolvedTracker.Frontend.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private const string ApiBaseUrl = "https://codesolvedtracker.onrender.com";
    
    public ApiService(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<DashboardData> GetDashboardAsync()
    {
        try
        {
            var url = $"{ApiBaseUrl}/api/stats/dashboard";
            Console.WriteLine($"Calling: {url}");
            var response = await _http.GetFromJsonAsync<DashboardData>(url);
            return response ?? new DashboardData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new DashboardData();
        }
    }
    
    public async Task<SkillGap> GetSkillGapAsync()
    {
        try
        {
            var url = $"{ApiBaseUrl}/api/predictions/skill-gap";
            var response = await _http.GetFromJsonAsync<SkillGap>(url);
            return response ?? new SkillGap();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new SkillGap();
        }
    }
    
    public async Task<PracticePlan> GetPracticePlanAsync()
    {
        try
        {
            var url = $"{ApiBaseUrl}/api/predictions/practice-plan";
            var response = await _http.GetFromJsonAsync<PracticePlan>(url);
            return response ?? new PracticePlan();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new PracticePlan();
        }
    }
}
