using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Utility;
using SuperSocket.Common;

namespace SensorHub.Servers.Commands.CASICCommands
{
    [CasicCommandFilter]
    public class CasicCmd : CommandBase<CasicSession, StringRequestInfo>
    {
        //public static String currentSystemDate="";
        //public static CasicSession casicSession = null;
        //public static String devCode;
        //public static String cell="";

        public override string Name
        {
            get
            {
                return "Casic";
            }
        }

        public String getDeviceTypeByPdu(String pduType) 
        {
            Int16 btpduType = Int16.Parse(pduType, System.Globalization.NumberStyles.HexNumber);
            int opearType = btpduType  & 0x7F;
            String devType = "";
            switch (opearType)
            {
                case 0:
                    devType = "未知设备";
                    break;
                case 1:
                    devType = "多功能漏损监测仪";
                    break;
                case 2:
                    devType = "噪声记录仪";
                    break;
                case 3:
                    devType = "排水监控一体机";
                    break;
                case 4:
                    devType = "燃气智能监测终端";
                    break;
                case 5:
                    devType = "集中器";
                    break;
                case 6:
                    devType = "井盖传感器";
                    break;
                case 7:
                    devType = "液位监测仪";
                    break;
                case 8:
                    devType = "温度压力监测仪";
                    break;
                case 10:
                    devType = "远传水表";
                    break;
                case 11:
                    devType = "保温层下腐蚀速率监测仪";
                    break;
                case 12:
                    devType = "保温层下腐蚀环境参数监测仪";
                    break;
                default:
                    devType = "undefiend";
                    break;
            }
            return devType;
        }

        public String getOpeTypeByPdu(String pduType)
        {
            Int16 btpduType = Int16.Parse(pduType, System.Globalization.NumberStyles.HexNumber);

            int opearType = (btpduType >> 8) & 0xFF;
            String result="";
            switch (opearType) 
            {
                case 1:
                    result =  "GetRequest";
                    break;
                case 2:
                    result = "GetResponse";
                    break;
                case 3:
                    result = "SetRequest";
                    break;
                case 4:
                    result = "TrapRequest";
                    break;
                case 5:
                    result = "TrapResponse";
                    break;
                case 6:
                    result = "OnlineRequest";
                    break;
                case 7:
                    result = "OnlineResponse";
                    break;
                case 8:
                    result = "StartupRequest";
                    break;
                case 9:
                    result = "StartupResponse";
                    break;
                case 10:
                    result = "WakeupRequest";
                    break;
                case 11:
                    result = "WakeupResponse";
                    break;
                case 13:
                    result = "ClientRequest";
                    break;
                case 12:
                    result = "SetResponse";
                    break;
                default:
                    result = "undefined";
                    break;
            }
            return result;
        }

        private bool isfinishe(String pduType)
        {
            Int16 btpduType = Int16.Parse(pduType, System.Globalization.NumberStyles.HexNumber);
            int flag = btpduType & 0x80;
            return flag == 128 ? true : false; 
        }

        private List<Tag> getTags(String strTags,CasicSession session)
        {
            List<Tag> tags = new List<Tag>();
            try
            {
                int i = 0;
                while (i < strTags.Length)
                {
                    //get each tag from strTag and construct tag list; 
                    String oid = strTags.Substring(i, 8);

                    int len = Int16.Parse(strTags.Substring(i + 8, 4), System.Globalization.NumberStyles.HexNumber);
                    String value = strTags.Substring(i + 12, len * 2);
                    i = i + 12 + 2 * len;
                    //TODO LIST:Construct a tag according to oid
                    Tag tag = TagFactory.create(oid, len, value);
                    tags.Add(tag);
                }
            }
            catch (Exception e)
            {
                session.Logger.Error("集中器挂载设备上传协议出错:" + e.Message);
            }
        
            return tags;
        }

        private CellTag getCellTag(List<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                if (tag!=null && tag is CellTag)
                {
                    return (CellTag)tag;
                }
            }
            return null;
        }

        private SystemDateTag getSystemDateTag(List<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                if (tag != null && tag is SystemDateTag)
                {
                    return (SystemDateTag)tag;
                }
            }
            return null;
        }

        public override void ExecuteCommand(CasicSession session, StringRequestInfo requestInfo)
        {
            //casicSession = session;

            //TODO: construct the receving casic data
            String preamble = requestInfo.Parameters[0];
            String version = requestInfo.Parameters[1];
            String leng = requestInfo.Parameters[2];
            String deviceId = requestInfo.Parameters[3];
            String routeFlag = requestInfo.Parameters[4];
            String dstNodeAddr = requestInfo.Parameters[5];
            String pduType = requestInfo.Parameters[6];
            String seq = requestInfo.Parameters[7];
            String settings = requestInfo.Parameters[8];
            String crcs = requestInfo.Parameters[9];

            //print the receving data
            String devType = getDeviceTypeByPdu(pduType);
            String operType = getOpeTypeByPdu(pduType);
            session.Logger.Info("AD接收数据:" + requestInfo.Body);
            session.Logger.Info("当前版本:" + version);
            session.Logger.Info("设备编号:" + deviceId);
            session.Logger.Info("路由标志:" + routeFlag);
            session.Logger.Info("目标节点地址:" + dstNodeAddr);
            session.Logger.Info("设备类型:" + devType);
            session.Logger.Info("操作类型" + operType);
            session.Logger.Info("序列seq:" + seq);

            //update the device code
            //devCode = deviceId;

            //如果是集中器更新session，如果不是把设备切换成在线
            if (devType == "集中器")
            {
                session.HubAddr = deviceId;
                new BLL.AlarmConcentrator().setHubOnLine(deviceId);
            }
            else
            {
                new BLL.DevHub().setOnLineByDevcode(deviceId);
            }

            //判断是返回的设置确认数据帧，更新DeviceConfig表
            if (operType == "SetResponse")
            {
                new BLL.DeviceConfig().clearDeviceConfigByDevCode(deviceId);
                return;
            }

            //TODO LIST：更新会话中session信息
            this.updateSessionInfo(session, requestInfo);

            //upload periodically or config query according to oid
            //获取电量信息，系统时间，传递给对应的handler
            List<Tag> tags = this.getTags(settings, session);
            CellTag cellTag = this.getCellTag(tags);
            SystemDateTag systemDateTag = this.getSystemDateTag(tags);

            try
            {
                //TODO LIST:处理主动上报的数据、参数查询、参数设置
                TagHandler systemDateHandler = new SystemDateTagHandler();
                TagHandler noiseHandler = new NoiseTagHandler();
                TagHandler liquidHandler = new LiquidTagHandler();
                TagHandler wellHandler = new WellTagHandler();
                TagHandler sysTimeHandler = new SystemTimeTagHandler();
                TagHandler wakeupHandler = new WakeUpTagHandler();
                TagHandler cellTagHandler = new CellTagHandler();
                TagHandler waterMeterHadler = new WaterMeterTagHandler();
                TagHandler pressHandler = new PressTagHandler();
                TagHandler tempHandler = new TempTagHandler();
                TagHandler fsslHandler = new FSSLTagHandler();
                TagHandler fshjHandler = new FSHJTagHandler();
                TagHandler rqHandler = new RQTagHandler();
                TagHandler sensorExp0 = new SensorException0TagHandler();
                TagHandler sensorExp1 = new SensorException1TagHandler();

                systemDateHandler.NextHandler = noiseHandler;
                noiseHandler.NextHandler = liquidHandler;
                liquidHandler.NextHandler = waterMeterHadler;
                waterMeterHadler.NextHandler = wellHandler;
                wellHandler.NextHandler = tempHandler;
                tempHandler.NextHandler = pressHandler;
                pressHandler.NextHandler = rqHandler;
                rqHandler.NextHandler = sysTimeHandler;
                sysTimeHandler.NextHandler = wakeupHandler;
                wakeupHandler.NextHandler = cellTagHandler;
                cellTagHandler.NextHandler = fsslHandler;
                fsslHandler.NextHandler = fshjHandler;
                fshjHandler.NextHandler = sensorExp0;
                sensorExp0.NextHandler = sensorExp1;

                foreach (Tag tag in tags)
                {
                    //采用责任链的方式来处理各个tag:
                    //目前已经处理噪声、液位、系统时间、系统日期tag
                    systemDateHandler.handleTag(tag, deviceId,cellTag,
                        systemDateTag,session);
                }
                if (routeFlag == "03") //GPRS
                {
                    senderGPRSConfig(devType, deviceId, version, session);
                }
                else //433
                {
                    //下发返回的信息
                    if ((operType != "GetResponse") && (devType != "井盖传感器") &&
                        isfinishe(pduType) &&
                        (operType == "TrapRequest" || operType == "OnlineRequest")
                    )
                    {
                        HeadConfig headConfig = new HeadConfig(version, deviceId, pduType, seq, routeFlag, dstNodeAddr);
                        TrapRespConfig trapResp = new TrapRespConfig(headConfig);
                        SystemTimeConfig sysTimeConfig = new SystemTimeConfig(trapResp);
                        if (session.devMaps.ContainsKey(deviceId))
                        {
                            CasicSession.DeviceDTO dto = session.devMaps[deviceId];
                            trapResp.NoSeq = dto.Seq;
                        }
                        byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);

                        session.Logger.Info(devType + ":" + deviceId + " CRC校验前数据：" + BitConverter.ToString(btConfig));
                        String strCrc =
                            StringUtil.To16HexString((String.Format("{0:X}", (int) CodeUtils.CRC16_AD(btConfig))));
                        session.Logger.Info(devType + ":" + deviceId + " 生成的CRC校验：" + strCrc);
                        byte[] btcrc =
                        {
                            CodeUtils.String2Byte(strCrc.Substring(0, 2)),
                            CodeUtils.String2Byte(strCrc.Substring(2, 2))
                        };
                        byte[] afcrc = new byte[btConfig.Length + 2];
                        btConfig.CopyTo(afcrc, 0);
                        btcrc.CopyTo(afcrc, btConfig.Length);

                        session.Logger.Info(devType + ":" + deviceId + " CRC校验后数据：" + BitConverter.ToString(afcrc));
                        session.Send(afcrc, 0, afcrc.Length);

                        CasicSession.DeviceDTO dto0 = session.devMaps[deviceId];
                        dto0.Seq = 0x00;
                        session.devMaps[deviceId] = dto0;
                    }
                }
            }
            catch (Exception e)
            {
                session.Logger.Error(e.ToString());
            }
        }

        private void updateSessionInfo(CasicSession session, StringRequestInfo requestInfo)
        {
            String deviceId = requestInfo.Parameters[3];
            String pduType = requestInfo.Parameters[6];
            String devType = getDeviceTypeByPdu(pduType);
            String seq = requestInfo.Parameters[7]; 
            String sessionKey = deviceId;
            byte btSeq = byte.Parse(seq, System.Globalization.NumberStyles.HexNumber);

            if (!session.devMaps.ContainsKey(sessionKey))
            {
                //session.devMaps.Add(sessionKey, requestInfo.Body);
                CasicSession.DeviceDTO dev = new CasicSession.DeviceDTO();
                dev.DevCode = deviceId;
                dev.DevType = devType;
                dev.Detail = requestInfo.Body;
                dev.Company = "203";
                dev.Status = "在线";
                dev.IsWakeUp = false;
                //dev.Seq = 0x01;
                dev.Seq = (UInt16)(0x01 << (btSeq - 1));
                session.devMaps.Add(sessionKey, dev);
            }
            else 
            {
                //更新序列状态
                CasicSession.DeviceDTO dto = session.devMaps[sessionKey];
                if (dto.Seq == 0)
                {
                    //dto.Seq = 0x01;
                    dto.Seq = (UInt16)(0x01 << (btSeq - 1));
                }
                else
                {
                    dto.Seq = (UInt16)(dto.Seq | 0x01 << (btSeq-1));
                }
                session.devMaps[sessionKey] = dto;
            }
        }

        private void senderGPRSConfig(string devType,string deviceId,string version,CasicSession session)
        {
            Dictionary<String, String> setMap = new Dictionary<String, String>();
            Model.DeviceConfigInfo config = null;
            if (devType == "燃气智能监测终端")
            {
                config = new BLL.DeviceConfig().GetDeviceConfByDeviceCodeAndSensorCode(
                     deviceId, "000044");
                List<Model.DeviceConfigInfo> configs = new List<DeviceConfigInfo>();
                if (config != null)
                {
                    configs.Add(config);
                }
                List<Model.DeviceConfigInfo> validConfigs =
                    new BLL.DeviceConfig().removeTimeoutConfig(configs);
                if (validConfigs != null&&validConfigs.Count!=0)
                {
                    String content = validConfigs[0].FrameContent;
                    String[] rqParams = content.Split(',');
                    //燃气
                    //String ver = rqParams[0]; //版本号
                    String rep = rqParams[1]; //重传次数
                    String itr = rqParams[2]; //采集间隔
                    String period = rqParams[3]; //上传周期
                    String rq_netoid = rqParams[4]; //网络ID

                    setMap.Add("rq_itr", itr);
                    setMap.Add("rq_rept", rep);
                    setMap.Add("rq_period", period);
                    setMap.Add("rq_netoid", rq_netoid);
                }

            }
            else if (devType == "液位监测仪")
            {
                config = new BLL.DeviceConfig().GetDeviceConfByDeviceCodeAndSensorCode(
                    deviceId, "000034");
                List<Model.DeviceConfigInfo> configs = new List<DeviceConfigInfo>();
                if (config != null)
                {
                    configs.Add(config);
                }
                List<Model.DeviceConfigInfo> validConfigs =
                    new BLL.DeviceConfig().removeTimeoutConfig(configs);
                if (validConfigs != null&&validConfigs.Count!=0)
                {
                    String content = validConfigs[0].FrameContent;
                    String[] slParams = content.Split(',');

                    //液位计
                    //String ver = slParams[0];//版本号
                    String yw_thresh = slParams[1];//报警阈值
                    String yw_netid = slParams[2];//网络ID 0-255
                    String yw_rept = slParams[3];//重传次数
                    String yw_height = slParams[4];//高度

                    setMap.Add("yw_thresh", yw_thresh);
                    setMap.Add("yw_netid", yw_netid);
                    setMap.Add("yw_rept", yw_rept);
                    setMap.Add("yw_height", yw_height);
                }

            }

            else if (devType == "温度压力监测仪")
            {
                config = new BLL.DeviceConfig().GetDeviceConfByDeviceCodeAndSensorCode(
                    deviceId, "000050");
                List<Model.DeviceConfigInfo> configs = new List<DeviceConfigInfo>();
                if (config != null)
                {
                    configs.Add(config);
                }
                List<Model.DeviceConfigInfo> validConfigs =
                    new BLL.DeviceConfig().removeTimeoutConfig(configs);
                if (validConfigs != null && validConfigs.Count != 0)
                {
                    String content = validConfigs[0].FrameContent;
                    String[] slParams = content.Split(',');
                    //温度压力
                    //String ver = slParams[0]; //版本号
                    String itr = slParams[1]; //采集间隔
                    String rep = slParams[2]; //重传次数
                    String cnt = slParams[3]; //采集次数
                    String stime = slParams[4]; //采集开始时间
                    String tmp_netoid = slParams[5]; //网络ID

                    setMap.Add("tmp_itr", itr);
                    setMap.Add("tmp_rept", rep);
                    setMap.Add("tmp_cnt", cnt);
                    setMap.Add("tmp_stime", stime);
                    setMap.Add("tmp_netoid", tmp_netoid);
                }
            }

            else if (devType == "噪声记录仪")
            {
                config = new BLL.DeviceConfig().GetDeviceConfByDeviceCodeAndSensorCode(
                    deviceId, "000032");
                List<Model.DeviceConfigInfo> configs = new List<DeviceConfigInfo>();
                if (config != null)
                {
                    configs.Add(config);
                }
                List<Model.DeviceConfigInfo> validConfigs =
                    new BLL.DeviceConfig().removeTimeoutConfig(configs);
                if (validConfigs != null && validConfigs.Count != 0)
                {
                    String content = validConfigs[0].FrameContent;
                    String[] slParams = content.Split(',');
                    //噪声
                    //String ver = slParams[0];//版本号
                    String stime = slParams[1];//获取采集开始时间
                    String itrl = slParams[2];//采集间隔
                    String cnt = slParams[3];//采集次数
                    String rept = slParams[4];//重试次数
                    String sl_netid = slParams[5];//网络ID

                    setMap.Add("sl_stime", stime);
                    setMap.Add("sl_itrl", itrl);
                    setMap.Add("sl_cnt", cnt);
                    setMap.Add("sl_rept", rept);
                    setMap.Add("sl_netid", sl_netid);
                }
            }

            CasicSender sender = new CasicSender(null);
            sender.doSendCasicConfig(deviceId, version, devType, setMap, session,"3");
        }

    }
}
