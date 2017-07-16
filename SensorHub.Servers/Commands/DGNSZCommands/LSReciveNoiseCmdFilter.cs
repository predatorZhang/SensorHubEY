using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SensorHub.BLL;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.DGNSZCommands
{
    public class LSReciveNoiseCmdFilter : CommandFilterAttribute
    {
        public override void OnCommandExecuted(SuperSocket.SocketBase.CommandExecutingContext commandContext)
        {
            StringRequestInfo request = (StringRequestInfo)commandContext.RequestInfo;

            commandContext.Session.Logger.Info("多功能漏损监测仪：漏损躁声开始上传！");
            commandContext.Session.Logger.Info(request.Body);
            
            //解析上传的时间是否合法:采集时间：不是2014年立即丢弃
            //String uptime = request.Parameters[8];
            //if (Int32.Parse(uptime.Substring(8, 2), System.Globalization.NumberStyles.HexNumber) < 14)
            //{
            //    commandContext.Cancel = true;
            //}
        }

        public override void OnCommandExecuting(SuperSocket.SocketBase.CommandExecutingContext commandContext)
        {
            DeviceLog bllLog = new DeviceLog();
            Device bllDevice = new Device();
            DGNSZSession session = (DGNSZSession)commandContext.Session;

            Model.DeviceLogInfo log = new Model.DeviceLogInfo();
            log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
            log.MESSAGE = "噪声数据上传";
            log.OPERATETYPE = "上报";
            log.LOGTIME = DateTime.Now;
            bllLog.insert(log);
        }
    }
}
