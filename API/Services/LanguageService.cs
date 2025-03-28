using API.Monitoring;
using Messages;
using Serilog;

namespace API.Services;

public class LanguageService
{
    private static LanguageService? _instance;
    
    public static LanguageService Instance
    {
        get { return _instance ??= new LanguageService(); }
    }
    
    private LanguageService()
    { }
    
    public LanguageResponse GetLanguages()
    {
        using (var activity = MonitorService.ActivitySource.StartActivity("LanguageService.GetLanguages"))
        {
            MonitorService.Log.Information("Fetching list of supported languages from GreetingService");

            var languages = GreetingService.Instance.GetLanguages(); // kald én gang

            activity?.SetTag("language.count", languages.Length);
            MonitorService.Log.Debug("Supported languages: {@Languages}", languages);

            return new LanguageResponse
            {
                Languages = languages
            };
        }
    }
}