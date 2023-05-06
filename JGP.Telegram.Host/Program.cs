using JGP.Logging.NativeLogging;
using JGP.Telegram.Data;
using JGP.Telegram.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace JGP.Telegram.Host;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => ConfigureServices(services))
            .Build();
        
        using var scope = host.Services.CreateScope();
        scope.ServiceProvider.EnsureMigrationOfContext<ChatContext>();

        await host.RunAsync();
    } 
    
    private static void EnsureMigrationOfContext<T>(this IServiceProvider provider) where T : DbContext
    {
        var context = provider.GetService<T>();
        context?.Database.Migrate();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton(configuration);
        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
            builder.AddNativeLogger(options =>
            {
                configuration.GetSection("Logging").GetSection("Native").GetSection("Options").Bind(options);
            });
        });
        
        services.AddMemoryCache();

        var connectionString = configuration.GetConnectionString("ChatContext");
        services.AddDbContext<ChatContext>(options =>
            options.UseSqlServer(connectionString, builder =>
                builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null)));
        services.AddTransient<IChatContext, ChatContext>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IBotRunner, BotRunner>();
        
        services.AddHostedService<TelegramBotWorker>();
    }
}