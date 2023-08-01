using System.Text.Json.Serialization;

namespace JGP.Telegram.Core;

/// <summary>
///     Class memory
/// </summary>
public class Memory
{
    /// <summary>
    ///     Gets or sets the value of the user
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("user")]
    public string User { get; set; }

    /// <summary>
    ///     Gets or sets the value of the request
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("request")]
    public string? Request { get; set; }

    /// <summary>
    ///     Gets or sets the value of the response
    /// </summary>
    /// <value>System.Nullable&lt;string&gt;</value>
    [JsonPropertyName("response")]
    public string? Response { get; set; }

    /// <summary>
    ///     Gets or sets the value of the message date
    /// </summary>
    /// <value>System.DateTimeOffset</value>
    [JsonPropertyName("messageDate")]
    public DateTimeOffset MessageDate { get; set; }
}