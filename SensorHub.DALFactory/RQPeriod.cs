using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SensorHub.DALFactory
{
    public class RQPeriod
    {
        public static SensorHub.IDAL.IRQPeriod Create()
        {
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".RQPeriod";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (SensorHub.IDAL.IRQPeriod)Assembly.Load(path).CreateInstance(className);
        }
    }
}
