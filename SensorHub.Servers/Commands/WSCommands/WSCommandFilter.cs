using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;

namespace SensorHub.Servers.Commands.WSCommands
{
    public class WSCommandFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            StringRequestInfo request = (StringRequestInfo)commandContext.RequestInfo;
            string code = request.Parameters[0];
            object obj = new BLL.Device().getDeviceIdByCode(code);
            if (null == obj || string.IsNullOrEmpty(obj.ToString()))
            {
                commandContext.Cancel = true;
            }
            else
            {
                WSSession session = (WSSession)commandContext.Session;
                session.ID = Convert.ToInt32(obj);
            }
        }
        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {
        }
    }
}
