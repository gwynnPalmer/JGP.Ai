namespace JGP.Telegram.Core.Configuration;

/// <summary>
///     Class app settings
/// </summary>
public class AppSettings
{
    /// <summary>
    ///     The configuration section name
    /// </summary>
    public const string ConfigurationSectionName = "AppSettings";

    /// <summary>
    ///     Gets or sets the value of the bot token
    /// </summary>
    /// <value>System.String</value>
    public string TelegramApiKey { get; set; }

    /// <summary>
    ///     Gets or sets the value of the open ai api key
    /// </summary>
    /// <value>System.String</value>
    public string OpenAiApiKey { get; set; }
}