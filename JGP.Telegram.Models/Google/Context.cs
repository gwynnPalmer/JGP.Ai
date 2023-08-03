using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class context
/// </summary>
public class Context
{
    /// <summary>
    ///     Gets or sets the value of the title
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}