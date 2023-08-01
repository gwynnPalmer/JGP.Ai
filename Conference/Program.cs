using System.Text;

namespace Conference;

/// <summary>
///     Class program
/// </summary>
public static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <returns>System.Threading.Tasks.Task</returns>
    public static async Task Main()
    {
        var apiKey = GetApiKey();
        
        var host = BuildHost(apiKey);
        var member1 = BuildMember(apiKey, 1);
        var member2 = BuildMember(apiKey, 2);

        var context = new StringBuilder();
        
        var userMessage = Console.ReadLine();
        context.Append("User: " + userMessage);
        
        var initialResponse = await host.SubmitAsync(context.ToString(), "You will address Member1");
        context.AppendLine("-----")
            .AppendLine("-----")
            .AppendLine(initialResponse);

        try
        {
            while (!context.ToString().Contains("TERMINATE", StringComparison.OrdinalIgnoreCase))
            {
        
                var member1Response = await member1.SubmitAsync(context.ToString(), "You will address Member2, you can abandon the conversation by saying 'terminate'");
                context.AppendLine("-----")
                    .AppendLine("-----")
                    .AppendLine(member1Response);
        
                var member2Response = await member2.SubmitAsync(context.ToString(), "You will address the host, you can abandon the conversation by saying 'terminate'");
                context.AppendLine("-----")
                    .AppendLine("-----")
                    .AppendLine(member2Response);
        
                var hostResponse = await host.SubmitAsync(context.ToString(), "You may address Member1 if you feel there is more to discuss, say 'terminate' at any point to end the conversation.");
                context.AppendLine("-----")
                    .AppendLine("-----")
                    .AppendLine(hostResponse);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            Console.WriteLine(context.ToString());
        }
    }

    private static ConferenceMember BuildMember(string apiKey, int index)
    {
        var memberMessage = new StringBuilder()
            .AppendLine($"You are Member{index}, part of a committee with two other people. You are a member of the committee.")
            .AppendLine("The user will be the first person to speak.")
            .AppendLine("They will likely ask questions, and you will discuss them with the other committee members.")
            .AppendLine("The user will first address the chair, who will address the next member and so on, eventually going back to the chair who will respond to the user.")
            .AppendLine($"You must start your response with 'Member{index}:'.")
            .ToString();
        
        var random = new Random();
        
        var temperature = random.NextDouble();
        while (temperature < 0.7)
        {
            temperature = random.NextDouble();
        }

        var member1 = new ConferenceMember(apiKey, temperature,
            ConferenceMember.HeirarchicalPosition.Contributor, memberMessage)
        {
            index = index
        };
        return member1;
    }

    private static ConferenceMember BuildHost(string apiKey)
    {
        var hostMessage = new StringBuilder()
            .AppendLine("You are the chair of a committee.")
            .AppendLine("The user will likely ask questions, and you will discuss them with the other committee members.")
            .AppendLine("The user will first address you, and you will address the next member and so on, eventually going back to you who will respond to the user.")
            .AppendLine("You must start your response with 'Host:'.")
            .AppendLine("Say 'terminate' at any point to end the conversation.")
            .ToString();

        var host = new ConferenceMember(apiKey, 0.1, ConferenceMember.HeirarchicalPosition.Host, hostMessage)
        {
            index = 0
        };
        return host;
    }

    /// <summary>
    ///     Gets the api key
    /// </summary>
    /// <exception cref="Exception">OpenAI API key not found</exception>
    /// <returns>The open ai api key</returns>
    private static string GetApiKey()
    {
        var openAiApiKey = Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.User) ??
                           Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.Machine);

        if (string.IsNullOrWhiteSpace(openAiApiKey)) throw new Exception("OpenAI API key not found");

        return openAiApiKey;
    }
}