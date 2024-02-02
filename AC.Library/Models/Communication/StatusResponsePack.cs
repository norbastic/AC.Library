using System.Text.Json.Serialization;

namespace AC.Library.Models.Communication;

public class StatusResponsePack
{
    [JsonPropertyName("t")]
    public string Type { get; set; }
    [JsonPropertyName("mac")]
    public string MAC { get; set; }
    [JsonPropertyName("r")]
    public int Rvalue { get; set; }
    [JsonPropertyName("cols")]
    public string[] Columns { get; set; }
    [JsonPropertyName("dat")]
    public int[] Data { get; set; }
}

