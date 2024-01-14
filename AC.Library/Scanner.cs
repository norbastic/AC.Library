using AC.Library.Interfaces;

namespace AC.Library;

using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Models;
using Utils;

public class Scanner
{
    private readonly ILogger _logger;
    private readonly IUdpClientWrapper _udpClientWrapper;

    public Scanner(ILogger<Scanner> logger, IUdpClientWrapper udpClientWrapper)
    {
        _logger = logger;
        _udpClientWrapper = udpClientWrapper;
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
           
            if (responsePackInfo?.Type != "pack")
            {
                continue;
            }
            
            if (responsePackInfo.Pack == null)
            {
                continue;
            }

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

    private async Task<List<DeviceDiscoveryResponse>> DiscoverLocalDevices(string broadcastAddress)
    {
        var responses = new List<DeviceDiscoveryResponse>();

        using var udp = _udpClientWrapper;
        udp.EnableBroadcast = true;
        
        _logger.LogDebug("Sending scan packet");

        var bytes = Encoding.ASCII.GetBytes("{ \"t\": \"scan\" }");
        var sent = await udp.SendAsync(bytes, bytes.Length, broadcastAddress, 7000);

        _logger.LogDebug($"Sent bytes: {sent}");

        for (int i = 0; i < 20; ++i)
        {
            if (udp.Available > 0)
            {
                var result = await udp.ReceiveAsync();
                responses.Add(new DeviceDiscoveryResponse()
                {
                    Json = Encoding.ASCII.GetString(result.Buffer),
                    Address = result.RemoteEndPoint.Address.ToString()
                });

                _logger.LogDebug($"Got response from {result.RemoteEndPoint.Address}");
            }
            await Task.Delay(100);
        }
        
        return responses;
    }
}