using Fourth.TradersTask.API.Constants;
using Fourth.TradersTask.API.Middleware;
using Fourth.TradersTask.API.Validators;
using Fourth.TradersTask.Application;
using Fourth.TradersTask.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add validation
builder.Services.AddScoped<PaginationParamsValidator>();
builder.Services.AddScoped<CustomerIdValidator>();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(ApiConstants.ApiVersion, new OpenApiInfo
    {
        Title = ApiConstants.ApiTitle,
        Version = ApiConstants.ApiVersion,
        Description = ApiConstants.ApiDescription,
        Contact = new OpenApiContact
        {
            Name = "Fourth",
            Url = new Uri("https://www.fourth.com")
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

// Configure logging
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
app.UseHttpsRedirection();

// Static files middleware (required for Swagger UI assets)
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/{ApiConstants.ApiVersion}/swagger.json", $"{ApiConstants.ApiTitle} {ApiConstants.ApiVersion}");
        options.RoutePrefix = string.Empty;
        options.DisplayRequestDuration();
    });
}

app.UseCors("AllowAll");

// Custom middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation(
    "Application {ApplicationName} starting in {Environment} environment",
    typeof(Program).Assembly.GetName().Name,
    app.Environment.EnvironmentName);

await app.RunAsync();

// Make Program visible for integration tests
public partial class Program { }