using JGP.Logging.NativeLogging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

namespace App.WindowsService.Application.Configuration;

/// <summary>
///     Class logging configuration
/// </summary>
public static class LoggingConfiguration
{
    /// <summary>
    ///     Configures the configuration
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <param name="services">The services</param>
    public static void Configure(IConfiguration configuration, IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
            builder.AddNativeLogger(options =>
            {
                configuration.GetSection("Logging").GetSection("Native").GetSection("Options").Bind(options);
            });
        });

        LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(services);
    }
}