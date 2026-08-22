using RewardsSystem.Middleware;
using RewardsSystem.Repositories;
using RewardsSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers.
builder.Services.AddControllers();

// Swagger / OpenAPI, so the API can be explored and tested easily (also from Postman).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection: repository (data access) and service (business logic) layers.
// Registered as singletons because the in-memory repository must keep its state
// for the lifetime of the application.
builder.Services.AddSingleton<IPointsRepository, InMemoryPointsRepository>();
builder.Services.AddScoped<IRewardsService, RewardsService>();

var app = builder.Build();

// Global exception handling: converts any exception into a clear JSON response
// instead of letting the API crash or leak internal details.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

// Exposed for integration testing (WebApplicationFactory).
public partial class Program { }
