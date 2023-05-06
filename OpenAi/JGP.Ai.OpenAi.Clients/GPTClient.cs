using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace JGP.Ai.OpenAi.Clients;

/// <summary>
///     Interface for the GPTClient, specify the method signature for SubmitAsync.
/// </summary>
public interface IGPTClient
{
    /// <summary>
    ///     Submit the message to the GPT model for processing
    /// </summary>
    /// <param name="message">The message to submit to the GPT-3 model</param>
    /// <param name="systemMessage">A context message.</param>
    /// <returns>The GPT model response as a string</returns>
    Task<string?> SubmitAsync(string? message, string? systemMessage = null);
}

/// <summary>
///     Client to interact with the GPT-3 language model provided by OpenAI using the OpenAI API.
/// </summary>
public class GPTClient : IGPTClient
{
    /// <summary>
    ///     The gpt api
    /// </summary>
    private static readonly IOpenAIAPI GPTApi;

    /// <summary>
    ///     The conversation
    /// </summary>
    private readonly Conversation _conversation;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GPTClient" /> class
    /// </summary>
    /// <exception cref="InvalidOperationException">No API key found </exception>
    static GPTClient()
    {
        var token = Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.User);
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("No API key found", new ArgumentNullException(nameof(token)));
        }

        GPTApi = new OpenAIAPI(token);
    }

    /// <summary>
    ///     Create a new conversation with the GPT model using the specified System message.
    /// </summary>
    /// <param name="systemMessage">The message to initiate the conversation with.</param>
    public GPTClient(string? systemMessage = null)
    {
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