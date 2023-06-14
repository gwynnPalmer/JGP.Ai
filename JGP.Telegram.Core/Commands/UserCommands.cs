namespace JGP.Telegram.Core.Commands;

/// <summary>
///     Class user create command
/// </summary>
public class UserCreateCommand
{
    /// <summary>
    ///     Gets or sets the value of the name
    /// </summary>
    /// <value>System.String</value>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the value of the chat ids
    /// </summary>
    /// <value>List&lt;long&gt;</value>
    public List<long> ChatIds { get; set; }
}