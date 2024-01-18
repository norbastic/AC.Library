using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AC.Library.Interfaces;

namespace AC.Library.Test;

public class ScannerTests
{
    private const string UdpBase64Buffer =
        "eyJ0IjoicGFjayIsImkiOjEsInVpZCI6MCwiY2lkIjoiNTAyY2M2MDJkZmE4IiwidGNpZCI6IjUxODQ5ZDQ5OTAyZiIsInBhY2siOiJMUDI0RWswT2FZb2d4czNpUUxqTDREc2tJYkFlOU5xclVJL2ZLL3orSUwyTWE5bE0yUnFJL0t5dHZKMzJJc0dTWlhyT3IrTWFrVnp6WEhiZ2hQZXlpam5XTXphTFFhYXcxYUZYbEU5azcxTDBjTW04YnNyL3k0Rmt4dW1wUmcxdDB4VjgrL200N09UQk5hWC84YVVsMVNSaG0xN1JxK3FtSEpYR1NDenVoN3VEaDRNVEtZd1QxQndOak4yaXIrMGVuS1lidDBpSURzZHA4L2Z0WGxBOUhpMDJweVZHbFh1cEpIaFh0dFQzR2htODBncTlIeEs4TG9hOFdYVmpnWmNQNFZmNU1qS3hhNjBYdDVKMW9JK2xzNmZLNERzcXFlZ2MrR1I0NEdOeVVzd1lEcHdvd2Z4S2h4Sko0c2tUM2RZYXpUb3pRdjA5K0JVUzhkNGxmM0E3WHBKQ3RsL1hMSDAyL2JqS3NBcllzcDA9In0=";
    private const string MockIpAddress = "192.168.1.5";
    
    private readonly ILogger<Scanner> _scannerLogger;
    
    public ScannerTests()
    {
        var mock = new Mock<ILogger<Scanner>>();
        _scannerLogger = mock.Object;
    }
    
    [Fact]
    public async void ScanSuccessful()
    {
        var udpWrapper = TestSetup.CreateBroadcastMock(Convert.FromBase64String(UdpBase64Buffer), MockIpAddress);
        var scanner = new Scanner(_scannerLogger, udpWrapper);
        var result = await scanner.Scan("192.168.1.255");

        Assert.Contains(result, device => device.Address == MockIpAddress);
        Assert.Contains(result, device => device.Type.Equals("gree"));
    }
}