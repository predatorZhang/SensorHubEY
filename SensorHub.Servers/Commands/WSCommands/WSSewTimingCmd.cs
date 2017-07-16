using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.WSCommands
{
    [WSCommandFilter]
    public class WSSewTimingCmd : CommandBase<WSSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "SewTiming";
            }
        }

        public override void ExecuteCommand(WSSession session, StringRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("污水校时!");
                session.Logger.Info(requestInfo.Body);

                string[] bt = requestInfo.Body.Split(',');
                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = bt[0];
                }
            }
            catch (Exception e)
            {
                session.Logger.Error("污水校时异常!");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
        }
    }
}
