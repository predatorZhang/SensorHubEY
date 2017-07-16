using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface ICasicWaterMeter
    {
        void insert(List<Model.CasicWaterMeter> djs);

        int queryCountByDevAndUpTime(String devId, DateTime upTime);

        float getLastData(Model.CasicWaterMeter tempInfo);
    }
}
