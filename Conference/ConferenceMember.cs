using JGP.Ai.OpenAi.Clients;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace Conference;

/// <summary>
///     Class conference member
/// </summary>
/// <seealso cref="IGPTClient" />
internal class ConferenceMember : IGPTClient
{
    /// <summary>
    ///     Enum heirarchical position
    /// </summary>
    public enum HeirarchicalPosition
    {
        /// <summary>
        ///     The host heirarchical position
        /// </summary>
        Host = 0,

        /// <summary>
        ///     The contributor heirarchical position
        /// </summary>
        Contributor = 1
    }

    /// <summary>
    ///     The conversation
    /// </summary>
    private readonly Conversation _conversation;

    /// <summary>
    ///     The system message
    /// </summary>
    private readonly string? SystemMessage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConferenceMember" /> class
    /// </summary>
    /// <param name="openAiApiKey">The open ai api key</param>
    /// <param name="temperature">The temperature</param>
    /// <param name="position">The position</param>
    /// <param name="systemMessage">The system message</param>
    public ConferenceMember(string openAiApiKey, double temperature, HeirarchicalPosition position,
        string? systemMessage = null)
    {
        var gptApi = new OpenAIAPI(openAiApiKey);
        _conversation = gptApi.Chat.CreateConversation(new ChatRequest
        {
            Temperature = temperature,
            Model = new Model("gpt-3.5-turbo-16k-0613")
        });

        Position = position;

        SystemMessage = systemMessage;
    }

    /// <summary>
    ///     Gets or sets the value of the position
    /// </summary>
    /// <value>HeirarchicalPosition</value>
    public HeirarchicalPosition Position { get; set; }

    /// <summary>
    ///     Gets or sets the value of the index
    /// </summary>
    /// <value>int</value>
    public int index { get; set; }

    /// <summary>
    ///     Submits the message
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="systemMessage">The system message</param>
    /// <returns>Task&lt;string?&gt;</returns>
    public async Task<string?> SubmitAsync(string? message, string? systemMessage = null)
    {
        systemMessage = string.IsNullOrWhiteSpace(systemMessage)
            ? SystemMessage
            : SystemMessage + Environment.NewLine + systemMessage;
        
        if (!string.IsNullOrWhiteSpace(systemMessage))
        {
            _conversation.AppendSystemMessage(systemMessage);
        }

        _conversation.AppendUserInput(message);
        return await _conversation.GetResponseFromChatbotAsync();
    }
}