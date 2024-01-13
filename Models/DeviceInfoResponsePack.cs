namespace AC.Library.Models;

using System.Text.Json.Serialization;

internal class DeviceInfoResponsePack
{
    [JsonPropertyName("bc")]
    public string? BrandCode { get; set; }

    [JsonPropertyName("brand")]
    public string? Brand { get; set; }

    [JsonPropertyName("catalog")]
    public string? Catalog { get; set; }

    [JsonPropertyName("mac")]
    public string? ClientId { get; set; }

    [JsonPropertyName("mid")]
    public string? ModelId { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("name")]
    public string? FriendlyName { get; set; }

    [JsonPropertyName("series")]
    public string? Series { get; set; }

    [JsonPropertyName("vender")]
    public string? Vendor { get; set; }

    [JsonPropertyName("ver")]
    public string? FirmwareVersion { get; set; }

    [JsonPropertyName("lock")]
    public int LockState { get; set; }
}