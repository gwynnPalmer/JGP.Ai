using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class cse thumbnail
/// </summary>
public class CseThumbnail
{
    /// <summary>
    ///     Gets or sets the value of the src
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("src")]
    public string? Src { get; set; }

    /// <summary>
    ///     Gets or sets the value of the width
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("width")]
    public string? Width { get; set; }

    /// <summary>
    ///     Gets or sets the value of the height
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("height")]
    public string? Height { get; set; }
}