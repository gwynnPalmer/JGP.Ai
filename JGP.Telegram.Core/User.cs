using System.Text.Json;
using System.Text.Json.Serialization;
using JGP.Telegram.Core.Commands;

namespace JGP.Telegram.Core;

/// <summary>
///     Class user
/// </summary>
/// <seealso cref="IEquatable{User}" />
/// <seealso cref="IEqualityComparer{User}" />
public class User : IEquatable<User>, IEqualityComparer<User>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="User" /> class
    /// </summary>
    protected User()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="User" /> class
    /// </summary>
    /// <param name="command">The command</param>
    /// <exception cref="ArgumentNullException"></exception>
    public User(UserCreateCommand command)
    {
        _ = command ?? throw new ArgumentNullException(nameof(command));

        Id = Guid.NewGuid();
        Name = command.Name;
        ChatIds = command.ChatIds;
        Token = Guid.NewGuid();
        IsEnabled = true;
        CreatedDate = DateTimeOffset.UtcNow;
        LastModifiedDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    /// <value>System.Guid</value>
    public Guid Id { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the name
    /// </summary>
    /// <value>System.String</value>
    public string Name { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the chat ids
    /// </summary>
    /// <value>List&lt;long&gt;</value>
    public List<long> ChatIds { get; protected set; } = new();

    /// <summary>
    ///     Gets or sets the value of the token
    /// </summary>
    /// <value>System.Guid</value>
    public Guid Token { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the is enabled
    /// </summary>
    /// <value>Interop+BOOL</value>
    public bool IsEnabled { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the created date
    /// </summary>
    /// <value>System.DateTimeOffset</value>
    public DateTimeOffset CreatedDate { get; protected set; }

    /// <summary>
    ///     Gets or sets the value of the last modified date
    /// </summary>
    /// <value>System.DateTimeOffset</value>
    public DateTimeOffset LastModifiedDate { get; protected set; }

    #region DOMAIN METHODS

    /// <summary>
    ///     Disables this instance
    /// </summary>
    public void Disable()
    {
        IsEnabled = false;
        LastModifiedDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Enables this instance
    /// </summary>
    public void Enable()
    {
        IsEnabled = true;
        LastModifiedDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Refreshes the token
    /// </summary>
    public void RefreshToken()
    {
        Token = Guid.NewGuid();
        LastModifiedDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Adds the chat using the specified chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    public void AddChatId(long chatId)
    {
        if (ChatIds.Contains(chatId)) return;
        if (ChatIds.Count >= 3) throw new InvalidOperationException("You can only add up to 3 chat ids");

        ChatIds.Add(chatId);
        LastModifiedDate = DateTimeOffset.UtcNow;
    }

    #endregion

    #region OVERRIDES & ESSENTIALS

    /// <summary>
    ///     Describes whether this instance equals
    /// </summary>
    /// <param name="other">The other</param>
    /// <returns>Interop+BOOL</returns>
    public bool Equals(User? other)
    {
        if (other is null) return false;

        return Id == other.Id
               && Name == other.Name
               && ChatIds == other.ChatIds
               && Token == other.Token
               && IsEnabled == other.IsEnabled
               && CreatedDate == other.CreatedDate
               && LastModifiedDate == other.LastModifiedDate
               && ListEquals(ChatIds, other.ChatIds);
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

        return obj is User other && Equals(other);
    }

    /// <summary>
    ///     Describes whether this instance equals
    /// </summary>
    /// <param name="x">The </param>
    /// <param name="y">The </param>
    /// <returns>Interop+BOOL</returns>
    public bool Equals(User? x, User? y)
    {
        return x?.Equals(y) ?? false;
    }

    /// <summary>
    ///     Describes whether this instance list equals
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="list1">The list</param>
    /// <param name="list2">The list</param>
    /// <returns>Interop+BOOL</returns>
    private bool ListEquals<T>(List<T>? list1, List<T>? list2)
    {
        if (list1 is null && list2 is null) return true;
        if (list1 is null && list2 is not null) return false;
        if (list1 is not null && list2 is null) return false;
        if (list1.Count != list2.Count) return false;

        return list1
            .OrderBy(x => x)
            .SequenceEqual(list2.OrderBy(x => x));
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
    public int GetHashCode(User obj)
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