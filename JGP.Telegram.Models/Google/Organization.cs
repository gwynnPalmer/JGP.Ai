using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class organization
/// </summary>
public class Organization
{
    /// <summary>
    ///     Gets or sets the value of the telephone
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("telephone")]
    public string? Telephone { get; set; }

    /// <summary>
    ///     Gets or sets the value of the url
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}