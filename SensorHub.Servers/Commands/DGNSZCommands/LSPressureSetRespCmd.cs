using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Command;
using SensorHub.BLL;
namespace SensorHub.Servers.Commands.DGNSZCommands
{
    [LSPressureSetRespCmdFilter]
    public class LSPressureSetRespCmd : CommandBase<DGNSZSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "LSPRESSURESETRESP";
            }
        }

        public override void ExecuteCommand(DGNSZSession session, StringRequestInfo requestInfo)
        {
            try
            {
                byte[] set = Utility.ApplicationContext.getInstance().getLSSectionCloseConfig();
                session.Send(set, 0, set.Length);
                session.Logger.Info("LSSECTION结束帧已经发送！" + BitConverter.ToString(set));
            }
            catch (Exception e)
            {
                session.Logger.Error("SECTION结束帧发送失败！");
                session.Logger.Error(e.ToString());
            }
        }
    }
}
