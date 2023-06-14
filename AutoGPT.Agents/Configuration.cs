namespace AutoGPT.Agents;

public class Configuration
{
    public bool DebugMode { get; set; } = false;
    
    public bool ContinuousMode { get; set; } = false;
    
    public int ContinuousLimit { get; set; } = 0;
    
    public bool SpeechMode { get; set; } = false;
    
    public bool SkipRePrompt { get; set; } = false;
    
    public bool AllowDownloads { get; set; } = false;
    
    public bool SkipNews { get; set; } = false;

    private string SettingsFileLocation { get; } = Environment.GetEnvironmentVariable("AI_SETTINGS_FILE", EnvironmentVariableTarget.User);
    
    public string FastLlmModel = Environment.GetEnvironmentVariable("FAST_LLM_MODEL", EnvironmentVariableTarget.User);
    
    private string SmartLlmModel = Environment.GetEnvironmentVariable("SMART_LLM_MODEL", EnvironmentVariableTarget.User);
    
    private int FastTokenLimit => GetEnvironmentalIntegerOrDefault("FAST_TOKEN_LIMIT", 4000);
    
    private int SmartTokenLimit => GetEnvironmentalIntegerOrDefault("SMART_TOKEN_LIMIT", 8000);
    
    private int BrowseChunkMaxLength => GetEnvironmentalIntegerOrDefault("BROWSE_CHUNK_MAX_LENGTH", 300);
    
    private string BrowseSpacyLanguageModel => GetEnvironmentalStringOrDefault("BROWSE_SPACY_LANGUAGE_MODEL", "en_core_web_sm");
    
    private string OpenApiKey => GetEnvironmentalStringOrDefault("OPEN_API_KEY", string.Empty);
    
    private float Temperature => 0; //TODO: Variable
    
    private bool UseAzure => Environment.GetEnvironmentVariable("USE_AZURE", EnvironmentVariableTarget.User) == "true";
    
    private bool ExecuteLocalCommands => Environment.GetEnvironmentVariable("EXECUTE_LOCAL_COMMANDS", EnvironmentVariableTarget.User) == "true";
    
    private bool RestrictToWorkspace => Environment.GetEnvironmentVariable("RESTRICT_TO_WORKSPACE", EnvironmentVariableTarget.User) == "true";

    public string OpenApiType { get; set; } = "azure";
    
    public string OpenApiBase { get; set; } = string.Empty;
    
    public string OpenApiVersion { get; set; } = "2023-03-15-preview";

    public Configuration()
    {
        if (UseAzure)
        {
            LoadAzureConfiguration();
            //TODO: OpenAi Api:
            var apiType = OpenApiType; // Some config variable or...
            var apiBase = OpenApiBase; // Some config variable or...
            var apiVersion = OpenApiVersion; // Some config variable or...
            object azureModelToDeploymentIdMap = null; // Some config variable or...
        }
    }

    private static int GetEnvironmentalIntegerOrDefault(string variable, int defaultValue)
    {
        var environmentalVariable = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);
        if (string.IsNullOrEmpty(environmentalVariable)) return defaultValue;

        return int.TryParse(environmentalVariable, out var value)
            ? value
            : defaultValue;
    }
    
    private static string GetEnvironmentalStringOrDefault(string variable, string defaultValue)
    {
        var environmentalVariable = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);
        return string.IsNullOrEmpty(environmentalVariable) ? defaultValue : environmentalVariable;
    }

    private void LoadAzureConfiguration()
    {
        
    }
}