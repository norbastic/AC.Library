namespace AC.Library.Models;

using System.Text.Json.Serialization;

internal class PackInfo
{
    [JsonPropertyName("t")]
    public string? Type { get; set; }

    [JsonPropertyName("cid")]
    public string? ClientId { get; set; }
}

internal class ResponsePackInfo : PackInfo
{
    [JsonPropertyName("uid")]
    [JsonIgnore]
    public int? UID { get; set; }

    [JsonPropertyName("tcid")]
    public string? TargetClientId { get; set; }

    [JsonPropertyName("pack")]
    public string? Pack { get; set; }
}