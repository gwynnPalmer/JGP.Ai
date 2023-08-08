using JGP.Telegram.Core.Commands;
using JGP.Telegram.Core.Configuration;
using JGP.Telegram.Services;
using JGP.Telegram.Services.Clients;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace App.WindowsService.Handlers;

/// <summary>
///     Class voice note handler
/// </summary>
public class VoiceNoteHandler
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
    ///     The user service
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="VoiceNoteHandler" /> class
    /// </summary>
    /// <param name="botClient">The bot client</param>
    /// <param name="userService">The user service</param>
    /// <param name="appSettings">The app settings</param>
    public VoiceNoteHandler(ITelegramBotClient botClient, IUserService userService, AppSettings appSettings)
    {
        _botClient = botClient;
        _userService = userService;
        _appSettings = appSettings;
    }

    /// <summary>
    ///     Handles the voice using the specified client
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="update">The update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.ValueTask</returns>
    public async ValueTask HandleVoiceAsync(DedicatedClient client, Update update, CancellationToken cancellationToken)
    {
        var chatId = update.Message?.Chat.Id;

        if (update.Message?.Voice == null) return;

        var fileUrl = await GetVoiceNoteFileUrlAsync(update, cancellationToken);

        var systemMessage = GetSystemMessage(client, chatId);

        var (request, response) = await client.SubmitVoiceNoteAsync(chatId, fileUrl, systemMessage);

        var chatLogCommand = new ChatLogCreateCommand(chatId.ToString(), request, response);

        var replyTask = _botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
        var logTask = _userService.AddChatLogAsync(chatLogCommand, cancellationToken);

        await Task.WhenAll(replyTask, logTask);
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

    /// <summary>
    ///     Gets the system message using the specified client
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="chatId">The chat id</param>
    /// <returns>The system message</returns>
    private static string? GetSystemMessage(DedicatedClient client, long? chatId)
    {
        var systemMessage = string.IsNullOrWhiteSpace(client.User)
            ? null
            : $"You are speaking with {client.User}, ChatId: {chatId}. Remember to explain your chain of thought when responding.";

        return systemMessage;
    }
}