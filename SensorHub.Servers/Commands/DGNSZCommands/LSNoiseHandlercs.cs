using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Utility;

namespace SensorHub.Servers.Commands.DGNSZCommands
{
    public class LSNoiseHandlercs : AbstractHander
    {
        public static readonly string flag = "FF";
        public override bool isThisLevel(StringRequestInfo requestInfo)
        {
            if (flag.Equals(requestInfo.Parameters[0]))
            {
                return true;
            }
            return false;
        }

        public override void handleSection(DGNSZSession session)
        {
            //如果传上来的LSSECTION帧中无法获取MacID,则对多功能监测仪下发噪声校时
            if (string.IsNullOrEmpty(session.MacID))
            {
                byte[] set = Utility.ApplicationContext.getInstance().getLSNoiseCALIConfig();
                session.Send(set, 0, set.Length);
                session.Logger.Info("多功能漏损监测仪下发噪声校时配置:" + BitConverter.ToString(set, 0));
                return;
            }

            //如果能能够从数据库中获取到噪声配置信息，则将噪声配置下发给多功能检测仪
            DeviceConfigInfo conf = (new DeviceConfig()).GetLatestConfigByDeviceCodeAndSensorCode(session.MacID, "000032");
            if (null != conf)
            {
                string content = conf.FrameContent;
                string[] items = content.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] sendData = new byte[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    sendData[i] = byte.Parse(items[i], System.Globalization.NumberStyles.HexNumber);
                }
                sendData[20] = StringUtil.SEC;
                sendData[21] = StringUtil.MIN;
                sendData[22] = StringUtil.HOR;
                sendData[23] = StringUtil.WEEK;
                sendData[24] = StringUtil.DAY;
                sendData[25] = StringUtil.MON;
                sendData[26] = StringUtil.YEAR;
                session.Send(sendData, 0, sendData.Length);
                session.Logger.Info("多功能漏损监测仪噪声（" + session.MacID + "）配置下发成功:" + BitConverter.ToString(sendData));
                session.NoiseConf = conf;
                session.NoiseConf.SendTime = DateTime.Now;
            }
            else
            //如果从数据库中获取不到噪声配置，在将噪声的默认配置下发给多功能监测仪。
            {
                byte[] set = Utility.ApplicationContext.getInstance().getLSNoiseDefaultConfig();
                session.Send(set, 0, set.Length);
                session.Logger.Info("多功能漏损监测仪噪声（" + session.MacID + "）默认配置下发成功:" + BitConverter.ToString(set, 0));
            }
        }
    }
}
