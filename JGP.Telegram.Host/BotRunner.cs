using System.Text;
using App.WindowsService.Handlers;
using JGP.Telegram.Core.Configuration;
using JGP.Telegram.Services;
using JGP.Telegram.Services.Clients;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = JGP.Telegram.Core.User;

namespace App.WindowsService;

/// <summary>
///     Interface bot runner
/// </summary>
/// <seealso cref="IDisposable" />
public interface IBotRunner : IDisposable
{
    /// <summary>
    ///     Starts the cancellation token
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    Task StartAsync(CancellationToken cancellationToken = default);
}

/// <summary>
///     Class bot runner
/// </summary>
/// <seealso cref="IBotRunner" />
public class BotRunner : IBotRunner
{
    /// <summary>
    ///     The app settings
    /// </summary>
    private readonly AppSettings _appSettings;

    /// <summary>
    ///     The bot client
    /// </summary>
    private readonly ITelegramBotClient _botClient;

    /// <summary>
    ///     The dedicated clients
    /// </summary>
    private readonly List<DedicatedClient> _dedicatedClients = new();

    /// <summary>
    ///     The logger
    /// </summary>
    private readonly ILogger<BotRunner> _logger;

    /// <summary>
    ///     The memory cache
    /// </summary>
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    ///     The memory service
    /// </summary>
    private readonly IMemoryService _memoryService;

    /// <summary>
    ///     The message orchestrator
    /// </summary>
    private readonly MessageOrchestrator _messageOrchestrator;

    /// <summary>
    ///     The user service
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BotRunner" /> class
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="memoryCache">The memory cache</param>
    /// <param name="memoryService">The memory service</param>
    /// <param name="userService">The user service</param>
    /// <param name="appSettings">The app settings</param>
    public BotRunner(ILogger<BotRunner> logger, IMemoryCache memoryCache, IMemoryService memoryService,
        IUserService userService, AppSettings appSettings)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _memoryService = memoryService;
        _userService = userService;
        _appSettings = appSettings;
        _botClient = new TelegramBotClient(appSettings.TelegramApiKey);
        _messageOrchestrator = new MessageOrchestrator(_botClient, _userService, appSettings);
    }

    #region DISPOSAL

    /// <summary>
    ///     Disposes this instance
    /// </summary>
    public void Dispose()
    {
        _memoryService.Dispose();
        _userService.Dispose();
    }

    #endregion

    /// <summary>
    ///     Starts the cancellation token
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _botClient.StartReceiving(HandleUpdateAsync, HandlePollingError, receiverOptions, cancellationToken);

        _ = await _botClient.GetMeAsync(cancellationToken);
    }

    /// <summary>
    ///     Gets the dedicated client using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="user">The user</param>
    /// <returns>The client</returns>
    private DedicatedClient GetDedicatedClient(long chatId, string? user = null)
    {
        _dedicatedClients.RemoveAll(client =>
            client.ChatId != chatId && client.LastMessage < DateTimeOffset.UtcNow.AddMinutes(-30));

        var client = _dedicatedClients.Find(client => client.ChatId == chatId);
        if (client is not null) return client;

        client = new DedicatedClient(_memoryService, _appSettings, chatId)
        {
            User = user
        };

        _dedicatedClients.Add(client);
        return client;
    }

    /// <summary>
    ///     Handles the update using the specified telegram bot client
    /// </summary>
    /// <param name="telegramBotClient">The telegram bot client</param>
    /// <param name="update">The update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is null) return;

        var chatId = update.Message.Chat.Id;

        try
        {
            var user = await GetUserAsync(chatId);
            if (user is null)
            {
                await _messageOrchestrator.HandleVerificationProcessAsync(update, chatId, cancellationToken);
                return;
            }

            if (!user.IsEnabled)
            {
                await ReplyDisabledAsync(chatId, user, cancellationToken);
                return;
            }

            var client = GetDedicatedClient(chatId, user.Name);
            if (update.Message.Text is not null)
            {
                await _messageOrchestrator.HandleMessageAsync(client, update, cancellationToken);
                return;
            }

            if (update.Message.Voice is not null)
            {
                await _messageOrchestrator.HandleVoiceAsync(client, update, cancellationToken);
                return;
            }

            await ReplyFailureAsync(chatId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling update for chat {ChatId}", chatId);
            await ReplyFailureAsync(chatId, cancellationToken);
        }
    }

    /// <summary>
    ///     Handles the polling error using the specified bot client
    /// </summary>
    /// <param name="botClient">The bot client</param>
    /// <param name="exception">The exception</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private Task HandlePollingError(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Error while polling");
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Gets the user using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <returns>The user</returns>
    private async ValueTask<User?> GetUserAsync(long? chatId)
    {
        if (chatId is null) return null;

        if (_memoryCache.TryGetValue(chatId.Value.ToString(), out User? user) && user is not null) return user;

        user = await _userService.GetUserAsync(chatId.Value);
        if (user is not null) _memoryCache.Set(chatId.Value.ToString(), user, TimeSpan.FromMinutes(10));

        return user;
    }

    /// <summary>
    ///     Replies the failure using the specified cancellation token
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="user">The user</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task ReplyFailureAsync(long chatId, CancellationToken cancellationToken)
    {
        const string response = "I'm sorry, something went very wrong.";
        _ = await _botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Replies the disabled using the specified cancellation token
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="user">The user</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task ReplyDisabledAsync(long chatId, User user, CancellationToken cancellationToken)
    {
        var response = new StringBuilder()
            .AppendLine($"Hi {user.Name}. You have been disabled from using this bot.")
            .AppendLine("If you believe this is a mistake, please contact Josh directly.")
            .ToString();

        _ = await _botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
    }
}