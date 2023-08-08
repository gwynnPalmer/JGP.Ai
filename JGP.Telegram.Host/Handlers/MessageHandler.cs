using JGP.Telegram.Core.Commands;
using JGP.Telegram.Services;
using JGP.Telegram.Services.Clients;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace App.WindowsService.Handlers;

/// <summary>
///     Class message handler
/// </summary>
public class MessageHandler
{
    /// <summary>
    ///     The bot client
    /// </summary>
    private readonly ITelegramBotClient _botClient;

    /// <summary>
    ///     The user service
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageHandler" /> class
    /// </summary>
    /// <param name="botClient">The bot client</param>
    /// <param name="userService">The user service</param>
    public MessageHandler(ITelegramBotClient botClient, IUserService userService)
    {
        _botClient = botClient;
        _userService = userService;
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

    /// <summary>
    ///     Handles the message using the specified client
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="update">The update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.ValueTask</returns>
    public async ValueTask HandleMessageAsync(DedicatedClient client, Update update, CancellationToken cancellationToken)
    {
        var messageText = update.Message?.Text;
        if (string.IsNullOrWhiteSpace(messageText)) return;

        var chatId = update.Message?.Chat.Id;

        var systemMessage = GetSystemMessage(client, chatId);

        var response = await client.SubmitAsync(chatId, messageText, systemMessage);

        var chatLogCommand = new ChatLogCreateCommand(chatId.ToString(), messageText, response);

        var replyTask = _botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
        var logTask = _userService.AddChatLogAsync(chatLogCommand, cancellationToken);

        await Task.WhenAll(replyTask, logTask);
    }
}