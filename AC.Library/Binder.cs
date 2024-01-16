namespace AC.Library;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using Utils;
using Models;
using Interfaces;

public class Binder {
    private readonly ILogger<Binder> _logger;
    private readonly IUdpClientWrapper _udpClientWrapper;

    public Binder(ILogger<Binder> logger, IUdpClientWrapper udpClientWrapper)
    {
        _logger = logger;
        _udpClientWrapper = udpClientWrapper;
    }

    private string? GetKeyFromUdpResponse(UdpReceiveResult udpReceiveResult)
    {
        var responseJson = Encoding.ASCII.GetString(udpReceiveResult.Buffer);
        var responsePackInfo = JsonSerializer.Deserialize<ResponsePackInfo>(responseJson);
        if (!ResponseChecker.IsReponsePackInfoValid(responsePackInfo!)) return null;
        
        var decryptedData = Crypto.DecryptGenericData(responsePackInfo!.Pack);
        if (decryptedData == null)
        {
            return null;
        }
            
        var bindResponse = JsonSerializer.Deserialize<BindResponsePack>(decryptedData);
        return bindResponse?.Key;
    }
    
    /// <summary>
    /// It binds one AC device to the current machine
    /// </summary>
    /// <param name="macAddress">MAC address of the device</param>
    /// <param name="ipAddress">IP address of the device</param>
    /// <returns>Returns with the [key] if successful, otherwise null</returns>
    public async Task<string?> BindOne(string macAddress, string ipAddress)
    {
        var bindRequestPack = new BindRequestPack() { MAC = macAddress };
        var encryptedData = Crypto.EncryptGenericData(JsonSerializer.Serialize(bindRequestPack));
        if (encryptedData == null)
        {
            return null;
        }
        var request = Request.Create(macAddress, encryptedData, 1);
        var requestJson = JsonSerializer.Serialize(request);
        var datagram = Encoding.ASCII.GetBytes(requestJson);
        var udpHandler = new UdpHandler(_udpClientWrapper);
        var udpResponse = (await udpHandler.SendReceiveRequest(datagram, ipAddress, 5000))
            .FirstOrDefault();
        var key = GetKeyFromUdpResponse(udpResponse);
        _logger.LogDebug($"Success. Key: {key}");
        return key;
    }
}

