using System;
using System.Net;

namespace Devices.Onvif.EventReader
{
    public interface IConnectionParameters
    {
        Uri ConnectionUri { get; }

        NetworkCredential Credentials { get; }

        TimeSpan ConnectionTimeout { get; }
    }
}
