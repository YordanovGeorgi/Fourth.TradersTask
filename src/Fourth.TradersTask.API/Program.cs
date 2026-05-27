using Fourth.TradersTask.API.Middleware;
using Fourth.TradersTask.Application;
using Fourth.TradersTask.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Northwind Traders API",
        Version = "v1",
        Description = "Backend API for managing Northwind Traders customer information",
        Contact = new OpenApiContact
        {
            Name = "Fourth",
            Url = new Uri("https://example.com")
        }
    });

    // Include XML comments for documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    if (builder.Environment.IsDevelopment())
    {
        config.AddDebug();
    }
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind Traders API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("Application starting...");

await app.RunAsync();

// Make Program visible for integration tests
public partial class Program { }