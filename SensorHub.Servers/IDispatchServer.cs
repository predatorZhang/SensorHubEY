using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers
{
    public interface IDispatchServer
    {
        void DispatchMessage(string deviceCode, string message);
    }
}
