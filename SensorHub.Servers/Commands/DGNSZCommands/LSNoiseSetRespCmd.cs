using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;
using SensorHub.Model;
using SensorHub.Servers.Commands.DGNSZCommands;
namespace SensorHub.Servers.Commands
{
    [LSNoiseSetRespCmdFilter]
    public class LSNoiseSetRespCmd : CommandBase<DGNSZSession, StringRequestInfo>
    {
        public  const String FLOW_SENSOR_CODE = "000031";
        public override string Name
        {
            get
            {
                return "LSNOISESETRESP";
            }
        }

        public override void ExecuteCommand(DGNSZSession session, StringRequestInfo requestInfo)
        {
            try
            {
                DeviceConfigInfo conf = (new DeviceConfig()).GetLatestConfigByDeviceCodeAndSensorCode(session.MacID, FLOW_SENSOR_CODE);
                if (conf != null)
                {
                    string content = conf.FrameContent;
                    string[] items = content.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    byte[] sendData = new byte[items.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        sendData[i] = byte.Parse(items[i], System.Globalization.NumberStyles.HexNumber);
                    }
                    session.Send(sendData, 0, sendData.Length);
                    session.FlowConf = conf;
                    session.FlowConf.SendTime = DateTime.Now;
                    session.Logger.Info("多功能漏损监测仪流量（" + session.MacID + "）配置下发成功:" + BitConverter.ToString(sendData));
                    return;
                }

                //流量默认配置帧
                byte[] set = Utility.ApplicationContext.getInstance().getLSFlowDefaultConfig();
                session.Send(set, 0, set.Length);
                session.Logger.Info("多功能漏损监测仪流量（" + session.MacID + "）默认配置下发成功:" + BitConverter.ToString(set));
            }
            catch (Exception e)
            {
                session.Logger.Error("===================================================================");
                session.Logger.Error("服务器下发流量设置失败！");
                session.Logger.Error(e.ToString());
                session.Logger.Error("===================================================================");
            }
        }
    }
}
