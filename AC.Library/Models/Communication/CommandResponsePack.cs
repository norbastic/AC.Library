using System.Text.Json.Serialization;

namespace AC.Library.Models.Communication;

internal class CommandResponsePack
{
    [JsonPropertyName("opt")]
    public List<string> Columns { get; set; } = new List<string>();

    [JsonPropertyName("p")]
    public List<int> Values { get; set; } = new List<int>();
}
