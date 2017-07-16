using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    //TODO LIST；定义井盖报警的枚举类
    public enum WellAlarmEnum
    {
        NORMAL = -1, //设备正常
        NO_DATA = 0, //7天无数据
        WELL_OPEN = 1, //井盖开启
        SYS_ERROR = 2, //时钟故障
        BATTERY_LOW = 3 //低电压
    }

    public class WellInfo 
    {
        private const string TypeName = "井盖状态监测仪";

        public void saveAlarm(Model.AlarmRecordInfo alarm)
        {
            //set messageStatus = -1 where messageStatus=0 and message = 1
            List<int> messageList = new List<int>();
            messageList.Add((int) WellAlarmEnum.WELL_OPEN);
            new BLL.AlarmRecord().deleteByMessage(alarm.DEVICE_CODE, messageList);

            //add alarmRecord which messageStatus = 0 and message=1
            new BLL.AlarmRecord().save(alarm);
        }

        public void saveDeviceStatus(String devCode, WellAlarmEnum wellAlarmEnum)
        {
            Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
            alarm.DEVICE_ID = Convert.ToInt32(new BLL.Device().getDeviceIdByCode(devCode));
            alarm.DEVICE_CODE = devCode;
            alarm.MESSAGE = (int)wellAlarmEnum + "";
            alarm.RECORDDATE = System.DateTime.Now;
            alarm.MESSAGE_STATUS = 0;
            alarm.ACTIVE = true;
            alarm.DEVICE_TYPE_NAME = TypeName;

            new BLL.AlarmRecord().save(alarm);
        }

        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="wellInfo">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.WellSensorInfo> wellInfo)
        {
            if (wellInfo.Count<=0)
            {
                return;
            }
            IWellInfo dal = SensorHub.DALFactory.WellInfo.Create();
            List<Model.WellSensorInfo> list = new List<WellSensorInfo>();
            foreach (Model.WellSensorInfo dj in wellInfo)
            {
                //井盖数据不上传采集时间，所以不用进行存储前的保存确认
              //  if (dal.queryCountByDevAndUpTime(dj.DEVID, dj.LOGTIME) <= 0)
              //  {
                    list.Add(dj);
              //  }
            }
            if (list.Count > 0)
            {
                dal.insert(wellInfo);
            }

        }


    }
}
