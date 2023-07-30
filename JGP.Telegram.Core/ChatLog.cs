using System.Text.Json;
using System.Text.Json.Serialization;
using JGP.Telegram.Core.Commands;

namespace JGP.Telegram.Core;

/// <summary>
///     Class chat log
/// </summary>
/// <seealso cref="IEquatable{ChatLog}" />
/// <seealso cref="IEqualityComparer{ChatLog}" />
public class ChatLog : IEquatable<ChatLog>, IEqualityComparer<ChatLog>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ChatLog" /> class
    /// </summary>
    protected ChatLog()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ChatLog" /> class
    /// </summary>
    /// <param name="command">The command</param>
    public ChatLog(ChatLogCreateCommand command)
    {
        Id = Guid.NewGuid();
        ChatId = command.ChatId;
        MessageDate = DateTimeOffset.UtcNow;
        Request = command.Request;
        Response = command.Response;
    }

    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    /// <value>System.Guid</value>
    public Guid Id { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the chat id
    /// </summary>
    /// <value>System.String</value>
    public string ChatId { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the message date
    /// </summary>
    /// <value>System.DateTimeOffset</value>
    public DateTimeOffset MessageDate { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the request
    /// </summary>
    /// <value>System.Nullable&lt;string&gt;</value>
    public string? Request { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the response
    /// </summary>
    /// <value>System.Nullable&lt;string&gt;</value>
    public string? Response { get; protected set; }

    #region OVERRIDES & ESSENTIALS

    /// <summary>
    ///     Describes whether this instance equals
    /// </summary>
    /// <param name="other">The other</param>
    /// <returns>Interop+BOOL</returns>
    public bool Equals(ChatLog? other)
    {
        if (other is null) return false;

        return Id == other.Id
               && ChatId == other.ChatId
               && MessageDate == other.MessageDate
               && Request == other.Request
               && Response == other.Response;
    }

    /// <summary>
    ///     Describes whether this instance equals
    /// </summary>
    /// <param name="obj">The obj</param>
    /// <returns>Interop+BOOL</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return obj is ChatLog other && Equals(other);
    }

    /// <summary>
    ///     Describes whether this instance equals
    /// </summary>
    /// <param name="x">The </param>
    /// <param name="y">The </param>
    /// <returns>Interop+BOOL</returns>
    public bool Equals(ChatLog? x, ChatLog? y)
    {
        return x?.Equals(y) ?? false;
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    ///     Gets the hash code using the specified obj
    /// </summary>
    /// <param name="obj">The obj</param>
    /// <returns>int</returns>
    public int GetHashCode(ChatLog obj)
    {
        return obj.GetHashCode();
    }

    /// <summary>
    ///     Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        return JsonSerializer.Serialize(this, options);
    }

    #endregion
}