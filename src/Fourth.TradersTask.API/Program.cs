using FluentValidation;
using FluentValidation.AspNetCore;
using Fourth.TradersTask.API.Constants;
using Fourth.TradersTask.API.Middleware;
using Fourth.TradersTask.API.Validators;
using Fourth.TradersTask.Application;
using Fourth.TradersTask.Application.Models;
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

builder.Services.AddFluentValidationAutoValidation();

// Add validation
builder.Services.AddScoped<IValidator<GetCustomersQueryParameters>, QueryParametersValidator>();

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
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");

    foreach (var xmlFile in xmlFiles)
    {
        options.IncludeXmlComments(xmlFile);
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

/// <summary>
/// Make Program visible for integration tests
/// </summary>
public partial class Program { }