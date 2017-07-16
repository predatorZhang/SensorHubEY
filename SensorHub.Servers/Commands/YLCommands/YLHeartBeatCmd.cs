using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Utility;
using SensorHub.Servers.Commands.YLCommands;
namespace SensorHub.Servers.Commands
{
  
    public class YLHeartBeatCmd : CommandBase<YLSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "0x01";
            }
        }

        public override void ExecuteCommand(YLSession session, BinaryRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("集中器心跳：" + BitConverter.ToString(requestInfo.Body));
                byte[] BeginMark = new byte[] { 0x48, 0x43 };
                byte[] EndMark = new byte[] { 0xAF };
                byte[] heartBeat = new byte[BeginMark.Length + EndMark.Length + requestInfo.Body.Length];
                BeginMark.CopyTo(heartBeat, 0);
                requestInfo.Body.CopyTo(heartBeat, BeginMark.Length);
                EndMark.CopyTo(heartBeat, BeginMark.Length + requestInfo.Body.Length);

                //TODO LIST:更新集中器的ID信息
                if (session.HubID == null)
                {
                    String ss = BitConverter.ToString(requestInfo.Body).Replace("-","");
                    session.HubID = ss.Substring(10,12);
                }

                //TODO LIST:更新session中的设备信息
                if (session.DeviceID == null)
                {
                    session.DeviceID ="FFFF";
                }

                session.Send(heartBeat, 0, heartBeat.Length);

                session.Logger.Info("服务器->设备：心跳:" + BitConverter.ToString(requestInfo.Body));

            }
            catch (Exception e)
            {
                session.Logger.Error("渗漏心跳接收异常：" + e.Message);
            }
        }
   
    }
}
