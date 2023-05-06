using JGP.Core.Services;
using JGP.Telegram.Core;
using JGP.Telegram.Core.Commands;
using JGP.Telegram.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JGP.Telegram.Services;

/// <summary>
///     Interface user service
/// </summary>
/// <seealso cref="IDisposable" />
public interface IUserService : IDisposable
{
    #region CHAT LOGS

    /// <summary>
    ///     Adds the chat log using the specified command
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> AddChatLogAsync(ChatLogCreateCommand command, CancellationToken cancellationToken = default);

    #endregion

    #region USERS

    /// <summary>
    ///     Adds the chat using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> AddChatAsync(Guid userId, long chatId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates the user using the specified command
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> CreateUserAsync(UserCreateCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Disables the user using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> DisableUserAsync(Guid token, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Disables the user using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> DisableUserAsync(long chatId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Enables the user using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> EnableUserAsync(Guid token, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Enables the user using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> EnableUserAsync(long chatId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the user using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;User?&gt;</returns>
    Task<User?> GetUserAsync(Guid token, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the user using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;User?&gt;</returns>
    Task<User?> GetUserAsync(long chatId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Refreshes the user token using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> RefreshUserTokenAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Refreshes the user token using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    Task<ActionReceipt> RefreshUserTokenAsync(long chatId, CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
///     Class user service
/// </summary>
/// <seealso cref="IUserService" />
public class UserService : IUserService
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
    ///     Initializes a new instance of the <see cref="UserService" /> class
    /// </summary>
    /// <param name="chatContext">The chat context</param>
    /// <param name="logger">The logger</param>
    public UserService(IChatContext chatContext, ILogger<UserService> logger)
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
    ///     Adds the chat using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> AddChatAsync(Guid userId, long chatId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _chatContext.Users
                .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

            if (user == null)
            {
                return ActionReceipt.GetErrorReceipt("User not found");
            }

            user.AddChatId(chatId);
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding chat id {ChatId} to user {UserId}", chatId.ToString(),
                userId.ToString());
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    /// <summary>
    ///     Creates the user using the specified command
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> CreateUserAsync(UserCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _chatContext.Users
                .AnyAsync(user => user.Name == command.Name, cancellationToken);

            if (exists) return ActionReceipt.GetErrorReceipt("User already exists");

            var user = new User(command);
            await _chatContext.Users.AddAsync(user, cancellationToken);
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    /// <summary>
    ///     Disables the user using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> DisableUserAsync(Guid token, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _chatContext.Users
                .FirstOrDefaultAsync(user => user.Token == token, cancellationToken);

            if (user == null) return ActionReceipt.GetErrorReceipt("User not found");

            user.Disable();
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling user for id {UserId}", token.ToString());
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    /// <summary>
    ///     Disables the user using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> DisableUserAsync(long chatId, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _chatContext.Users
                .ToListAsync(cancellationToken);

            var user = users
                .FirstOrDefault(user => user.ChatIds.Contains(chatId));

            if (user == null) return ActionReceipt.GetErrorReceipt("User not found");

            user.Disable();
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling user for chat id {ChatId}", chatId.ToString());
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    /// <summary>
    ///     Enables the user using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> EnableUserAsync(Guid token, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _chatContext.Users
                .FirstOrDefaultAsync(user => user.Token == token, cancellationToken);

            if (user == null) return ActionReceipt.GetErrorReceipt("User not found");

            user.Enable();
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling user for id {UserId}", token.ToString());
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    /// <summary>
    ///     Enables the user using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> EnableUserAsync(long chatId, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _chatContext.Users
                .ToListAsync(cancellationToken);

            var user = users
                .FirstOrDefault(user => user.ChatIds.Contains(chatId));

            if (user == null) return ActionReceipt.GetErrorReceipt("User not found");

            user.Enable();
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling user for chat id {ChatId}", chatId.ToString());
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    /// <summary>
    ///     Gets the user using the specified token
    /// </summary>
    /// <param name="token">The token</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;User?&gt;</returns>
    public async Task<User?> GetUserAsync(Guid token, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _chatContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Token == token, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user for id {UserId}", token.ToString());
            return null;
        }
    }

    /// <summary>
    ///     Gets the user using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;User?&gt;</returns>
    public async Task<User?> GetUserAsync(long chatId, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _chatContext.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return users
                .FirstOrDefault(user => user.ChatIds.Contains(chatId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user for chat id {ChatId}", chatId.ToString());
            return null;
        }
    }

    /// <summary>
    ///     Refreshes the user token using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> RefreshUserTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _chatContext.Users
                .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

            if (user == null) return ActionReceipt.GetErrorReceipt("User not found");

            user.RefreshToken();
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing user token for id {UserId}", userId.ToString());
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    /// <summary>
    ///     Refreshes the user token using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> RefreshUserTokenAsync(long chatId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _chatContext.Users
                .FirstOrDefaultAsync(user => user.ChatIds.Contains(chatId), cancellationToken);

            if (user == null) return ActionReceipt.GetErrorReceipt("User not found");

            user.RefreshToken();
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing user token for chat id {ChatId}", chatId.ToString());
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    #region CHAT LOGS

    /// <summary>
    ///     Adds the chat log using the specified command
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task&lt;ActionReceipt&gt;</returns>
    public async Task<ActionReceipt> AddChatLogAsync(ChatLogCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var chatLog = new ChatLog(command);
            await _chatContext.ChatLogs.AddAsync(chatLog, cancellationToken);
            var affectedCount = await _chatContext.SaveChangesAsync(cancellationToken);
            return ActionReceipt.GetSuccessReceipt(affectedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding chat log for chat id {ChatId}", command.ChatId);
            return ActionReceipt.GetErrorReceipt(ex);
        }
    }

    #endregion
}