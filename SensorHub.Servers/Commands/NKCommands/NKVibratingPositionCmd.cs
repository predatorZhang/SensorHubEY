using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.NKCommands
{
    [GXCommandFilter]
    public class NKVibratingPositionCmd : CommandBase<GXSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "VibratingPosition";
            }
        }
        public override void ExecuteCommand(GXSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //VibratingPosition：上传时间，设备ID，{间隔米数 振动数据}/r/n
                session.Logger.Info("震动曲线开始:\n" + requestInfo.Body);                         
                string[] body = requestInfo.Body.Split(',');
                Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();

                //报警记录编号
                alarm.RECORDCODE = "NK_GX_VIB_POS_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");

                //报警信息
                //string msg = "管线开挖报警：@devId:@val:@time";      
               // msg = msg.Replace("@time", body[0]);
               // msg = msg.Replace("@devId", body[1]);
               // msg = msg.Replace("@val", body[2]);
                //alarm.MESSAGE = msg;
                alarm.MESSAGE = "管线开挖报警";

                //报警项目名称
                alarm.ITEMNAME = "光纤震动报警";

                //报警项目值
                string[] vals = body[2].Split(' ');
                alarm.ITEMVALUE = vals[0].Replace("{", "") + "," + vals[1].Replace("}", "");

                //报警设备ID
                alarm.DEVICE_ID = (int)session.ID;

                //报警记录日期
                //转换bt[1] 20131212101010 bug fix by predator
                string sdate = body[0].Substring(0, 4) + "-" + body[0].Substring(4, 2) + "-" + body[0].Substring(6, 2) 
                    + " " + body[0].Substring(8, 2) + ":" + body[0].Substring(10, 2) + ":" + body[0].Substring(12, 2);
                alarm.RECORDDATE = Convert.ToDateTime(sdate);

                //报警记录消息是否发送
                alarm.MESSAGE_STATUS = 0;

                //报警记录是否被处理
                alarm.ACTIVE = true;

                alarm.DEVICE_CODE = body[1];
                alarm.DEVICE_TYPE_NAME = "光纤";

                List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();
                list.Add(alarm);
                new BLL.AlarmRecord().insert(list);
            }
            catch (Exception e)
            {
                session.Logger.Error("振动定位数据采集失败！\n" + e.ToString());
            }
        }
    }
}
