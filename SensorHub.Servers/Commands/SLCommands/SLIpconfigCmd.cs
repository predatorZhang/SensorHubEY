using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Utility;
using SensorHub.Servers.Commands.SLCommands;
namespace SensorHub.Servers.Commands
{
    [SLIpconfigCmdFilter]
    public class SLIpconfigCmd : CommandBase<SZSLNoiseSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "IPCONFIG";
            }
        }

        public override void ExecuteCommand(SZSLNoiseSession session, StringRequestInfo requestInfo)
        {
            try
            {
                string dst = ApplicationContext.getInstance().getSLIpConfig();
                session.Send(dst);
                session.Logger.Info("服务器->设备：下发渗漏预警IPCONFIG配置帧:" + dst);
            }
            catch (Exception e)
            {
                session.Logger.Error("渗漏预警IP配置异常：" + e.Message);
              
            }
        }
   
    }
}
