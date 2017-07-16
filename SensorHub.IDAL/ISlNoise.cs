using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface ISlNoise
    {
        void insert (List<Model.SlNoiseInfo> djs);
        int queryCountByDevAndUpTime(String devId, DateTime upTime);
        float getLastData(SlNoiseInfo slNoiseInfo);
        int getAlarmNumsByArrange(float highValue, DateTime beginTime, DateTime endTime, string devCode);
    }
}
