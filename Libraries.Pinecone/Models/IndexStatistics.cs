using System.Text.Json.Serialization;

namespace Libraries.Pinecone.Models;

public class IndexStatistics
{
    [JsonPropertyName("namespaces")]
    public object? Namespaces { get; set; }
    
    [JsonPropertyName("dimension")]
    public int Dimension { get; set; }
    
    [JsonPropertyName("indexFullness")]
    public int IndexFullness { get; set; }
    
    [JsonPropertyName("totalVectorCount")]
    public int TotalVectorCount { get; set; }
    
}