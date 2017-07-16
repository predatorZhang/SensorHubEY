using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class DjLiquid
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.DjLiquidInfo> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }

            IDjLiquid dal = SensorHub.DALFactory.DjLiquid.Create();
            List<Model.DjLiquidInfo> list = new List<DjLiquidInfo>();

            foreach (Model.DjLiquidInfo dj in djs)
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

        public AlarmRuleInfo getAlarmRuleByDevcode(String devCode)
        {
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
            AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(devCode);
            return alarmRuleInfo;
        }

        public void saveSZAlarmInfo(List<Model.DjLiquidInfo> djs)
        {
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();
            BLL.Device deviceBLL = new SensorHub.BLL.Device();

            foreach (Model.DjLiquidInfo dj in djs)
            {
                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = dj.DEVID;
                alarmRecordInfo.DEVICE_ID = Convert.ToInt32(deviceBLL.getDeviceIdByCode(dj.DEVID));
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceBLL.getDevTypeByCode(dj.DEVID);
                alarmRecordInfo.ITEMNAME = "液位值";
                alarmRecordInfo.ITEMVALUE = dj.LIQUIDDATA;
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;
                alarmRecordInfo.MESSAGE = "液位超限";
        
                list.Add(alarmRecordInfo);
             
            }
            alarmRecordDal.insert(list);
        }
        public void saveAlarmInfo(List<Model.DjLiquidInfo> djs)
        {
            
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
            foreach (Model.DjLiquidInfo dj in djs)
            {
                AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(dj.DEVID);
                AlarmRecordInfo alarmRecordInfo = getAlarmRecord(alarmRuleInfo, dj);
                if (null != alarmRecordInfo)
                {
                    List<int> devStates = new List<int>();
                    devStates.Add((int)LiquidAlarmEnum.OVER_THRESH);
                    new BLL.AlarmRecord().deleteByMessage(alarmRecordInfo.DEVICE_CODE, devStates);
                    new BLL.AlarmRecord().save(alarmRecordInfo);
                }
            }
        }

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


        public void updateDevStatus(List<DjLiquidInfo> djLiquidInfoList)
        {
            foreach (DjLiquidInfo djLiquidInfo in djLiquidInfoList)
            {
                Model.DeviceStatus deviceStatus = new Model.DeviceStatus();
                deviceStatus = new BLL.DeviceStatus().getByDevCode(djLiquidInfo.DEVID);
                if (deviceStatus == null)
                {
                    continue;
                }
                deviceStatus.LASTTIME = DateTime.Now;
                deviceStatus.STATUS = false;
                new BLL.DeviceStatus().update(deviceStatus);
            }
        }

        public float getLastData(DjLiquidInfo liquidInfo)
        {
            return SensorHub.DALFactory.DjLiquid.Create().getLastData(liquidInfo);
        }

        private AlarmRecordInfo getAlarmRecord(AlarmRuleInfo alarmRuleInfo, DjLiquidInfo liquidInfo)
        {
            if (null != alarmRuleInfo)
            {
                float curValue = float.Parse(liquidInfo.LIQUIDDATA);
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                IDjLiquid liquidDal = SensorHub.DALFactory.DjLiquid.Create();

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = liquidInfo.DEVID;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(liquidInfo.DEVID);
                alarmRecordInfo.ITEMNAME = "液位值";
                alarmRecordInfo.ITEMVALUE = curValue.ToString();
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.MESSAGE = (int) LiquidAlarmEnum.OVER_THRESH + "";

                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;

                if (alarmRuleInfo.HighValue != null && curValue > alarmRuleInfo.HighValue)
                {
                    alarmRecordInfo.MESSAGE = (int)LiquidAlarmEnum.OVER_THRESH + "";
                    return alarmRecordInfo;
                }

                if (alarmRuleInfo.Saltation != null)
                {
                    float lastData = liquidDal.getLastData(liquidInfo);
                    if (-1 != lastData && Math.Abs(curValue - lastData) > alarmRuleInfo.Saltation)
                    {
                        alarmRecordInfo.MESSAGE = "液位突变";
                     //   return alarmRecordInfo;
                        return null;
                    }
                }
            }
            return null;
        }

        private const string TYPENAME = "全量程液位监测仪";
        public void setDeviceStatus(String devCode, String state)
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

        public enum LiquidAlarmEnum
        {
            NORMAL = -1, //设备正常
            NO_DATA = 0, //7天无数据
            COLLECT_FAIL = 1, //井盖开启
            DATA_EXCEPTION = 2, //时钟故障
            OVER_THRESH = 3 //低电压
        }
    }
}
