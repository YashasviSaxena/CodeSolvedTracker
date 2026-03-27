using Microsoft.EntityFrameworkCore;
using CodeSolvedTracker.Infrastructure.Data;
using CodeSolvedTracker.Infrastructure.Repositories;
using CodeSolvedTracker.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Simple database setup
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=codingtracker.db"));

// Simple CORS - allow everything for testing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register services
builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddScoped<IPredictionService, PredictionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Configure for Render.com
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");
app.Run();

