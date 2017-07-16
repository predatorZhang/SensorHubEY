using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Utility;

namespace SensorHub.Servers.Commands.AdlerAlarmCommands
{
    public class AlarmSaltation : CommandBase<ALSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "SaltationAlarm";
            }
        }

        public override void ExecuteCommand(ALSession session, StringRequestInfo requestInfo)
        {
            try
            {
                
                session.Logger.Info("给水管线数据突变报警："+requestInfo.Body);
  
                string[] body = requestInfo.Body.Split(',');
                List<Model.AlarmRecordInfo> alarms = new List<Model.AlarmRecordInfo>();
                
                for (int i = 1; i < body.Length; i++)
                {
                    string[] data = body[i].Split(' ');
                    string regId = data[0].Substring(1,data[0].Length-1);
                    string devId = data[1];
                    string type = data[2];
                  //  string value = data[3].Substring(0, data[3].Length - 1);

                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.DEVICE_ID = Convert.ToInt32(new BLL.Device().getDeviceIdByCode(devId));
                    alarm.DEVICE_CODE = devId;
                  //  alarm.ITEMVALUE = value;

                    switch (type)
                    {
                        case "pressure":
                            alarm.RECORDCODE = "AD_DJ_SALTAION_ALARM_PRESSURE" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                            //alarm.MESSAGE = "給水管线压力数据突变:【分区ID," + regId + "】,【设备ID," + devId + "】";
                            alarm.MESSAGE = "压力突变";
                            break;
                        case "flow":
                            alarm.RECORDCODE = "AD_DJ_SALTAION_ALARM_FLOW" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                            alarm.MESSAGE = "流量突变";
                           // alarm.MESSAGE = "給水管线流量数据突变:【分区ID," + regId + "】,【设备ID," + devId + "】";
                            break;
                        case "noise":
                            alarm.RECORDCODE = "AD_DJ_SALTAION_ALARM_NOISE" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                            alarm.MESSAGE = "噪声突变";
                        //   alarm.MESSAGE = "給水管线噪声数据突变:【分区ID," + regId + "】,【设备ID," + devId + "】";
                            break;
                        case "liquid":
                            alarm.RECORDCODE = "AD_DJ_SALTAION_ALARM_LIQUID" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                          //  alarm.MESSAGE = "給水管线液位数据突变:【分区ID," + regId + "】,【设备ID," + devId + "】";
                            alarm.MESSAGE = "液位突变";
                          break;
                    }
                    alarm.RECORDDATE = StringUtil.toDateTime(body[0]);
                    alarm.MESSAGE_STATUS = 0;
                    alarm.ACTIVE = true;
                    alarms.Add(alarm);
                }
                new BLL.AlarmRecord().insert(alarms);
            }
            catch (Exception e)
            {
                session.Logger.Error("给水数据突变报警异常："+requestInfo.Body+" (异常信息："+e.ToString()+")");
            }
        }
    }
}
