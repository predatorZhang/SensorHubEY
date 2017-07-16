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
    [SLResetCmdFilter]
    public class SLResetCmd : CommandBase<SZSLNoiseSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "SLRESET";
            }
        }

        public override void ExecuteCommand(SZSLNoiseSession session, StringRequestInfo requestInfo)
        {
            try
            {
                string macID = requestInfo.Parameters[4];
                if (session.MacID == null)
                {
                    session.MacID = macID;
                } 
            }
            catch (Exception e)
            {
                session.Logger.Error("渗漏预警RESET异常："+e.Message);
            }
        }
    }
}
