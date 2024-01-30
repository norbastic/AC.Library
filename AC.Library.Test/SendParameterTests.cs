using System;
using System.Collections.Generic;
using AC.Library.Models;
using Xunit;

namespace AC.Library.Test;

public class SendParameterTests
{
    private const string sendCommandResponse =
        "eyJ0IjoicGFjayIsImkiOjAsInVpZCI6MCwiY2lkIjoiZjQ5MTFlZDM2Yzc1IiwidGNpZCI6IiIsInBhY2siOiJ6dFVIRkdmQ1BnclJCelVtK1BWSzdIa0ZrRW1GYVJsOFZJd3FPMVNjV3l1N05obnNVRFYzYWVyVE5ZbkRFbHlhbmN5YVVMNExqMHJGUkxuZ2JyanBXUWxacG1LcUNrT01PSHlvV3Y2eTAzYz0ifQ==";

    private const string toSend =
        "eyJpIjowLCJ0Y2lkIjoiZjQ5MTFlZDM2Yzc1IiwidWlkIjowLCJwYWNrIjoia0hUemp6RnNGVEpuRmc4amN2SE93MkJTM1R5VE9RRXhKLzZjdm1HUzJSbVd6NHI2dEYwa1BzZ1FIZnhDTGZvbXFzWVV1RUoxeGhPUS9PR1R3M254NVE9PSIsInQiOiJwYWNrIiwiY2lkIjoiYXBwIn0";
    
    [Fact]
    public async void SendTest()
    {
        var acDevice = new AirConditionerModel
        {
            Address = "192.168.1.5",
            Name = "shortmacaddress",
            Id = "fullmacaddress",
            PrivateKey = "PRIVATEKEY"
        };
        var udpClientWrapper =
            TestSetup.CreateSendParameterUdpWrapper(
                Convert.FromBase64String(toSend),
                Convert.FromBase64String(sendCommandResponse),
                acDevice.Address);
        var deviceController = new DeviceController(acDevice, udpClientWrapper);
        var parameter = new Dictionary<TemperatureParam, int>
        {
            { TemperatureParam.Temperature, 20 }
        };
        var result = await deviceController.SetDeviceParameter(parameter);
        Assert.True(result);
    }
}