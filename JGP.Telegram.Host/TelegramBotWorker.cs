namespace App.WindowsService;

/// <summary>
///     Class telegram bot worker
/// </summary>
/// <seealso cref="BackgroundService" />
public class TelegramBotWorker : BackgroundService
{
    /// <summary>
    ///     The logger
    /// </summary>
    private readonly ILogger<TelegramBotWorker> _logger;

    /// <summary>
    ///     The service scope factory
    /// </summary>
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TelegramBotWorker" /> class
    /// </summary>
    /// <param name="serviceScopeFactory">The service scope factory</param>
    /// <param name="logger">The logger</param>
    public TelegramBotWorker(IServiceScopeFactory serviceScopeFactory, ILogger<TelegramBotWorker> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    /// <summary>
    ///     Executes the stopping token
    /// </summary>
    /// <param name="stoppingToken">The stopping token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var botRunner = scope.ServiceProvider.GetRequiredService<IBotRunner>();
        await botRunner.StartAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("TelegramBotWorker running at: {Time}", DateTimeOffset.UtcNow.ToString());
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}