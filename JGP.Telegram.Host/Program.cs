using JGP.Logging.NativeLogging;
using JGP.Telegram.Core.Configuration;
using JGP.Telegram.Data;
using JGP.Telegram.Host;
using JGP.Telegram.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

using var scope = host.Services.CreateScope();
EnsureMigrationOfContext<ChatContext>(scope.ServiceProvider);

await host.RunAsync();

static void EnsureMigrationOfContext<T>(IServiceProvider provider) where T : DbContext
{
    var context = provider.GetService<T>();
    context?.Database.Migrate();
}

static void ConfigureServices(IServiceCollection services)
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables()
        .Build();
    
    var appSettings = ConfigureAppSettings(configuration);
    services.AddSingleton(appSettings);

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

    LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(services);

    services.AddMemoryCache();

    services.AddWindowsService(options => options.ServiceName = "JGP.Telegram.Host");

    var connectionString = configuration.GetConnectionString("ChatContext");
    services.AddDbContext<ChatContext>(options =>
        options.UseSqlServer(connectionString, builder =>
            builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null)));
    services.AddTransient<IChatContext, ChatContext>();
    services.AddTransient<IUserService, UserService>();
    services.AddTransient<IBotRunner, BotRunner>();

    services.AddHostedService<TelegramBotWorker>();
}

static AppSettings ConfigureAppSettings(IConfiguration configuration)
{
    var appSettings = new AppSettings();
    configuration.GetSection(AppSettings.ConfigurationSectionName).Bind(appSettings);
    
    var telegramApiKey = Environment.GetEnvironmentVariable("JGP_TELEGRAM_JGPTBOT_APIKEY", EnvironmentVariableTarget.User) ?? Environment.GetEnvironmentVariable("JGP_TELEGRAM_JGPTBOT_APIKEY", EnvironmentVariableTarget.Machine);
    if (!string.IsNullOrEmpty(telegramApiKey)) appSettings.TelegramApiKey = telegramApiKey;
    
    var openAiApiKey = Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.User) ?? Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.Machine);
    if (!string.IsNullOrEmpty(openAiApiKey)) appSettings.OpenAiApiKey = openAiApiKey;

    return appSettings;
}