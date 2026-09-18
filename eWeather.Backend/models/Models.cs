using Microsoft.EntityFrameworkCore;
using eWeather.Shared.Models;
namespace eWeather.Backend.Models;
public class EWeatherContext : DbContext
{
    public DbSet<WeerMeting> WeerMetingen { get; set; }
    public EWeatherContext(DbContextOptions<EWeatherContext> options)
    : base(options)
    {
    }
}

// Klasse voor de Config bestand opties
public class WeerAPIOpties
{
    public int OphaalIntervalMinuten { get; set; }
    public int BewaarPeriodeDagen { get; set; }
}