using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.BLL
{
    public class AlarmRecord
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="flow">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.AlarmRecordInfo> alarms)
        {
            removeOldRecord(alarms);
            IDAL.IAlarmRecord dal = SensorHub.DALFactory.AlarmRecord.Create();
            dal.insert(alarms);
        }

        public void removeOldRecord(List<Model.AlarmRecordInfo> alarms)
        {
             IDAL.IAlarmRecord dal = SensorHub.DALFactory.AlarmRecord.Create();
            foreach(Model.AlarmRecordInfo alarm in alarms)
            {
                dal.updateStatus(alarm);
            }
        }

        public void saveGXPressAlarm(Model.AlarmRecordInfo alarm)
        {
            IDAL.IAlarmRecord dal = SensorHub.DALFactory.AlarmRecord.Create();
            dal.saveGXAlarm(alarm);

        }

        public void removeByDevCode(String devCode)
        {
            IDAL.IAlarmRecord dal = SensorHub.DALFactory.AlarmRecord.Create();
            dal.remove(devCode);
            //dal.saveGXAlarm(alarm);
        }


        public void save(AlarmRecordInfo alarm)
        {
            IDAL.IAlarmRecord dal = SensorHub.DALFactory.AlarmRecord.Create();
            dal.add(alarm);//增加设备状态            
        }
        
        public void deleteByMessage(String devCode, List<int> devStatuslList)
        {
            IDAL.IAlarmRecord dal = SensorHub.DALFactory.AlarmRecord.Create();
            dal.removeDeviceStatus(devCode, devStatuslList);
        }
    }
}
