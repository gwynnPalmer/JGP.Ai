using JGP.Telegram.Core.Configuration;
using JGP.Telegram.Services;
using JGP.Telegram.Services.Clients;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace App.WindowsService.Handlers;

/// <summary>
///     Class message orchestrator
/// </summary>
public class MessageOrchestrator
{
    /// <summary>
    ///     The message handler
    /// </summary>
    private readonly MessageHandler _messageHandler;

    /// <summary>
    ///     The user verification handler
    /// </summary>
    private readonly UserVerificationHandler _userVerificationHandler;

    /// <summary>
    ///     The voice note handler
    /// </summary>
    private readonly VoiceNoteHandler _voiceNoteHandler;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageOrchestrator" /> class
    /// </summary>
    /// <param name="botClient">The bot client</param>
    /// <param name="userService">The user service</param>
    /// <param name="appSettings">The app settings</param>
    public MessageOrchestrator(ITelegramBotClient botClient, IUserService userService, AppSettings appSettings)
    {
        _messageHandler = new MessageHandler(botClient, userService);
        _userVerificationHandler = new UserVerificationHandler(botClient, userService);
        _voiceNoteHandler = new VoiceNoteHandler(botClient, userService, appSettings);
    }

    /// <summary>
    ///     Handles the verification process using the specified update
    /// </summary>
    /// <param name="update">The update</param>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    public async Task HandleVerificationProcessAsync(Update update, long chatId, CancellationToken cancellationToken)
    {
        await _userVerificationHandler.HandleVerificationProcessAsync(update, chatId, cancellationToken);
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
        await _voiceNoteHandler.HandleVoiceAsync(client, update, cancellationToken);
    }

    /// <summary>
    ///     Handles the message using the specified client
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="update">The update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.ValueTask</returns>
    public async ValueTask HandleMessageAsync(DedicatedClient client, Update update,
        CancellationToken cancellationToken)
    {
        await _messageHandler.HandleMessageAsync(client, update, cancellationToken);
    }
}