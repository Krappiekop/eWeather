namespace eWeather.Backend.Services;

using eWeather.Backend.Models;
using eWeather.Shared.Models;
using Microsoft.EntityFrameworkCore;

public class WeerDataService
{
    private readonly EWeatherContext _context;

    public WeerDataService(EWeatherContext context)
    {
        _context = context;
    }

    public List<WeerMeting> GetWeerData(DateOnly startDate, DateOnly? endDate, string station)
    {
        var effectiveEndDate = endDate ?? startDate.AddDays(7);
        var beginGrens = startDate.ToDateTime(TimeOnly.MinValue);
        var eindGrens = effectiveEndDate.AddDays(1).ToDateTime(TimeOnly.MinValue);

        return _context.WeerMetingen
            .Where(m => m.Station == station
                && m.Tijdstip >= beginGrens
                && m.Tijdstip < eindGrens)
            .OrderBy(m => m.Tijdstip)
            .ToList();
    }
    public List<WeerMeting> GetActualWeerData()
    {
        return _context.WeerMetingen
            .GroupBy(m => m.Station)
            .Select(g => g.OrderByDescending(m => m.Tijdstip).First())
            .ToList();
    }

}