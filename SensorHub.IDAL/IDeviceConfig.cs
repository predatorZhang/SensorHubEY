using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using System.Collections;
namespace SensorHub.IDAL
{
    public interface IDeviceConfig
    {
         void Update(DeviceConfigInfo deviceConfig);

         DeviceConfigInfo getDeviceConfigByDeviceCodeAndSensorCode(string deviceCode, string sensorCode);

         DeviceConfigInfo getLatestConfigByDeviceCodeAndSensorCode(string deviceCode, string sensorCode);

         List<DeviceConfigInfo> getDeviceConfigBySensorCode(string sensorCode);

         List<String> queryTimeoutDevice(List<DeviceConfigInfo> configs);

         void clearDeviceConfig(String devCode);

         void incRetryCountByDevcode(String devCode);

    }
}
