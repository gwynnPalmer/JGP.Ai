using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class item
/// </summary>
public class Item
{
    /// <summary>
    ///     Gets or sets the value of the kind
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("kind")]
    public string? Kind { get; set; }

    /// <summary>
    ///     Gets or sets the value of the title
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    ///     Gets or sets the value of the html title
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("htmlTitle")]
    public string? HtmlTitle { get; set; }

    /// <summary>
    ///     Gets or sets the value of the link
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("link")]
    public string? Link { get; set; }

    /// <summary>
    ///     Gets or sets the value of the display link
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("displayLink")]
    public string? DisplayLink { get; set; }

    /// <summary>
    ///     Gets or sets the value of the snippet
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("snippet")]
    public string? Snippet { get; set; }

    /// <summary>
    ///     Gets or sets the value of the html snippet
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("htmlSnippet")]
    public string? HtmlSnippet { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cache id
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cacheId")]
    public string? CacheId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the formatted url
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("formattedUrl")]
    public string? FormattedUrl { get; set; }

    /// <summary>
    ///     Gets or sets the value of the html formatted url
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("htmlFormattedUrl")]
    public string? HtmlFormattedUrl { get; set; }

    /// <summary>
    ///     Gets or sets the value of the pagemap
    /// </summary>
    /// <value>Pagemap</value>
    [JsonPropertyName("pagemap")]
    public PageMap? PageMap { get; set; }
}