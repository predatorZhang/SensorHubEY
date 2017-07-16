using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface IAKFSSL
    {
        void insert(List<Model.AKFSSLInfo> fssl);
        int queryCountByDevAndUpTime(String devId, DateTime upTime);
   
    }
}
