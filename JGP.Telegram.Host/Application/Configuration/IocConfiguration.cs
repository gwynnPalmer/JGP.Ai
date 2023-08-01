using JGP.Telegram.Data;
using JGP.Telegram.Services;
using Microsoft.EntityFrameworkCore;

namespace App.WindowsService.Application.Configuration;

/// <summary>
///     Class ioc configuration
/// </summary>
public static class IocConfiguration
{
    /// <summary>
    ///     Configures the configuration
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <param name="services">The services</param>
    public static void Configure(IConfiguration configuration, IServiceCollection services)
    {
        LoggingConfiguration.Configure(configuration, services);

        services.AddMemoryCache();

        services.AddWindowsService(options => options.ServiceName = "JGP.Telegram.Host");

        AddContext(configuration, services);
        RegisterServices(services);

        services.AddHostedService<TelegramBotWorker>();
    }

    /// <summary>
    ///     Registers the services using the specified services
    /// </summary>
    /// <param name="services">The services</param>
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IMemoryService, MemoryService>();
        services.AddTransient<IBotRunner, BotRunner>();
    }

    /// <summary>
    ///     Adds the context using the specified configuration
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <param name="services">The services</param>
    private static void AddContext(IConfiguration configuration, IServiceCollection services)
    {
        var connectionString = configuration.GetConnectionString("ChatContext");

        services.AddDbContext<ChatContext>(options =>
            options.UseSqlServer(connectionString, builder =>
                builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null)));

        services.AddTransient<IChatContext, ChatContext>();
    }

    /// <summary>
    ///     Ensures the migration of context using the specified service provider
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="serviceProvider">The service provider</param>
    public static void EnsureMigrationOfContext<T>(this IServiceProvider serviceProvider) where T : DbContext
    {
        var context = serviceProvider.GetService<T>();
        context?.Database.Migrate();
    }
}