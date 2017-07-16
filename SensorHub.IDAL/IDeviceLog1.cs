using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface IDeviceLog1
    {
        void add(Model.DeviceLogInfo1 log);
    }
}
