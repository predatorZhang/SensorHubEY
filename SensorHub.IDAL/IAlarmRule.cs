using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface IAlarmRule
    {
        
        object getOverTimeByDevId(int devId);

        object getOverTimeByIdAndType(int devId, String typeCode);

        AlarmRuleInfo getAlarmRule(String devCode);

        AlarmRuleInfo getAlarmRule(String devCode, String sensorType);

    }
}
