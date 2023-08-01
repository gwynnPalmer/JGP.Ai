using System.Text.Json.Serialization;

namespace JGP.Telegram.Core.FunctionParameters;

/// <summary>
///     Class memory function parameters
/// </summary>
public class MemoryFunctionParameters
{
    /// <summary>
    ///     Gets or sets the value of the key word
    /// </summary>
    /// <value>System.Nullable&lt;string&gt;</value>
    [JsonPropertyName("keyword")]
    public string? Keyword { get; set; }

    /// <summary>
    ///     Gets or sets the value of the skip
    /// </summary>
    /// <value>int</value>
    [JsonPropertyName("skip")]
    public int Skip { get; set; } = 0;

    /// <summary>
    ///     Gets or sets the value of the take
    /// </summary>
    /// <value>int</value>
    [JsonPropertyName("take")]
    public int Take { get; set; } = 5;

    /// <summary>
    ///     Gets or sets the value of the chat id
    /// </summary>
    /// <value>long</value>
    [JsonPropertyName("chatId")]
    public long ChatId { get; set; }
}