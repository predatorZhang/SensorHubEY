using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
   public class Device
    {
        public object getDeviceIdByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            return DALFactory.Device.Create().getDevIdByCode(code);
        }

        public String getDevTypeByCode(String devCode)
        {
            if (string.IsNullOrEmpty(devCode))
            {
                return null;
            }
            return DALFactory.Device.Create().getDevTypeByCode(devCode);
        }

        /// <summary>
        /// 根据设备ID查询设备类型
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        public string getDevTypeByDevId(int devId)
        {
            return DALFactory.Device.Create().getDevTypeByDevId(devId);
        }

        public List<String> getAllDevCode()
        {
            return DALFactory.Device.Create().getAllDevCode();
        }

        public List<Model.SensorType> getSensorTypeByID(int devId)
        {
            return DALFactory.Device.Create().getSensorTypeByDevId(devId);
        }

        public float getWellDepByDevcode(String devCode)
        {
            return DALFactory.Device.Create().getWellDepthByDevCode(devCode);
        }
    }
}
