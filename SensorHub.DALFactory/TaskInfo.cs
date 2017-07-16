using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SensorHub.DALFactory
{
    public class TaskInfo
    {
        public static SensorHub.IDAL.ITaskInfo Create()
        {
            /// Look up the DAL implementation we should be using
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".TaskInfo";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (SensorHub.IDAL.ITaskInfo)Assembly.Load(path).CreateInstance(className);
        }
    }
}
