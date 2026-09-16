using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using eWeather.Backend.Models;
using eWeather.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<EWeatherContext>(options =>
    options.UseSqlite("Data Source=eweather.db"));

builder.Services.AddHostedService<DataUploadService>();
builder.Services.AddScoped<WeerDataService>();

builder.Services.AddHttpClient("json", client => 
{ 
    // BaseAddress is het vaste basisdeel van de url.
    // Bij elke aanroep vanuit deze client geef ik straks alleen nog het resterende pad mee,
    // bijvoorbeeld "2.0/feed/json", en dat wordt automatisch achter deze BaseAddress geplakt.
    client.BaseAddress = new Uri("https://data.buienradar.nl/"); 
});

builder.Services.Configure<WeerAPIOpties>(builder.Configuration.GetSection(nameof(WeerAPIOpties)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/weerdata", (DateOnly StartDate, DateOnly? EndDate, string Station, [FromServices] WeerDataService weerDataService) =>
{
    if (EndDate is null || EndDate >= StartDate)
    {
        return Results.Ok(weerDataService.GetWeerData(StartDate, EndDate, Station));
    }
    else
    {
        return Results.BadRequest("Einddatum moet groter of gelijk zijn aan Begindatum.");
    }
})
.WithName("GetWeerData");


app.MapGet("/weerdata/actueel", ([FromServices] WeerDataService weerDataService) =>
{
    return weerDataService.GetActualWeerData();
})
.WithName("GetActueelWeerData");

app.Run();