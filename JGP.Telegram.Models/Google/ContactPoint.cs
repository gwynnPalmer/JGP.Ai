using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class contactpoint
/// </summary>
public class ContactPoint
{
    /// <summary>
    ///     Gets or sets the value of the url
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}