using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.NKCommands
{
    [GXCommandFilter]
    public class NKStressCurveCmd : CommandBase<GXSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "StressCurve";
            }
        }
        public override void ExecuteCommand(GXSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //StressCurve：上传时间，设备ID，{间隔米数0 应力数据0}，{间隔米数1 应力数据1}/r/n
                session.Logger.Info("诺可压力曲线采集开始！");
                session.Logger.Info(requestInfo.Body);

                string[] body = requestInfo.Body.Split(',');
                BLL.NKStressCurve bll = new BLL.NKStressCurve();

                Model.NKStressCurveInfo stressCurve = new Model.NKStressCurveInfo();
                string sdate = body[0].Substring(0, 4) + "-" + body[0].Substring(4, 2) + "-" + body[0].Substring(6, 2)
                    + " " + body[0].Substring(8, 2) + ":" + body[0].Substring(10, 2) + ":" + body[0].Substring(12, 2);
                stressCurve.UPTIME = Convert.ToDateTime(sdate);
                stressCurve.DEVID = body[1];                
                stressCurve.LOGTIME = DateTime.Now;
                StringBuilder dis = new StringBuilder();
                StringBuilder val = new StringBuilder();
                for (int i = 2; i < body.Length; i++)
                {
                    string[] data = body[i].Split(' ');
                    dis.Append(data[0].Remove(0, 1)).Append(",");
                    val.Append(data[1].Remove(data[1].Length - 1, 1)).Append(",");
                }
                stressCurve.DISTANCE = dis.Remove(dis.Length - 1, 1).ToString();
                stressCurve.STRESS = val.Remove(val.Length - 1, 1).ToString();
                bll.insert(stressCurve);

                //TODO LIST:判断是否应力有超限报警？有的话插入到报警记录中
                string devType = new BLL.Device().getDevTypeByCode(stressCurve.DEVID);
                bll.saveAlarm(stressCurve.DISTANCE, stressCurve.STRESS, stressCurve.DEVID,
                    (int)session.ID, devType);

            }
            catch (Exception e)
            {
                session.Logger.Error("诺可压力曲线数据采集失败!");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
        }
    }
}
