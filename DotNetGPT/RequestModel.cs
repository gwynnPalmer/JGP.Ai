using System.Text.Json.Serialization;

namespace DotNetGPT;

/// <summary>
///     Class request model
/// </summary>
public class RequestModel
{
    /// <summary>
    ///     Gets or sets the value of the model
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("model")]
    public string Model { get; set; } = "gpt-3.5-turbo-0613";

    /// <summary>
    ///     Gets or sets the value of the messages
    /// </summary>
    /// <value>List&lt;Message&gt;</value>
    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the functions
    /// </summary>
    /// <value>List&lt;Function&gt;</value>
    [JsonPropertyName("functions")]
    public List<Function> Functions { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the function call
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("function_call")]
    public string FunctionCall { get; set; } = "auto";
}

/// <summary>
///     Class message
/// </summary>
public class Message
{
    /// <summary>
    ///     Gets or sets the value of the role
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("role")]
    public string Role { get; set; }

    /// <summary>
    ///     Gets or sets the value of the content
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    ///     Gets or sets the value of the name
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

/// <summary>
///     Class function
/// </summary>
public class Function
{
    /// <summary>
    ///     Gets or sets the value of the name
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the value of the description
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    ///     Gets or sets the value of the parameters
    /// </summary>
    /// <value>System.Reflection.Metadata.Parameter</value>
    [JsonPropertyName("parameters")]
    public Parameter Parameters { get; set; }
}

/// <summary>
///     Class parameter
/// </summary>
public class Parameter
{
    /// <summary>
    ///     Gets or sets the value of the type
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     Gets or sets the value of the properties
    /// </summary>
    /// <value>Dictionary&lt;string, Property&gt;</value>
    [JsonPropertyName("properties")]
    public Dictionary<string, Property> Properties { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the required
    /// </summary>
    /// <value>List&lt;string&gt;</value>
    [JsonPropertyName("required")]
    public List<string> Required { get; set; } = new();
}

/// <summary>
///     Class property
/// </summary>
public class Property
{
    /// <summary>
    ///     Gets or sets the value of the type
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     Gets or sets the value of the description
    /// </summary>
    /// <value>System.String</value>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    ///     Gets or sets the value of the properties
    /// </summary>
    /// <value>Dictionary&lt;string, Property&gt;</value>
    [JsonPropertyName("properties")]
    public Dictionary<string, Property> Properties { get; set; } = new();

    /// <summary>
    ///     Gets or sets the value of the enum
    /// </summary>
    /// <value>List&lt;string&gt;</value>
    [JsonPropertyName("enum")]
    public List<string> Enum { get; set; } = new();
}