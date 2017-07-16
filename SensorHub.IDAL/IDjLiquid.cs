using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface IDjLiquid
    {
        void insert(List<Model.DjLiquidInfo> liquid);
        int queryCountByDevAndUpTime(String devId, DateTime upTime);
        float getLastData(DjLiquidInfo liquidInfo);
    }
}
