using JGP.Telegram.Core.Configuration;

namespace App.WindowsService.Application.Configuration;

/// <summary>
///     Class app settings configuration
/// </summary>
public static class AppSettingsConfiguration
{
    /// <summary>
    ///     Configures the configuration
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <returns>The app settings</returns>
    public static AppSettings Configure(IConfiguration configuration)
    {
        var appSettings = new AppSettings();
        configuration.GetSection(AppSettings.ConfigurationSectionName).Bind(appSettings);

        var telegramApiKey =
            Environment.GetEnvironmentVariable("JGP_TELEGRAM_JGPTBOT_APIKEY", EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable("JGP_TELEGRAM_JGPTBOT_APIKEY", EnvironmentVariableTarget.Machine);
        if (!string.IsNullOrEmpty(telegramApiKey)) appSettings.TelegramApiKey = telegramApiKey;

        var openAiApiKey =
            Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable("JGP_CHATGPT_APIKEY", EnvironmentVariableTarget.Machine);
        if (!string.IsNullOrEmpty(openAiApiKey)) appSettings.OpenAiApiKey = openAiApiKey;

        var googleApiKey =
            Environment.GetEnvironmentVariable("JGP_GOOGLE_AI_APIKEY", EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable("JGP_GOOGLE_AI_APIKEY", EnvironmentVariableTarget.Machine);
        if (!string.IsNullOrEmpty(googleApiKey)) appSettings.GoogleApiKey = googleApiKey;

        var googleSearchEngineId =
            Environment.GetEnvironmentVariable("JGP_GOOGLE_CUSTOMSEARCHID", EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable("JGP_GOOGLE_CUSTOMSEARCHID", EnvironmentVariableTarget.Machine);
        if (!string.IsNullOrEmpty(googleSearchEngineId)) appSettings.GoogleSearchEngineId = googleSearchEngineId;

        return appSettings;
    }
}