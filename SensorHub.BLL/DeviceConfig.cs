using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;
namespace SensorHub.BLL
{
    public class DeviceConfig
    {
        public void Update(DeviceConfigInfo conf)
        {
            IDeviceConfig iDeviceConfig = SensorHub.DALFactory.DeviceConfig.Create();
            iDeviceConfig.Update(conf);
        }

        public DeviceConfigInfo GetDeviceConfByDeviceCodeAndSensorCode(string deviceCode, string sensorCode)
        {
            IDeviceConfig iDeviceConfig = SensorHub.DALFactory.DeviceConfig.Create();
            return  iDeviceConfig.getDeviceConfigByDeviceCodeAndSensorCode(deviceCode, sensorCode);
        }

        public DeviceConfigInfo GetLatestConfigByDeviceCodeAndSensorCode(string deviceCode, string sensorCode)
        {
            IDeviceConfig iDeviceConfig = SensorHub.DALFactory.DeviceConfig.Create();
            return iDeviceConfig.getLatestConfigByDeviceCodeAndSensorCode(deviceCode, sensorCode);
        }

        public List<DeviceConfigInfo> getDeviceConfigBySensorCode(string sensorCode)
        {
            IDeviceConfig iDeviceConfig = SensorHub.DALFactory.DeviceConfig.Create();
            return iDeviceConfig.getDeviceConfigBySensorCode(sensorCode);
        }

        public void incRetryCount(String devCode)
        {
            IDeviceConfig iDeviceConfig = SensorHub.DALFactory.DeviceConfig.Create();
            iDeviceConfig.incRetryCountByDevcode(devCode);
        }

        public List<DeviceConfigInfo> removeTimeoutConfig(List<DeviceConfigInfo> configs)
        {
            IDeviceConfig iDeviceConfig = SensorHub.DALFactory.DeviceConfig.Create();
            List<String> timeoutDevs = iDeviceConfig.queryTimeoutDevice(configs);

            List<DeviceConfigInfo> restTimeConfig = new List<DeviceConfigInfo>();
            foreach (DeviceConfigInfo config in configs)
            {
                if (!timeoutDevs.Contains(config.DeviceCode))
                {
                    restTimeConfig.Add(config);
                }
            }
            return restTimeConfig;
        }


        public void clearDeviceConfigByDevCode(String devCode)
        {
            IDeviceConfig iDeviceConfig = SensorHub.DALFactory.DeviceConfig.Create();
             iDeviceConfig.clearDeviceConfig(devCode);
        }
    }
}
