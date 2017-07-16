using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Utility;
using SensorHub.Servers.Commands.SLCommands;
namespace SensorHub.Servers.Commands
{
    [SLRepeatCmdFilter]
    public class SLRepeatCmd : CommandBase<SZSLNoiseSession, StringRequestInfo>
    {
        private static string sensorTypeCode = "000032";
        public override string Name
        {
            get
            {
                return "REPEAT";
            }
        }

        public override void ExecuteCommand(SZSLNoiseSession session, StringRequestInfo requestInfo)
        {
            try
            {

                if (session.MacID == null)
                    return;

                DeviceConfigInfo conf = (new DeviceConfig()).GetDeviceConfByDeviceCodeAndSensorCode(session.MacID,
                                      sensorTypeCode);

                if (conf != null)
                {
                    string content = conf.FrameContent;
                    string[] items = content.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    byte[] sendData = new byte[items.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        sendData[i] = byte.Parse(items[i], System.Globalization.NumberStyles.HexNumber);
                    }
                    //修正时间
                    sendData[15] = byte.Parse(DateTime.Now.ToString("ss"));
                    sendData[16] = byte.Parse(DateTime.Now.ToString("mm"));
                    sendData[17] = byte.Parse(DateTime.Now.ToString("HH"));
                    sendData[18] = byte.Parse("0" + ((int)DateTime.Now.DayOfWeek).ToString());
                    sendData[19] = byte.Parse(DateTime.Now.ToString("dd"));
                    sendData[20] = byte.Parse(DateTime.Now.ToString("MM"));
                    sendData[21] = byte.Parse(DateTime.Now.ToString("yy"));

                    session.Send(sendData, 0, sendData.Length);

                    session.Conf = conf;
                    session.Conf.SendTime = DateTime.Now;
                    session.Conf.Status = true;//考虑到冯对每次下发的配置需求
                    new DeviceConfig().Update(session.Conf);

                    session.Logger.Info("服务器->设备：渗漏预警配置信息成功:" + BitConverter.ToString(sendData));
                    return;
                }
                byte[] set = ApplicationContext.getInstance().getDefaultSLConfig();
                session.Send(set, 0, set.Length);
                session.Logger.Info("服务器->设备：渗漏预警默认配置信息成功：" + BitConverter.ToString(set));
            }
            catch (Exception e)
            {
                session.Logger.Error("服务器->设备：渗漏噪声配置信息出错:" + e.Message);
            }
        }
    }
}
