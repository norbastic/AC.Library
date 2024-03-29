using System.Net.Sockets;
using AC.Library.Interfaces;

namespace AC.Library.Utils;

public class UdpClientWrapper: IUdpClientWrapper
{
    private readonly UdpClient _udpClient = new();

    public async Task<int> SendAsync(byte[] datagram, int bytes, string hostname, int port)
    {
        return await _udpClient.SendAsync(datagram, bytes, hostname, port);
    }

    public async Task<UdpReceiveResult> ReceiveAsync()
    {
        return await _udpClient.ReceiveAsync();
    }

    public bool EnableBroadcast
    {
        get => _udpClient.EnableBroadcast;
        set => _udpClient.EnableBroadcast = value;
    }

    public int Available => _udpClient.Available;

    public void Close()
    {
        _udpClient.Close();
    }

    public void Dispose()
    {
        _udpClient.Dispose();
    }
}