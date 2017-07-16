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
    public class SLUploadNoiseCmdFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            StringRequestInfo request = (StringRequestInfo)commandContext.RequestInfo;
            string devCode = request.Parameters[4];
            commandContext.Session.Logger.Info("渗漏预警设备（" + devCode + ")" + "->服务器：噪声数据上传" + request.Body);

            //解析上传的时间是否合法:采集时间：不是2014年立即丢弃
            //String uptime = request.Parameters[7];
            //if (Int32.Parse(uptime.Substring(8, 2), System.Globalization.NumberStyles.HexNumber) < 14)
            // {
            //     commandContext.Cancel = true; 
            // }
        }

        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {

            BLL.DeviceLog bllLog = new DeviceLog();
            BLL.Device bllDevice = new Device();
            SZSLNoiseSession session = (SZSLNoiseSession)commandContext.Session;

            try
            {
                Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                log.MESSAGE = "渗漏噪声数据上传";
                log.OPERATETYPE = "上报";
                log.LOGTIME = DateTime.Now;
                bllLog.insert(log);
            }
            catch(Exception e)
            {
                commandContext.Session.Logger.Error("设备重启记录保存异常" + e.ToString());
            }
           
        }
    }
}
