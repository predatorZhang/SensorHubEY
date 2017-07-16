using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.NKCommands
{
    [GXCommandFilter]
    public class TemperatureCurve : CommandBase<GXSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "TemperatureCurve";
            }
        }
        public override void ExecuteCommand(GXSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //TemperatureCurve：上传时间，设备ID，{间隔米数0 温度数据0}，{间隔米数1 温度数据1}/r/n
                session.Logger.Info("诺可温度曲线采集开始！");
                session.Logger.Info(requestInfo.Body);

                string[] body = requestInfo.Body.Split(',');

                Model.NKTemperatureCurveInfo temperatureCurve = new Model.NKTemperatureCurveInfo();

                //设备ID
                temperatureCurve.DEVID = body[1];

                //距离、温度值
                if (body.Length > 2)
                {
                    StringBuilder dsb = new StringBuilder();
                    StringBuilder tsb = new StringBuilder();

                    for (int i = 2; i < body.Length; i++)
                    {
                        string[] data = body[i].Split(' ');
                        dsb.Append(data[0].Remove(0, 1)).Append(",");
                        tsb.Append(data[1].Remove(data[1].Length - 1, 1)).Append(",");
                    }

                    temperatureCurve.DISTANCE = dsb.Remove(dsb.Length - 1, 1).ToString();
                    temperatureCurve.TEMPERATURE = tsb.Remove(tsb.Length - 1, 1).ToString();
                }
                
                //采集时间
                string sdate = body[0].Substring(0, 4) + "-" + body[0].Substring(4, 2) + "-" + body[0].Substring(6, 2)
                    + " " + body[0].Substring(8, 2) + ":" + body[0].Substring(10, 2) + ":" + body[0].Substring(12, 2);
                temperatureCurve.UPTIME = Convert.ToDateTime(sdate);

                //上传时间
                temperatureCurve.LOGTIME = DateTime.Now;

                new BLL.NKTemperatureCurve().insert(temperatureCurve);

                //温度报警规则
                string devType = new BLL.Device().getDevTypeByCode(temperatureCurve.DEVID);
                new BLL.NKTemperatureCurve().saveAlarm(temperatureCurve.DISTANCE, temperatureCurve.TEMPERATURE, temperatureCurve.DEVID,
                     (int)session.ID, devType);

            }
            catch (Exception e)
            {
                session.Logger.Error("诺可温度曲线数据采集失败!");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
        }
    }
}
