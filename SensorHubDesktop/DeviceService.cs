using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketEngine;
using SensorHub.Servers;
using SensorHub.Utility;
using SensorHub.Servers.Commands.CASICCommands;

namespace SensorHubDesktop
{
    public class DeviceService
    {
        //在线设备列表
        private List<DeviceDTO> devices = new List<DeviceDTO>();
        private IBootstrap bootstrap;

        private const String CASIC_SERVER_NAME = "CasicServer";
        private const String YL_SERVER = "YLServer";
        private const String WS_SERVER = "WSServer";

        public DeviceService(IBootstrap bootstrap)
        {
            this.bootstrap = bootstrap;
        }

        public DeviceService()
        {
            
        }

        public void wakeUpCasicDev(DeviceDTO dto)
        {
            //判断当前是够已经唤醒，如果已经唤醒则不再下发配置信息
            //判断设备是否已经唤醒
            //TODO LIST:获取会话信息，发送数据
            var server0 = bootstrap.GetServerByName(CASIC_SERVER_NAME);
            CasicServer casicServer0 = server0 as CasicServer;
            CasicSession session0 = casicServer0.GetSessionByID(dto.SessionId) as CasicSession;
            CasicSession.DeviceDTO dev = session0.devMaps[dto.Name];
            if (dev.IsWakeUp || dev.DevType == "集中器")
            {
                return;
            }

            String[] parameters = dto.Tag.Split(',');

            //TODO: construct the receving casic data
            String preamble = parameters[0];
            byte btpreamble = byte.Parse(preamble, System.Globalization.NumberStyles.HexNumber);

            String version = parameters[1];
            byte btVersion = byte.Parse(version, System.Globalization.NumberStyles.HexNumber);

            String deviceId = parameters[3];//6个字节
            byte[] btDevId = new byte[6];
            btDevId[0] = byte.Parse(deviceId.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[1] = byte.Parse(deviceId.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[2] = byte.Parse(deviceId.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[3] = byte.Parse(deviceId.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[4] = byte.Parse(deviceId.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[5] = byte.Parse(deviceId.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);

            String routeFlag = parameters[4];
            byte btRouteFlag = byte.Parse(routeFlag, System.Globalization.NumberStyles.HexNumber);

            String dstNodeAddr = parameters[5];//2个字节
            byte[] btDstNode = new byte[2];
            btDstNode[0] = byte.Parse(dstNodeAddr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            btDstNode[1] = byte.Parse(dstNodeAddr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

            String sPtuType = parameters[6];//2个字节
            Int16 btpduType = Int16.Parse(sPtuType, System.Globalization.NumberStyles.HexNumber);
            Int16 opearType = (Int16)(btpduType & 0x7F);
            Int16 pdu = (Int16)(2688 + opearType); //0X0A80
            byte[] btPdu0 = BitConverter.GetBytes(pdu);
            byte[] btPdu = { btPdu0[1], btPdu0[0] };

          //  btDstNode[0] = byte.Parse(dstNodeAddr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
          //  btDstNode[1] = byte.Parse(dstNodeAddr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

            String seq = parameters[7];
            byte btSeq = byte.Parse(seq, System.Globalization.NumberStyles.HexNumber);

            byte[] tag = {0x60,0x00,0x02,0x00,0x00,0x01,0x00};

            byte[] totoalLen = { 0x00, 0x13 };

            //wrap the whole data
            byte[] result = new byte[1 + 1 + 2 + 6 + 1 + 2 + 2 + 1 + 7];
            result[0] = btpreamble;
            result[1] = btVersion;
            totoalLen.CopyTo(result, 2);
            btDevId.CopyTo(result, 4);
            result[10] = btRouteFlag;
            btDstNode.CopyTo(result, 11);
            btPdu.CopyTo(result, 13);
            result[15] = btSeq;
            tag.CopyTo(result, 16);

            //增加CRC校验
            String strCrc = StringUtil.To16HexString(String.Format("{0:X}", (int)CodeUtils.CRC16_AD(result)));
            byte[] btcrc = { CodeUtils.String2Byte(strCrc.Substring(0, 2)), CodeUtils.String2Byte(strCrc.Substring(2, 2)) };
            byte[] afcrc = new byte[result.Length + 2];
            result.CopyTo(afcrc, 0);
            btcrc.CopyTo(afcrc, result.Length);

            //TODO LIST:获取会话信息，发送数据
            var server = bootstrap.GetServerByName(CASIC_SERVER_NAME);
            CasicServer casicServer = server as CasicServer;
            CasicSession session = casicServer.GetSessionByID(dto.SessionId) as CasicSession;
            session.Logger.Info("下发唤醒信息：" + "设备编号：" + deviceId+""+BitConverter.ToString(afcrc));
            session.Send(afcrc, 0, afcrc.Length);
        }
        
        
        //TODO LIST:获取所有在线的设备列表
        private List<DeviceDTO> getOnlineCasicDev() 
        {
            List<DeviceDTO> devices = new List<DeviceDTO>();

            var server = bootstrap.GetServerByName(CASIC_SERVER_NAME);
            if (server == null)
            {
                return devices;
            }
            CasicServer casicServer = server as CasicServer;
        //    CasicSession session = casicServer.GetAllSessions() as CasicSession;
            foreach (CasicSession session in casicServer.GetAllSessions())
            {
                String sessionId = session.SessionID;
                //casic的设备一个session中，可能存在集中器，噪声、液位传感器都多种数据信息
                foreach (KeyValuePair<string, CasicSession.DeviceDTO> kvp in session.devMaps)
                {
                    String devInfo = kvp.Key;
                    CasicSession.DeviceDTO dto = kvp.Value;
                    String devType = dto.DevType;
                    String devID = dto.DevCode;

                    DeviceDTO devDto = new DeviceDTO();
                    devDto.SessionId = sessionId;
                    devDto.Company = "203所";
                    devDto.TypeName = devType;
                    devDto.Name = devID;
                    devDto.Tag = dto.Detail;
                    devDto.Status = "在线";
                    devDto.ServerName = "CasicServer";

                    devices.Add(devDto);

                }
 
            }
        

            return devices;
         
        }

        private List<DeviceDTO> getOnlineYLDev()
        {
            List<DeviceDTO> devices = new List<DeviceDTO>();

            var server = bootstrap.GetServerByName(YL_SERVER);
            if (server == null)
            {
                return devices;
            }
            YLServer casicServer = server as YLServer;
            //    CasicSession session = casicServer.GetAllSessions() as CasicSession;
            foreach (YLSession session in casicServer.GetAllSessions())
            {
                String sessionId = session.SessionID;
                if (session.DeviceID != null)
                {
                    DeviceDTO devDto = new DeviceDTO();
                    devDto.SessionId = sessionId;
                    devDto.Company = "203所";
                    devDto.TypeName = "雨量计";
                    devDto.Name = session.HubID;
                    devDto.Tag = "";
                    devDto.Status = "在线";
                    devDto.ServerName = "YLServer";
                    devices.Add(devDto);
                }

            }


            return devices;
        }

        private List<DeviceDTO> getOnlineWSDev()
        {
            List<DeviceDTO> devices = new List<DeviceDTO>();

            var server = bootstrap.GetServerByName(WS_SERVER);
            if (server == null)
            {
                return devices;
            }
            WSServer casicServer = server as WSServer;
            //    CasicSession session = casicServer.GetAllSessions() as CasicSession;
            foreach (WSSession session in casicServer.GetAllSessions())
            {
                String sessionId = session.SessionID;
                if (session.MacID != null)
                {
                    DeviceDTO devDto = new DeviceDTO();
                    devDto.SessionId = sessionId;
                    devDto.Company = "203所";
                    devDto.TypeName = "有害气体监测仪";
                    devDto.Name = session.MacID;
                    devDto.Tag = "";
                    devDto.Status = "在线";
                    devDto.ServerName = "WSServer";
                    devices.Add(devDto);
                }

            }


            return devices;
 
        }

        public List<DeviceDTO> getAllOnlineDev()
        {
            List<DeviceDTO> casicDevs = getOnlineCasicDev();
            List<DeviceDTO> ylDevs = getOnlineYLDev();
            List<DeviceDTO> wsDevs = getOnlineWSDev();

            //添加雨量计设备
            foreach(DeviceDTO dev in ylDevs)
            {
                casicDevs.Add(dev);
            }
            //添加污水有害气体设备
            foreach (DeviceDTO dev0 in wsDevs)
            {
                casicDevs.Add(dev0);
            }
            return casicDevs;
        }

        /**
         *发送前端设备配置信息
         * **/
        public bool sendDeviceInfo(DeviceDTO deviceDto, Dictionary<String, String> settings)
        {
            String serverName = deviceDto.ServerName;
            String sessionID = deviceDto.SessionId;
            bool reuslt = false;
            switch (serverName)
            {
                case CASIC_SERVER_NAME:
                    //TODO LIST:根据传感器类型，发送对应的配置信息
                    reuslt = this.sendCasicConfig(deviceDto, settings);
                    break;
                case YL_SERVER:
                    //TODO LIST：进行雨量计的校时操作
                    reuslt = this.sendYLConfig(deviceDto, settings);
                    break;
                case WS_SERVER:
                    reuslt = this.sendWSConfig(deviceDto, settings);
                    break;
                default:
                    break;
            }
            return reuslt;

        }


        //甄玉龙增加燃气校时配置帧
        public void calibraRQ(DeviceDTO dto)
        {
            var server = bootstrap.GetServerByName(WS_SERVER);
            WSServer casicServer = server as WSServer;
            WSSession session = casicServer.GetSessionByID(dto.SessionId) as WSSession;

            //下发校时信息
            String preTimeCal = "SewTiming:" + session.MacID + ",";
            String postTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            String timeCal = preTimeCal + postTime;

            byte[] data1 = new byte[timeCal.Length + 2];
            Encoding.ASCII.GetBytes(timeCal, 0, timeCal.Length, data1, 0);
            data1[timeCal.Length] = 0x0D;
            data1[timeCal.Length + 1] = 0x0A;
            session.Send(data1, 0, data1.Length);
            session.Logger.Info("校时信息:" + timeCal);
     
        }

        private bool sendWSConfig(DeviceDTO dto, Dictionary<String, String> settings)
        {
            var server = bootstrap.GetServerByName(WS_SERVER);
            WSServer casicServer = server as WSServer;
            WSSession session = casicServer.GetSessionByID(dto.SessionId) as WSSession;

            String period = settings["ws_period"];
            if (period == "")
            {
                return false;
            }

            String sdata0 = "SewAcquireInterval:" + session.MacID + "," + period;
            byte[] data0 = new byte[sdata0.Length + 2];
            Encoding.ASCII.GetBytes(sdata0, 0, sdata0.Length, data0, 0);
            data0[sdata0.Length] = 0x0D;
            data0[sdata0.Length + 1] = 0x0A;
            session.Send(data0, 0, data0.Length);
            session.Logger.Info("有害气体配置信息:" + sdata0);

            //下发校时信息
            /*
            String preTimeCal = "SewTiming:" + session.MacID + ",";
            String postTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            String timeCal = preTimeCal + postTime;

            byte[] data1 = new byte[timeCal.Length + 2];
            Encoding.ASCII.GetBytes(timeCal, 0, timeCal.Length, data1, 0);
            data1[timeCal.Length] = 0x0D;
            data1[timeCal.Length + 1] = 0x0A;
            session.Send(data1, 0, data1.Length);
            session.Logger.Info("校时信息:" + timeCal);
             * */
            return true;

        }


        /**
         * 发送雨量计校时信息
         **/
        private bool sendYLConfig(DeviceDTO dto, Dictionary<String, String> settings)
        {
            var server = bootstrap.GetServerByName(YL_SERVER);
            YLServer casicServer = server as YLServer;
            YLSession session = casicServer.GetSessionByID(dto.SessionId) as YLSession;

            byte[] set = ApplicationContext.getInstance().getUpdateTime();
            //更新session中devID的编号
            String devName = settings["yw_deviceId"];
            session.DeviceID = devName;
       
            byte[] devCodes = new byte[2];
            devCodes[0] = byte.Parse(devName.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
            devCodes[1] = byte.Parse(devName.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

            set[2] = devCodes[0];
            set[3] = devCodes[1];
            byte[] sendCode = CodeUtils.yl_addCrc(set);
            //TODO LIST:获取会话信息，发送数据
            //session.Send(set, 0, set.Length);
            session.Send(sendCode, 0, sendCode.Length);
            session.Logger.Info("雨量计配置信息下发成功:" + BitConverter.ToString(sendCode));
            return true;
        }

        /**
         * 发送203所自研设备：渗漏预警仪、液位监测仪、井盖传感器的配置信息
         * **/
        private bool sendCasicConfig(DeviceDTO dto,Dictionary<String,String>settings)
        {
            //判断设备是否已经唤醒
            //TODO LIST:获取会话信息，发送数据
            /*
            var server0 = bootstrap.GetServerByName(CASIC_SERVER_NAME);
            CasicServer casicServer0 = server0 as CasicServer;
            CasicSession session0 = casicServer0.GetSessionByID(dto.SessionId) as CasicSession;
            CasicSession.DeviceDTO dev = session0.devMaps[dto.Name];
            if (!dev.IsWakeUp && dev.DevType!="集中器")
            {
                return false;
            }
             * */
           
            String[] parameters = dto.Tag.Split(',');

            //TODO: construct the receving casic data
            String preamble = parameters[0];
            byte btpreamble = byte.Parse(preamble, System.Globalization.NumberStyles.HexNumber);

            String version = parameters[1];
            byte btVersion = byte.Parse(version, System.Globalization.NumberStyles.HexNumber);

            String deviceId = parameters[3];//6个字节
            byte[] btDevId = new byte[6];
            btDevId[0] = byte.Parse(deviceId.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
            btDevId[1] = byte.Parse(deviceId.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
            btDevId[2] = byte.Parse(deviceId.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
            btDevId[3] = byte.Parse(deviceId.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[4] = byte.Parse(deviceId.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[5] = byte.Parse(deviceId.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);

            String routeFlag = parameters[4];
            byte btRouteFlag = byte.Parse(routeFlag, System.Globalization.NumberStyles.HexNumber);

            String dstNodeAddr = parameters[5];//2个字节
            byte[] btDstNode = new byte[2];
            btDstNode[0] = byte.Parse(dstNodeAddr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            btDstNode[1] = byte.Parse(dstNodeAddr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

            String seq =parameters[7];
            byte btSeq = byte.Parse(seq, System.Globalization.NumberStyles.HexNumber);

            byte[] btPdu = new byte[2]; ;//2个字节
            byte[] tag=null;
            
            switch (dto.TypeName)
            {
                case "渗漏预警仪":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x82;
                    tag = this.getNosieTag(settings);
                    break;
                case "液位监测仪":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x87;
                    tag = this.getYWTag(settings);
                    break;
                case "集中器":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x85;
                    tag = this.getHubTag(settings);
                    break;
                case "温度压力监测仪":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x88;
                    tag = this.getTempTag(settings);
                    break;
                    /*
                case "压力计":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x89;
                    tag = this.getPressTag(settings);
                    break;
                     * */
                case "远传水表":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x8A;
                    tag = this.getWaterMeterTag(settings);
                    break;
                case "井盖传感器":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x86;
                    break;
                default:
                    break;
            }
      
            int totalLen = 6 + 1 + 2 + 2 + 1 + tag.Length;
            byte[] btLens = new byte[2];
            byte[] btlens0 = BitConverter.GetBytes(totalLen);
            btLens[0] = btlens0[1];
            btLens[1] = btlens0[0];

            //wrap the whole data
            byte[] result = new byte[1 + 1 + 2 + 6 + 1 + 2 + 2 + 1 + tag.Length];
            result[0] = btpreamble;
            result[1] = btVersion;
            btLens.CopyTo(result, 2);
            btDevId.CopyTo(result, 4);
            result[10] = btRouteFlag;
            btDstNode.CopyTo(result, 11);
            btPdu.CopyTo(result, 13);
            result[15] = btSeq;
            tag.CopyTo(result, 16);

          
          //  session.Logger.Info("CRC校验前数据：" + BitConverter.ToString(result));

            //增加CRC校验
            String strCrc = StringUtil.To16HexString(String.Format("{0:X}", (int)CodeUtils.CRC16_AD(result)));
            byte[] btcrc = { CodeUtils.String2Byte(strCrc.Substring(0, 2)), CodeUtils.String2Byte(strCrc.Substring(2, 2)) };
            byte[] afcrc = new byte[result.Length + 2];
            result.CopyTo(afcrc, 0);
            btcrc.CopyTo(afcrc, result.Length);
            
            //TODO LIST:获取会话信息，发送数据
            var server = bootstrap.GetServerByName(CASIC_SERVER_NAME);
            CasicServer casicServer = server as CasicServer;
            CasicSession session = casicServer.GetSessionByID(dto.SessionId) as CasicSession;
            session.Logger.Info("手动下发渗漏噪声配置信息：" + BitConverter.ToString(afcrc));
            session.Send(afcrc, 0, afcrc.Length);
            return true;
        }
        
        /**
         * 获取温度数据的tag信息
         * **/
        private byte[] getTempTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            byte[] result = new byte[this.getTempTagCount(settings)];
            
            //获取采集开始时间
            if (settings.ContainsKey("temp_stime"))
            {
                byte[] oid1 = { 0x10, 0x00, 0x01, 0x04, 0x00, 0x02 };
                String sl_stime = settings["temp_stime"];
                Int16 stime1 = (Int16)(Convert.ToInt16(sl_stime.Substring(0, 2)) * 60 + Convert.ToInt16(sl_stime.Substring(3, 2)));
                byte[] btime1 = BitConverter.GetBytes(stime1);
                byte[] setting_sl_stime = new byte[8];
                oid1.CopyTo(setting_sl_stime, 0);
                setting_sl_stime[6] = btime1[1];
                setting_sl_stime[7] = btime1[0];
                setting_sl_stime.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("temp_period"))
            {
                //周期
                String yw_per = settings["temp_period"];
                byte[] yw_oid7 = { 0x10, 0x00, 0x00, 0x62, 0x00, 0x02 };
                Int16 per = (Convert.ToInt16(yw_per));
                byte[] yw_bper = BitConverter.GetBytes(per);
                byte[] setting_yw_per = new byte[8];
                yw_oid7.CopyTo(setting_yw_per, 0);
                setting_yw_per[6] = yw_bper[1];
                setting_yw_per[7] = yw_bper[0];
                setting_yw_per.CopyTo(result, flag);
                flag = flag + 8;

            }

            SystemTimeConfig sysTimeConfig = new SystemTimeConfig(null);
            byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
            btConfig.CopyTo(result, flag);
            
            return result;
        }

        /**
        * 获取温度配置个数
        * **/
        private int getTempTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;
            if (settings.ContainsKey("temp_stime"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("temp_period"))
            {
                flag = flag + 8;

            }

            flag = flag + 12;
            return flag;
        }


          /**
         * 获取压力数据的tag信息
         * **/
        private byte[] getWaterMeterTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            byte[] result = new byte[this.getWaterMeterTagCount(settings)];
            
            //获取采集开始时间
            if (settings.ContainsKey("waterMeter_stime"))
            {
                byte[] oid1 = { 0x10, 0x00, 0x01, 0x04, 0x00, 0x02 };
                String sl_stime = settings["waterMeter_stime"];
                Int16 stime1 = (Int16)(Convert.ToInt16(sl_stime.Substring(0, 2)) * 60 + Convert.ToInt16(sl_stime.Substring(3, 2)));
                byte[] btime1 = BitConverter.GetBytes(stime1);
                byte[] setting_sl_stime = new byte[8];
                oid1.CopyTo(setting_sl_stime, 0);
                setting_sl_stime[6] = btime1[1];
                setting_sl_stime[7] = btime1[0];
                setting_sl_stime.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("waterMeter_period"))
            {
                //周期
                String yw_per = settings["waterMeter_period"];
                byte[] yw_oid7 = { 0x10, 0x00, 0x00, 0x62, 0x00, 0x02 };
                Int16 per = (Convert.ToInt16(yw_per));
                byte[] yw_bper = BitConverter.GetBytes(per);
                byte[] setting_yw_per = new byte[8];
                yw_oid7.CopyTo(setting_yw_per, 0);
                setting_yw_per[6] = yw_bper[1];
                setting_yw_per[7] = yw_bper[0];
                setting_yw_per.CopyTo(result, flag);
                flag = flag + 8;
            }

            SystemTimeConfig sysTimeConfig = new SystemTimeConfig(null);
            byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
            btConfig.CopyTo(result, flag);
            
            return result;
        }

        /**
        * 获取压力配置个数
        * **/
        private int getWaterMeterTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;
            if (settings.ContainsKey("waterMeter_stime"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("waterMeter_period"))
            {
                flag = flag + 8;

            }

            flag = flag + 12;
            return flag;
        }

        /**
         * 获取噪声数据的tag信息
         * **/
        private byte[] getNosieTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            int cc = getNoiseTagCount(settings);
            byte[] result = new byte[cc];

            SystemTimeConfig sysTimeConfig = new SystemTimeConfig(null);
            byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
            btConfig.CopyTo(result, flag);
            flag = flag + 12;

            //获取采集开始时间
            if (settings.ContainsKey("sl_stime"))
            {
                byte[] oid1 = { 0x10, 0x00, 0x01, 0x04, 0x00, 0x02 };
                String sl_stime = settings["sl_stime"];
                Int16 stime1 = (Int16)(Convert.ToInt16(sl_stime.Substring(0, 2)) * 60 + Convert.ToInt16(sl_stime.Substring(3, 2)));
                byte[] btime1 = BitConverter.GetBytes(stime1);
                byte[] setting_sl_stime = new byte[8];
                oid1.CopyTo(setting_sl_stime, 0);
                setting_sl_stime[6] = btime1[1];
                setting_sl_stime[7] = btime1[0];
                setting_sl_stime.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("sl_itrl"))
            {
                //采集间隔
                byte[] oid2 = { 0x10, 0x00, 0x01, 0x05, 0x00, 0x02 };
                String sl_itr = settings["sl_itrl"];
                Int16 sitr1 = (Convert.ToInt16(sl_itr));
                byte[] bsitr1 = BitConverter.GetBytes(sitr1);
                byte[] setting_sl_itr = new byte[8];
                oid2.CopyTo(setting_sl_itr, 0);
                setting_sl_itr[6] = bsitr1[1];
                setting_sl_itr[7] = bsitr1[0];
                setting_sl_itr.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("sl_cnt"))
            {
                //采集次数
                String sl_cnt = settings["sl_cnt"];
                byte[] oid3 = { 0x10, 0x00, 0x01, 0x06, 0x00, 0x02 };
                Int16 scnt1 = (Convert.ToInt16(sl_cnt));
                byte[] bscnt1 = BitConverter.GetBytes(scnt1);
                byte[] setting_sl_cnt = new byte[8];
                oid3.CopyTo(setting_sl_cnt, 0);
                setting_sl_cnt[6] = bscnt1[1];
                setting_sl_cnt[7] = bscnt1[0];
                setting_sl_cnt.CopyTo(result,flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("sl_rept"))
            {
                //重试次数
                String sl_rept = settings["sl_rept"];
                byte[] oid4 = { 0x10, 0x00, 0x00, 0x0A, 0x00, 0x02 };
                Int16 rept = (Convert.ToInt16(sl_rept));
                byte[] brept = BitConverter.GetBytes(rept);
                byte[] setting_sl_rept = new byte[8];
                oid4.CopyTo(setting_sl_rept, 0);
                setting_sl_rept[6] = brept[1];
                setting_sl_rept[7] = brept[0];
                setting_sl_rept.CopyTo(result, flag);
                flag = flag + 8;
            }

            return result;

        }


        /**
         * 获取噪声配置个数
         * **/
        private int getNoiseTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;
            if (settings.ContainsKey("sl_stime"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("sl_itrl"))
            {
                flag = flag + 8;
               
            }

            if (settings.ContainsKey("sl_cnt"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("sl_rept"))
            {
                flag = flag + 8;
            }
            flag = flag + 12;//校时
            return flag;
        }

        /**
         * 获取液位数据的tag信息
         * **/
        private byte[] getYWTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            byte[] result = new byte[getYWTagCount(settings)];
            if (settings.ContainsKey("yw_itr"))
            {
                //采集间隔
                byte[] yw_oid2 = { 0x10, 0x00, 0x01, 0x05, 0x00, 0x02 };
                String yw_itr = settings["yw_itr"];
                Int16 yw_sitr1 = (Convert.ToInt16(yw_itr));
                byte[] yw_bsitr1 = BitConverter.GetBytes(yw_sitr1);
                byte[] setting_yw_itr = new byte[8];
                yw_oid2.CopyTo(setting_yw_itr, 0);
                setting_yw_itr[6] = yw_bsitr1[1];
                setting_yw_itr[7] = yw_bsitr1[0];
                setting_yw_itr.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("yw_cnt"))
            {
                //采集次数
                String yw_cnt = settings["yw_cnt"];
                byte[] yw_oid3 = { 0x10, 0x00, 0x01, 0x06, 0x00, 0x02 };
                Int16 yw_scnt1 = (Convert.ToInt16(yw_cnt));
                byte[] yw_bscnt1 = BitConverter.GetBytes(yw_scnt1);
                byte[] setting_yw_cnt = new byte[8];
                yw_oid3.CopyTo(setting_yw_cnt, 0);
                setting_yw_cnt[6] = yw_bscnt1[1];
                setting_yw_cnt[7] = yw_bscnt1[0];
                setting_yw_cnt.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("yw_rept"))
            {
                //重试次数
                String sl_rept = settings["yw_rept"];
                byte[] oid4 = { 0x10, 0x00, 0x00, 0x0A, 0x00, 0x01 };
                byte rept = (Convert.ToByte(sl_rept));
                //byte[] brept = BitConverter.GetBytes(rept);
                byte[] setting_sl_rept = new byte[7];
                oid4.CopyTo(setting_sl_rept, 0);
                setting_sl_rept[6] = rept;
                //setting_sl_rept[7] = brept[0];
                setting_sl_rept.CopyTo(result, flag);
                flag = flag + 7;
            }

            if (settings.ContainsKey("yw_reset"))
            {
                //重启
                String yw_reset = settings["yw_reset"];
                byte[] oid5 = { 0x10, 0x00, 0x00, 0x61, 0x00, 0x01 };
                byte reset = (Convert.ToByte(yw_reset));
                byte[] setting_yw_reset = new byte[7];
                oid5.CopyTo(setting_yw_reset, 0);
                setting_yw_reset[6] = reset;
                setting_yw_reset.CopyTo(result, flag);
                flag = flag + 7;
            }

            if (settings.ContainsKey("yw_height"))
            {
                //高度
                String yw_height = settings["yw_height"];
                byte[] oid6 = { 0x10, 0x00, 0x00, 0x60, 0x00, 0x04 };
                float height = (Convert.ToSingle(yw_height));
                byte[] bHeight = BitConverter.GetBytes(height);
                byte[] setting_yw_height = new byte[10];
                oid6.CopyTo(setting_yw_height, 0);
                setting_yw_height[6] = bHeight[0];
                setting_yw_height[7] = bHeight[1];
                setting_yw_height[8] = bHeight[2];
                setting_yw_height[9] = bHeight[3];
                setting_yw_height.CopyTo(result, flag);
                flag = flag + 10;
            }

            if (settings.ContainsKey("yw_period"))
            {

                //周期
                String yw_per = settings["yw_period"];
                byte[] yw_oid7 = { 0x10, 0x00, 0x00, 0x62, 0x00, 0x02 };
                Int16 per = (Convert.ToInt16(yw_per));
                byte[] yw_bper = BitConverter.GetBytes(per);
                byte[] setting_yw_per = new byte[8];
                yw_oid7.CopyTo(setting_yw_per, 0);
                setting_yw_per[6] = yw_bper[1];
                setting_yw_per[7] = yw_bper[0];
                setting_yw_per.CopyTo(result, flag);
                flag = flag + 8;

            }

            SystemTimeConfig sysTimeConfig = new SystemTimeConfig(null);
            byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
            btConfig.CopyTo(result, flag);

            return result;
          
        }

        /**
        * 获取液位配置个数
        * **/
        private int getYWTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;

            if (settings.ContainsKey("yw_itr"))
            {
                flag = flag + 8;

            }

            if (settings.ContainsKey("yw_cnt"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("yw_rept"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("yw_reset"))
            {
                flag = flag + 7;
            }

            if (settings.ContainsKey("yw_height"))
            {
                flag = flag + 10;
            }

            if (settings.ContainsKey("yw_period"))
            {
                flag = flag + 8;
            }
            flag = flag + 12;//校时
            return flag;
        }

        /**
         * 获取集中器的配置信息
         * **/
        private byte[] getHubTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            int cc = getHubTagCount(settings);
            byte[] result = new byte[cc];

            if (settings.ContainsKey("hub_ip"))
            {
                //获取iptag信息
                String adIp = settings["hub_ip"];
                byte[] btIp = ASCIIEncoding.ASCII.GetBytes(adIp);
                Int16 ipLen0 = (Int16)btIp.Length;
                byte[] btiplens0 = BitConverter.GetBytes(ipLen0);
                byte[] iplens = new byte[2];
                iplens[0] = btiplens0[1];
                iplens[1] = btiplens0[0];

                byte[] ipOid = { 0x10, 0x00, 0x00, 0x22 };
                byte[] ipTag = new byte[4 + 2 + btIp.Length];
                ipOid.CopyTo(ipTag, 0);
                iplens.CopyTo(ipTag, 4);
                btIp.CopyTo(ipTag, 6);

                ipTag.CopyTo(result, flag)
;
                flag = flag + 4 + 2 + btIp.Length;
            }

            if (settings.ContainsKey("hub_port"))
            {
                //获取porttag信息
                String adPort = settings["hub_port"];
                byte[] btAdPort = ASCIIEncoding.ASCII.GetBytes(adPort);
                Int16 portLen = (Int16)btAdPort.Length;
                byte[] btportlens0 = BitConverter.GetBytes(portLen);
                byte[] portlens = new byte[2];
                portlens[0] = btportlens0[1];
                portlens[1] = btportlens0[0];

                byte[] btPortOid = { 0x10, 0x00, 0x00, 0x23 };
                byte[] portTag = new byte[4 + 2 + btAdPort.Length];
                btPortOid.CopyTo(portTag, 0);
                portlens.CopyTo(portTag, 4);
                btAdPort.CopyTo(portTag, 6);
                portTag.CopyTo(result, flag);
                flag = flag + 4 + 2 + btAdPort.Length;
            }
            return result;
        }

        private int getHubTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;
            if (settings.ContainsKey("hub_ip"))
            {
                //获取iptag信息
                String adIp = settings["hub_ip"];
                byte[] btIp = ASCIIEncoding.ASCII.GetBytes(adIp);
                flag =flag + 4 + 2 + btIp.Length;

            }

            if (settings.ContainsKey("hub_port"))
            {
                //获取porttag信息
                String adPort = settings["hub_port"];
                byte[] btAdPort = ASCIIEncoding.ASCII.GetBytes(adPort);
                flag = flag + 4 + 2 + btAdPort.Length;
            }
            return flag;
        }

    }
}
