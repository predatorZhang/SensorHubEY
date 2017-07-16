using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface IConfig
    {
        Model.ConfigInfo getConfigByDevId(string devId);
    }
}
