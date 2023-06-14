using App.WindowsService.Application.Configuration;
using JGP.Telegram.Data;

namespace App.WindowsService;

/// <summary>
///     Class program
/// </summary>
public static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <returns>System.Threading.Tasks.Task</returns>
    public static async Task Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices)
            .Build();

        using var scope = host.Services.CreateScope();
        scope.ServiceProvider.EnsureMigrationOfContext<ChatContext>();

        await host.RunAsync();
    }


    /// <summary>
    ///     Configures the services using the specified services
    /// </summary>
    /// <param name="services">The services</param>
    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        var appSettings = AppSettingsConfiguration.Configure(configuration);
        services.AddSingleton(appSettings);

        services.AddSingleton(configuration);

        IocConfiguration.Configure(configuration, services);
    }
}