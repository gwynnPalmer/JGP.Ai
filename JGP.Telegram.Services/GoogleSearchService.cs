using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using JGP.DotNetGPT.Core.Models;
using JGP.Telegram.Models.Google;
using JGP.Telegram.Services.Builders;

namespace JGP.Telegram.Services;

/// <summary>
///     Class google search service
/// </summary>
public class GoogleSearchService
{
    /// <summary>
    ///     The custom search route
    /// </summary>
    private const string CustomSearchRoute = "/customsearch/v1";

    /// <summary>
    ///     The from seconds
    /// </summary>
    private static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri("https://www.googleapis.com"),
        Timeout = TimeSpan.FromSeconds(10)
    };

    /// <summary>
    ///     The api key
    /// </summary>
    private readonly string _apiKey;

    /// <summary>
    ///     The search engine id
    /// </summary>
    private readonly string _searchEngineId;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GoogleSearchService" /> class
    /// </summary>
    /// <param name="apiKey">The api key</param>
    /// <param name="searchEngineId">The search engine id</param>
    public GoogleSearchService(string apiKey, string searchEngineId)
    {
        _apiKey = apiKey;
        _searchEngineId = searchEngineId;
    }

    /// <summary>
    ///     Searches the parameters json
    /// </summary>
    /// <param name="parametersJson">The parameters json</param>
    /// <returns>ValueTask&lt;string&gt;</returns>
    public async ValueTask<string> SearchAsync(string? parametersJson)
    {
        if (string.IsNullOrEmpty(parametersJson)) return "No parameters were provided";

        var parameters = JsonSerializer.Deserialize<FunctionParameters>(parametersJson);
        if (parameters == null) return "Invalid parameters were provided";

        var result = await SearchAsync(parameters.Query, parameters.MaxResults);

        return result == null
            ? "No results were found"
            : result.ToTransformedJson();
    }

    /// <summary>
    ///     Searches the query
    /// </summary>
    /// <param name="query">The query</param>
    /// <returns>Task&lt;GoogleSearchResultModel?&gt;</returns>
    public async Task<GoogleSearchResultModel?> SearchAsync(string query, int maxResults = 8)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, BuildRequestUrl(query, maxResults));
        using var response = await Client.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GoogleSearchResultModel>(content);
    }

    /// <summary>
    ///     Builds the request url using the specified query
    /// </summary>
    /// <param name="query">The query</param>
    /// <returns>System.String</returns>
    private string BuildRequestUrl(string query, int maxResults = 8)
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key", _apiKey },
            { "cx", _searchEngineId },
            { "q", query },
            { "num", maxResults.ToString() }
        };

        var collection = dictionary
            .Select(kvp => HttpUtility.UrlEncode(kvp.Key) + "=" + HttpUtility.UrlEncode(kvp.Value));

        return CustomSearchRoute + "?" + string.Join("&", collection);
    }

    /// <summary>
    ///     Gets the function
    /// </summary>
    /// <returns>MS.Internal.Xml.XPath.Function</returns>
    public static Function GetFunction()
    {
        return new Function
        {
            Name = "GoogleSearch",
            Description =
                "Searches Google for the specified query, useful in conjunction with a url browsing function.",
            Parameters = new Parameter
            {
                Type = "object",
                Properties = new Dictionary<string, Property>
                {
                    {
                        "query", new Property
                        {
                            Type = "string",
                            Description = "The query to search for"
                        }
                    },
                    {
                        "maxResults", new Property
                        {
                            Type = "number",
                            Description = "The maximum number of results to return (default: 8)"
                        }
                    }
                },
                Required = new List<string>
                {
                    "query"
                }
            }
        };
    }

    /// <summary>
    ///     Class function parameters
    /// </summary>
    public class FunctionParameters
    {
        /// <summary>
        ///     Gets or sets the value of the query
        /// </summary>
        /// <value>System.Nullable&lt;string&gt;</value>
        [JsonPropertyName("query")]
        public string? Query { get; set; }

        /// <summary>
        ///     Gets or sets the value of the max results
        /// </summary>
        /// <value>int</value>
        [JsonPropertyName("maxResults")]
        public int MaxResults { get; set; } = 8;
    }
}