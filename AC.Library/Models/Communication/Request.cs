using System.Text.Json.Serialization;

namespace AC.Library.Models.Communication;

internal class Request : PackInfo
{
    [JsonPropertyName("i")]
    public int I { get; set; }

    [JsonPropertyName("tcid")]
    public string TargetClientId { get; set; } = string.Empty;

    [JsonPropertyName("uid")]
    public int UID { get; set; }

    [JsonPropertyName("pack")]
    public string Pack { get; set; } = string.Empty;

    public static Request Create(string targetClientId, string pack, int i = 0)
    {
        return new Request()
        {
            ClientId = "app",
            Type = "pack",
            I = i,
            TargetClientId = targetClientId,
            Pack = pack,
            UID = 0
        };
    }
}