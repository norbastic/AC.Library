using System.Text.Json.Serialization;

namespace AC.Library.Models;

public class StatusReuestPack
{
    [JsonPropertyName("t")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("mac")]
    public string MAC { get; set; } = string.Empty;
    [JsonPropertyName("cols")]
    public List<string> Columns { get; set; } = new();
}