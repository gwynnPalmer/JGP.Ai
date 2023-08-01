using System.Text;
using JGP.Core.Services;
using JGP.Telegram.Core.Commands;
using JGP.Telegram.Core.Configuration;
using JGP.Telegram.Services.Clients;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = JGP.Telegram.Core.User;

namespace JGP.Telegram.Services;

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
    public BotRunner(ILogger<BotRunner> logger, IMemoryCache memoryCache, IMemoryService memoryService, IUserService userService,
        AppSettings appSettings)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _memoryService = memoryService;
        _userService = userService;
        _appSettings = appSettings;

        if (string.IsNullOrWhiteSpace(appSettings.TelegramApiKey))
        {
            throw new ArgumentNullException(nameof(appSettings.TelegramApiKey));
        }

        _botClient = new TelegramBotClient(appSettings.TelegramApiKey);
    }

    #region DISPOSAL

    /// <summary>
    ///     Disposes this instance
    /// </summary>
    public void Dispose()
    {
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

        _botClient.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions, cancellationToken);
        _ = await _botClient.GetMeAsync(cancellationToken);
    }

    #region DEDICATED CLIENTS

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

        client = new DedicatedClient(_memoryService, _appSettings.OpenAiApiKey, chatId)
        {
            User = user
        };

        _dedicatedClients.Add(client);
        return client;
    }

    #endregion

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
                await HandleVerificationProcessAsync(update, chatId, cancellationToken);
                return;
            }

            if (!user.IsEnabled)
            {
                await ReplyDisabledAsync(cancellationToken, chatId, user);
                return;
            }

            var client = GetDedicatedClient(chatId, user.Name);
            if (update.Message.Text is not null)
            {
                await HandleMessageAsync(client, update, cancellationToken);
                return;
            }

            if (update.Message.Voice is not null)
            {
                await HandleVoiceAsync(client, update, cancellationToken);
                return;
            }

            await ReplyFailureAsync(cancellationToken, chatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling update for chat {ChatId}", chatId);
            await ReplyFailureAsync(cancellationToken, chatId);
        }
    }

    /// <summary>
    ///     Handles the polling error using the specified bot client
    /// </summary>
    /// <param name="botClient">The bot client</param>
    /// <param name="exception">The exception</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Error while polling");
    }

    #region REPLY HANDLERS

    /// <summary>
    ///     Replies the failure using the specified cancellation token
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="chatId">The chat id</param>
    /// <param name="user">The user</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task ReplyFailureAsync(CancellationToken cancellationToken, long chatId)
    {
        const string response = "I'm sorry, something went very wrong.";
        _ = await _botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Replies the disabled using the specified cancellation token
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="chatId">The chat id</param>
    /// <param name="user">The user</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task ReplyDisabledAsync(CancellationToken cancellationToken, long chatId, User user)
    {
        var response = new StringBuilder()
            .AppendLine($"Hi {user.Name}. You have been disabled from using this bot.")
            .AppendLine("If you believe this is a mistake, please contact Josh directly.")
            .ToString();

        _ = await _botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Handles the message using the specified client
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="update">The update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.ValueTask</returns>
    private async ValueTask HandleMessageAsync(DedicatedClient client, Update update,
        CancellationToken cancellationToken)
    {
        var messageText = update.Message?.Text;
        if (string.IsNullOrWhiteSpace(messageText)) return;

        var chatId = update.Message?.Chat.Id;

        var systemMessage = string.IsNullOrWhiteSpace(client.User)
            ? null
            : $"You are speaking with {client.User}, ChatId: {chatId}";

        var response = await client.SubmitAsync(chatId, messageText, systemMessage);

        var chatLogCommand = new ChatLogCreateCommand(chatId.ToString(), messageText, response);

        _ = await _botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
        _ = await _userService.AddChatLogAsync(chatLogCommand, cancellationToken);
    }

    /// <summary>
    ///     Handles the voice using the specified client
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="update">The update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.ValueTask</returns>
    private async ValueTask HandleVoiceAsync(DedicatedClient client, Update update, CancellationToken cancellationToken)
    {
        var chatId = update.Message?.Chat.Id;

        if (update.Message?.Voice == null) return;

        var fileUrl = await GetVoiceNoteFileUrlAsync(update, cancellationToken);
        _logger.LogInformation("Voice note file url: {FileUrl}", fileUrl);

        var systemMessage = string.IsNullOrWhiteSpace(client.User)
            ? null
            : $"You are speaking with {client.User}, ChatId: {chatId}";

        var (request, response) = await client.SubmitVoiceNoteAsync(chatId, fileUrl, systemMessage);
        _logger.LogInformation("Voice note request: {Request}", request);
        _logger.LogInformation("Voice note response: {Response}", response);

        var chatLogCommand = new ChatLogCreateCommand(chatId.ToString(), request, response);

        _ = await _botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
        _ = await _userService.AddChatLogAsync(chatLogCommand, cancellationToken);
    }

    /// <summary>
    ///     Gets the voice note file url using the specified update
    /// </summary>
    /// <param name="update">The update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;string&gt;</returns>
    private async Task<string> GetVoiceNoteFileUrlAsync(Update update, CancellationToken cancellationToken)
    {
        var voice = update.Message?.Voice;
        var fileId = voice?.FileId;
        var fileInfo = await _botClient.GetFileAsync(fileId, cancellationToken);
        var filePath = fileInfo.FilePath;
        return $"https://api.telegram.org/file/bot{_appSettings.TelegramApiKey}/{filePath}";
    }

    #endregion

    #region USERS

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
    ///     Handles the verification process using the specified update
    /// </summary>
    /// <param name="update">The update</param>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task HandleVerificationProcessAsync(Update update, long chatId, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(update.Message.Text) &&
            update.Message.Text.StartsWith("Token:", StringComparison.OrdinalIgnoreCase))
        {
            await ProcessVerificationAsync(update, cancellationToken, chatId);
            return;
        }

        await ReplyUnverifiedAsync(cancellationToken, chatId);
    }

    /// <summary>
    ///     Processes the verification using the specified update
    /// </summary>
    /// <param name="update">The update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="chatId">The chat id</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task ProcessVerificationAsync(Update update, CancellationToken cancellationToken, long chatId)
    {
        var user = await AuthorizeUserAsync(update);
        if (user is not null)
        {
            _logger.LogInformation($"User {user.Name} has been verified");
            var verifiedResponse = $"Hi {user.Name}, You have been verified";
            _ = await _botClient.SendTextMessageAsync(chatId, verifiedResponse, cancellationToken: cancellationToken);
        }
        else
        {
            const string unverifiedResponse = "We were unable to verify you at this time";
            _ = await _botClient.SendTextMessageAsync(chatId, unverifiedResponse, cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    ///     Authorizes the user using the specified update
    /// </summary>
    /// <param name="update">The update</param>
    /// <returns>ValueTask&lt;User?&gt;</returns>
    private async ValueTask<User?> AuthorizeUserAsync(Update update)
    {
        try
        {
            var chatId = update.Message?.Chat.Id;
            var message = update.Message?.Text;
            if (chatId is null || message is null) return null;

            var tokenString = message.Split("Token:", StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault()?
                .Trim();

            var token = Guid.TryParse(tokenString, out var guid) ? guid : Guid.Empty;
            if (token == Guid.Empty) return null;

            var user = await _userService.GetUserAsync(token);
            if (user is null) return null;

            var receipt = await _userService.AddChatAsync(user.Id, chatId.Value);
            return receipt.Outcome == ActionOutcome.Success ? user : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    ///     Replies the unverified using the specified cancellation token
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="chatId">The chat id</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task ReplyUnverifiedAsync(CancellationToken cancellationToken, long chatId)
    {
        _logger.LogWarning("User is not verified");

        var unverifiedResponse = new StringBuilder()
            .AppendLine("I'm sorry, You are not verified.")
            .AppendLine(" Please respond with \"Token:[TOKEN]\" to authenticate, or contact the bot owner.")
            .ToString();

        _ = await _botClient.SendTextMessageAsync(chatId, unverifiedResponse, cancellationToken: cancellationToken);
    }

    #endregion
}