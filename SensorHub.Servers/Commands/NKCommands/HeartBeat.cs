using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.NKCommands
{
    public class HeartBeat : CommandBase<GXSession, StringRequestInfo>
    {
        public override void ExecuteCommand(GXSession session, StringRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("光纤心跳开始:\n" + requestInfo.Body);
                string[] body = requestInfo.Body.Split(',');
                if (body.Length == 2)
                {
                    session.MacID = body[0];
                }
                session.Logger.Info("光纤心跳结束:\n" + requestInfo.Body);
            }
            catch (Exception e)
            {
                session.Logger.Error("光纤接收失败！\n" + e.Message);
            }
        }
    }
}
