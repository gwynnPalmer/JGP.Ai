using System.Text;
using JGP.Core.Services;
using JGP.Telegram.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = JGP.Telegram.Core.User;

namespace App.WindowsService.Handlers;

/// <summary>
///     Class user verification handler
/// </summary>
public class UserVerificationHandler
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
    ///     Initializes a new instance of the <see cref="UserVerificationHandler" /> class
    /// </summary>
    /// <param name="botClient">The bot client</param>
    /// <param name="userService">The user service</param>
    public UserVerificationHandler(ITelegramBotClient botClient, IUserService userService)
    {
        _userService = userService;
        _botClient = botClient;
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
        if (!string.IsNullOrWhiteSpace(update.Message.Text) &&
            update.Message.Text.StartsWith("Token:", StringComparison.OrdinalIgnoreCase))
        {
            await ProcessVerificationAsync(update, chatId, cancellationToken);
            return;
        }

        await ReplyUnverifiedAsync(chatId, cancellationToken);
    }

    /// <summary>
    ///     Processes the verification using the specified update
    /// </summary>
    /// <param name="update">The update</param>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task ProcessVerificationAsync(Update update, long chatId, CancellationToken cancellationToken)
    {
        var user = await AuthorizeUserAsync(update);
        if (user is not null)
        {
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
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private async Task ReplyUnverifiedAsync(long chatId, CancellationToken cancellationToken)
    {
        var unverifiedResponse = new StringBuilder()
            .AppendLine("I'm sorry, You are not verified.")
            .AppendLine(" Please respond with \"Token:[TOKEN]\" to authenticate, or contact the bot owner.")
            .ToString();

        _ = await _botClient.SendTextMessageAsync(chatId, unverifiedResponse, cancellationToken: cancellationToken);
    }
}