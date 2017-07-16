using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SensorHub.BLL;
using SensorHub.Model;

namespace SensorHub.Servers.Commands.DGNSZCommands
{
    public class LSNoiseSetRespCmdFilter : CommandFilterAttribute
    {
        public override void OnCommandExecuted(SuperSocket.SocketBase.CommandExecutingContext commandContext)
        {
            return;
        }

        public override void OnCommandExecuting(SuperSocket.SocketBase.CommandExecutingContext commandContext)
        {
            BLL.DeviceLog bllLog = new DeviceLog();
            BLL.Device bllDevice = new Device();

            DGNSZSession session = (DGNSZSession)commandContext.Session;
            DeviceLogInfo log = null;

            //如果没有设备id，则进行了校时操作。
            if (string.IsNullOrEmpty(session.MacID))
            {
                session.Logger.Info("多功能漏损监测仪校时成功！");

                byte[] sectionClose = Utility.ApplicationContext.getInstance().getLSSectionCloseConfig();
                session.Send(sectionClose, 0, sectionClose.Length);
                session.Logger.Info("LSSECTION结束帧已经发送！" + BitConverter.ToString(sectionClose));
                commandContext.Cancel = true;
                return;
            }

            //设备下发了噪声的默认配置
            if (session.NoiseConf == null)
            {
                session.Logger.Info("多功能漏损监测仪噪声（" + session.MacID + "）默认配置成功！");

                log = new DeviceLogInfo();
                log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                log.MESSAGE = "噪声初始化！";
                log.OPERATETYPE = "下发";
                log.LOGTIME = DateTime.Now;
                bllLog.insert(log);
                return;
            }

            //设备配置下发
            session.Logger.Info("多功能漏损监测仪噪声（" + session.MacID + "）配置成功:" + session.NoiseConf.FrameContent);

            session.NoiseConf.Status = true;
            new DeviceConfig().Update(session.NoiseConf);

            string[] config = session.NoiseConf.FrameContent.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            string hour1 = byte.Parse(config[9], System.Globalization.NumberStyles.HexNumber).ToString();
            string second1 = byte.Parse(config[10], System.Globalization.NumberStyles.HexNumber).ToString();
            string interval1 = byte.Parse(config[11], System.Globalization.NumberStyles.HexNumber).ToString();
            string count1 = byte.Parse(config[12], System.Globalization.NumberStyles.HexNumber).ToString();

            string hour2 = byte.Parse(config[17], System.Globalization.NumberStyles.HexNumber).ToString();
            string second2 = byte.Parse(config[18], System.Globalization.NumberStyles.HexNumber).ToString();
            string interval2 = byte.Parse(config[19], System.Globalization.NumberStyles.HexNumber).ToString();
            log = new DeviceLogInfo();
            log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
            log.MESSAGE = "噪声配置下发。密集开始时间" + hour1 + ":" + second1 + "；密集间隔" + interval1 + "；密集样本数:"
                + count1 + "；松散开始时间" + hour2 + ":" + second2 + "；松散间隔:" + interval2;
            log.OPERATETYPE = "下发";
            log.LOGTIME = DateTime.Now;
            bllLog.insert(log);
        }
    }
}
