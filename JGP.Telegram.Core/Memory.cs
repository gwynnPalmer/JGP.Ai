using System.Text.Json.Serialization;

namespace JGP.Telegram.Core;

/// <summary>
///     Class memory
/// </summary>
public class Memory
{
    /// <summary>
    ///     Gets or sets the value of the speaker
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("speaker")]
    public string Speaker { get; set; }

    /// <summary>
    ///     Gets or sets the value of the message
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    ///     Gets or sets the value of the message date
    /// </summary>
    /// <value>System.DateTimeOffset</value>
    [JsonPropertyName("messageDate")]
    public DateTimeOffset MessageDate { get; set; }
}