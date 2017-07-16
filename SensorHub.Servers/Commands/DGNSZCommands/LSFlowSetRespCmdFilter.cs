using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SensorHub.BLL;
using SensorHub.Model;

namespace SensorHub.Servers.Commands.DGNSZCommands
{
    [LSPressureSetRespCmdFilter]
    public class LSFlowSetRespCmdFilter : CommandFilterAttribute
    {
        
        public override void OnCommandExecuted(SuperSocket.SocketBase.CommandExecutingContext commandContext)
        {
            return;
        }

        public override void OnCommandExecuting(SuperSocket.SocketBase.CommandExecutingContext commandContext)
        {
            DeviceLog bllLog = new DeviceLog();
            Device bllDevice = new Device();

            DGNSZSession session = (DGNSZSession)commandContext.Session;
            DeviceLogInfo log = null;

            if (session.FlowConf != null)
            {
                session.Logger.Info("多功能漏损监测仪流量（" + session.MacID + "）配置成功！");

                session.FlowConf.Status = true;
                new DeviceConfig().Update(session.FlowConf);

                string[] config = session.FlowConf.FrameContent.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                string interval1 = Int16.Parse(config[8] + config[9], System.Globalization.NumberStyles.HexNumber).ToString();
                string interval2 = Int16.Parse(config[10] + config[11], System.Globalization.NumberStyles.HexNumber).ToString();
                log = new DeviceLogInfo();
                log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                log.MESSAGE = "流量配置下发.采集间隔" + interval1 + "；发送间隔" + interval2;
                log.OPERATETYPE = "下发";
                log.LOGTIME = DateTime.Now;
                bllLog.insert(log);
            }
            else
            {
                session.Logger.Info("多功能漏损监测仪流量（" + session.MacID + "）默认配置成功！");

                log = new DeviceLogInfo();
                log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                log.MESSAGE = "流量初始化!";
                log.OPERATETYPE = "下发";
                log.LOGTIME = DateTime.Now;
                bllLog.insert(log);
            }
        }
    }
}
