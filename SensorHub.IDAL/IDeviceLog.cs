using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface IDeviceLog
    {
        void add(Model.DeviceLogInfo log);
    }
}
