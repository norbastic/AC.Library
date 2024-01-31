namespace AC.Library.Models;

using System.Text.Json.Serialization;

internal class DeviceInfoResponsePack
{
    [JsonPropertyName("bc")]
    public string BrandCode { get; set; } = string.Empty;

    [JsonPropertyName("brand")]
    public string Brand { get; set; } = string.Empty;

    [JsonPropertyName("catalog")]
    public string Catalog { get; set; } = string.Empty;

    [JsonPropertyName("mac")]
    public string ClientId { get; set; } = string.Empty;

    [JsonPropertyName("mid")]
    public string ModelId { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string FriendlyName { get; set; } = string.Empty;

    [JsonPropertyName("series")]
    public string Series { get; set; } = string.Empty;

    [JsonPropertyName("vender")]
    public string Vendor { get; set; } = string.Empty;

    [JsonPropertyName("ver")]
    public string FirmwareVersion { get; set; } = string.Empty;

    [JsonPropertyName("lock")]
    public int LockState { get; set; }
}