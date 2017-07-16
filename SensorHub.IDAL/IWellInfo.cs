using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface IWellInfo
    {
        void insert(List<Model.WellSensorInfo> djs);

        int queryCountByDevAndUpTime(String devId, DateTime upTime);
    }
}
