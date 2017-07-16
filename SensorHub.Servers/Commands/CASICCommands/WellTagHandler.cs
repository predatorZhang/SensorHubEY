using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.BLL;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public  class WellTagHandler:TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType == 7 ? true : false;
        }

        //0000 0071 前四个字节暂时不用
        public override void execute(Tag tag,String devCode,CellTag cellTag,
            SystemDateTag systemDateTag,CasicSession session)
        {
            //TODO LIST:解析井盖传感器数据并保存
            UploadTag wellTag = tag as UploadTag;
            int itv = wellTag.CollectInter;
            String collecTime = wellTag.CollectTime;
            int len = wellTag.Len;
            String dataValue = wellTag.DataValue;

            session.Logger.Info("井盖数据上传TAG：oid：" + wellTag.Oid + " 采集间隔: " +
  itv + "采集时间：" + collecTime + "上传数值：" + dataValue);

            int num = len / 1; //上传的井盖数据个数
            List<Model.WellSensorInfo> djs = new List<WellSensorInfo>();

            //井盖数据tag之前，无系统日期tag数据
           // DateTime baseTime = Convert.ToDateTime(CasicCmd.currentSystemDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                //上传的有状态数据、设备ID、可能有电池电量等数据
                WellSensorInfo wellInfo = new WellSensorInfo();
                wellInfo.DEVID = devCode;
                wellInfo.LOGTIME = DateTime.Now;
                byte btStatus = byte.Parse(dataValue, System.Globalization.NumberStyles.HexNumber);
                string descn = "";
                switch (btStatus)
                {
                    case 0:
                        descn = "状态正常";
                        new BLL.WellInfo().saveDeviceStatus(wellInfo.DEVID, WellAlarmEnum.NORMAL);
                        break;
                    case 1:
                        descn = "井盖开启";
                        saveWellAlarm(wellInfo.DEVID,session);
                        break;
                    case 2:
                        descn = "时钟故障";
                        new BLL.WellInfo().saveDeviceStatus(wellInfo.DEVID, WellAlarmEnum.SYS_ERROR);
                        break;
                    case 3:
                        descn = "低电压";
                        new BLL.WellInfo().saveDeviceStatus(wellInfo.DEVID, WellAlarmEnum.BATTERY_LOW);
                        break;
                    default:
                        descn = "未知状态";
                        break;
                }
                wellInfo.DESCN = descn;
                wellInfo.STATUS = btStatus + "";
                djs.Add(wellInfo);
            }
            new BLL.WellInfo().insert(djs);
            session.Logger.Info("井盖数据保存成功");
        }

        private void saveWellAlarm(String devCode,CasicSession session)
        {
            try
            {
                List<Model.AlarmRecordInfo> alarms = new List<Model.AlarmRecordInfo>();
                Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                alarm.DEVICE_ID = Convert.ToInt32(new BLL.Device().getDeviceIdByCode(devCode));
                alarm.DEVICE_CODE = devCode;
                alarm.MESSAGE = (int)WellAlarmEnum.WELL_OPEN+"";
                alarm.RECORDDATE = System.DateTime.Now;
                alarm.MESSAGE_STATUS = 0;
                alarm.ACTIVE = true;
                alarm.DEVICE_TYPE_NAME = "井盖状态监测仪";
                alarms.Add(alarm);
                new BLL.WellInfo().saveAlarm(alarm);
              
            }
            catch (Exception e)
            {
                session.Logger.Error("井盖传感器报警插入出错：请检查该设备是否已注册:" + devCode);
            }
        }
    }

    
}
