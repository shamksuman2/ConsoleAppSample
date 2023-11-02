using System;
using System.Threading;
using System.Threading.Tasks;
using Devices.Onvif.Events;

namespace Devices.Onvif.EventReader
{
    public interface IDeviceEventReceiver
    {
        IConnectionParameters ConnectionParameters { get; }

        event EventHandler<DeviceEvent> EventReceived;

        Task ConnectAsync(CancellationToken cancellationToken);

        Task ReceiveAsync(CancellationToken cancellationToken);
    }
}
