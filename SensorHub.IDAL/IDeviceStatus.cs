using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.IDAL
{
    public interface IDeviceStatus
    {
        //更新
        void update(DeviceStatus devStatus);

        //根据设备号，删除设备记录表
        bool removeByDevCode(String devCode);

        //追加设备记录信息
        bool add(Model.DeviceStatus devStatus);

        List<Model.DeviceStatus> getUnAlarmDeviceStatus();


        Model.DeviceStatus getByDevCodeAndType(String devCode, String typeCode);

        Model.DeviceStatus getByDevCode(String devCode);

        List<Model.DeviceStatus> getAllAlarmDeviceStatus();

        bool isNotExist(String devCode);

        bool isNotExist(String devCode,String sensorTypeCode);

    }
}
