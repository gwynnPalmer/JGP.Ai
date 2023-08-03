using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class search information
/// </summary>
public class SearchInformation
{
    /// <summary>
    ///     Gets or sets the value of the search time
    /// </summary>
    /// <value>System.Nullable&lt;double&gt;</value>
    [JsonPropertyName("searchTime")]
    public double? SearchTime { get; set; }

    /// <summary>
    ///     Gets or sets the value of the formatted search time
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("formattedSearchTime")]
    public string? FormattedSearchTime { get; set; }

    /// <summary>
    ///     Gets or sets the value of the total results
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("totalResults")]
    public string? TotalResults { get; set; }

    /// <summary>
    ///     Gets or sets the value of the formatted total results
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("formattedTotalResults")]
    public string? FormattedTotalResults { get; set; }
}