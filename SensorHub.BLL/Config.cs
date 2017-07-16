using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class Config
    {
        public Model.ConfigInfo getConfigByDevId(string devId)
        {
            if (null == devId)
            {
                return null;
            }
            return DALFactory.Config.Create().getConfigByDevId(devId);
        }
    }
}
