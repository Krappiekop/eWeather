using Microsoft.Extensions.Options;
using eWeather.Backend.Models;
using eWeather.Shared.Models;

namespace eWeather.Backend.Services;
public class DataUploadService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly HttpClient _client;
    private readonly IOptions<WeerAPIOpties> _opties;

    public DataUploadService(IServiceScopeFactory scopeFactory, IHttpClientFactory clientFactory, IOptions<WeerAPIOpties> options)
    {
        _scopeFactory = scopeFactory;
        _client = clientFactory.CreateClient("json");
        _opties = options;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EWeatherContext>();
                var data = await _client.GetFromJsonAsync<BuienradarJSON>("2.0/feed/json");

                var nieuweMetingen = data.Actual.StationMeasurements.Select(s => new WeerMeting
                {
                    Tijdstip = DateTime.UtcNow,
                    Station = s.StationName,
                    Temperature = s.Temperature,
                    FeelTemperature = s.FeelTemperature,
                    GroundTemperature = s.GroundTemperature,
                    SunPower = s.SunPower,
                    RainFallLastHour = s.RainFallLastHour,
                    WindDirection = s.WindDirection
                }).ToList();
                context.WeerMetingen.AddRange(nieuweMetingen);
                await context.SaveChangesAsync();


                var GrensDatum = DateTime.UtcNow.AddDays(-_opties.Value.BewaarPeriodeDagen);

                // Gegevens ophalen die aan voorwaarden voldoen (waarvan tijdstip ouder is dan de grensdatum)
                var OudeGegevens = context.WeerMetingen
                    .Where(x => x.Tijdstip < GrensDatum)
                    .ToList();

                // Weghalen van de geselecteerde gegevens uit de dataset
                if (OudeGegevens.Any())
                {
                    context.WeerMetingen.RemoveRange(OudeGegevens);
                    await context.SaveChangesAsync();
                }

                Console.WriteLine($"Aantal metingen opgeslagen: {nieuweMetingen.Count}");
                Console.WriteLine($"Aantal metingen verwijderd: {OudeGegevens.Count}");
                Console.WriteLine($"Aantal metingen in database: {context.WeerMetingen.Count()}");
            }

            await Task.Delay(TimeSpan.FromMinutes(_opties.Value.OphaalIntervalMinuten), stoppingToken);
        }
    }
}