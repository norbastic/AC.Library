using System.Text.Json;
using AC.Library.Interfaces;
using AC.Library.Models;
using AC.Library.Models.Communication;
using AC.Library.Template;

namespace AC.Library;

public class GetDeviceStatusOperation<T> : DeviceCommunicationTemplate where T : IParameter
{
    private readonly List<T> _columns;

    public GetDeviceStatusOperation(AirConditionerModel airConditionerModel, IUdpClientWrapper udpClientWrapper, List<T> columns)
        : base(airConditionerModel, udpClientWrapper)
    {
        _columns = columns;
    }

    protected override object CreateRequest()
    {
        return new StatusRequestPack
        {
            Type = "status",
            MAC = _airConditionerModel.Id,
            Columns = _columns.Select(x => x.Value).ToList()
        };
    }

    protected override StatusResponsePack ProcessResponseJson(string json)
    {
        return JsonSerializer.Deserialize<StatusResponsePack>(json);
    }
}