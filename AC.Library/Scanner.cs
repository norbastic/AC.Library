namespace AC.Library;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Models;
using Utils;
using Interfaces;

public class Scanner
{
    private readonly ILogger _logger;
    private readonly IUdpClientWrapper _udpClientWrapper;

    public Scanner(ILogger<Scanner> logger, IUdpClientWrapper udpClientWrapper)
    {
        _logger = logger;
        _udpClientWrapper = udpClientWrapper;
    }
    
    private async Task<List<DeviceDiscoveryResponse>> DiscoverLocalDevices(string broadcastAddress)
    { 
        var udpHandler = new UdpHandler(_udpClientWrapper);
        var bytes = Encoding.ASCII.GetBytes("{ \"t\": \"scan\" }");

        var result = await udpHandler.SendReceiveBroadcastRequest(bytes, broadcastAddress);
        var responses = result
            .Select(x => new DeviceDiscoveryResponse
            {
                Json = Encoding.ASCII.GetString(x.Buffer),
                Address = x.RemoteEndPoint.Address.ToString()
            })
            .ToList();
        
        return responses;
    }
    
    /// <summary>
    /// Scans the local network for available AC device(s)
    /// </summary>
    /// <param name="broadcastAddresses">Broadcast address of the network. E.g. 192.168.0.255</param>
    /// <returns>A list of ScannedDevice object</returns>
    public async Task<List<ScannedDevice>> Scan(string broadcastAddresses)
    {
        var foundUnits = new List<ScannedDevice>();
        var responses = await DiscoverLocalDevices(broadcastAddresses);

        foreach (var response in responses)
        {
            if (response.Json == null)
            {
                continue;
            }
            
            var responsePackInfo = JsonSerializer.Deserialize<ResponsePackInfo>(response.Json);
            if (!ResponseChecker.IsReponsePackInfoValid(responsePackInfo)) continue;

            var decryptedPack = Crypto.DecryptGenericData(responsePackInfo.Pack);
            if (decryptedPack == null)
            {
                continue;
            }
            var packInfo = JsonSerializer.Deserialize<PackInfo>(decryptedPack);
            
            if (packInfo?.Type != "dev")
            {
                continue;
            }

            var deviceInfo = JsonSerializer.Deserialize<DeviceInfoResponsePack>(decryptedPack);
            foundUnits.Add(new ScannedDevice
            {
                Id = deviceInfo?.ClientId,
                Name = deviceInfo?.FriendlyName,
                Address = response.Address,
                Type = deviceInfo?.Model
            });
        }
        
        return foundUnits;
    }
}