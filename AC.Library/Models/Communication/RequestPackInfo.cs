using System.Text.Json.Serialization;

namespace AC.Library.Models.Communication;

internal class RequestPackInfo
{
    [JsonPropertyName("t")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("uid")]
    [JsonIgnore]
    public int? UID { get; set; }

    [JsonPropertyName("mac")]
    public string MAC { get; set; } = string.Empty;
}
