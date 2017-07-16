using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.DGNSZCommands
{
    //多功能漏损监测仪：旧版：苏州
    [LSFlowSetRespCmdFilter]
    public class LSFlowSetRespCmd : CommandBase<DGNSZSession, StringRequestInfo>
    {
        public  const string PRESS_SENSOR_CODE = "000033";
        public override string Name
        {
            get
            {
                return "LSFLOWSETRESP";
            }
        }

        public override void ExecuteCommand(DGNSZSession session, StringRequestInfo requestInfo)
        {
            try
            {
                DeviceConfigInfo conf = (new DeviceConfig()).GetLatestConfigByDeviceCodeAndSensorCode(session.MacID, PRESS_SENSOR_CODE);
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
                    session.PressConf = conf;
                    session.PressConf.SendTime = DateTime.Now;
                    session.Logger.Info("多功能漏损监测仪压力（" + session.MacID + "）配置下发成功:" + content);
                    return;
                }

                byte[] set = Utility.ApplicationContext.getInstance().getLSPressDefaultConfig();
                session.Send(set, 0, set.Length);
                session.Logger.Info("多功能漏损监测仪压力（" + session.MacID + "）默认配置下发成功:" + BitConverter.ToString(set));
            }
            catch (Exception e)
            {
                session.Logger.Error("===================================================================");
                session.Logger.Error("多功能漏损监测仪（" + session.MacID + "）配置失败！");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
                session.Logger.Error("===================================================================");
            }
        }
    }
}
