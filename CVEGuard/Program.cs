using CVEGuard;
using CVEGuard.Library;
using CVEGuard.Library.Contracts;
using CVEGuard.Library.Services;
using CVEGuard.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// Fetch MySQL connection string from environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Version 1:{connectionString}");

var ollamaUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://ollama:11434"; // Default to internal service
var ollamaModal = builder.Configuration["Ollama:modal"] ?? "mistral";
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string not configured. Set 'ConnectionStrings__DefaultConnection' in environment variables.");
}

// Register the DbContext with the dependency injection container
builder.Services.AddDbContext<CVEGuardDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddScoped<ICVEService, CVEService>();

builder.Services.AddScoped<ICVERepository, CVERepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICVELoaderService, CVELoaderService>();
builder.Services.AddScoped<ICVEService, CVEService>();
builder.Services.AddScoped<TimedCVELoadingService>();

// Enable CORS (Modify as needed for security)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors(); // Ensure CORS is enabled

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("OpenApi/v1/OpenApiDoc.json");
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
    });
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CVEGuardDbContext>();
    dbContext.Database.EnsureCreated();
}


TimedCVELoadingService timedService;
timedService = new TimedCVELoadingService(app.Services);
await timedService.StartAsync(new CancellationToken());

// Define an API endpoint to query MySQL using UserService
app.MapGet("/", async (ICVEService cveService) =>
{
    var uri = new Uri(ollamaUrl);
    Console.WriteLine($"{ollamaModal}:{uri}");
    var cves = await cveService.TodaysCVESummaryAsync(uri, ollamaModal);
    return Results.Content(cves, "text/html");
});

// Define an API endpoint to query MySQL using UserService
app.MapGet("/cves", async (ICVEService cveService) =>
{
    var cves = await cveService.GetAllAsync();
    return Results.Ok(cves);
});



app.MapOpenApi();
app.Run();

