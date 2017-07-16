using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface IDjPress
    {
        void insert(List<Model.DjPressInfo> djs);
        int queryCountByDevAndUpTime(String devId, DateTime upTime);
        float getLastData(Model.DjPressInfo pressInfo);
    }
}
