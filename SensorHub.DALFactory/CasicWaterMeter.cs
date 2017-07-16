using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SensorHub.DALFactory
{
    public class CasicWaterMeter
    {
        public static SensorHub.IDAL.ICasicWaterMeter Create()
        {
            /// Look up the DAL implementation we should be using
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".CasicWaterMeter";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (SensorHub.IDAL.ICasicWaterMeter)Assembly.Load(path).CreateInstance(className);
        }
    }
}
