using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    //TODO LIST:注意报警消息的提交
    public class CasicWaterMeter
    {
        private const string PRESS_TYPE = "00033";
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.CasicWaterMeter> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }

            ICasicWaterMeter dal = SensorHub.DALFactory.CasicWaterMeter.Create();
            List<Model.CasicWaterMeter> list = new List<Model.CasicWaterMeter>();

            foreach (Model.CasicWaterMeter dj in djs)
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

        public void saveAlarmInfo(List<Model.CasicWaterMeter> djs)
        {
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
           // IAlarmRecord alarmRecordDal = SensorHub.DALFactory.AlarmRecord.Create();
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            foreach (Model.CasicWaterMeter dj in djs)
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
            Model.DeviceStatus deviceStatus = new BLL.DeviceStatus().getByDevCode(devCode);
            if (deviceStatus != null)
            {
                deviceStatus.LASTTIME = DateTime.Now;
                deviceStatus.STATUS = false;
                new BLL.DeviceStatus().update(deviceStatus);
            }
        }

        public void updateDevStatus(List<Model.CasicWaterMeter> djPressInfoList)
        {
            foreach (Model.CasicWaterMeter pressInfo in djPressInfoList)
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

        public float getLastData(Model.CasicWaterMeter pressInfo)
        {
            return SensorHub.DALFactory.CasicWaterMeter.Create().getLastData(pressInfo);
        }

        private AlarmRecordInfo getAlarmRecord(AlarmRuleInfo alarmRuleInfo, Model.CasicWaterMeter pressInfo)
        {
            if (null != alarmRuleInfo)
            {
                float curValue = float.Parse(pressInfo.Data);
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                ICasicWaterMeter pressDal = SensorHub.DALFactory.CasicWaterMeter.Create();

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = pressInfo.DEVCODE;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(pressInfo.DEVCODE);
                alarmRecordInfo.ITEMNAME = "水表值";
                alarmRecordInfo.ITEMVALUE = curValue.ToString();
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;

                if (alarmRuleInfo.HighValue != 0 && curValue > alarmRuleInfo.HighValue)
                {
                    alarmRecordInfo.MESSAGE = "超额用水";
                    return alarmRecordInfo;
                }

                if (alarmRuleInfo.Saltation != 0)
                {
                    float lastData = pressDal.getLastData(pressInfo);
                    if (-1 != lastData && Math.Abs(curValue - lastData) > alarmRuleInfo.Saltation)
                    {
                        alarmRecordInfo.MESSAGE = "用水量异常";
                        return alarmRecordInfo;
                    }
                }
            }
            return null;
        }
    }
}
