using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    //TODO LIST:注意报警消息的提交
    public class CasicTemp
    {
        private const string TEMP_TYPE = "000050";
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.CasicTemp> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }

            ICasicTemp dal = SensorHub.DALFactory.CasicTemp.Create();
            List<Model.CasicTemp> list = new List<Model.CasicTemp>();

            foreach (Model.CasicTemp dj in djs)
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

        public void saveAlarmInfo(List<Model.CasicTemp> djs)
        {
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
            //IAlarmRecord alarmRecordDal = SensorHub.DALFactory.AlarmRecord.Create();
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            foreach (Model.CasicTemp dj in djs)
            {
                AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(dj.DEVCODE, TEMP_TYPE);
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
            Model.DeviceStatus deviceStatus = new BLL.DeviceStatus().getByDevCodeAndType(devCode, TEMP_TYPE);
            if (deviceStatus != null)
            {
                deviceStatus.LASTTIME = DateTime.Now;
                deviceStatus.STATUS = false;
                new BLL.DeviceStatus().update(deviceStatus);
            }
        }

        public void updateDevStatus(List<Model.CasicTemp> djTempInfoList)
        {
            foreach (Model.CasicTemp djTempInfo in djTempInfoList)
            {
                Model.DeviceStatus deviceStatus = new Model.DeviceStatus();
                deviceStatus = new BLL.DeviceStatus().getByDevCode(djTempInfo.DEVCODE);
                if (deviceStatus == null)
                {
                    continue;
                }
                deviceStatus.LASTTIME = DateTime.Now;
                deviceStatus.STATUS = false;
                new BLL.DeviceStatus().update(deviceStatus);
            }
        }

        public float getLastData(Model.CasicTemp casicTemp)
        {
            return SensorHub.DALFactory.CasicTemp.Create().getLastData(casicTemp);
        }

        private AlarmRecordInfo getAlarmRecord(AlarmRuleInfo alarmRuleInfo, Model.CasicTemp tempInfo)
        {
            if (null != alarmRuleInfo)
            {
                float curValue = float.Parse(tempInfo.Data);
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                ICasicTemp tempDal = SensorHub.DALFactory.CasicTemp.Create();

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = tempInfo.DEVCODE;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(tempInfo.DEVCODE);
                alarmRecordInfo.ITEMNAME = "温度值";
                alarmRecordInfo.ITEMVALUE = curValue.ToString();
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;

                if (alarmRuleInfo.HighValue != 0 && curValue > alarmRuleInfo.HighValue)
                {
                    alarmRecordInfo.MESSAGE = "温度超限";
                    return alarmRecordInfo;
                }

                if (alarmRuleInfo.Saltation != 0)
                {
                    float lastData = tempDal.getLastData(tempInfo);
                    if (-1 != lastData && Math.Abs(curValue - lastData) > alarmRuleInfo.Saltation)
                    {
                        alarmRecordInfo.MESSAGE = "温度突变";
                        return alarmRecordInfo;
                      
                    }
                }
            }
            return null;
        }
    }
}
