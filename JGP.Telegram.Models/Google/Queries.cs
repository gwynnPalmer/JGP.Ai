using System.Text.Json.Serialization;

namespace JGP.Telegram.Models.Google;

/// <summary>
///     Class queries
/// </summary>
public class Queries
{
    /// <summary>
    ///     Gets or sets the value of the request
    /// </summary>
    /// <value>List&lt;Request&gt;</value>
    [JsonPropertyName("request")]
    public List<Request?> Request { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the next page
    /// </summary>
    /// <value>List&lt;NextPage&gt;</value>
    [JsonPropertyName("nextPage")]
    public List<NextPage?> NextPage { get; set; } = new();
}