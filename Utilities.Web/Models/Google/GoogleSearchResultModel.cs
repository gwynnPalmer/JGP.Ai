using System.Text.Json.Serialization;

namespace Utilities.Web.Models.Google;

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

/// <summary>
///     Class contactpoint
/// </summary>
public class ContactPoint
{
    /// <summary>
    ///     Gets or sets the value of the url
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

/// <summary>
///     Class context
/// </summary>
public class Context
{
    /// <summary>
    ///     Gets or sets the value of the title
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}

/// <summary>
///     Class cse image
/// </summary>
public class CseImage
{
    /// <summary>
    ///     Gets or sets the value of the src
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("src")]
    public string? Src { get; set; }
}

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

/// <summary>
///     Class metatag
/// </summary>
public class Metatag
{
    /// <summary>
    ///     Gets or sets the value of the msapplication tilecolor
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("msapplication-tilecolor")]
    public string? MsapplicationTilecolor { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og image
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:image")]
    public string? OgImage { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter title
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:title")]
    public string? TwitterTitle { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter card
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:card")]
    public string? TwitterCard { get; set; }

    /// <summary>
    ///     Gets or sets the value of the theme color
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("theme-color")]
    public string? ThemeColor { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter url
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:url")]
    public string? TwitterUrl { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og title
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:title")]
    public string? OgTitle { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter aria text
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:aria-text")]
    public string? TwitterAriaText { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og aria text
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:aria-text")]
    public string? OgAriaText { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og description
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:description")]
    public string? OgDescription { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter image
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:image")]
    public string? TwitterImage { get; set; }

    /// <summary>
    ///     Gets or sets the value of the referrer
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("referrer")]
    public string? Referrer { get; set; }

    /// <summary>
    ///     Gets or sets the value of the fb app id
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("fb:app_id")]
    public string? FbAppId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter site
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:site")]
    public string? TwitterSite { get; set; }

    /// <summary>
    ///     Gets or sets the value of the viewport
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("viewport")]
    public string? Viewport { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter description
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:description")]
    public string? TwitterDescription { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og url
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:url")]
    public string? OgUrl { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og type
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:type")]
    public string? OgType { get; set; }

    /// <summary>
    ///     Gets or sets the value of the fb pages
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("fb:pages")]
    public string? FbPages { get; set; }

    /// <summary>
    ///     Gets or sets the value of the pinterest rich pin
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("pinterest-rich-pin")]
    public string? PinterestRichPin { get; set; }

    /// <summary>
    ///     Gets or sets the value of the next head count
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("next-head-count")]
    public string? NextHeadCount { get; set; }

    /// <summary>
    ///     Gets or sets the value of the msapplication tileimage
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("msapplication-tileimage")]
    public string? MsapplicationTileimage { get; set; }

    /// <summary>
    ///     Gets or sets the value of the p domain verify
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("p:domain_verify")]
    public string? PDomainVerify { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og site name
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:site_name")]
    public string? OgSiteName { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter account id
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:account_id")]
    public string? TwitterAccountId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc content id
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:content_id")]
    public string? CdcContentId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the article published time
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("article:published_time")]
    public string? ArticlePublishedTime { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc maintained by
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:maintained_by")]
    public string? CdcMaintainedBy { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc first published
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:first_published")]
    public string? CdcFirstPublished { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og image type
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:image:type")]
    public string? OgImageType { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter creator
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:creator")]
    public string? TwitterCreator { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc version
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:version")]
    public string? CdcVersion { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc last published
    /// </summary>
    /// <value>System.Nullable&lt;DateTime&gt;</value>
    [JsonPropertyName("cdc:last_published")]
    public DateTime? CdcLastPublished { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc last reviewed
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:last_reviewed")]
    public string? CdcLastReviewed { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc build
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:build")]
    public string? CdcBuild { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc content source
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:content_source")]
    public string? CdcContentSource { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc last updated
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:last_updated")]
    public string? CdcLastUpdated { get; set; }

    /// <summary>
    ///     Gets or sets the value of the dc date
    /// </summary>
    /// <value>System.Nullable&lt;DateTime&gt;</value>
    [JsonPropertyName("dc.date")]
    public DateTime? DcDate { get; set; }

    /// <summary>
    ///     Gets or sets the value of the article author
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("article:author")]
    public string? ArticleAuthor { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc wcms build
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:wcms_build")]
    public string? CdcWcmsBuild { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter image src
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:image:src")]
    public string? TwitterImageSrc { get; set; }

    /// <summary>
    ///     Gets or sets the value of the cdc template version
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("cdc:template_version")]
    public string? CdcTemplateVersion { get; set; }

    /// <summary>
    ///     Gets or sets the value of the apple mobile web app capable
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("apple-mobile-web-app-capable")]
    public string? AppleMobileWebAppCapable { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og author
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:author")]
    public string? OgAuthor { get; set; }

    /// <summary>
    ///     Gets or sets the value of the mobile web app capable
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("mobile-web-app-capable")]
    public string? MobileWebAppCapable { get; set; }

    /// <summary>
    ///     Gets or sets the value of the format detection
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("format-detection")]
    public string? FormatDetection { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter app id googleplay
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:app:id:googleplay")]
    public string? TwitterAppIdGoogleplay { get; set; }

    /// <summary>
    ///     Gets or sets the value of the apple itunes app
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("apple-itunes-app")]
    public string? AppleItunesApp { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og image width
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:image:width")]
    public string? OgImageWidth { get; set; }

    /// <summary>
    ///     Gets or sets the value of the og image height
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("og:image:height")]
    public string? OgImageHeight { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter app id ipad
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:app:id:ipad")]
    public string? TwitterAppIdIpad { get; set; }

    /// <summary>
    ///     Gets or sets the value of the twitter app id iphone
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("twitter:app:id:iphone")]
    public string? TwitterAppIdIphone { get; set; }

    /// <summary>
    ///     Gets or sets the value of the google play app
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("google-play-app")]
    public string? GooglePlayApp { get; set; }

    /// <summary>
    ///     Gets or sets the value of the ahrefs site verification
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("ahrefs-site-verification")]
    public string? AhrefsSiteVerification { get; set; }

    /// <summary>
    ///     Gets or sets the value of the facebook domain verification
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("facebook-domain-verification")]
    public string? FacebookDomainVerification { get; set; }

    /// <summary>
    ///     Gets or sets the value of the msvalidate 01
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("msvalidate.01")]
    public string? Msvalidate01 { get; set; }

    /// <summary>
    ///     Gets or sets the value of the fb admins
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("fb:admins")]
    public string? FbAdmins { get; set; }
}

/// <summary>
///     Class next page
/// </summary>
public class NextPage
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

/// <summary>
///     Class organization
/// </summary>
public class Organization
{
    /// <summary>
    ///     Gets or sets the value of the telephone
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("telephone")]
    public string? Telephone { get; set; }

    /// <summary>
    ///     Gets or sets the value of the url
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

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

/// <summary>
///     Class url
/// </summary>
public class Url
{
    /// <summary>
    ///     Gets or sets the value of the type
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    ///     Gets or sets the value of the template
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("template")]
    public string? Template { get; set; }
}