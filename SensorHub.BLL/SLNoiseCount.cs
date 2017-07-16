using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.BLL
{
   public class SLNoiseCount
    {
       /*
       private static int SL_CONTINUE_ALARM_COUNT = int.Parse(System.Configuration.
           ConfigurationSettings.AppSettings["SL_CONTINUE_ALARM_COUNT"]);
     */
        public bool IncrementSLNoiseAlarmCount(String devCode,AlarmRuleInfo alarmRuleInfo)
        {
            return DALFactory.SLNoiseCount.Create().IncrementSLNoiseAlarmCount(devCode,
                (int)alarmRuleInfo.Overtime);
        }

    }
}
