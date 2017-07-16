using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;
using SensorHub.Utility;
using SensorHub.Servers.Commands.SLCommands;
namespace SensorHub.Servers.Commands.SLCommands
{

    public class SLRepeatCmdFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            commandContext.Session.Logger.Info("设备->服务器：上传repeat帧");

        }

        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {
            SZSLNoiseSession session = (SZSLNoiseSession)commandContext.Session;

            BLL.DeviceLog bllLog = new DeviceLog();
            BLL.Device bllDevice = new Device();
            Model.DeviceLogInfo log = new Model.DeviceLogInfo();
            log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
            log.OPERATETYPE = "下发";
            log.LOGTIME = DateTime.Now;

            if (session.Conf != null)
            {
                //记录实际下发信息
                string content = session.Conf.FrameContent;
                string[] items = content.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] sendData = new byte[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    sendData[i] = byte.Parse(items[i], System.Globalization.NumberStyles.HexNumber);
                }
                log.MESSAGE="更新渗漏配置：密集开始时间："+sendData[4]+":"+sendData[5]+
                   "样本间隔:"+sendData[6]+"采集个数:"+sendData[7];
            }
            else
            {
                //记录默认配置信息
                byte[] set = ApplicationContext.getInstance().getDefaultSLConfig();
                log.MESSAGE = "更新渗漏配置：密集开始时间：" + set[4] + ":" + set[5] +
                      "样本间隔:" + set[6] + "采集个数:" + set[7];
            }
            bllLog.insert(log);
        }
    }
}
