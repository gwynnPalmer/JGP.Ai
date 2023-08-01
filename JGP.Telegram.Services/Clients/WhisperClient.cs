using System.Net.Http.Headers;
using System.Text.Json;
using DotNetGPT.Models;

namespace JGP.Telegram.Services.Clients;

/// <summary>
///     Class whisper client
/// </summary>
public class WhisperClient
{
    /// <summary>
    ///     The endpoint
    /// </summary>
    private const string Endpoint = "https://api.openai.com/v1/audio/transcriptions";

    /// <summary>
    ///     The model
    /// </summary>
    private const string Model = "whisper-1";

    /// <summary>
    ///     The from seconds
    /// </summary>
    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    /// <summary>
    ///     The api key
    /// </summary>
    private readonly string ApiKey;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WhisperClient" /> class
    /// </summary>
    /// <param name="apiKey">The api key</param>
    public WhisperClient(string apiKey)
    {
        ApiKey = apiKey;
    }

    /// <summary>
    ///     Submits the api key
    /// </summary>
    /// <param name="apiKey">The api key</param>
    /// <param name="filePath">The file path</param>
    /// <returns>ValueTask&lt;WhisperResponseModel?&gt;</returns>
    public async ValueTask<WhisperResponseModel?> SubmitAsync(string filePath)
    {
        if (!File.Exists(filePath)) return null;

        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/wav");

        var formData = new MultipartFormDataContent
        {
            { new StringContent(Model), "model" },
            { fileContent, "file", "example.wav" }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = formData,
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", ApiKey)
            }
        };

        using var response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WhisperResponseModel>(responseContent);
    }
}