using System.Text.Json;
using AC.Library.Interfaces;
using AC.Library.Models;
using AC.Library.Models.Communication;
using AC.Library.Template;

namespace AC.Library;

public class SetDeviceParameterOperation<TParam, TValue> : DeviceCommunicationTemplate
    where TParam : IParameter
    where TValue : IParameterValue
{
    private TParam _param;
    private TValue _value;

    public SetDeviceParameterOperation(
        AirConditionerModel airConditionerModel,
        IUdpClientWrapper udpClientWrapper,
        TParam param,
        TValue value)
        : base(airConditionerModel, udpClientWrapper)
    {
        _param = param;
        _value = value;
    }

    protected override object CreateRequest()
    {
        return CommandRequestPack.Create(_airConditionerModel.Id, _param.Value, _value.Value);
    }

    protected override object ProcessResponseJson(string json)
    {
        var setParameterResponse = JsonSerializer.Deserialize<CommandResponsePack>(json);
        if (setParameterResponse == null) {
            return false;
        }
        return _param.Value.Equals(setParameterResponse.Columns.First());
    }
}