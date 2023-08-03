using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class search result model
/// </summary>
public class GoogleSearchResultModel
{
    /// <summary>
    ///     Gets or sets the value of the kind
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("kind")]
    public string? Kind { get; set; }

    /// <summary>
    ///     Gets or sets the value of the url
    /// </summary>
    /// <value>Url</value>
    [JsonPropertyName("url")]
    public Url? Url { get; set; }

    /// <summary>
    ///     Gets or sets the value of the queries
    /// </summary>
    /// <value>Queries</value>
    [JsonPropertyName("queries")]
    public Queries? Queries { get; set; }

    /// <summary>
    ///     Gets or sets the value of the context
    /// </summary>
    /// <value>Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraphBuilder+Context</value>
    [JsonPropertyName("context")]
    public Context? Context { get; set; }

    /// <summary>
    ///     Gets or sets the value of the search information
    /// </summary>
    /// <value>SearchInformation</value>
    [JsonPropertyName("searchInformation")]
    public SearchInformation? SearchInformation { get; set; }

    /// <summary>
    ///     Gets or sets the value of the items
    /// </summary>
    /// <value>List&lt;Item&gt;</value>
    [JsonPropertyName("items")]
    public List<Item?> Items { get; set; } = new();
}