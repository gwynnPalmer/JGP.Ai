using JGP.Telegram.Core;
using JGP.Telegram.Core.FunctionParameters;
using JGP.Telegram.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
    /// <param name="keyword">The keyword</param>
    /// <param name="skip">The skip</param>
    /// <param name="take">The take</param>
    /// <returns>Task&lt;List&lt;Memory&gt;&gt;</returns>
    Task<List<Memory>> GetMemoriesAsync(string keyword, int skip = 0, int take = 5);

    /// <summary>
    ///     Gets the memories using the specified parameters
    /// </summary>
    /// <param name="parameters">The parameters</param>
    /// <returns>Task&lt;List&lt;Memory&gt;&gt;</returns>
    Task<List<Memory>> GetMemoriesAsync(MemoryFunctionParameters parameters);
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
    private readonly ILogger<UserService> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MemoryService" /> class
    /// </summary>
    /// <param name="chatContext">The chat context</param>
    /// <param name="logger">The logger</param>
    public MemoryService(IChatContext chatContext, ILogger<UserService> logger)
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
    /// <param name="keyword">The keyword</param>
    /// <param name="skip">The skip</param>
    /// <param name="take">The take</param>
    /// <exception cref="NotImplementedException"></exception>
    /// <returns>Task&lt;List&lt;Memory&gt;&gt;</returns>
    public async Task<List<Memory>> GetMemoriesAsync(string keyword, int skip = 0, int take = 5)
    {
        try
        {
            var chatLogs = await _chatContext.ChatLogs
                .AsNoTracking()
                .Where(x => x.Request.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                            || x.Response.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.MessageDate)
                .Skip(skip)
                .Take(take)
                .AsSplitQuery()
                .ToArrayAsync();

            var users = new List<User>();

            for (var i = 0; i < chatLogs.Length; i++)
            {
                var chatLog = chatLogs[i];
                var user = await _chatContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ChatIds.Contains(long.Parse(chatLog.ChatId)));

                if (user != null && !users.Contains(user)) users.Add(user);
            }

            if (chatLogs.Length == 0) return new List<Memory>();

            var memories = new List<Memory>();

            for (var i = 0; i < chatLogs.Length; i++)
            {
                var chatLog = chatLogs[i];
                var user = users
                    .Find(user => user.ChatIds.Contains(long.Parse(chatLog.ChatId)));

                var userMemory = new Memory
                {
                    Speaker = user?.Name ?? "User",
                    Message = chatLog.Request,
                    MessageDate = chatLog.MessageDate
                };

                var botMemory = new Memory
                {
                    Speaker = "Me (Bot)",
                    Message = chatLog.Response,
                    MessageDate = chatLog.MessageDate
                };

                memories.Insert(memories.Count, userMemory);
                memories.Insert(memories.Count, botMemory);
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
    /// <param name="parameters">The parameters</param>
    /// <returns>Task&lt;List&lt;Memory&gt;&gt;</returns>
    public async Task<List<Memory>> GetMemoriesAsync(MemoryFunctionParameters parameters)
    {
        return await GetMemoriesAsync(parameters.Keyword, parameters.Skip, parameters.Take);
    }
}