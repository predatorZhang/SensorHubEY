using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Utility;

namespace SensorHub.Servers.Commands.AdlerAlarmCommands
{
    public class AlarmOverrunCmd : CommandBase<ALSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "OverrunAlarm";
            }
        }

        public override void ExecuteCommand(ALSession session, StringRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("数据超限报警：上传：" + requestInfo.Body);
                string[] body = requestInfo.Body.Split(',');
                List<Model.AlarmRecordInfo> alarms = new List<Model.AlarmRecordInfo>();
               
                for (int i = 1; i < body.Length; i++)
                {
                    string[] data = body[i].Split(' ');
                    string regId = data[0].Substring(1,data[0].Length-1);
                    string devId = data[1];
                    string type = data[2];
                    string val = data[3].Substring(0,data[3].Length-1);

                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.DEVICE_ID =  Convert.ToInt32(new BLL.Device().getDeviceIdByCode(devId));
                    alarm.DEVICE_CODE = devId;

                    switch (type)
                    {
                        case "pressure":
                            alarm.RECORDCODE = "AD_DJ_OVER_RUN_ALARM_PRESSURE" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                          //  alarm.MESSAGE = "给水管线压力数据超限:【分区ID:" + regId + "】,【设备ID:" + devId + "】,【压力值:" + val + "】";
                            alarm.MESSAGE = "压力超限";
                            alarm.ITEMNAME = "压力";
                            break;
                        case "flow":
                            alarm.RECORDCODE = "AD_DJ_OVER_RUN_ALARM_FLOW" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                          //  alarm.MESSAGE = "给水管线流量数据超限:【分区ID," + regId + "】,【设备ID," + devId + "】";
                            alarm.MESSAGE = "流量超限";  
                          alarm.ITEMNAME = "流量";
                            break;
                        case "noise":
                            alarm.RECORDCODE = "AD_DJ_OVER_RUN_ALARM_NOISE" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                           // alarm.MESSAGE = "给水管线噪声数据超限:【分区ID," + regId + "】,【设备ID," + devId + "】";
                            alarm.MESSAGE = "噪声超限";   
                           alarm.ITEMNAME = "噪声";
                            break;
                        case "liquid":
                            alarm.RECORDCODE = "AD_DJ_OVER_RUN_ALARM_LIQUID" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                         //   alarm.MESSAGE = "给水管线液位数据超限:【分区ID," + regId + "】,【设备ID," + devId + "】";
                            alarm.MESSAGE = "液位超限";   
                         alarm.ITEMNAME = "液位";
                            break;
                    }
                    alarm.ITEMVALUE = val;
                    alarm.RECORDDATE = StringUtil.toDateTime(body[0]);
                    alarm.MESSAGE_STATUS = 0;
                    alarm.ACTIVE = true;
                    alarms.Add(alarm);
                }
                new BLL.AlarmRecord().insert(alarms);
            }
            catch (Exception e)
            {
                session.Logger.Error("超限报警处理异常：" + requestInfo.Body + "异常消息:" + e.ToString());
            }
        }
    }
}
