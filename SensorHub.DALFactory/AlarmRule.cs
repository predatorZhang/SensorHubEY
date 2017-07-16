using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SensorHub.DALFactory
{
    public class AlarmRule
    {
        public static SensorHub.IDAL.IAlarmRule Create()
        {
            /// Look up the DAL implementation we should be using
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".AlarmRule";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (SensorHub.IDAL.IAlarmRule)Assembly.Load(path).CreateInstance(className);
        }
    }
}
