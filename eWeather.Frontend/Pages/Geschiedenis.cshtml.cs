using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using eWeather.Shared.Models;

namespace eWeather.Frontend.Pages;

public class GeschiedenisModel : PageModel
{
    private readonly HttpClient _client;
    [BindProperty(SupportsGet = true)] public string GekozenWeerStation { get; set; }
    [BindProperty(SupportsGet = true)] public DateOnly StartDate { get; set; }
    [BindProperty(SupportsGet = true)] public DateOnly? EndDate { get; set; }
    public string FoutMelding { get; set; }

    // Property waarin ik straks de opgehaalde JSON tekst opslaan, zodat de Razor pagina (Index.cshtml) deze via @Model.RuweData kan tonen.
    public List<WeerMeting> Data { get; set; }
    public List<WeerMeting> DataPeriodeStation { get; set; }

    // Constructor van IndexModel, wordt automatisch aangeroepen zodra ASP.NET Core een nieuwe IndexModel aanmaakt voor een bezoek aan de pagina.
    public GeschiedenisModel(IHttpClientFactory factory)
    {
        // Haalt de specifieke client op die we in Program.cs hadden geregistreerd onder de naam "json", inclusief de BaseAddress die daar is ingesteld.
        _client = factory.CreateClient("WeerDataVerbinding");
    }

    public async Task OnGetAsync()
    {
        try
        {
            Data = await _client.GetFromJsonAsync<List<WeerMeting>>("weerdata/actueel");
            // Data = await _client.GetFromJsonAsync<BuienradarJSON>("2.0/feed/json");
        }
        catch (HttpRequestException)
        {
            FoutMelding = "De gegevens van weerdata/actueel konden niet worden opgehaald, probeer het later opnieuw.";
        }

        if (!string.IsNullOrWhiteSpace(GekozenWeerStation) && StartDate != default)
        {
            string startDatum = StartDate.ToString("yyyy-MM-dd");
            string? eindDatum = EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : null;
            string WeerStation = Uri.EscapeDataString(GekozenWeerStation);
            
            string url = $"weerdata?StartDate={startDatum}";
            if (!string.IsNullOrWhiteSpace(eindDatum))
            {
                url += $"&EndDate={eindDatum}";
            }
            url += $"&Station={WeerStation}";

            try
            {
                DataPeriodeStation = await _client.GetFromJsonAsync<List<WeerMeting>>(url);
                // Data = await _client.GetFromJsonAsync<BuienradarJSON>("2.0/feed/json");
            }
            catch (HttpRequestException)
            {
                FoutMelding = "De gegevens van /weerdata konden niet worden opgehaald, probeer het later opnieuw.";
            }
        }
    }
}
