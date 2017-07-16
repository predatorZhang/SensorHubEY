using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SensorHub.BLL;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.DGNSZCommands
{
    public class LSReceivePressCmdFilter: CommandFilterAttribute
    {
        public override void OnCommandExecuted(SuperSocket.SocketBase.CommandExecutingContext commandContext)
        {
            //StringRequestInfo request = (StringRequestInfo)commandContext.RequestInfo;
            ////解析上传的时间是否合法:采集时间：不是2014年立即丢弃
            //String uptime = request.Parameters[9];
            //if (Int32.Parse(uptime.Substring(8, 2), System.Globalization.NumberStyles.HexNumber) < 14)
            //{
            //    commandContext.Cancel = true;
            //}
        }

        public override void OnCommandExecuting(SuperSocket.SocketBase.CommandExecutingContext commandContext)
        {
            BLL.DeviceLog bllLog = new DeviceLog();
            BLL.Device bllDevice = new Device();
            DGNSZSession session = (DGNSZSession)commandContext.Session;

            Model.DeviceLogInfo log = new Model.DeviceLogInfo();
            log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
            log.MESSAGE = "压力数据上报";
            log.OPERATETYPE = "上报";
            log.LOGTIME = DateTime.Now;
            bllLog.insert(log);
        }
    }
}
