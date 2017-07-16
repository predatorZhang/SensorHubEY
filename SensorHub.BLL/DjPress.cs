using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class DjPress
    {
        private const string PRESS_TYPE = "000033";
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.DjPressInfo> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }
            IDjPress dal = SensorHub.DALFactory.DjPress.Create();
            List<Model.DjPressInfo> list = new List<DjPressInfo>();
            foreach (Model.DjPressInfo dj in djs)
            {
                if (dal.queryCountByDevAndUpTime(dj.DEVID, dj.UPTIME) <= 0)
                {
                    list.Add(dj);
                }
            }
            if (list.Count > 0)
            {
                dal.insert(list);
            }

        }

        public void saveAlarmInfo(List<Model.DjPressInfo> djs)
        {
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
           // IAlarmRecord alarmRecordDal = SensorHub.DALFactory.AlarmRecord.Create();
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            foreach (Model.DjPressInfo dj in djs)
            {
                AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(dj.DEVID, PRESS_TYPE);
                AlarmRecordInfo alarmRecordInfo = getAlarmRecord(alarmRuleInfo, dj);
                if (null != alarmRecordInfo)
                {
                    list.Add(alarmRecordInfo);
                }
            }
            if (list.Count > 0)
            {
                alarmRecordDal.insert(list);
            }
        }

        //TODO LIST:需要修改
        public void updateDevStatus(String devCode)
        {
            Model.DeviceStatus deviceStatus = new BLL.DeviceStatus().getByDevCodeAndType(devCode, PRESS_TYPE);
            if (deviceStatus != null)
            {
                deviceStatus.LASTTIME = DateTime.Now;
                deviceStatus.STATUS = false;
                new BLL.DeviceStatus().update(deviceStatus);
            }
        }

        public float getLastData(Model.DjPressInfo pressInfo)
        {
            return SensorHub.DALFactory.DjPress.Create().getLastData(pressInfo);
        }

        private AlarmRecordInfo getAlarmRecord(AlarmRuleInfo alarmRuleInfo, Model.DjPressInfo pressInfo)
        {
            if (null != alarmRuleInfo)
            {
                float curValue = float.Parse(pressInfo.PRESSDATA);
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                IDjPress pressDal = SensorHub.DALFactory.DjPress.Create();

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = pressInfo.DEVID;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(pressInfo.DEVID);
                alarmRecordInfo.ITEMNAME = "压力值";
                alarmRecordInfo.ITEMVALUE = curValue.ToString();
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;

                if (alarmRuleInfo.HighValue != 0 && curValue > alarmRuleInfo.HighValue)
                {
                    alarmRecordInfo.MESSAGE = "压力超限";
                    return alarmRecordInfo;
                }

                if (alarmRuleInfo.Saltation != 0)
                {
                    float lastData = pressDal.getLastData(pressInfo);
                    if (-1 != lastData && Math.Abs(curValue - lastData) > alarmRuleInfo.Saltation)
                    {
                        alarmRecordInfo.MESSAGE = "压力突变";
                       // return alarmRecordInfo;
                        return null;
                    }
                }
            }
            return null;
        }
   
    }
}
