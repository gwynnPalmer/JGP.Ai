using DotNetGPT.Models;
using JGP.Telegram.Core;
using JGP.Telegram.Core.FunctionParameters;
using JGP.Telegram.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JGP.Telegram.Services;

/// <summary>
///     Interface memory service
/// </summary>
/// <seealso cref="IDisposable" />
public interface IMemoryService : IDisposable
{
    /// <summary>
    ///     Gets the memories using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="keyword">The keyword</param>
    /// <param name="skip">The skip</param>
    /// <param name="take">The take</param>
    /// <returns>Task&lt;List&lt;Memory&gt;&gt;</returns>
    Task<List<Memory>> GetMemoriesAsync(long chatId, string keyword, int skip = 0, int take = 5);

    /// <summary>
    ///     Gets the memories using the specified parameters
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="parameters">The parameters</param>
    /// <returns>Task&lt;List&lt;Memory&gt;&gt;</returns>
    Task<List<Memory>> GetMemoriesAsync(long chatId, MemoryFunctionParameters parameters);

    /// <summary>
    ///     Gets the memories using the specified parameters json
    /// </summary>
    /// <param name="parametersJson">The parameters json</param>
    /// <returns>Task&lt;string&gt;</returns>
    Task<string> GetMemoriesAsync(string parametersJson);

    /// <summary>
    ///     Gets the memory function
    /// </summary>
    /// <returns>MS.Internal.Xml.XPath.Function</returns>
    Function GetMemoryFunction();
}

/// <summary>
///     Class memory service
/// </summary>
/// <seealso cref="IMemoryService" />
public class MemoryService : IMemoryService
{
    /// <summary>
    ///     The chat context
    /// </summary>
    private readonly IChatContext _chatContext;

    /// <summary>
    ///     The logger
    /// </summary>
    private readonly ILogger<MemoryService> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MemoryService" /> class
    /// </summary>
    /// <param name="chatContext">The chat context</param>
    /// <param name="logger">The logger</param>
    public MemoryService(IChatContext chatContext, ILogger<MemoryService> logger)
    {
        _chatContext = chatContext;
        _logger = logger;
    }

    #region DISPOSAL

    /// <summary>
    ///     Disposes this instance
    /// </summary>
    public void Dispose()
    {
        _chatContext.Dispose();
    }

    #endregion

    /// <summary>
    ///     Gets the memories using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="keyword">The keyword</param>
    /// <param name="skip">The skip</param>
    /// <param name="take">The take</param>
    /// <returns>Task&lt;List&lt;Memory&gt;&gt;</returns>
    public async Task<List<Memory>> GetMemoriesAsync(long chatId, string keyword, int skip = 0, int take = 5)
    {
        try
        {
            keyword = keyword
                .Split(' ')[0]
                .ToLower();

            var users = await _chatContext.Users
                .AsNoTracking()
                .ToArrayAsync();

            var user = users
                .FirstOrDefault(user => user.ChatIds.Contains(chatId));

            var chatIdString = chatId.ToString();

            var chatLogs = await _chatContext.ChatLogs
                .AsNoTracking()
                .Where(x => x.ChatId == chatIdString && (x.Request.Contains(keyword) || x.Response.Contains(keyword)))
                .OrderBy(x => x.MessageDate)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();

            if (chatLogs.Length == 0) return new List<Memory>();

            var memories = new List<Memory>();

            for (var i = 0; i < chatLogs.Length; i++)
            {
                var chatLog = chatLogs[i];

                var userMemory = new Memory
                {
                    User = user?.Name ?? "User",
                    Request = chatLog.Request,
                    Response = chatLog.Response,
                    MessageDate = chatLog.MessageDate
                };

                memories.Insert(memories.Count, userMemory);
            }

            return memories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting memories");
            return new List<Memory>();
        }
    }

    /// <summary>
    ///     Gets the memories using the specified parameters
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="parameters">The parameters</param>
    /// <returns>Task&lt;List&lt;Memory&gt;&gt;</returns>
    public async Task<List<Memory>> GetMemoriesAsync(long chatId, MemoryFunctionParameters parameters)
    {
        return await GetMemoriesAsync(chatId, parameters.Keyword, parameters.Skip, parameters.Take);
    }

    /// <summary>
    ///     Gets the memories using the specified parameters json
    /// </summary>
    /// <param name="parametersJson">The parameters json</param>
    /// <returns>Task&lt;string&gt;</returns>
    public async Task<string> GetMemoriesAsync(string parametersJson)
    {
        try
        {
            var parameters = JsonConvert.DeserializeObject<MemoryFunctionParameters>(parametersJson);
            var results = await GetMemoriesAsync(parameters.ChatId, parameters.Keyword, parameters.Skip, parameters.Take);

            return results.Count == 0
                ? "No results found"
                : JsonConvert.SerializeObject(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting memories");

            while (ex.InnerException != null) ex = ex.InnerException;

            return ex.Message;
        }
    }

    /// <summary>
    ///     Gets the memory function
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    /// <returns>MS.Internal.Xml.XPath.Function</returns>
    public Function GetMemoryFunction()
    {
        return new Function
        {
            Name = "SearchMemories",
            Description = "Searches all chat history with the user for messages containing the specified keyword.",
            Parameters = new Parameter
            {
                Type = "object",
                Properties = new Dictionary<string, Property>
                {
                    {
                        "chatId", new Property
                        {
                            Type = "integer",
                            Description = "The chat id"
                        }
                    },
                    {
                        "keyword", new Property
                        {
                            Type = "string",
                            Description = "The single keyword to search for"
                        }
                    },
                    {
                        "skip", new Property
                        {
                            Type = "integer",
                            Description = "The number of memories to skip (defaults to 0)"
                        }
                    },
                    {
                        "take", new Property
                        {
                            Type = "integer",
                            Description = "The number of memories to take (defaults to 5)"
                        }
                    }
                },
                Required = new List<string>
                {
                    "keyword"
                }
            }
        };
    }
}