using JGP.DotNetGPT;
using JGP.DotNetGPT.Clients;

namespace DotNetGPT_Playground;

internal static class Program
{
    public static async Task Main()
    {
        var openAiApiKey = Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.User) ??
                           Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.Machine);

        var chatClient = ChatClient.Create(openAiApiKey, "gpt-4-0613");
        var input = Console.ReadLine();

        while(!string.IsNullOrWhiteSpace(input))
        {
            var response = await chatClient.SubmitAsync(input);

            if (response.IsSuccess())
            {
                Console.WriteLine(response.Choices[0].Message.Content);
            }
            else
            {
                Console.WriteLine(response.Error.Message);
            }

            input = Console.ReadLine();
        }
    }
}