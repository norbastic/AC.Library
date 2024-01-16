﻿using AC.Library.Interfaces;
using AC.Library.Models;
using AC.Library.Utils;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace AC.Library;

public class DeviceController
{
    private readonly AirConditionerModel _airConditionerModel;
    private readonly IUdpClientWrapper _udpClientWrapper;

    public DeviceController(AirConditionerModel airConditionerModel, IUdpClientWrapper udpClientWrapper)
    {
        _airConditionerModel = airConditionerModel;
        _udpClientWrapper = udpClientWrapper;
    }

    private CommandResponsePack? GetCommandResponsePackFromUdpResponse(UdpReceiveResult udpReceiveResult)
    {
        var responseJson = Encoding.ASCII.GetString(udpReceiveResult.Buffer);
        var response = JsonSerializer.Deserialize<ResponsePackInfo>(responseJson);
        if (response == null)
        {
            return null;
        }
        var decryptedJson = Crypto.DecryptData(response.Pack, _airConditionerModel!.PrivateKey!);
        if (decryptedJson == null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<CommandResponsePack>(decryptedJson);
    }

    public async Task<bool> SetDeviceParameter(Dictionary<string, int> parameters)
    {
        if (_airConditionerModel.PrivateKey == null)
        {
            throw new Exception("Device [PrivateKey] is required!");
        }
        var pack = CommandRequestPack.Create(_airConditionerModel.Id, parameters);
        var packJson = JsonSerializer.Serialize(pack);
        var encryptedData = Crypto.EncryptData(packJson, _airConditionerModel.PrivateKey) ?? throw new Exception("Could not encrypt pack json.");
        
        var request = Request.Create(_airConditionerModel.Id, encryptedData);
        var bytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(request));
        var udpHandler = new UdpHandler(_udpClientWrapper);
        var udpResponse = (await udpHandler.SendReceiveRequest(bytes, _airConditionerModel.Address)).FirstOrDefault();
        var commandResponse = GetCommandResponsePackFromUdpResponse(udpResponse);

        if (commandResponse == null) {
            return false;
        }

        if (parameters.ContainsKey(commandResponse.Columns.First()))
        {
            return true;
        }

        return false;
    }
}