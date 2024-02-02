using System.Text.Json.Serialization;

namespace AC.Library.Models.Communication;

internal class CommandRequestPack : RequestPackInfo
{
    [JsonPropertyName("opt")]
    public List<string> Columns { get; set; } = new List<string>();

    [JsonPropertyName("p")]
    public List<int> Values { get; set; } = new List<int>();

    public static CommandRequestPack Create(string clientId, Dictionary<string, int> parameters)
    {
        return new CommandRequestPack()
        {
            Type = "cmd",
            MAC = clientId,
            Columns = parameters.Select(x => x.Key).ToList(),
            Values = parameters.Select(x => x.Value).ToList(),
            UID = null
        };
    }
    
    public static CommandRequestPack Create(string clientId, string param, int value)
    {
        return new CommandRequestPack()
        {
            Type = "cmd",
            MAC = clientId,
            Columns = new List<string>{param},
            Values = new List<int>{value},
            UID = null
        };
    }

}