using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceRegistration
{
    using System.Threading.Tasks;

    public interface IRegistrator
    {
        Task RegisterDevice();
    }
}
