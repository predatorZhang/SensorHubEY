using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class DeviceLog
    {
        public void insert(Model.DeviceLogInfo log)
        {
            try
            {
                if (log.DEVICE_ID <= 0)
                {
                    return;
                }
                IDAL.IDeviceLog dal = DALFactory.DeviceLog.Create();
                dal.add(log);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
