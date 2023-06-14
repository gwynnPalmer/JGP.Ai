namespace AutoGPT.Agents;

public class Agent
{
    public string Name { get; protected set; }

    public object Memory { get; protected set; }

    public object FullMessageHistory { get; protected set; } // List<ChatMessage>
    
    public int NextActionCount { get; protected set; }
    
    public string SystemPrompt { get; protected set; }
    
    public string TriggeringPrompt { get; protected set; }

    public void StartInteractionLoop()
    {
        var configuration = new Configuration();
        var loopCount = 0;
        var commandName = string.Empty;
        var arguments = new List<string>();
        var userInput = string.Empty;

        while (true)
        {
            loopCount++;
            
            if (configuration is { ContinuousMode: true, ContinuousLimit: > 0 } && loopCount > configuration.ContinuousLimit) break;
            
            
        }
    }
}