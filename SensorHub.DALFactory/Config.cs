﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SensorHub.DALFactory
{
    public class Config
    {
        public static SensorHub.IDAL.IConfig Create()
        {
            /// Look up the DAL implementation we should be using
            string path = System.Configuration.ConfigurationSettings.AppSettings["orcDAL"];
            string className = path + ".Config";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (SensorHub.IDAL.IConfig)Assembly.Load(path).CreateInstance(className);
        }
    }
}
