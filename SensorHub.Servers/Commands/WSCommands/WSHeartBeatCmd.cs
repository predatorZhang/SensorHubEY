using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.WSCommands
{
    public class WSHeartBeatCmd : CommandBase<WSSession, StringRequestInfo>
    {
        // Static constants
        private const string SENSOR_TYPE = "000035";

        public override string Name
        {
            get
            {
                return "HeartBeat";
            }
        }

        public override void ExecuteCommand(WSSession session, StringRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("HeartBeat:" + requestInfo.Body);

                string[] bt = requestInfo.Body.Split(',');
                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = bt[0];
                }
                string stime = bt[1];
                string sdate = stime.Substring(0, 4) + "-" + stime.Substring(4, 2) + "-" + stime.Substring(6, 2)
                  + " " + stime.Substring(8, 2) + ":" + stime.Substring(10, 2) + ":" + stime.Substring(12, 2);
                DateTime upTime = Convert.ToDateTime(sdate);


                TimeSpan ts = DateTime.Now.Subtract(upTime);
                double miniutes = ts.TotalMinutes;
                if (miniutes > 10.0)
                {
                    //下发校时
                    String preTimeCal = "SewTiming:" + session.MacID + ",";
                    String postTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    String timeCal = preTimeCal + postTime;

                    byte[] data1 = new byte[timeCal.Length + 2];
                    Encoding.ASCII.GetBytes(timeCal, 0, timeCal.Length, data1, 0);
                    data1[timeCal.Length] = 0x0D;
                    data1[timeCal.Length + 1] = 0x0A;
                   
                    session.Send(data1, 0, data1.Length);
                    session.Logger.Info("污水下发校时:" + BitConverter.ToString(data1)+
                        "上传时间:"+upTime+"系统时间:"+DateTime.Now);
                    
                }
                session.Send("HeartBeat:" + requestInfo.Body);

                //增加：进行对应的配置信息下发操作
                DeviceConfigInfo conf = (new DeviceConfig()).GetDeviceConfByDeviceCodeAndSensorCode(session.MacID, SENSOR_TYPE);
                if (conf == null) { return; };
                conf.SendTime = DateTime.Now;
                session.WsConf = conf;
                session.WsConf.Status = true;
                string content = conf.FrameContent;
                String sdata0 = "SewAcquireInterval:" + session.MacID + "," + content;
                byte[] data0 = new byte[sdata0.Length + 2];
                Encoding.ASCII.GetBytes(sdata0, 0, sdata0.Length, data0, 0);
                data0[sdata0.Length] = 0x0D;
                data0[sdata0.Length + 1] = 0x0A;
                session.Send(data0, 0, data0.Length);
                session.Logger.Info("有害气体配置信息:" + sdata0);

                session.WsConf.Status = true;
                session.WsConf.SendTime = DateTime.Now;
                new DeviceConfig().Update(session.WsConf);
            }
            catch (Exception e)
            {
                session.Logger.Error("污水心跳异常!");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
        }
    }
}
