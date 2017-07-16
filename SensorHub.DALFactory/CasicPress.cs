using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SensorHub.DALFactory
{
    public class CasicPress
    {
        public static SensorHub.IDAL.ICasicPress Create()
        {
            /// Look up the DAL implementation we should be using
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".CasicPress";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (SensorHub.IDAL.ICasicPress)Assembly.Load(path).CreateInstance(className);
        }
    }
}
