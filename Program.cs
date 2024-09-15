using API_Aggregation.Models;
using API_Aggregation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register HttpClient for making external API calls
builder.Services.AddHttpClient();

// Register repositories for dependency injection
builder.Services.AddScoped<IPopulationService, PopulationService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<ICatService, CatBreedService>();
builder.Services.AddScoped<ISpotifyService, SpotifyService>();

// Add controllers and support for minimal API endpoints
builder.Services.AddControllers();

// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bind OAuthSettings from appsettings.json
builder.Services.Configure<OAuthSettings>(builder.Configuration.GetSection("OAuthSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Map controller routes
app.MapControllers();

app.Run();
