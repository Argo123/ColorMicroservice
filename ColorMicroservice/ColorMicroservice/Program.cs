using Microsoft.AspNetCore;
using NLog.Web;

namespace ColorMicroservice.API;

internal sealed class Program
{
    public static Task Main(string[] args)
        => CreateWebHostBuilder(args).Build().RunAsync();

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        var builder = WebHost.CreateDefaultBuilder(args);

        return builder
            .UseStartup<Startup>()
            .ConfigureLogging(logging => logging.ClearProviders())
            .ConfigureAppConfiguration(configuration =>
                configuration.AddEnvironmentVariables())
            .UseNLog();
    }
}