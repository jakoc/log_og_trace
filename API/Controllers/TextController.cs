using API.Models;
using API.Services;
using Messages;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TextController : ControllerBase
{
    
    [HttpGet]
    public IActionResult Get(string languageCode)
    {
        try
        {
            var greeting = GreetingService.Instance.Greet(new GreetingRequest { LanguageCode = languageCode });
            var planet = PlanetService.Instance.GetPlanet();
        
            var response = new GetGreetingModel.Response
            {
                Greeting = greeting.Greeting,
                Planet = planet.Planet
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An error occurred");
        }
    }
}