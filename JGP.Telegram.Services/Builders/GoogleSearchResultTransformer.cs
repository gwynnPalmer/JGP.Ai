using System.Text.Json;
using System.Text.Json.Serialization;
using JGP.Telegram.Models.Google;

namespace JGP.Telegram.Services.Builders;

/// <summary>
///     Class google search result transformer
/// </summary>
public static class GoogleSearchResultTransformer
{
    /// <summary>
    ///     The when writing null
    /// </summary>
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    ///     Returns the transformed model using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>The model</returns>
    public static GoogleSearchResultModel ToTransformedModel(this GoogleSearchResultModel model)
    {
        model.Kind = null;
        model.Url = null;
        model.Queries = null;
        model.Context = null;
        model.SearchInformation = null;

        for (var i = 0; i < model.Items.Count; i++)
        {
            var item = model.Items[i];
            item.Kind = null;
            item.HtmlTitle = null;
            item.DisplayLink = null;
            item.HtmlSnippet = null;
            item.CacheId = null;
            item.HtmlFormattedUrl = null;
            item.PageMap = null;
        }

        return model;
    }

    /// <summary>
    ///     Returns the json using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>System.String</returns>
    public static string ToTransformedJson(this GoogleSearchResultModel model)
    {
        return JsonSerializer.Serialize(model.ToTransformedModel(), _jsonSerializerOptions);
    }
}