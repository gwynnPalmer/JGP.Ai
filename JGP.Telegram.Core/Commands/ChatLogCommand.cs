namespace JGP.Telegram.Core.Commands;

/// <summary>
///     Class chat log command
/// </summary>
public class ChatLogCreateCommand
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ChatLogCreateCommand" /> class
    /// </summary>
    public ChatLogCreateCommand()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ChatLogCreateCommand" /> class
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="request">The request</param>
    /// <param name="response">The response</param>
    public ChatLogCreateCommand(string chatId, string? request, string? response)
    {
        ChatId = chatId;
        Request = request;
        Response = response;
    }

    /// <summary>
    ///     Gets or sets the value of the chat id
    /// </summary>
    /// <value>System.String</value>
    public string ChatId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the request
    /// </summary>
    /// <value>System.Nullable&lt;string&gt;</value>
    public string? Request { get; set; }

    /// <summary>
    ///     Gets or sets the value of the response
    /// </summary>
    /// <value>System.Nullable&lt;string&gt;</value>
    public string? Response { get; set; }
}