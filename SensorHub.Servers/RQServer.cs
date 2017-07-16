using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Servers.Commands;
using SensorHub.Utility;
namespace SensorHub.Servers
{
    //二院，燃气Server
    public class RQServer : AppServer<RQSession, BinaryRequestInfo>, IDispatchServer
    {
        public IDispatchServer predatorServer;

        public RQServer()
            : base(new DefaultReceiveFilterFactory<RQFilter, BinaryRequestInfo>())
        {
        }

        protected override void OnStarted()
        {
            predatorServer = this.Bootstrap.GetServerByName("PredatorServer") as IDispatchServer;
            base.OnStarted();

            ApplicationContext.getInstance().updateSequence("SEQ_XT_RQ_PERIOD", "DBID", "XT_RQ_PERIOD");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_DEVICE_LOG_ID", "DBID", "ALARM_DEVICE_LOG");
        }

        //send the config message to the device
        public void DispatchMessage(string deviceCode, string message)
        {
            //convert message type to byte[]
            try
            {
                string[] msgArray = message.Split(new char[] { '-' });
                byte[] msgByte = new byte[msgArray.Length];
                for (int i = 0; i < msgArray.Length; i++)
                {
                    msgByte[i] = byte.Parse(msgArray[i], System.Globalization.NumberStyles.HexNumber);
                }

                var sessions = this.GetSessions(s => s.MacID == deviceCode);
                foreach (var s in sessions)
                {
                    s.Send(msgByte, 0, msgByte.Length);
                }

            }
            catch (Exception e)
            {
                Logger.Error("燃气配置信息发送失败-设备ID:" + deviceCode + "命令内容:" + message);
            }

        }
    }
}
