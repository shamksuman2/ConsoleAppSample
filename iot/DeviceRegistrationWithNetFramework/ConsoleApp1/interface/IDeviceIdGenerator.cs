namespace DeviceRegWithNetFramework
{
    using System;

    public interface IDeviceIdGenerator
    {
        Guid GenerateDeviceId(int uuIdNumber);
    }
}

