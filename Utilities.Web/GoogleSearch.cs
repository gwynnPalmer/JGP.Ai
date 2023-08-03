using System.Text.Json;
using System.Web;
using Utilities.Web.Models.Google;

namespace Utilities.Web;

/// <summary>
///     Class google search
/// </summary>
public class GoogleSearch
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
    ///     Initializes a new instance of the <see cref="GoogleSearch" /> class
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public GoogleSearch()
    {
        var apiKey = Environment.GetEnvironmentVariable("JGP_GOOGLE_AI_APIKEY", EnvironmentVariableTarget.User);
        if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));

        var searchEngineId =
            Environment.GetEnvironmentVariable("JGP_GOOGLE_CUSTOMSEARCHID", EnvironmentVariableTarget.User);
        if (string.IsNullOrEmpty(searchEngineId)) throw new ArgumentNullException(nameof(searchEngineId));

        _apiKey = apiKey;
        _searchEngineId = searchEngineId;
    }

    public GoogleSearch(string apiKey, string searchEngineId)
    {
        _apiKey = apiKey;
        _searchEngineId = searchEngineId;
    }

    /// <summary>
    ///     Gets the result links using the specified query
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="maxResults">The max results</param>
    /// <returns>Task&lt;List&lt;string&gt;&gt;</returns>
    public async Task<List<string>> GetResultLinksAsync(string query, int maxResults = 8)
    {
        var result = await SearchAsync(query, maxResults);
        return result?.Items?
            .Where(x => !string.IsNullOrEmpty(x.Link))
            .Select(x => x.Link).ToList() ?? new List<string>();
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

    public async Task<string> JsonSearchAsync(string query, int maxResults = 8)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, BuildRequestUrl(query, maxResults));
        using var response = await Client.SendAsync(request);

        if (!response.IsSuccessStatusCode) return "No results found.";

        return await response.Content.ReadAsStringAsync();
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
}