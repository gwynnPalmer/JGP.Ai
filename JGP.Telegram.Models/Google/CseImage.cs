using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class cse image
/// </summary>
public class CseImage
{
    /// <summary>
    ///     Gets or sets the value of the src
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("src")]
    public string? Src { get; set; }
}