using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AC.Library.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AC.Library.Test;

public class BinderTests
{
    private const string ScanResultBase64 = "eyJ0IjoicGFjayIsImkiOjEsInVpZCI6MCwiY2lkIjoiZjQ5MTFlZDM2Yzc1IiwidGNpZCI6IiIsInBhY2siOiJMUDI0RWswT2FZb2d4czNpUUxqTDRKQmwvWlhoRjBQR2J6eFpTb1hpZExDTWE5bE0yUnFJL0t5dHZKMzJJc0dTWlhyT3IrTWFrVnp6WEhiZ2hQZXlpam5XTXphTFFhYXcxYUZYbEU5azcxTDBjTW04YnNyL3k0Rmt4dW1wUmcxdEtzLzM0eGhCdU1TeFhmTmZ2RWdTNTZnb3NlV2NVYWFTdWVCdFNPZDBjN0tEaDRNVEtZd1QxQndOak4yaXIrMGVuS1lidDBpSURzZHA4L2Z0WGxBOUh2eHd3aUNJelN5MWIzWi9QaVFrN0JlODBncTlIeEs4TG9hOFdYVmpnWmNQNFZmNU1qS3hhNjBYdDVKMW9JK2xzeFV1WFRIa2d1bkxnNzZXV0d5K2V1bz0ifQ==";

    private const string BindResultBase64 =
        "eyJ0IjoicGFjayIsImkiOjEsInVpZCI6MCwiY2lkIjoiZjQ5MTFlZDM2Yzc1IiwidGNpZCI6IiIsInBhY2siOiJUMnRHdTlKVHNaUExNaG9QTy9tQmNrcHVXbnV2ejhMUUJ5TzZ3SitBaUFyblVEbENDREhpbFNUSHFTbG5qRkljc3NQcGk4WXFteGFoVkZ3bjNzS2xxWXpEMWs2amp4aXdFMnJNclRnN1huaz0ifQ==";

    private const string BindCommand =
        "{\"i\":1,\"tcid\":\"f4911ed36c75\",\"uid\":0,\"pack\":\"KMAcSuiBACBDHRReu/TdXlOGAWG3fyMJNBdv22JNXkBpOma5GRS/34RKDj8oZv\\u002Bt\",\"t\":\"pack\",\"cid\":\"app\"}";
    private const string TargetIp = "192.168.1.148";
    
    private readonly ILogger<Scanner> _scannerLogger;
    private readonly ILogger<Binder> _binderLogger;

    public BinderTests()
    {
        var mockScannerLogger = new Mock<ILogger<Scanner>>();
        var mockLogger = new Mock<ILogger<Binder>>();
        _scannerLogger = mockScannerLogger.Object;
        _binderLogger = mockLogger.Object;
    }
    
    [Fact]
    public async void BindSuccessful()
    {
        var mockUdp = new Mock<IUdpClientWrapper>();
        mockUdp.Setup(x => x.EnableBroadcast)
            .Returns(true);
        mockUdp.Setup(x => x.ReceiveAsync())
            .ReturnsAsync(new UdpReceiveResult(
                Convert.FromBase64String(ScanResultBase64),
                IPEndPoint.Parse($"{TargetIp}:7000"))
            );
        mockUdp.SetupSequence(x => x.Available)
            .Returns(1)
            .Returns(0);
        
        var bytesToSend = Encoding.ASCII.GetBytes(BindCommand);
        var mockBindUdp = new Mock<IUdpClientWrapper>();
        mockBindUdp.Setup(x => x.EnableBroadcast)
            .Returns(false);
        mockBindUdp.Setup(x => x.ReceiveAsync())
            .ReturnsAsync(new UdpReceiveResult(
                Convert.FromBase64String(BindResultBase64),
                IPEndPoint.Parse($"{TargetIp}:7000"))
            );
        mockBindUdp.Setup(x => x.SendAsync(bytesToSend, bytesToSend.Length, TargetIp, 7000))
            .ReturnsAsync(bytesToSend.Length);
        mockBindUdp.SetupSequence(x => x.Available)
            .Returns(1)
            .Returns(0);
        
        var scanner = new Scanner(_scannerLogger, mockUdp.Object);
        var scannedDevices = await scanner.Scan("192.168.1.255");
        Assert.True(scannedDevices.Count > 0);
        
        var toBind = scannedDevices.FirstOrDefault();
        var binder = new Binder(_binderLogger, mockBindUdp.Object);
        var receivedKey = await binder.BindOne(toBind.Id, TargetIp);
        Assert.True(!string.IsNullOrEmpty(receivedKey));
    }
}