using System.Text.Json.Serialization;

namespace AC.Library.Models.Communication;

internal class PackInfo
{
    [JsonPropertyName("t")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("cid")]
    public string ClientId { get; set; } = string.Empty;
}

internal class ResponsePackInfo : PackInfo
{
    [JsonPropertyName("uid")]
    [JsonIgnore]
    public int? UID { get; set; }

    [JsonPropertyName("tcid")]
    public string TargetClientId { get; set; } = string.Empty;

    [JsonPropertyName("pack")]
    public string Pack { get; set; } = string.Empty;
}