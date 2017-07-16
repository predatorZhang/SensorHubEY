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
    [LSReciveFlowCmdFilter]
    public class LSReciveFlowCmd : CommandBase<DGNSZSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "LSFLOW";
            }
        }

        public override void ExecuteCommand(DGNSZSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //session.Logger.Info("===================================================================");
                session.Logger.Info("多功能漏损监测仪：流量数据已经上传！");
                session.Logger.Info(requestInfo.Body);

                string[] data = requestInfo.Body.Split(',');

                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = data[5];
                }

                string cfg = data[6];
                string flow = data[10];

                //获取采集时间
                string year = (Int32.Parse(data[9].Substring(8, 2), System.Globalization.NumberStyles.HexNumber) + 2000).ToString();
                string mon = Int32.Parse(data[9].Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string day = Int32.Parse(data[9].Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string hor = Int32.Parse(data[9].Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string min = Int32.Parse(data[9].Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                DateTime upTime = Convert.ToDateTime(year + "-" + mon + "-" + day + " " + hor + ":" + min + ":00");

                //采集间隔
                int interval = Int16.Parse(cfg.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);

                //获取一共有多少个瞬时流量数据
                int count = 24 * 60 / Int16.Parse(cfg.Substring(4, 4), System.Globalization.NumberStyles.HexNumber) / Int16.Parse(cfg.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);

                List<Model.DjFlowInfo> djs = new List<DjFlowInfo>();
                for (int i = 0; i < count; i++)
                {
                    Model.DjFlowInfo dj = new DjFlowInfo();

                    //设备ID
                    dj.DEVID = session.MacID;

                    //瞬时流量
                    string insStr = flow.Substring(i * 8, 8);
                    byte[] insBt ={
                                   
                                   byte.Parse(insStr.Substring(2,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(insStr.Substring(0,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(insStr.Substring(6,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(insStr.Substring(4,2),System.Globalization.NumberStyles.HexNumber)
                                   
                                   };
                    float finsData = BitConverter.ToSingle(insBt, 0);
                    if (finsData < -6000)
                    {
                        dj.INSDATA = "0";
                    }
                    else
                    {
                        dj.INSDATA = finsData.ToString();
                    }

                    //获取净累计流量
                    string netStr = flow.Substring(8 * count+8*i, 8);
                    byte[] ntBt = { 
                                    byte.Parse(netStr.Substring(2,2), System.Globalization.NumberStyles.HexNumber),
                                    byte.Parse(netStr.Substring(0,2), System.Globalization.NumberStyles.HexNumber),
                                    byte.Parse(netStr.Substring(6,2), System.Globalization.NumberStyles.HexNumber),
                                    byte.Parse(netStr.Substring(4,2), System.Globalization.NumberStyles.HexNumber)
                                   };
                    dj.NETDATA = BitConverter.ToSingle(ntBt, 0).ToString();




                    //获取正累计流量
                    string psStr = flow.Substring(8 * 2*count+8*i, 8);
                    byte[] psBt ={
                                  byte.Parse(psStr.Substring(2,2), System.Globalization.NumberStyles.HexNumber),
                                  byte.Parse(psStr.Substring(0,2), System.Globalization.NumberStyles.HexNumber),
                                  byte.Parse(psStr.Substring(6,2), System.Globalization.NumberStyles.HexNumber),
                                  byte.Parse(psStr.Substring(4,2), System.Globalization.NumberStyles.HexNumber)
                                 };
                    dj.POSDATA = BitConverter.ToSingle(psBt, 0).ToString();

                    //获取负累计流量
                    string ngStr = flow.Substring(8*3*count+8*i, 8);
                    byte[] ngBt ={
                                  byte.Parse(ngStr.Substring(2,2), System.Globalization.NumberStyles.HexNumber),
                                  byte.Parse(ngStr.Substring(0,2), System.Globalization.NumberStyles.HexNumber),
                                  byte.Parse(ngStr.Substring(6,2), System.Globalization.NumberStyles.HexNumber),
                                  byte.Parse(ngStr.Substring(4,2), System.Globalization.NumberStyles.HexNumber)
                                  };
                    float fnegData = BitConverter.ToSingle(ngBt, 0);
                    if (fnegData < -6000)
                    {
                        dj.NEGDATA = "0";
                    }
                    else
                    {
                        dj.NEGDATA = fnegData.ToString();
                    }



                    //电池电量
                    dj.CELL = Int16.Parse(data[11], System.Globalization.NumberStyles.HexNumber).ToString();

                    //采集时间
                    dj.UPTIME = upTime;

                    //记录时间
                    dj.LOGTIME = DateTime.Now;

                    djs.Add(dj);

                    upTime = upTime.AddMinutes(-interval * (count - i - 1));
                }

                new BLL.DjFlow().insert(djs);
                session.Logger.Info("多功能漏损监测仪器：流量数据已经保存！");                
            }
            catch (Exception e)
            {
                session.Logger.Error("===================================================================");
                session.Logger.Error("多功能漏损监测仪器：流量数据保存失败！");
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
