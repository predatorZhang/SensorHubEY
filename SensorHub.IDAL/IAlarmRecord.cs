using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.IDAL
{
    public interface IAlarmRecord
    {
        void insert(List<Model.AlarmRecordInfo> alarms);
        void updateStatus(Model.AlarmRecordInfo alarm);
        void saveGXAlarm(Model.AlarmRecordInfo alarm);
        void remove(String devCode);
        void removeDeviceStatus(String devCode, List<int> devStatuslList);
        void add(Model.AlarmRecordInfo alarm);
    }
}
