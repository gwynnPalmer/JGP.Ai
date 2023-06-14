// using System.Text;
// using System.Text.Json.Serialization;
//
// namespace AutoGPT.Agents;
//
// public class ChatMessage
// {
//     [JsonPropertyName("role")]
//     public string? Role { get; set; }
//     
//     [JsonPropertyName("content")]
//     public string? Content { get; set; }
// }
//
// public class Context
// {
//     public int NextMessageToAddIndex { get; set; }
//     
//     public int CurrentTokensUsed { get; set; }
//     
//     public int InsertionIndex { get; set; }
//     
//     public List<ChatMessage> CurrentContext { get; set; }
//     
//     public void Extend(List<ChatMessage> messages)
//     {
//         throw new NotImplementedException();
//     }
// }
//
// public class Chat
// {
//     private Configuration _configuration;
//
//     public Chat(Configuration configuration)
//     {
//         _configuration = configuration;
//     }
//     
//     public ChatMessage CreateChatMessage(string role, string content)
//     {
//         return new ChatMessage
//         {
//             Role = role,
//             Content = content
//         };
//     }
//
//     public Context GenerateContext(string prompt, string relevantMemory, List<ChatMessage> fullMessageHistory, object model)
//     {
//         var currentContext = new List<ChatMessage>()
//         {
//             CreateChatMessage("system", prompt),
//             CreateChatMessage("system", $"The current time and date is {DateTimeOffset.UtcNow}"),
//             CreateChatMessage("system", new StringBuilder("This reminds you of these events from your past:")
//                 .AppendLine(relevantMemory)
//                 .AppendLine()
//                 .ToString())
//         };
//         
//         var nextMessageToAddIndex = fullMessageHistory.Count - 1;
//         var insertionIndex = currentContext.Count;
//         var currentTokensUsed = 0; //TODO: Implement TokenCounter.
//
//         return new Context()
//         {
//             NextMessageToAddIndex = nextMessageToAddIndex,
//             CurrentTokensUsed = currentTokensUsed,
//             InsertionIndex = insertionIndex,
//             CurrentContext = currentContext
//         };
//     }
//
//     public string ChatWithAi(string prompt, string userInput, List<ChatMessage> fullMessageHistory,
//         IPermanentMemory permanentMemory, int tokenLimit)
//     {
//         while (true)
//         {
//             var model = _configuration.FastLlmModel;
//             // Log token limit.
//             var sendTokenLimit = tokenLimit - 1000;
//             
//             var relevantMemory =  fullMessageHistory.Any()
//                 ? permanentMemory.GetRelevant() //TODO: This should take some parameters
//                 : string.Empty;
//             
//             // Log permanent memory stats.
//             
//             var context = GenerateContext(prompt, relevantMemory, fullMessageHistory, model);
//
//             while (context.CurrentTokensUsed > 2500)
//             {
//                 relevantMemory = string.Empty; // Need to figure out this [:-1] operation.
//                 context = GenerateContext(prompt, relevantMemory, fullMessageHistory, model);
//             }
//
//             ITokenCounter tokenCounter;
//             context.CurrentTokensUsed += tokenCounter.CountMessageTokens(new List<ChatMessage>()
//             {
//                 CreateChatMessage("user", userInput)
//             }, model);
//
//             while (context.NextMessageToAddIndex >= 0)
//             {
//                 try
//                 {
//                     var messageToAdd = fullMessageHistory[context.NextMessageToAddIndex];
//                     var tokensToAdd = tokenCounter.CountMessageTokens(new List<ChatMessage>()
//                     {
//                         messageToAdd
//                     }, model);
//                 
//                     if (context.CurrentTokensUsed + tokensToAdd > sendTokenLimit) break;
//                 
//                     context.CurrentContext.Insert(context.InsertionIndex, messageToAdd);
//                 
//                     context.CurrentTokensUsed += tokensToAdd;
//                     context.NextMessageToAddIndex--;
//                 
//                     context.Extend(new List<ChatMessage>()
//                     {
//                         CreateChatMessage("user", userInput)
//                     });
//                 
//                     var tokensRemaining = tokenLimit - context.CurrentTokensUsed;
//                 
//                     // Log loads of stuff.
//
//                     foreach (var message in context.CurrentContext)
//                     {
//                         if (message.Role == "system" && message.Content == prompt) continue;
//                         // Log the message.
//                     }
//                     //Log end of context.
//                 
//                     ILlmUtilityClass llmUtilityClass;
//                     var assistantReply = llmUtilityClass.CreateChatCompletion(context.CurrentContext, model, 0.9f, tokensRemaining);
//                 
//                     fullMessageHistory.Add(CreateChatMessage("user", userInput));
//                     fullMessageHistory.Add(CreateChatMessage("system", assistantReply));
//                     return assistantReply;
//                 }
//                 catch (Exception ex)
//                 {
//                     Thread.Sleep(10);
//                     // This is definitely going to be async, so change this to Task.Delay at some point.
//                     // In fact there's likely going to be a nicer way of doing this than sleeping.
//                 }
//             }
//             
//         }
//     }
// }
//
// public interface IPermanentMemory
// {
//     public string GetRelevant();
//     
//     public string GetStats();
// }
//
// public interface ITokenCounter
// {
//     public int CountMessageTokens(List<ChatMessage> chatMessages, string model);
// }
//
// public interface ILlmUtilityClass
// {
//     public string CreateChatCompletion(List<ChatMessage> messages, string model, float temperature, int maxTokens);
// }