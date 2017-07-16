using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
   public class AlarmConcentrator
    {
        public void setHubOffLine(string concenCode)
        {
            IDAL.IAlarmConcentrator dal = SensorHub.DALFactory.AlarmConcentrator.Create();
            dal.setHubOffLine(concenCode);
        }

        public void setHubOnLine(string concenCode)
        {
            IDAL.IAlarmConcentrator dal = SensorHub.DALFactory.AlarmConcentrator.Create();
            dal.setHubOnLine(concenCode);
        }
    }
}
