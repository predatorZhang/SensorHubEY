using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;
using System;

namespace SensorHub.BLL
{
    public class DeviceStatus
    {
        public void update(Model.DeviceStatus devStatus)
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            iDeviceStatus.update(devStatus);
        }

        public bool remove(String devCode)
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            return iDeviceStatus.removeByDevCode(devCode);
 
        }

        public bool save(Model.DeviceStatus devStatus)
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            return iDeviceStatus.add(devStatus);
        }

        public List<Model.DeviceStatus> getUnAlarmDeviceStatus()
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            return iDeviceStatus.getUnAlarmDeviceStatus();
        }

        public Model.DeviceStatus getByDevCode(String devCode)
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            return iDeviceStatus.getByDevCode(devCode);
        }

        public Model.DeviceStatus getByDevCodeAndType(String devCode, String sensorTypeCode)
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            return iDeviceStatus.getByDevCodeAndType(devCode, sensorTypeCode);
        }

        public List<Model.DeviceStatus> getAllAlarmDeviceStatus()
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            return iDeviceStatus.getAllAlarmDeviceStatus();
        }

        public bool isNotExist(String devCode)
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            return iDeviceStatus.isNotExist(devCode);
        }

        public bool isNotExist(String devCode,String typeCode)
        {
            IDeviceStatus iDeviceStatus = SensorHub.DALFactory.DeviceStatus.Create();
            return iDeviceStatus.isNotExist(devCode, typeCode);
        }
    }
}
