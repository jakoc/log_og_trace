using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using ILogger = Serilog.ILogger;

namespace API.Monitoring;

public static class MonitorService
{
    public static readonly string ServiceName = Assembly.GetCallingAssembly().GetName().Name ?? "Unknown";
    public static TracerProvider TraceProvider;
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
    
    public static ILogger Log => Serilog.Log.Logger;

    static MonitorService()
    {
        //OpenTelemetry
        TraceProvider = Sdk.CreateTracerProviderBuilder()
            .AddConsoleExporter()
            .AddZipkinExporter(o =>
            {
                o.Endpoint = new Uri("http://zipkin:9411/api/v2/spans");
            })
            .AddSource(ActivitySource.Name)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
            .Build();
        
        //Serilog
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();
        
    }
    
}