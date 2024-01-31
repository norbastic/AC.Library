using System.Net.Sockets;
using AC.Library.Interfaces;
using AC.Library.Models;
using AC.Library.Template;

namespace AC.Library;

public class SetDeviceParameterOperation<TParam, TValue> : DeviceCommunicationTemplate
{
    public SetDeviceParameterOperation(AirConditionerModel airConditionerModel, IUdpClientWrapper udpClientWrapper, IParameter parameter) : base(airConditionerModel, udpClientWrapper)
    {
    }

    protected override object CreateRequest()
    {
        throw new NotImplementedException();
    }

    protected override object ProcessResponse(UdpReceiveResult udpResponse)
    {
        throw new NotImplementedException();
    }
}