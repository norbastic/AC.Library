namespace AC.Library.Models;

using System.Text.Json.Serialization;
public class BindResponsePack
{
    [JsonPropertyName("mac")]
    public string? MAC { get; set; }

    [JsonPropertyName("key")]
    public string? Key { get; set; }
}