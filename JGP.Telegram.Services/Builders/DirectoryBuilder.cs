namespace JGP.Telegram.Services.Builders;

/// <summary>
///     Class directory builder
/// </summary>
public static class DirectoryBuilder
{
    /// <summary>
    ///     Gets the value of the default
    /// </summary>
    /// <value>System.String</value>
    public static string Default => DefaultDirectory();

    /// <summary>
    ///     Defaults the directory
    /// </summary>
    /// <returns>System.String</returns>
    private static string DefaultDirectory()
    {
        var directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\JGP.TELEGRAM";
        _ = Directory.CreateDirectory(directory);
        return directory;
    }

    /// <summary>
    ///     Builds the chat id
    /// </summary>
    /// <param name="chatId">The chat id</param>
    /// <returns>System.String</returns>
    public static string Build(long chatId)
    {
        var directory = $"{Default}\\{chatId}";
        _ = Directory.CreateDirectory(directory);
        return directory;
    }
}