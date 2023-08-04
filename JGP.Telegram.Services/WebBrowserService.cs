using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HtmlAgilityPack;
using JGP.DotNetGPT.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace JGP.Telegram.Services;

public class WebBrowserService : IDisposable
{
    /// <summary>
    ///     The implicit wait
    /// </summary>
    private const int ImplicitWait = 60;

    /// <summary>
    ///     The wait
    /// </summary>
    private readonly WebDriverWait? _wait;

    private readonly IWebDriver _webDriver;

    public WebBrowserService()
    {
        var options = new FirefoxOptions
        {
            AcceptInsecureCertificates = true
        };
        options.AddArgument("-headless");

        var service = FirefoxDriverService.CreateDefaultService(@"C:\\Development\\WebDrivers");
        service.LogLevel = FirefoxDriverLogLevel.Info;
        service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        //service.Host = "::1";

        _webDriver = new FirefoxDriver(service, options);
        _webDriver.Manage().Cookies.DeleteAllCookies();
        _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ImplicitWait);
        _webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ImplicitWait);
        _webDriver.Manage().Window.Maximize();

        _wait ??= new WebDriverWait(_webDriver, TimeSpan.FromSeconds(ImplicitWait))
        {
            PollingInterval = TimeSpan.FromMilliseconds(500),
            Message = "Timeout occurred",
            Timeout = TimeSpan.FromSeconds(ImplicitWait)
        };
    }

    public void Dispose()
    {
        _webDriver?.Close();
        _webDriver?.Quit();
        _webDriver?.Dispose();
    }

    /// <summary>
    ///     Browses the parameters json
    /// </summary>
    /// <param name="parametersJson">The parameters json</param>
    /// <returns>The text</returns>
    public async ValueTask<string?> BrowseAsync(string? parametersJson)
    {
        if (string.IsNullOrWhiteSpace(parametersJson))
        {
            return "Error: Invalid parameters - parameters cannot be empty";
        }

        var parameters = JsonSerializer.Deserialize<FunctionParameters>(parametersJson);
        if (parameters is null)
        {
            return "Error: Invalid parameters";
        }

        var url = parameters.Url;
        var text = await BrowseUrlAsync(url);
        return text;
    }

    /// <summary>
    ///     Browses the url using the specified url
    /// </summary>
    /// <param name="url">The url</param>
    /// <returns>Task&lt;string&gt;</returns>
    private async ValueTask<string> BrowseUrlAsync(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return "Error: Invalid URL - URL cannot be empty";
        }

        //var response = await HttpClient.GetAsync(url);
        _webDriver.Navigate().GoToUrl(url);
        _wait.Until(driver => driver.Url == url);
        _wait.Until(driver => driver.PageSource.Length > 0);

        var html = _webDriver.PageSource;
        return string.IsNullOrWhiteSpace(html)
            ? "Error: No HTML was returned"
            : ConvertHtmlToText(html);
    }

    /// <summary>
    ///     Converts raw HTML into a human-readable string by removing all tags and scripts.
    /// </summary>
    /// <param name="html">The raw HTML string.</param>
    /// <returns>A human-readable string containing the text content of the HTML.</returns>
    private static string ConvertHtmlToText(string html)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        // Collect all script and style nodes
        var nodesToRemove = htmlDocument.DocumentNode.DescendantsAndSelf()
            .Where(n => n.Name is "script" or "style")
            .ToList(); // Convert to list to avoid modifying the collection during enumeration

        // Remove collected nodes
        foreach (var node in nodesToRemove)
        {
            node.Remove();
        }

        // Replace anchor tags with their text content and URL in brackets
        foreach (var anchor in htmlDocument.DocumentNode.Descendants("a").ToList())
        {
            var linkText = anchor.InnerText.Trim();
            var href = anchor.GetAttributeValue("href", string.Empty);
            var replacementText = $"{linkText} ({href})";
            var replacementNode = HtmlNode.CreateNode(replacementText);
            anchor.ParentNode.ReplaceChild(replacementNode, anchor);
        }

        // Extract the text content
        var text = htmlDocument.DocumentNode.DescendantsAndSelf()
            .Where(n => n.NodeType == HtmlNodeType.Text)
            .Select(n => n.InnerText.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine(s), sb => sb.ToString());

        return text;
    }


    /// <summary>
    ///     Gets the function
    /// </summary>
    /// <returns>JGP.DotNetGPT.Models.Function</returns>
    public static Function GetFunction()
    {
        return new Function
        {
            Name = "BrowseWebsite",
            Description =
                "Use Selenium to browse a given URL and returns the available text content. Useful in conjunction with a 'Search' function.",
            Parameters = new Parameter
            {
                Type = "object",
                Properties = new Dictionary<string, Property>
                {
                    {
                        "url", new Property
                        {
                            Type = "string",
                            Description = "The URL to browse."
                        }
                    }
                },
                Required = new List<string>
                {
                    "url"
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
        ///     Gets or sets the value of the url
        /// </summary>
        /// <value>System.Nullable&lt;string&gt;</value>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}