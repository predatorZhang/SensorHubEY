using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SensorHub.DALFactory
{
    public class DeviceLog1
    {
        public static SensorHub.IDAL.IDeviceLog1 Create()
        {
            /// Look up the DAL implementation we should be using
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".DeviceLog1";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (SensorHub.IDAL.IDeviceLog1)Assembly.Load(path).CreateInstance(className);
        }
    }
}
