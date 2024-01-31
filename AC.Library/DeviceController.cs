using AC.Library.Interfaces;
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
    
    private async Task<bool> SetDeviceParameter(string param, int value)
    {
        if (_airConditionerModel.PrivateKey == null)
        {
            throw new Exception("Device [PrivateKey] is required!");
        }
        var pack = CommandRequestPack.Create(_airConditionerModel.Id, param, value);
        var packJson = JsonSerializer.Serialize(pack);
        var encryptedData = Crypto.EncryptData(packJson, _airConditionerModel.PrivateKey) ?? throw new Exception("Could not encrypt pack json.");
        var request = Request.Create(_airConditionerModel.Id, encryptedData);
        var bytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(request));
        var udpHandler = new UdpHandler(_udpClientWrapper);
        var udpResponse = (await udpHandler.SendReceiveRequest(bytes, _airConditionerModel.Address)).FirstOrDefault();
        if (udpResponse.Buffer == null)
        {
            return false;
        }
        var commandResponse = GetCommandResponsePackFromUdpResponse(udpResponse);

        if (commandResponse == null) {
            return false;
        }

        if (param.Equals(commandResponse.Columns.First()))
        {
            return true;
        }

        return false;
    }

    public async Task<string?> GetDeviceStatus<T>(List<T> columns) where T : StringEnum
    {
        var statusRequest = new StatusReuest
        {
            Type = "status",
            MAC = _airConditionerModel.Id,
            Columns = columns.Select(x => x.Value).ToList()
        };

        var packJson = JsonSerializer.Serialize(statusRequest);
        var encryptedData = Crypto.EncryptData(packJson, _airConditionerModel.PrivateKey) ?? throw new Exception("Could not encrypt pack json.");
        var request = Request.Create(_airConditionerModel.Id, encryptedData);
        var bytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(request));
        var udpHandler = new UdpHandler(_udpClientWrapper);
        var udpResponse = (await udpHandler.SendReceiveRequest(bytes, _airConditionerModel.Address)).FirstOrDefault();
        if (udpResponse.Buffer == null)
        {
            return string.Empty;
        }
        var responseJson = Encoding.ASCII.GetString(udpResponse.Buffer);
        var response = JsonSerializer.Deserialize<ResponsePackInfo>(responseJson);
        if (response == null)
        {
            return null;
        }
        
        return Crypto.DecryptData(response.Pack, _airConditionerModel!.PrivateKey!);
    }
    
    public async Task<bool> SetDeviceParameter<TParam, TValue>(Dictionary<TParam, TValue> param) where TParam : StringEnum
    {
        var entry = param.FirstOrDefault();
        var key = entry.Key.Value;
        var value = Convert.ToInt32(entry.Value);

        return await SetDeviceParameter(key, value);
    }
}