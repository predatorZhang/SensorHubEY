using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class DjNoise
    {
        private const string NOISE_TYPE = "000032";
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.DjNoiseInfo> djs)
        {
            if (djs.Count<=0)
            {
                return;
            }
            IDjNoise dal = SensorHub.DALFactory.DjNoise.Create();
            List<Model.DjNoiseInfo> list = new List<DjNoiseInfo>();
            foreach (Model.DjNoiseInfo dj in djs)
            {
                if (dal.queryCountByDevAndUpTime(dj.DEVID, dj.UPTIME) <= 0)
                {
                    list.Add(dj);
                }
            }
            if (list.Count > 0)
            {
                dal.insert(djs);
            }

        }

        public void saveAlarmInfo(List<Model.DjNoiseInfo> djs)
        {
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
           // IAlarmRecord alarmRecordDal = SensorHub.DALFactory.AlarmRecord.Create();
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            foreach (Model.DjNoiseInfo dj in djs)
            {
                AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(dj.DEVID, NOISE_TYPE);
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

        private AlarmRecordInfo getAlarmRecord(AlarmRuleInfo alarmRuleInfo, DjNoiseInfo slNoiseInfo)
        {
            if (null != alarmRuleInfo)
            {
                float curValue = float.Parse(slNoiseInfo.DDATA);
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                IDjNoise slNoiseDal = SensorHub.DALFactory.DjNoise.Create();

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = slNoiseInfo.DEVID;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(slNoiseInfo.DEVID);
                alarmRecordInfo.ITEMNAME = "噪声值";
                alarmRecordInfo.ITEMVALUE = curValue.ToString();
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;

                if (alarmRuleInfo.HighValue != 0 && curValue > alarmRuleInfo.HighValue)
                {
                    alarmRecordInfo.MESSAGE = "管线泄漏";
                    return alarmRecordInfo;
                }

                if (alarmRuleInfo.LowValue != 0 && curValue < alarmRuleInfo.LowValue)
                {
                    alarmRecordInfo.MESSAGE = "噪声低于下限";
                    //return alarmRecordInfo;
                    return null;
                }

                if (alarmRuleInfo.Saltation != 0)
                {
                    float lastData = slNoiseDal.getLastData(slNoiseInfo);
                    if (-100 != lastData && Math.Abs(curValue - lastData) > alarmRuleInfo.Saltation)
                    {
                        alarmRecordInfo.MESSAGE = "噪声异常";
                       // return alarmRecordInfo;
                        return null;
                    }
                }
            }
            return null;
        }

        public void updateDevStatus(String devCode)
        {
            Model.DeviceStatus deviceStatus = new BLL.DeviceStatus().getByDevCodeAndType(devCode,NOISE_TYPE);
            if (deviceStatus != null)
            {
                deviceStatus.LASTTIME = DateTime.Now;
                deviceStatus.STATUS = false;
                new BLL.DeviceStatus().update(deviceStatus);
            }
        }

    }
}
