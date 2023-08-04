using JGP.Ai.OpenAi.Clients;

namespace JGP.Ai.OpenAi.ConversationRunner;

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
        /*
         * Basic conversation.
         */
        IGPTClient participantA = new GPTClient("You are Participant A. You are in a conversation with Participant B.");
        IGPTClient participantB = new GPTClient("You are Participant B. You are in a conversation with Participant A.");

        /*
         * Board meeting conversation.
         */
        // IGPTClient participantA = new GPTClient("You are Participant A. You are a Board Member for a company that provides discounts for 3rd Party Services and Products to new Parents via a Members-Only website. You are in a board meeting with Participant B. The topic is \"How can we leverage AI\".");
        // IGPTClient participantB = new GPTClient("You are Participant B. You are a Board Member for a company that provides discounts for 3rd Party Services and Products to new Parents via a Members-Only website. You are in a board meeting with Participant A. The topic is \"How can we leverage AI\".");

        /*
         * Start-Up conversation.
         */
        // IGPTClient participantA = new GPTClient("You are Participant A. You are a talented C# Software Engineer. You are in a conversation with Participant B an equally talented C# Software Engineer. Together you want to create a Start-Up Company. The topic is \"What can we develop that leverages AI\".");
        // IGPTClient participantB = new GPTClient("You are Participant B. You are a talented C# Software Engineer. You are in a conversation with Participant A an equally talented C# Software Engineer. Together you want to create a Start-Up Company. The topic is \"What can we develop that leverages AI\".");

        /*
         * Real-Time coding conversation.
         */
        // IGPTClient participantA = new GPTClient("You are Participant A. You are a talented C# Software Engineer. You are pair-programming with Participant B, an equally talented C# Software Engineer. Together you are writing a C# class library capable of performing complex mathematical calculations.");
        // IGPTClient participantB = new GPTClient("You are Participant B. You are a talented C# Software Engineer. You are in a conversation with Participant A an equally talented C# Software Engineer. Together you are writing a C# class library capable of performing complex mathematical calculations..");


        await ExecuteConversation(participantA, participantB);
    }

    /// <summary>
    ///     Executes the conversation using the specified participant a
    /// </summary>
    /// <param name="participantA">The participant</param>
    /// <param name="participantB">The participant</param>
    /// <returns>System.Threading.Tasks.Task</returns>
    private static async Task ExecuteConversation(IGPTClient participantA, IGPTClient participantB)
    {
        //const string systemMessage = "Use the keyword \"##terminate\" in your response to end the discussion at any time. Before concluding the conversation you must produce a 5 point action plan for the next meeting.";
        const string systemMessage =
            "Use the keyword \"##terminate\" in your response to end the discussion at any time.";

        //const string systemMessage = "Use the keyword \"##terminate\" to end the discussion at any time. Before concluding the conversation you must produce a functional piece of C# code that is capable of performing complex mathematical calculations.";

        var message = "Participant A, please initiate the conversation with Participant B.";
        var currentParticipant = "ParticipantA";

        while (!message.Contains("##terminate", StringComparison.OrdinalIgnoreCase))
        {
            //message = await participantA.SubmitAsync(message, systemMessage);
            message = currentParticipant switch
            {
                "ParticipantA" => await participantA.SubmitAsync(message, systemMessage),
                "ParticipantB" => await participantB.SubmitAsync(message),
                _ => throw new InvalidOperationException($"Unknown participant: {currentParticipant}")
            };

            Console.WriteLine($"{currentParticipant}: {message}");

            // Swap the roles of participants for the next iteration.
            (participantA, participantB) = (participantB, participantA);
            currentParticipant = currentParticipant == "ParticipantA" ? "ParticipantB" : "ParticipantA";
        }
    }
}