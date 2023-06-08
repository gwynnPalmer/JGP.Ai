using JGP.Ai.OpenAi.Clients;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace JGP.Telegram.Services.Clients;

/// <summary>
///     Class telegram open ai client
/// </summary>
/// <seealso cref="IGPTClient" />
public class TelegramOpenAiClient : IGPTClient
{
    /// <summary>
    ///     The conversation
    /// </summary>
    private readonly Conversation _conversation;

    /// <summary>
    ///     The gpt api
    /// </summary>
    private readonly IOpenAIAPI GPTApi;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TelegramOpenAiClient" /> class
    /// </summary>
    /// <param name="openAiApiKey">The open ai api key</param>
    /// <param name="systemMessage">The system message</param>
    public TelegramOpenAiClient(string openAiApiKey, string? systemMessage = null)
    {
        GPTApi = new OpenAIAPI(openAiApiKey);
        _conversation = GPTApi.Chat.CreateConversation();
        _conversation.Model = Model.ChatGPTTurbo;

        if (!string.IsNullOrWhiteSpace(systemMessage))
        {
            _conversation.AppendSystemMessage(systemMessage);
        }
    }

    /// <summary>
    ///     Submit user input to the GPT model for processing
    /// </summary>
    /// <param name="message">The message to submit to the GPT-3 model</param>
    /// <param name="systemMessage">A context message.</param>
    /// <returns>The GPT model response as a string</returns>
    public async Task<string?> SubmitAsync(string? message, string? systemMessage = null)
    {
        if (!string.IsNullOrWhiteSpace(systemMessage))
        {
            _conversation.AppendSystemMessage(systemMessage);
        }

        _conversation.AppendUserInput(message);
        return await _conversation.GetResponseFromChatbotAsync();
    }
}