using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Utility;

namespace SensorHub.Servers.Commands
{
    public class LSSectionCmd : CommandBase<SZLiquidSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "LSSETREQU";
            }
        }

        public override void ExecuteCommand(SZLiquidSession session, StringRequestInfo requestInfo)
        {
           
            try
            {
                session.Logger.Info("LSSETREQU：液位配置请求帧:" + requestInfo.Body);

                session.MacID = requestInfo.Parameters[5];
                string address = requestInfo.Parameters[3];
                byte add0 = byte.Parse(address.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte add1 = byte.Parse(address.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte add2 = byte.Parse(address.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                byte ss = byte.Parse(DateTime.Now.ToString("yy"));
                byte ss1 = byte.Parse(DateTime.Now.ToString("MM"));
                byte ss2 = byte.Parse(DateTime.Now.ToString("dd"));
                byte ss3 = byte.Parse(DateTime.Now.ToString("HH"));
                byte ss4 = byte.Parse(DateTime.Now.ToString("mm"));
                byte ss5 = byte.Parse(DateTime.Now.ToString("ss"));
                byte ss7 = byte.Parse("0" + ((int)DateTime.Now.DayOfWeek).ToString());

                BLL.DeviceLog bllLog = new DeviceLog();
                BLL.Device bllDevice = new Device();

                DeviceConfigInfo conf;
               
                conf = (new DeviceConfig()).GetDeviceConfByDeviceCodeAndSensorCode(session.MacID, "000034");

                if (conf != null)
                {
                    //状态传感配置信息

                    //50# 00#14 #01 #00#00#34# C1 #00#05 #00#00#00#00 #1.23 #2D#17#0F#04#0F#0A#0F#03#

                    string content = conf.FrameContent;
                    string[] items = content.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                    //byte[] sendData = new byte[items.Length];
                    byte[] sendData = new byte[26];//固定26字节
                    //for (int i = 0; i < items.Length; i++)
                    for (int i = 0; i < 10; i++)
                    {
                        sendData[i] = byte.Parse(items[i], System.Globalization.NumberStyles.HexNumber);
                    }
                    //TODO LIST:修正液位警界高度、探头安装高度、当前时间
                    BLL.DjLiquid djBll = new BLL.DjLiquid();
                    AlarmRuleInfo rule = djBll.getAlarmRuleByDevcode(session.MacID);
                    float highValue = rule != null ? rule.HighValue : 1000;
                    byte[] btHighValue = BitConverter.GetBytes(highValue);
                    sendData[10 + 0] = btHighValue[0];
                    sendData[10 + 1] = btHighValue[1];
                    sendData[10 + 2] = btHighValue[2];
                    sendData[10 + 3] = btHighValue[3];

                    float height =float.Parse(items[14]);
                    byte[] btHeight = BitConverter.GetBytes(height);
                    sendData[14 + 0] = btHeight[0];
                    sendData[14 + 1] = btHeight[1];
                    sendData[14 + 2] = btHeight[2];
                    sendData[14 + 3] = btHeight[3];
                    
                    if (requestInfo.Parameters[0] == "50")
                    {
                        //液位时间修正
                        sendData[18] = ss5;
                        sendData[19] = ss4;
                        sendData[20] = ss3;
                        sendData[21] = ss7;
                        sendData[22] = ss2;
                        sendData[23] = ss1;
                        sendData[24] = ss;
                        sendData[25] = 0x03;
                    }

                    string head = "LSSETREQU:";
                    byte[] btHead = System.Text.Encoding.Default.GetBytes(head);
                    byte[] result = new byte[sendData.Length + btHead.Length];

                    Buffer.BlockCopy(btHead,0,result,0,btHead.Length);
                    Buffer.BlockCopy(sendData, 0, result, btHead.Length, sendData.Length);

                    session.Send(result,0,result.Length);
                    
                    //
                    /*
                    String crcIn = "";
                    for (int i = 0; i < sendData.Length; i++)
                    {
                        crcIn += sendData[i].ToString("X2");
                    }
                    ushort crcOut = CodeUtils.QuickCRC16(crcIn, 0, crcIn.Length);
                    byte[] crcOutByte = BitConverter.GetBytes(crcOut);
                    sendData[7] = crcOutByte[1];
                    sendData[8] = crcOutByte[0];
                     * **/

                    session.Logger.Info("成功下发液位配置帧:" + BitConverter.ToString(result));
                    session.LiquidConf = conf;
                    session.LiquidConf.SendTime = DateTime.Now;

                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                    log.MESSAGE = "液位数据配置:采集间隔：" + sendData[9] + "分钟";
                    log.OPERATETYPE = "下发";
                    log.LOGTIME = DateTime.Now;
                    bllLog.insert(log);

                    return;
                }

                //无配置信息处理发送校时帧
                
                byte[] setTime = {  0x50, 
                                    0x00, 0x14,
                                    0x01,
                                    0x00, 0x00,0x34,
                                    0xC2,
                                    0x00, 0x05,
                                    0x00,0x00,0x00,0x00,
                                    0x00,0x00,0x00,0x00,
                                    ss5,  ss4,  ss3,
                                    ss7,  ss2,  ss1,  ss,
                                    0x03};

                string head0 = "LSSETREQU:";
                byte[] btHead0 = System.Text.Encoding.Default.GetBytes(head0);
                byte[] result0 = new byte[setTime.Length + btHead0.Length];

                Buffer.BlockCopy(btHead0, 0, result0, 0, btHead0.Length);
                Buffer.BlockCopy(setTime,0,result0,btHead0.Length,setTime.Length);

                session.Send(result0, 0, result0.Length);
                session.Logger.Info("成功下发液位计校时帧:" + BitConverter.ToString(result0, 0));

            }
            catch (Exception e)
            {
                session.Logger.Error(e.ToString());
            }
        }
    }
}
