using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface IYLiang
    {
        //void insert(List<Model.YLInfo> yliang);
        //int queryCountByDevAndUpTime(String devId, DateTime upTime);
        void insert(Model.YLInfo yliang);
    }
}
