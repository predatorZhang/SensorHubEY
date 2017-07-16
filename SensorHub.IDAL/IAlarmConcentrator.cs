using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface IAlarmConcentrator
    {
        void setHubOffLine(string concenCode);
        void setHubOnLine(string concenCode);
    }
}
