using System.Net.Sockets;

namespace AC.Library.Interfaces;

public interface IUdpClientWrapper : IDisposable
{
    Task<int> SendAsync(byte[] datagram, int bytes, string hostname, int port);
    Task<UdpReceiveResult> ReceiveAsync();
    bool EnableBroadcast { get; set; }
    int Available { get; }
    void Close();
}