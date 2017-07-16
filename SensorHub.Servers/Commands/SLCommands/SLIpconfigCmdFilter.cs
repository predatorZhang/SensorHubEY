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
    public class SLIpconfigCmdFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            commandContext.Session.Logger.Info("设备->服务器：上传渗漏预警IPCONFIG请求配置帧");
        }

        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {
            SZSLNoiseSession session = (SZSLNoiseSession)commandContext.Session;

                BLL.DeviceLog bllLog = new DeviceLog();
                BLL.Device bllDevice = new Device();

                Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                log.MESSAGE = "渗漏噪声服务器地址配置:" + ApplicationContext.getInstance().getSLIpConfig();
                log.OPERATETYPE = "下发";
                log.LOGTIME = DateTime.Now;

                bllLog.insert(log);
        }
    }
}
