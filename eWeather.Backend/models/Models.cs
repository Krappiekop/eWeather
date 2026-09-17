using Microsoft.EntityFrameworkCore;

namespace eWeather.Backend.Models;
public class EWeatherContext : DbContext
{
    public DbSet<WeerMeting> WeerMetingen { get; set; }
    public EWeatherContext(DbContextOptions<EWeatherContext> options)
    : base(options)
    {
    }
}

public class WeerMeting
{
    public int Id { get; set; }
    public DateTime Tijdstip { get; set; }
    public string Station { get; set; }
    public string Regio { get; set; }
    public float Temperature { get; set; }
    public float FeelTemperature { get; set; }
    public float GroundTemperature { get; set; }
    public float SunPower { get; set; }
    public float RainFallLastHour { get; set; }
    public string? WindDirection { get; set; }
}


// Klasse voor de Config bestand opties
public class WeerAPIOpties
{
    public int OphaalIntervalMinuten { get; set; }
    public int BewaarPeriodeDagen { get; set; }
}