using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class DjFlow
    {
        private const string FLOW_TYPE = "000031";
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.DjFlowInfo> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }
            IDjFlow dal = SensorHub.DALFactory.DjFlow.Create();
            List<Model.DjFlowInfo> list = new List<DjFlowInfo>();
            foreach (Model.DjFlowInfo dj in djs)
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

        public void saveAlarmInfo(List<Model.DjFlowInfo> djs)
        {
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
           // IAlarmRecord alarmRecordDal = SensorHub.DALFactory.AlarmRecord.Create();
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            foreach (Model.DjFlowInfo dj in djs)
            {
                AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(dj.DEVID, FLOW_TYPE);
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

        public void updateDevStatus(String devCode)
        {
            Model.DeviceStatus deviceStatus = new BLL.DeviceStatus().getByDevCodeAndType(devCode, FLOW_TYPE);
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

        private AlarmRecordInfo getAlarmRecord(AlarmRuleInfo alarmRuleInfo, Model.DjFlowInfo pressInfo)
        {
            if (null != alarmRuleInfo)
            {
                float curValue = float.Parse(pressInfo.INSDATA);
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                IDjFlow pressDal = SensorHub.DALFactory.DjFlow.Create();

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = pressInfo.DEVID;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(pressInfo.DEVID);
                alarmRecordInfo.ITEMNAME = "流量值";
                alarmRecordInfo.ITEMVALUE = curValue.ToString();
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;

                if (alarmRuleInfo.HighValue != 0 && curValue > alarmRuleInfo.HighValue)
                {
                    alarmRecordInfo.MESSAGE = "流量超限";
                    return alarmRecordInfo;
                }

                if (alarmRuleInfo.Saltation != 0)
                {
                    float lastData = pressDal.getLastData(pressInfo);
                    if (-1 != lastData && Math.Abs(curValue - lastData) > alarmRuleInfo.Saltation)
                    {
                        alarmRecordInfo.MESSAGE = "流量突变";
                       // return alarmRecordInfo;
                        return null;
                    }
                }
            }
            return null;
        }
   
    }
}
