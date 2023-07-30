using JGP.Ai.OpenAi.Clients;
using JGP.Telegram.Services.Builders;
using JGP.Telegram.Services.FileConverters;

namespace JGP.Telegram.Services.Clients;

/// <summary>
///     Class dedicated client
/// </summary>
/// <seealso cref="IGPTClient" />
/// <seealso cref="IVoiceClient" />
public class DedicatedClient : IGPTClient //, IVoiceClient
{
    /// <summary>
    ///     The initialization message
    /// </summary>
    private const string InitializationMessage =
        "You are deployed in the format of a patient and friendly Telegram bot created by Joshua Gwynn-Palmer.";

    /// <summary>
    ///     The from seconds
    /// </summary>
    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    /// <summary>
    ///     The gpt client
    /// </summary>
    private readonly IGPTClient GPTClient;

    /// <summary>
    ///     The whisper client
    /// </summary>
    private readonly WhisperClient WhisperClient;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DedicatedClient" /> class
    /// </summary>
    /// <param name="chatId">The chat id</param>
    public DedicatedClient(string openAiApiKey, long chatId)
    {
        ChatId = chatId;
        LastMessage = DateTimeOffset.UtcNow;
        GPTClient = new TelegramOpenAiClient(openAiApiKey, InitializationMessage);
        WhisperClient = new WhisperClient(openAiApiKey);
    }

    /// <summary>
    ///     Gets the value of the chat id
    /// </summary>
    /// <value>long</value>
    public long ChatId { get; }

    /// <summary>
    ///     Gets the value of the user
    /// </summary>
    /// <value>System.Nullable&lt;string&gt;</value>
    public string? User { get; set;  }

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

    /// <summary>
    ///     Submits the voice note using the specified voice note url
    /// </summary>
    /// <param name="voiceNoteUrl">The voice note url</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>ValueTask&lt;(string? request, string? response)&gt;</returns>
    public async ValueTask<(string? request, string? response)> SubmitVoiceNoteAsync(string? voiceNoteUrl, string? systemMessage = null)
    {
        var directory = DirectoryBuilder.Build(ChatId);
        var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.ogg";
        var audioPath = Path.Combine(directory, fileName);

        var bytes = await _httpClient.GetByteArrayAsync(voiceNoteUrl);
        await File.WriteAllBytesAsync(audioPath, bytes);
        return await SubmitVoiceNoteToWhisperAndChatGptAsync(audioPath, systemMessage);
    }

    /// <summary>
    ///     Submits the voice note using the specified file path
    /// </summary>
    /// <param name="filePath">The file path</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>ValueTask&lt;string?&gt;</returns>
    private async ValueTask<(string? request, string? response)> SubmitVoiceNoteToWhisperAndChatGptAsync(
        string? filePath, string? systemMessage = null)
    {
        LastMessage = DateTimeOffset.UtcNow;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            return ("N/A", "Error: No voice note found.");
        }

        filePath = new OggToWavConverter().ConvertToWav(filePath, ChatId);

        var whisperResponse = await WhisperClient.SubmitAsync(filePath);
        if (whisperResponse == null)
        {
            return ("N/A", "Error: No response received.");
        }

        if (!whisperResponse.IsSuccess)
        {
            return ("N/A", whisperResponse.Error?.Message);
        }

        var request = whisperResponse.Text;
        var response = await SubmitAsync(whisperResponse.Text, systemMessage);

        return (request, response);
    }
}