using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;

namespace SensorHub.Servers.Commands.RHRQCommands
{
    public class RHRQCmd : CommandBase<RHRQSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "RHRQ";
            }
        }

        public override void ExecuteCommand(RHRQSession session, StringRequestInfo requestInfo)
        {
            try
            {
                string[] bt = requestInfo.Body.Split(',');

                session.Logger.Info("RHRQ:" + requestInfo.Body);
                string stime = bt[0];
                string sdate = stime.Substring(0, 4) + "-" + stime.Substring(4, 2) + "-" + stime.Substring(6, 2)
                  + " " + stime.Substring(8, 2) + ":" + stime.Substring(10, 2) + ":" + stime.Substring(12, 2);
                DateTime upTime = Convert.ToDateTime(sdate);

                string devIds = bt[1];
                string[] devs = (devIds.Replace("{", "").Replace("}", "")).Split(',');
                    List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();
                foreach (string dev in devs)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.MESSAGE = "入户燃气泄漏";

                    //报警记录消息是否发送
                    alarm.MESSAGE_STATUS = 0;

                    //报警记录是否被处理
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = dev;
                    alarm.DEVICE_TYPE_NAME = "入户燃气报警器";

                    object obj = new BLL.Device().getDeviceIdByCode(dev);
                    //报警设备ID
                    alarm.DEVICE_ID = Convert.ToInt32(obj);
                    alarm.RECORDDATE = upTime;

                    list.Add(alarm);
              }
                 new BLL.AlarmRecord().insert(list);
            }
            catch (Exception e)
            {
                session.Logger.Error("入户燃气报警接收处理异常!");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
        }
    }
}
