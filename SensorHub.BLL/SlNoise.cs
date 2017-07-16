using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{

    public class SlNoise
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.SlNoiseInfo> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }

            ISlNoise dal = SensorHub.DALFactory.SlNoise.Create();
            List<Model.SlNoiseInfo> list = new List<SlNoiseInfo>();

            foreach (Model.SlNoiseInfo dj in djs)
            {
                if (dal.queryCountByDevAndUpTime(dj.SRCID, dj.UPTIME) <= 0)
                {
                    list.Add(dj);
                }
            }
            if (list.Count > 0)
            {
                dal.insert(djs);
            }
        }

        public void saveAlarmInfo(List<Model.SlNoiseInfo> djs)
        {
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
           // IAlarmRecord alarmRecordDal = SensorHub.DALFactory.AlarmRecord.Create();
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            foreach (Model.SlNoiseInfo dj in djs)
            {
                AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(dj.SRCID);
                AlarmRecordInfo alarmRecordInfo = getAlarmRecordByTime(alarmRuleInfo, dj);
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

        #region MyRegion 郑州热力需要连续两次上传间隔报警
        public void SaveZZAlarmInfo(List<Model.SlNoiseInfo> djsAll)
        {
            List<Model.SlNoiseInfo> djs = this.Filter(djsAll);
            if (djs.Count == 0)
            {
                return;
            }
            String devCode = djs[0].SRCID;
            IAlarmRule alarmRuleDal = SensorHub.DALFactory.AlarmRule.Create();
            AlarmRuleInfo alarmRuleInfo = alarmRuleDal.getAlarmRule(devCode);
            if (alarmRuleInfo == null || alarmRuleInfo.HighValue == 0)
            {
                return;
            }

            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            //如果最新一条消息没报警的话就清楚报警
            if(float.Parse(djs[djs.Count-1].DENSEDATA)<alarmRuleInfo.HighValue)
            {
                alarmRecordDal.removeByDevCode(devCode);
                return;
            }

            //判断是否都超过报警
            bool isAllOverThresh = true;
            foreach (SlNoiseInfo slNoiseInfo in djs)
            {
                if (float.Parse(slNoiseInfo.DENSEDATA) < alarmRuleInfo.HighValue) 
                {
                    isAllOverThresh = false;
                    break;
                }
            }

            //如果超过连续报警次数，将最新的数据插入到报警列表中
            if (isAllOverThresh && IsOverMaxSLAlarm(devCode, alarmRuleInfo))
            {
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
         
                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = devCode;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(devCode);
                alarmRecordInfo.ITEMNAME = "噪声值";
                alarmRecordInfo.ITEMVALUE = djs[djs.Count-1].DENSEDATA;
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;
                alarmRecordInfo.MESSAGE = (int)SlNoiseAlarmEnum.OVER_THRESH + "";

                List<int> devStates = new List<int>();
                devStates.Add((int)SlNoiseAlarmEnum.OVER_THRESH);
                new BLL.AlarmRecord().deleteByMessage(alarmRecordInfo.DEVICE_CODE, devStates);

                List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();
                list.Add(alarmRecordInfo);
                alarmRecordDal.insert(list);
            }
        }

        private bool IsOverMaxSLAlarm(String devCode, AlarmRuleInfo alarmRuleInfo)
        {
            SLNoiseCount slNoiseCount = new SLNoiseCount();
            return slNoiseCount.IncrementSLNoiseAlarmCount(devCode,alarmRuleInfo);
        }

        private List<Model.SlNoiseInfo> Filter(List<Model.SlNoiseInfo> djs)
        {
            List<Model.SlNoiseInfo> results = new List<SlNoiseInfo>();
            foreach (SlNoiseInfo slNoiseInfo in djs)
            {
                if (float.Parse(slNoiseInfo.DENSEDATA) < 40)//65535
                {
                    results.Add(slNoiseInfo);
                }
            }
            return results;
        }

        #endregion

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

        public void updateDevStatus(List<SlNoiseInfo> slNoiseInfoList)
        {
            foreach (SlNoiseInfo slNoiseInfo in slNoiseInfoList)
            {
                Model.DeviceStatus deviceStatus = new Model.DeviceStatus();
                deviceStatus = new BLL.DeviceStatus().getByDevCode(slNoiseInfo.SRCID);
                if (deviceStatus == null)
                {
                    continue;
                }
                deviceStatus.LASTTIME = DateTime.Now;
                deviceStatus.STATUS = false;
                new BLL.DeviceStatus().update(deviceStatus);
            }
        }

        public float getLastData(SlNoiseInfo slNoiseInfo)
        {
            return SensorHub.DALFactory.SlNoise.Create().getLastData(slNoiseInfo);
        }

        private AlarmRecordInfo getAlarmRecord(AlarmRuleInfo alarmRuleInfo, SlNoiseInfo slNoiseInfo)
        {
            if (null != alarmRuleInfo)
            {
                float curValue = float.Parse(slNoiseInfo.DENSEDATA);
                IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                ISlNoise slNoiseDal = SensorHub.DALFactory.SlNoise.Create();

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = slNoiseInfo.SRCID;
                alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(slNoiseInfo.SRCID);
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
                      //  return alarmRecordInfo;
                        return null;
                    }
                }
            }
            return null;
        }

        public AlarmRecordInfo getAlarmRecordByTime(AlarmRuleInfo alarmRuleInfo, SlNoiseInfo slNoiseInfo)
        {
            
            if (alarmRuleInfo != null)
            {
                string days = System.Configuration.ConfigurationSettings.AppSettings["SL_ALARM_DAYS"];
                int num_days = Convert.ToInt32(days);

                float curValue = float.Parse(slNoiseInfo.DENSEDATA);
                if (alarmRuleInfo.HighValue != 0 && curValue > alarmRuleInfo.HighValue)
                {
                    IDevice deviceDal = SensorHub.DALFactory.Device.Create();
                    AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                    alarmRecordInfo.ACTIVE = true;
                    alarmRecordInfo.DEVICE_CODE = slNoiseInfo.SRCID;
                    alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                    alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(slNoiseInfo.SRCID);
                    alarmRecordInfo.ITEMNAME = "噪声值";
                    alarmRecordInfo.ITEMVALUE = curValue.ToString();
                    alarmRecordInfo.MESSAGE_STATUS = 0;
                    alarmRecordInfo.RECORDCODE = "";
                    alarmRecordInfo.RECORDDATE = System.DateTime.Now;
                    alarmRecordInfo.MESSAGE = "管线泄漏";
                    return alarmRecordInfo;
                    //TODO LIST:查询过去7天的报警记录
                    /*
                    DateTime endTime = slNoiseInfo.UPTIME;
                    DateTime startTime = endTime.AddDays(-num_days);

                    ISlNoise slNoiseDal = SensorHub.DALFactory.SlNoise.Create();

                    int nums = slNoiseDal.getAlarmNumsByArrange(alarmRuleInfo.HighValue,
                        startTime, endTime, slNoiseInfo.SRCID);
                    if (nums == num_days)
                    {
                        IDevice deviceDal = SensorHub.DALFactory.Device.Create();

                        AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                        alarmRecordInfo.ACTIVE = true;
                        alarmRecordInfo.DEVICE_CODE = slNoiseInfo.SRCID;
                        alarmRecordInfo.DEVICE_ID = alarmRuleInfo.DeviceId;
                        alarmRecordInfo.DEVICE_TYPE_NAME = deviceDal.getDevTypeByCode(slNoiseInfo.SRCID);
                        alarmRecordInfo.ITEMNAME = "噪声值";
                        alarmRecordInfo.ITEMVALUE = curValue.ToString();
                        alarmRecordInfo.MESSAGE_STATUS = false;
                        alarmRecordInfo.RECORDCODE = "";
                        alarmRecordInfo.RECORDDATE = System.DateTime.Now;
                        alarmRecordInfo.MESSAGE = "管线泄漏";
                        return alarmRecordInfo;
                    }
                     * */
                }
            }
            return null;
        }

        
        public void testAlarm()
        {

           /*
            DateTime endTime = Convert.ToDateTime("2016-07-25 12:00:00");
            DateTime startTime = endTime.AddDays(-7);

            ISlNoise slNoiseDal = SensorHub.DALFactory.SlNoise.Create();
   
            int nums = slNoiseDal.getAlarmNumsByArrange((float)2.0,
                startTime, endTime, "212015090066");
            * */
        }


        private const string TYPENAME = "噪声监测仪";
        public void setDeviceStatus(String devCode, String state)
        {
            AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
            alarmRecordInfo.ACTIVE = true;
            alarmRecordInfo.DEVICE_CODE = devCode;
            alarmRecordInfo.DEVICE_ID = Convert.ToInt32(new BLL.Device().getDeviceIdByCode(devCode)); 
            alarmRecordInfo.DEVICE_TYPE_NAME = TYPENAME;
            alarmRecordInfo.ITEMNAME = "设备状态上报";
            alarmRecordInfo.MESSAGE = state == "0" ? "-1" : state;
            alarmRecordInfo.MESSAGE_STATUS = 0;
            alarmRecordInfo.RECORDCODE = "";
            alarmRecordInfo.RECORDDATE = System.DateTime.Now;

            new BLL.AlarmRecord().save(alarmRecordInfo);
        }

        public enum SlNoiseAlarmEnum
        {
            NO_DATA = 0, //7天无数据
            COLLECT_FAIL = 1, //采集失败
            DATA_EXCEPTION = 2, //数据异常
            OVER_THRESH = 3 //管线泄漏
        }

    }
}
