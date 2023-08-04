using JGP.DotNetGPT;
using JGP.DotNetGPT.Clients;
using JGP.DotNetGPT.Core.Constants;
using JGP.DotNetGPT.Core.Models;
using JGP.Telegram.Core.Configuration;
using JGP.Telegram.Services.Builders;
using JGP.Telegram.Services.FileConverters;

namespace JGP.Telegram.Services.Clients;

/// <summary>
///     Interface dedicated client
/// </summary>
/// <seealso cref="IChatClient" />
/// <seealso cref="IDisposable" />
public interface IDedicatedClient : IChatClient, IDisposable
{
    /// <summary>
    ///     Submits the message
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="message">The message</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>Task&lt;string?&gt;</returns>
    Task<string?> SubmitAsync(long? chatId, string? message, string? systemMessage = null);
}

/// <summary>
///     Class dedicated client
/// </summary>
/// <seealso cref="IDedicatedClient" />
public class DedicatedClient : IDedicatedClient
{
    /// <summary>
    ///     The initialization message
    /// </summary>
    private const string InitializationMessage =
        "You are deployed in the format of a patient and friendly Telegram bot created by Joshua Gwynn-Palmer.";

    /// <summary>
    ///     The from seconds
    /// </summary>
    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(60)
    };

    /// <summary>
    ///     The gpt client
    /// </summary>
    private readonly IChatClient _chatClient;

    /// <summary>
    ///     The function handler factory
    /// </summary>
    private readonly FunctionHandlerFactory _functionHandlerFactory = FunctionHandlerFactory.Create();

    /// <summary>
    ///     The google search service
    /// </summary>
    private readonly GoogleSearchService _googleSearchService;

    /// <summary>
    ///     The memory service
    /// </summary>
    private readonly IMemoryService _memoryService;

    /// <summary>
    ///     The whisper client
    /// </summary>
    private readonly WhisperClient _whisperClient;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DedicatedClient" /> class
    /// </summary>
    /// <param name="memoryService">The memory service</param>
    /// <param name="appSettings">The app settings</param>
    /// <param name="chatId">The chat id</param>
    public DedicatedClient(IMemoryService memoryService, AppSettings appSettings, long chatId)
    {
        ChatId = chatId;
        LastMessage = DateTimeOffset.UtcNow;

        _memoryService = memoryService;
        _whisperClient = new WhisperClient(appSettings.OpenAiApiKey);
        _googleSearchService = new GoogleSearchService(appSettings.GoogleApiKey, appSettings.GoogleSearchEngineId);

        var memoryFunction = MemoryService.GetFunction();
        var webScrapeFunction = WebBrowserService.GetFunction();
        var googleSearchFunction = GoogleSearchService.GetFunction();

        _chatClient = ChatClient
            .Create(appSettings.OpenAiApiKey, ModelConstants.GPT35Turbo16k0613)
            .SetClientTimeout(60)
            .AppendSystemMessage(InitializationMessage)
            .AppendFunction(memoryFunction)
            .AppendFunction(webScrapeFunction)
            .AppendFunction(googleSearchFunction);

        _functionHandlerFactory
            .AddFunctionHandler(memoryFunction.Name, async param => await _memoryService.GetMemoriesAsync(param))
            .AddFunctionHandler(webScrapeFunction.Name, async param =>
            {
                using var webBrowserService = new WebBrowserService();
                return await webBrowserService.BrowseAsync(param);
            })
            .AddFunctionHandler(googleSearchFunction.Name,
                async param => await _googleSearchService.SearchAsync(param));
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
    public string? User { get; init; }

    /// <summary>
    ///     Gets or sets the value of the last message
    /// </summary>
    /// <value>System.DateTimeOffset</value>
    public DateTimeOffset LastMessage { get; private set; }

    /// <summary>
    ///     Submits the message
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="message">The message</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>Task&lt;string?&gt;</returns>
    public async Task<string?> SubmitAsync(long? chatId, string? message, string? systemMessage = null)
    {
        if (!chatId.HasValue)
        {
            throw new ArgumentNullException(nameof(chatId), "The chat id is required");
        }

        LastMessage = DateTimeOffset.UtcNow;
        if (!string.IsNullOrWhiteSpace(systemMessage)) _chatClient.AppendSystemMessage(systemMessage);

        var response = await _chatClient.SubmitAsync(message);
        if (response is null) return null;

        while (response.IsFunctionCall())
        {
            try
            {
                var functionCall = response.Choices[0].Message.FunctionCall;

                var result = await _functionHandlerFactory
                    .ExecuteFunctionHandlerAsync(functionCall.Name, functionCall.Arguments);

                var functionResponse = result as string;

                response = await _chatClient.SubmitFunctionResponseAsync(functionCall.Name, functionResponse);
            }
            catch (Exception ex)
            {
                while (ex.InnerException is not null) ex = ex.InnerException;

                return
                    $"Something went wrong when executing a function: [{response?.Choices[0].Message?.FunctionCall?.Name}] {ex.Message}";
            }
        }

        return response.IsSuccess()
            ? response.Choices[0].Message?.Content
            : $"{response.Error?.Type}: {response.Error?.Message}";
    }

    /// <summary>
    ///     Disposes this instance
    /// </summary>
    public void Dispose()
    {
        _memoryService.Dispose();
    }

    /// <summary>
    ///     Submits the voice note using the specified voice note url
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="voiceNoteUrl">The voice note url</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>ValueTask&lt;(string? request, string? response)&gt;</returns>
    public async ValueTask<(string? request, string? response)> SubmitVoiceNoteAsync(long? chatId, string? voiceNoteUrl,
        string? systemMessage = null)
    {
        var directory = DirectoryBuilder.Build(ChatId);
        var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.ogg";
        var audioPath = Path.Combine(directory, fileName);

        var bytes = await HttpClient.GetByteArrayAsync(voiceNoteUrl);
        await File.WriteAllBytesAsync(audioPath, bytes);
        return await SubmitVoiceNoteToWhisperAndChatGptAsync(chatId, audioPath, systemMessage);
    }

    /// <summary>
    ///     Submits the voice note to whisper and chat gpt using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="filePath">The file path</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>ValueTask&lt;(string? request, string? response)&gt;</returns>
    private async ValueTask<(string? request, string? response)> SubmitVoiceNoteToWhisperAndChatGptAsync(long? chatId,
        string? filePath, string? systemMessage = null)
    {
        LastMessage = DateTimeOffset.UtcNow;

        if (string.IsNullOrWhiteSpace(filePath)) return ("N/A", "Error: No voice note found.");

        filePath = new OggToWavConverter().ConvertToWav(filePath, ChatId);

        var whisperResponse = await _whisperClient.SubmitAsync(filePath);

        if (whisperResponse == null) return ("N/A", "Error: No response received.");
        if (!whisperResponse.IsSuccess) return ("N/A", whisperResponse.Error?.Message);

        var request = whisperResponse.Text;
        var response = await SubmitAsync(chatId, whisperResponse.Text, systemMessage);

        return (request, response);
    }

    #region ICHATCLIENT

    /// <summary>
    ///     Appends the system message using the specified message
    /// </summary>
    /// <param name="message">The message</param>
    /// <returns>ChatClient</returns>
    public ChatClient AppendSystemMessage(string message)
    {
        return _chatClient.AppendSystemMessage(message);
    }

    /// <summary>
    ///     Appends the function using the specified function
    /// </summary>
    /// <param name="function">The function</param>
    /// <returns>ChatClient</returns>
    public ChatClient AppendFunction(Function function)
    {
        return _chatClient.AppendFunction(function);
    }

    /// <summary>
    ///     Removes the function using the specified name
    /// </summary>
    /// <param name="name">The name</param>
    /// <returns>ChatClient</returns>
    public ChatClient RemoveFunction(string name)
    {
        return _chatClient.RemoveFunction(name);
    }

    /// <summary>
    ///     Clears the functions
    /// </summary>
    /// <returns>ChatClient</returns>
    public ChatClient ClearFunctions()
    {
        return _chatClient.ClearFunctions();
    }

    /// <summary>
    ///     Submits the function response using the specified function name
    /// </summary>
    /// <param name="functionName">The function name</param>
    /// <param name="response">The response</param>
    /// <returns>Task&lt;ResponseModel?&gt;</returns>
    public async Task<ResponseModel?> SubmitFunctionResponseAsync(string functionName, string response)
    {
        return await _chatClient.SubmitFunctionResponseAsync(functionName, response);
    }

    /// <summary>
    ///     Submits the prompt
    /// </summary>
    /// <param name="prompt">The prompt</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>Task&lt;ResponseModel?&gt;</returns>
    async Task<ResponseModel?> IChatClient.SubmitAsync(string prompt, string? systemMessage)
    {
        return await _chatClient.SubmitAsync(prompt, systemMessage);
    }

    /// <summary>
    ///     Submits the request model
    /// </summary>
    /// <param name="requestModel">The request model</param>
    /// <returns>Task&lt;ResponseModel?&gt;</returns>
    public async Task<ResponseModel?> SubmitAsync(RequestModel requestModel)
    {
        return await _chatClient.SubmitAsync(requestModel);
    }

    #endregion
}