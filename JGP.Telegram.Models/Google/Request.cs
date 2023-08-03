using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class request
/// </summary>
public class Request
{
    /// <summary>
    ///     Gets or sets the value of the title
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    ///     Gets or sets the value of the total results
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("totalResults")]
    public string? TotalResults { get; set; }

    /// <summary>
    ///     Gets or sets the value of the search terms
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("searchTerms")]
    public string? SearchTerms { get; set; }

    /// <summary>
    ///     Gets or sets the value of the count
    /// </summary>
    /// <value>System.Nullable&lt;int&gt;</value>
    [JsonPropertyName("count")]
    public int? Count { get; set; }

    /// <summary>
    ///     Gets or sets the value of the start index
    /// </summary>
    /// <value>System.Nullable&lt;int&gt;</value>
    [JsonPropertyName("startIndex")]
    public int? StartIndex { get; set; }

    /// <summary>
    ///     Gets or sets the value of the input encoding
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("inputEncoding")]
    public string? InputEncoding { get; set; }

    /// <summary>
    ///     Gets or sets the value of the output encoding
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("outputEncoding")]
    public string? OutputEncoding { get; set; }

    /// <summary>
    ///     Gets or sets the value of the safe
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("safe")]
    public string? Safe { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cx
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cx")]
    public string? Cx { get; set; }

    /// <summary>
    ///     Gets or sets the value of the hl
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("hl")]
    public string? Hl { get; set; }
}