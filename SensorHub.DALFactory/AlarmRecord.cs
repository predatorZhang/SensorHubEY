using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SensorHub.DALFactory
{
    public class AlarmRecord
    {
        public static SensorHub.IDAL.IAlarmRecord Create()
        {
            /// Look up the DAL implementation we should be using
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".AlarmRecord";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (SensorHub.IDAL.IAlarmRecord)Assembly.Load(path).CreateInstance(className);
        }
    }
}
