using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface IAKFSHJ
    {
        void insert(List<Model.AKFSHJInfo> fshj);
        int queryCountByDevAndUpTime(String devId, DateTime upTime);
        String getHeatPipeTypeByDevCode(String devCode);
    }
}
