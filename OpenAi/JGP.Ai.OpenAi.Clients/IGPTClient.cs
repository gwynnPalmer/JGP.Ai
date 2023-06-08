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

public interface IVoiceClient
{
    /// <summary>
    ///     Submits the voice note using the specified file path
    /// </summary>
    /// <param name="filePath">The file path</param>
    /// <returns>ValueTask&lt;string?&gt;</returns>
    ValueTask<string?> SubmitVoiceNoteAsync(string? filePath);
}