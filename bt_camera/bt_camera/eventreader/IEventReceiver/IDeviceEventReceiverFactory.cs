namespace Devices.Onvif.EventReader
{
    public interface IDeviceEventReceiverFactory
    {
        IDeviceEventReceiver Create(IConnectionParameters connectionParameters);
    }
}
