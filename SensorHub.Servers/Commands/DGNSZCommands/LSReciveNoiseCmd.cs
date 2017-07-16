using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Command;
using SensorHub.Model;
using SensorHub.BLL;

namespace SensorHub.Servers.Commands.DGNSZCommands
{
    [LSReciveNoiseCmdFilter]
    public class LSReciveNoiseCmd : CommandBase<DGNSZSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "LSNOISE";
            }
        }

        public override void ExecuteCommand(DGNSZSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //LSNOISE:02,0075,01,FF,000032,51,000069130118,0200041E0500051E0500780A,1A0F0D090E,
                //FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF,FF,
                //FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
                //FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF,59,9F1E,03

                string[] data = requestInfo.Body.Split(',');
                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = data[6];
                }

                string cfg = data[7];//探头配置
                string lData = data[9];//松散噪声
                string dData = data[11];//密集噪声

                //密集噪声个数
                int count = Int32.Parse(cfg.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                //密集间隔
                int interval = Int16.Parse(cfg.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                //采集时间
                string year = (Int32.Parse(data[8].Substring(8, 2), System.Globalization.NumberStyles.HexNumber) + 2000).ToString();
                string mon = Int32.Parse(data[8].Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string day = Int32.Parse(data[8].Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString();
               // string time = Utility.CodeUtils.String2Byte(cfg.Substring(2, 2)) + ":" + Utility.CodeUtils.String2Byte(cfg.Substring(0, 2)) + ":00";
                string time = Utility.CodeUtils.String2Byte(cfg.Substring(0, 2)) + ":" + Utility.CodeUtils.String2Byte(cfg.Substring(2, 2)) + ":00";
                DateTime upTime = Convert.ToDateTime(year + "-" + mon + "-" + day + " " + time);

                List<Model.DjNoiseInfo> djs = new List<DjNoiseInfo>();
                for (int i = 0; i < count; i++)
                {
                    Model.DjNoiseInfo dj = new DjNoiseInfo();

                    //获取设备ID
                    dj.DEVID = session.MacID;

                    //密集开始时间
                    dj.DBEGIN = time;

                    //密集间隔
                    dj.DINTERVAL = interval.ToString();

                    //密集样本数
                    dj.DCOUNT = count.ToString();

                    //无线开始时间
                    dj.WARELESSOPEN = Utility.CodeUtils.String2Byte(cfg.Substring(8, 2)) + ":" + Utility.CodeUtils.String2Byte(cfg.Substring(10, 2)) + ":00";

                    //无线关闭时间
                    dj.WARELESSCLOSE = Utility.CodeUtils.String2Byte(cfg.Substring(12, 2)) + ":" + Utility.CodeUtils.String2Byte(cfg.Substring(14, 2)) + ":00";

                    //密集噪声
                    string dStr = dData.Substring(i * 4, 4);
                    dj.DDATA = int.Parse(dStr.Substring(2, 2) + dStr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString();

                    //电池电量
                    dj.CELL = Int16.Parse(data[10], System.Globalization.NumberStyles.HexNumber).ToString();

                    dj.UPTIME = upTime;

                    //录入时间
                    dj.LOGTIME = DateTime.Now;

                    djs.Add(dj);

                    upTime = upTime.AddMinutes(interval);                    
                }
                new BLL.DjNoise().insert(djs);
                new BLL.DjNoise().saveAlarmInfo(djs);
                new BLL.DjNoise().updateDevStatus(session.MacID);
                session.Logger.Info("多功能漏损监测仪：漏损躁声开始保存！");
            }
            catch (Exception e)
            {
                session.Logger.Error("===================================================================");
                session.Logger.Error("多功能漏损监测仪：漏损躁声保存异常");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
                session.Logger.Error("===================================================================");
            }
            finally
            {
                session.Logger.Info("===================================================================");
            }
        }
    }
}
