using System;

namespace Devices.Onvif.Events
{
    public class DeviceEvent
    {
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public string Message { get; }

        public DeviceEvent(string message)
        {
            Message = message;
        }
    }
}
