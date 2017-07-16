using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class DeviceLog1
    {
        public void insert(Model.DeviceLogInfo1 log)
        {
            try
            {
                IDAL.IDeviceLog1 dal = DALFactory.DeviceLog1.Create();
                dal.add(log);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
