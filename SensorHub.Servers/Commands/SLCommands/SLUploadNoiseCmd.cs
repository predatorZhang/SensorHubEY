using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Servers.Commands.SLCommands;
namespace SensorHub.Servers.Commands
{
    [SLUploadNoiseCmdFilter]
    public class SLUploadNoiseCmd : CommandBase<SZSLNoiseSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "SLNOISE";
            }
        }

        public override void ExecuteCommand(SZSLNoiseSession session, StringRequestInfo requestInfo)
        {
            try
            {
                string[] data = requestInfo.Body.Split(',');
        
                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = data[4];
                }

                string cfg = data[6];
                string ddata = data[10];               

                //密集样本数
                int count = Int32.Parse(cfg.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                //密集间隔
                int interval = Int16.Parse(cfg.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                string year = (Int32.Parse(data[7].Substring(8, 2), System.Globalization.NumberStyles.HexNumber) + 2000).ToString();
                string mon = Int32.Parse(data[7].Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string day = Int32.Parse(data[7].Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string time = Utility.CodeUtils.String2Byte(cfg.Substring(0, 2)) + ":" + Utility.CodeUtils.String2Byte(cfg.Substring(2, 2)) + ":00";
                DateTime upTime = Convert.ToDateTime(year + "-" + mon + "-" + day + " " + time);

                List<Model.SlNoiseInfo> djs = new List<SlNoiseInfo>();
                for (int i = 0; i < count; i++)
                {
                    Model.SlNoiseInfo dj = new SlNoiseInfo();

                    //源ID
                    dj.SRCID = data[4];

                    //目标ID
                    dj.DSTID = data[5];

                    //密集噪声
                    string dStr = ddata.Substring(i * 4, 4);
                    dj.DENSEDATA = int.Parse(dStr.Substring(2, 2) + dStr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString();

                    //电池电量
                    dj.CELL = Int16.Parse(data[9], System.Globalization.NumberStyles.HexNumber).ToString();

                    //采集时间
                    dj.UPTIME = upTime;

                    //记录时间
                    dj.LOGTIME = DateTime.Now;

                    djs.Add(dj);

                    upTime = upTime.AddMinutes(interval);
                }
                new BLL.SlNoise().insert(djs);
                new BLL.SlNoise().saveAlarmInfo(djs);
                new BLL.SlNoise().updateDevStatus(data[4]);
                session.Logger.Info("渗漏预警仪：渗漏噪声数据已经保存！");
            }
            catch (Exception e)
            {
                session.Logger.Error("渗漏预警仪：渗漏噪声数据保存失败："+e.ToString());
            }
        }
    }
}
