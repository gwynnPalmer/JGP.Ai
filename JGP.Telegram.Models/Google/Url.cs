using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class url
/// </summary>
public class Url
{
    /// <summary>
    ///     Gets or sets the value of the type
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    ///     Gets or sets the value of the template
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("template")]
    public string? Template { get; set; }
}