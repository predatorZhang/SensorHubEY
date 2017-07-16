using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.NKCommands
{
    [GXCommandFilter]
    public class VibratingCurve : CommandBase<GXSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "VibratingCurve";
            }
        }
        public override void ExecuteCommand(GXSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //VibratingCurve：上传时间，设备ID，{间隔米数0 振动数据0}，{间隔米数1 振动数据1}/r/n
                session.Logger.Info("诺可振动曲线采集开始！");
                session.Logger.Info(requestInfo.Body);

                string[] body = requestInfo.Body.Split(',');

                Model.NKVibratingCurveInfo vibratingCurve = new Model.NKVibratingCurveInfo();

                vibratingCurve.DEVID = body[1];
                
                if(body.Length>2){
                    StringBuilder vsb = new StringBuilder();
                    StringBuilder dsb = new StringBuilder();
                    for (int i = 2; i < body.Length; i++)
                    {
                        string[] data = body[i].Split(' ');
                        dsb.Append(data[0].Remove(0, 1)).Append(",");
                        vsb.Append(data[1].Remove(data[1].Length - 1, 1)).Append(",");
                    }
                    vibratingCurve.DISTANCE = dsb.Remove(dsb.Length - 1, 1).ToString();
                    vibratingCurve.VIBRATING = vsb.Remove(vsb.Length - 1, 1).ToString();
                }

                string sdate = body[0].Substring(0, 4) + "-" + body[0].Substring(4, 2) + "-" + body[0].Substring(6, 2)
                    + " " + body[0].Substring(8, 2) + ":" + body[0].Substring(10, 2) + ":" + body[0].Substring(12, 2);
                vibratingCurve.UPTIME = Convert.ToDateTime(sdate);
                vibratingCurve.LOGTIME = DateTime.Now;

                new BLL.NKVibratingCurve().insert(vibratingCurve);
            }
            catch (Exception e)
            {
                session.Logger.Error("诺可振动曲线数据采集失败!");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
        }
    }
}
