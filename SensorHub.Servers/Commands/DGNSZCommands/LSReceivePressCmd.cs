using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;

namespace SensorHub.Servers.Commands.DGNSZCommands
{
    [LSReceivePressCmdFilter]
    public class LSReceivePressCmd : CommandBase<DGNSZSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "LSPRESS";
            }
        }

        private string HexToASCII(string msg)
        {
            //modified by predator 2014-
            if (msg.Equals("000000000000000000"))
            {
                return "0.00";
            }
            byte[] buff = new byte[(msg.Length -2)/ 2];
            string message = "";
            for (int i = 0; i < buff.Length-1; i++)
            {
                buff[i] = byte.Parse(msg.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            message = Encoding.ASCII.GetString(buff);
            if (!string.IsNullOrEmpty(message))
            {
                double dmsg = (Convert.ToDouble(message)) * 100.13;
                string result = dmsg.ToString("0.00");
                return result;
            }
            return "0.00";
        }

        public override void ExecuteCommand(DGNSZSession session, StringRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("多功能漏损监测仪：压力数据已经上传！");
                session.Logger.Info(requestInfo.Body);

                //LSPRESS:40,002A,01,000033,51,000069130118,000105A0,039E,0D090E,1A0F0D090E,
                //000000000000000000,59,59,61B1,03
                string[] data = requestInfo.Body.Split(',');
                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = data[5];
                }

                string cfg = data[6];
                string press = data[10];

                //采集时间
                string year = (Int32.Parse(data[9].Substring(8, 2), System.Globalization.NumberStyles.HexNumber) + 2000).ToString();
                string mon = Int32.Parse(data[9].Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string day = Int32.Parse(data[9].Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string hor = Int32.Parse(data[9].Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string min = Int32.Parse(data[9].Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                DateTime upTime = Convert.ToDateTime(year + "-" + mon + "-" + day + " " + hor + ":" + min + ":00");


                //采集压力数据条数
                int count = 24 * 60 / Int16.Parse(cfg.Substring(4, 4), System.Globalization.NumberStyles.HexNumber) / Int16.Parse(cfg.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);

                //采集间隔
                int interval = Int16.Parse(cfg.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);

                List<Model.DjPressInfo> djs = new List<DjPressInfo>();
                for (int i = 0; i < count; i++)
                {
                    Model.DjPressInfo dj = new DjPressInfo();

                    //设备ID
                    dj.DEVID = session.MacID;

                    //压力数据
                    dj.PRESSDATA = HexToASCII(data[10].Substring(i * 18, 18));

                    //电池电量
                    dj.CELL = Int16.Parse(data[11], System.Globalization.NumberStyles.HexNumber).ToString();

                    //采集时间
                    dj.UPTIME = upTime;

                    //记录时间
                    dj.LOGTIME = DateTime.Now;

                    djs.Add(dj);

                    upTime = upTime.AddMinutes(-interval * (count - i - 1));
                }

                new BLL.DjPress().insert(djs);
                new BLL.DjPress().saveAlarmInfo(djs);
                new BLL.DjPress().updateDevStatus(session.MacID);
               
                session.Logger.Info("多功能漏损监测仪：压力数据已经保存！");
            }
            catch (Exception e)
            {
               
                session.Logger.Error("多功能漏损监测仪：压力数据保存失败！");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
                
            }
        }
    }
}
