using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceRegWithNetFramework
{
    public interface IDeviceBoundMethodHandler
    {
        Task Register();
    }

}
