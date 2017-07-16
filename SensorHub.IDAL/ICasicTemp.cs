using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface ICasicTemp
    {
        void insert(List<Model.CasicTemp> djs);

        int queryCountByDevAndUpTime(String devId, DateTime upTime);

        float getLastData(Model.CasicTemp tempInfo);
    }
}
