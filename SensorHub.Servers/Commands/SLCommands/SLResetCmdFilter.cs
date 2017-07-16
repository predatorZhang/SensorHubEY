using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;
using SensorHub.Utility;
namespace SensorHub.Servers.Commands.SLCommands
{
    public class SLResetCmdFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
              StringRequestInfo request = (StringRequestInfo)commandContext.RequestInfo;
              commandContext.Session.Logger.Info("设备->服务器：渗漏预警重启请求：" + request.Body);
        }

        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {

            SZSLNoiseSession session = (SZSLNoiseSession)commandContext.Session;
            BLL.DeviceLog bllLog = new DeviceLog();
            BLL.Device bllDevice = new Device();
            try
            {
                Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                log.MESSAGE = "设备重启";
                log.OPERATETYPE = "重启";
                log.LOGTIME = DateTime.Now;
                bllLog.insert(log);
            }
            catch(Exception e)
            {
                session.Logger.Error("设备重启记录保存异常"+e.ToString());
            }
           
        }
    }
}
