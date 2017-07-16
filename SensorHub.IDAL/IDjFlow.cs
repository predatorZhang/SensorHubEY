using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface IDjFlow
    {
        void insert(List<Model.DjFlowInfo> djs);
        int queryCountByDevAndUpTime(String devId, DateTime upTime);
        float getLastData(DjFlowInfo liquidInfo);
    }
}
