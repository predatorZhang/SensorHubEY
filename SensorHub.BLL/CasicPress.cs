using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
 
    public class CasicPress
    {
        private const string PRESS_TYPE = "000033";
        private const string TYPENAME = "压力监测仪";
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.CasicPress> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }

            ICasicPress dal = SensorHub.DALFactory.CasicPress.Create();
            List<Model.CasicPress> list = new List<Model.CasicPress>();

            foreach (Model.CasicPress dj in djs)
            {
                if (dal.queryCountByDevAndUpTime(dj.DEVCODE, dj.UpTime) <= 0)
                {
                    list.Add(dj);
                }
            }
            if (list.Count > 0)
            {
                dal.insert(djs);
            }
        }

        public void saveAlarmInfo(List<Model.CasicPress> djs)
        {
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
            //IAlarmRecord alarmRecordDal = SensorHub.DALFactory.AlarmRecord.Create();
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            foreach (Model.CasicPress dj in djs)
            {
                AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(dj.DEVCODE, PRESS_TYPE);
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

        public void updateDevStatus(List<Model.CasicPress> djPressInfoList)
        {
            foreach (Model.CasicPress pressInfo in djPressInfoList)
            {
                Model.DeviceStatus deviceStatus = new Model.DeviceStatus();
                deviceStatus = new BLL.DeviceStatus().getByDevCode(pressInfo.DEVCODE);
                if (deviceStatus == null)
                {
                    continue;
                }
                deviceStatus.LASTTIME = DateTime.Now;
                deviceStatus.STATUS = false;
                new BLL.DeviceStatus().update(deviceStatus);
            }
        }

        public float getLastData(Model.CasicPress pressInfo)
        {
            return SensorHub.DALFactory.CasicPress.Create().getLastData(pressInfo);
        }

        private AlarmRecordInfo getAlarmRecord(AlarmRuleInfo alarmRuleInfo, Model.CasicPress pressInfo)
        {
            if (null != alarmRuleInfo)
            {
                float curValue = float.Parse(pressInfo.Data);
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                ICasicPress pressDal = SensorHub.DALFactory.CasicPress.Create();

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = pressInfo.DEVCODE;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(pressInfo.DEVCODE);
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
                         return alarmRecordInfo;
                       // return null;
                    }
                }
            }
            return null;
        }

        public void setDeviceStatus(String devCode,String state)
        {
            AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
            alarmRecordInfo.ACTIVE = true;
            alarmRecordInfo.DEVICE_CODE = devCode;
            alarmRecordInfo.DEVICE_ID = Convert.ToInt32(new BLL.Device().getDeviceIdByCode(devCode)); ;
            alarmRecordInfo.DEVICE_TYPE_NAME = TYPENAME;
            alarmRecordInfo.ITEMNAME = "设备状态上报";
            alarmRecordInfo.MESSAGE = state == "0" ? "-1" : state;
            alarmRecordInfo.MESSAGE_STATUS = 0;
            alarmRecordInfo.RECORDCODE = "";
            alarmRecordInfo.RECORDDATE = System.DateTime.Now;

            new BLL.AlarmRecord().save(alarmRecordInfo);
        }

        public enum PressAlarmEnum
        {
            NO_DATA = 0, //7天无数据
            COLLECT_FAIL = 1, //采集失败
            DATA_EXCEPTION = 2,//数据异常
            NORMAL=-1,
        }
    }
}
