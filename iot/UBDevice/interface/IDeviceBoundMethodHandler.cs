using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceRegisteration
{
    public interface IDeviceBoundMethodHandler
    {
        Task Register();
    }

}
