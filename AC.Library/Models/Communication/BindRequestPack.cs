using System.Text.Json.Serialization;

namespace AC.Library.Models.Communication;

public class BindRequestPack
{
    [JsonPropertyName("t")]
    public string Type { get => "bind"; private set { } }

    [JsonPropertyName("uid")]
    public int UID { get => 0; private set { } }

    [JsonPropertyName("mac")]
    public string MAC { get; set; } = string.Empty;
}