using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SensorHub.DALFactory
{
    public class AlarmConcentrator
    {
        public static SensorHub.IDAL.IAlarmConcentrator Create()
        {
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".AlarmConcentrator";

            return (SensorHub.IDAL.IAlarmConcentrator)Assembly.Load(path).CreateInstance(className);
        }
    }
}
