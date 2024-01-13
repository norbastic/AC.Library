namespace AC.Library;

using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Utils;
using Models;

public class Binder {
    private readonly ILogger<Binder> _logger;

    public Binder(ILogger<Binder> logger)
    {
        _logger = logger;
    }
    
    public async Task<string?> BindOne(string macAddress, string ipAddress)
    {
        var bindRequestPack = new BindRequestPack() { MAC = macAddress };
        var request = Request.Create(macAddress, Crypto.EncryptGenericData(JsonSerializer.Serialize(bindRequestPack)), 1);
        var requestJson = JsonSerializer.Serialize(request);

        var datagram = Encoding.ASCII.GetBytes(requestJson);

        using var udp = new UdpClient();
        var sent = await udp.SendAsync(datagram, datagram.Length, ipAddress, 7000);

        if (sent != datagram.Length)
        {
            _logger.LogWarning("Binding request cannot be sent");
            return null;
        }

        for (int i = 0; i < 50; ++i)
        {
            if (udp.Available > 0)
            {
                var result = await udp.ReceiveAsync();
                if (result.RemoteEndPoint.Address.ToString() != ipAddress)
                {
                    _logger.LogWarning($"  Got binding response from the wrong device: {result.RemoteEndPoint.Address}");
                    continue;
                }

                var responseJson = Encoding.ASCII.GetString(result.Buffer);

                var responsePackInfo = JsonSerializer.Deserialize<ResponsePackInfo>(responseJson);

                if (responsePackInfo == null)
                {
                    continue;
                }
                    
                if (responsePackInfo.Type != "pack")
                {
                    continue;
                }

                var bindResponse = JsonSerializer.Deserialize<BindResponsePack>(Crypto.DecryptGenericData(responsePackInfo.Pack));

                _logger.LogDebug($"  Success. Key: {bindResponse?.Key}");

                return bindResponse?.Key;
            }

            await Task.Delay(100);
        }
    }
    
}

