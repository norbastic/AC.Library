using System;
using System.Collections.Generic;
using AC.Library.Interfaces;
using AC.Library.Models;
using AC.Library.Models.Communication;
using AC.Library.Utils;
using Xunit;

namespace AC.Library.Test;

public class SendParameterTests
{
    private const string sendCommandResponse =
        "eyJ0IjoicGFjayIsImkiOjAsInVpZCI6MCwiY2lkIjoiZjQ5MTFlZDM2Yzc1IiwidGNpZCI6IiIsInBhY2siOiJ6dFVIRkdmQ1BnclJCelVtK1BWSzdIa0ZrRW1GYVJsOFZJd3FPMVNjV3l1N05obnNVRFYzYWVyVE5ZbkRFbHlhbmN5YVVMNExqMHJGUkxuZ2JyanBXUWxacG1LcUNrT01PSHlvV3Y2eTAzYz0ifQ==";

    private const string toSend =
        "eyJpIjowLCJ0Y2lkIjoiZjQ5MTFlZDM2Yzc1IiwidWlkIjowLCJwYWNrIjoia0hUemp6RnNGVEpuRmc4amN2SE93MkJTM1R5VE9RRXhKLzZjdm1HUzJSbVd6NHI2dEYwa1BzZ1FIZnhDTGZvbXFzWVV1RUoxeGhPUS9PR1R3M254NVE9PSIsInQiOiJwYWNrIiwiY2lkIjoiYXBwIn0=";
    
    [Fact]
    public async void SendTest()
    {
        var acDevice = new AirConditionerModel
        {
            Address = "192.168.1.148",
            Name = "1ed36c75",
            Id = "f4911ed36c75",
            PrivateKey = "4Fg7Ij0Lm3Op6Rs9"
        };
        var udpClientWrapper =
            TestSetup.CreateSendParameterUdpWrapper(
                Convert.FromBase64String(toSend),
                Convert.FromBase64String(sendCommandResponse),
                acDevice.Address);
        
        var deviceSetter = new SetDeviceParameterOperation<TemperatureParam, TempParameterValue>(
            acDevice,
            udpClientWrapper,
            TemperatureParam.Temperature,
            new TempParameterValue(TemperatureValues._20));

        var result = (string) await deviceSetter.ExecuteOperationAsync();
        Assert.Equal(TemperatureParam.Temperature.Value, result);
    }
    
    [Fact]
    public async void GetStatusTest()
    {
        var acDevice = new AirConditionerModel
        {
            Address = "192.168.1.148",
            Name = "1ed36c75",
            Id = "f4911ed36c75",
            PrivateKey = "4Fg7Ij0Lm3Op6Rs9"
        };
        var toQuery = new List<IParameter>()
        {
            PowerParam.Power,
            TemperatureParam.Temperature
        };
        var statusGetter = new GetDeviceStatusOperation<IParameter>(acDevice, new UdpClientWrapper(), toQuery);
        var result = (StatusResponsePack) await statusGetter.ExecuteOperationAsync();
        Assert.True(result.Columns.Length == 2);
    }
}