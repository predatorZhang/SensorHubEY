using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.NKCommands
{
    public class GXCommandFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            StringRequestInfo request = (StringRequestInfo)commandContext.RequestInfo;
            string code = request.Body.Split(',')[1];
            object obj = new BLL.Device().getDeviceIdByCode(code);
            if (null == obj || string.IsNullOrEmpty(obj.ToString()))
            {
                commandContext.Cancel = true;
            }
            else
            {
                GXSession session = (GXSession)commandContext.Session;
                session.ID = Convert.ToInt32(obj);
            }
        }
        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {

        }
    }
}
