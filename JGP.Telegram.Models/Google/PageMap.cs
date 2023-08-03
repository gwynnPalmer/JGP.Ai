using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class pagemap
/// </summary>
public class PageMap
{
    /// <summary>
    ///     Gets or sets the value of the cse thumbnail
    /// </summary>
    /// <value>List&lt;CseThumbnail&gt;</value>
    [JsonPropertyName("cse_thumbnail")]
    public List<CseThumbnail?> CseThumbnail { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the metatags
    /// </summary>
    /// <value>List&lt;Metatag&gt;</value>
    [JsonPropertyName("metatags")]
    public List<Metatag?> Metatags { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the cse image
    /// </summary>
    /// <value>List&lt;CseImage&gt;</value>
    [JsonPropertyName("cse_image")]
    public List<CseImage?> CseImage { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the organization
    /// </summary>
    /// <value>List&lt;Organization&gt;</value>
    [JsonPropertyName("organization")]
    public List<Organization?> Organization { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the contactpoint
    /// </summary>
    /// <value>List&lt;Contactpoint&gt;</value>
    [JsonPropertyName("contactpoint")]
    public List<ContactPoint?> Contactpoint { get; set; } = new();
}