using API.Monitoring;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace API.Services;

public class PlanetService
{
    private static PlanetService? _instance;
    
    public static PlanetService Instance
    {
        get { return _instance ??= new PlanetService(); }
    }
    
    public PlanetResponse GetPlanet()
    {
        using (var activity = MonitorService.ActivitySource.StartActivity("PlanetService.GetPlanet"))
        {
            MonitorService.Log.Information("PlanetService called to select a random planet");
            var planets = new[]
            {
                "Mercury",
                "Venus",
                "Earth",
                "Mars",
                "Jupiter",
                "Saturn",
                "Uranus",
                "Neptune"
            };

            var index = new Random(DateTime.Now.Millisecond).Next(1, planets.Length + 1);
            var selectedPlanet = planets[index];
            
            activity?.SetTag("planet.selected", selectedPlanet);
            MonitorService.Log.Debug("Selected planet: {Planet}", selectedPlanet);
            return new PlanetResponse
            {
                Planet = planets[index]
            };
        }
    }
}