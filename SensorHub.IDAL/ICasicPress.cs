using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface ICasicPress
    {
        void insert(List<Model.CasicPress> djs);

        int queryCountByDevAndUpTime(String devId, DateTime upTime);

        float getLastData(Model.CasicPress pressInfo);
    }
}
