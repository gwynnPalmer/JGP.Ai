using JGP.Ai.OpenAi.Clients;

namespace JGP.Telegram.Services;

/// <summary>
///     Class dedicated client
/// </summary>
/// <seealso cref="IGPTClient" />
public class DedicatedClient : IGPTClient
{
    /// <summary>
    ///     The initialization message
    /// </summary>
    private const string InitializationMessage =
        "You are deployed in the format of a patient and friendly Telegram bot created by Joshua Gwynn-Palmer.";

    /// <summary>
    ///     The gpt client
    /// </summary>
    private readonly IGPTClient GPTClient;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DedicatedClient" /> class
    /// </summary>
    /// <param name="chatId">The chat id</param>
    public DedicatedClient(string openAiApiKey, long chatId)
    {
        ChatId = chatId;
        LastMessage = DateTimeOffset.UtcNow;
        GPTClient = new TelegramOpenAiClient(openAiApiKey, InitializationMessage);
    }

    /// <summary>
    ///     Gets the value of the chat id
    /// </summary>
    /// <value>long</value>
    public long ChatId { get; }

    /// <summary>
    ///     Gets or sets the value of the last message
    /// </summary>
    /// <value>System.DateTimeOffset</value>
    public DateTimeOffset LastMessage { get; private set; }

    /// <summary>
    ///     Submits the message
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>Task&lt;string?&gt;</returns>
    public async Task<string?> SubmitAsync(string? message, string? systemMessage = null)
    {
        LastMessage = DateTimeOffset.UtcNow;
        return await GPTClient.SubmitAsync(message, systemMessage);
    }
}