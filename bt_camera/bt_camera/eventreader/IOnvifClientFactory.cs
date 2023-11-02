using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Devices.Onvif.Security;

namespace Devices.Onvif.EventReader
{
    internal interface IOnvifClientFactory
    {
        TService CreateClient<TService>(Uri uri, IConnectionParameters connectionParameters, MessageVersion messageEncodingVersion);
        TService CreateClient<TService>(EndpointAddress address, IConnectionParameters connectionParameters, MessageVersion messageEncodingVersion);
        void SetSecurityToken(SecurityToken token);
    }
}