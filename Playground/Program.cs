using System.Net.Http.Headers;
using JGP.Telegram.Services.FileConverters;

namespace Playground;

public static class Program
{
    public static async Task Main()
    {
        var converter = new OggToWavConverter();
        var filePath = converter.ConvertToWav("C:\\Users\\Josh\\Desktop\\example.ogg", 12345);
        
        Console.WriteLine(filePath);
        
        using var client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(30),
            DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY")) }
        }; 
        
        const string Endpoint = "https://api.openai.com/v1/audio/transcriptions";
        
        const string Model = "whisper-1";
        
        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/wav");

        var formData = new MultipartFormDataContent()
        {
            { new StringContent(Model), "model" },
            { fileContent, "file", "example.ogg" }
        };
        
        var response = await client.PostAsync(Endpoint, formData);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine(responseContent);
    }
}