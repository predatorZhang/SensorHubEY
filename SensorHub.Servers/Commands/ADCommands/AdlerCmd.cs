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

namespace SensorHub.Servers.Commands.ADCommands
{
    public class AdlerCmd : CommandBase<AdlerSession, StringRequestInfo>
    {

        public static String currentSystemDate="";
        public static AdlerSession adlerSession=null;
        public static String devCode;

        public override string Name
        {
            get
            {
                return "Adler";
            }
        }

        private String getDeviceTypeByPdu(String pduType) 
        {
            byte btpduType = byte.Parse(pduType, System.Globalization.NumberStyles.HexNumber);
            int opearType = btpduType  & 0x0F;
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
                    devType = "渗漏预警";
                    break;
                case 3:
                    devType = "排水监控一体机";
                    break;
                case 4:
                    devType = "燃气监测设备";
                    break;
                default:
                    devType = "undefiend";
                    break;
            }
            return devType;
        }

        private String getOpeTypeByPdu(String pduType)
        {
            byte btpduType = byte.Parse(pduType, System.Globalization.NumberStyles.HexNumber);
            int opearType = (btpduType >> 4) & 0x07;
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
                default:
                    result = "undefined";
                    break;
            }
            return result;
        }

        private bool isfinishe(String pduType)
        {
            byte btpduType = byte.Parse(pduType, System.Globalization.NumberStyles.HexNumber);
            int flag = btpduType & 0x80;
            return flag == 128 ? true : false; 
        }

        private List<Tag> getTags(String strTags)
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
                throw;
            }
        
            return tags;
        }

        public override void ExecuteCommand(AdlerSession session, StringRequestInfo requestInfo)
        {

            adlerSession = session;

            String preamble = requestInfo.Parameters[0];
            String version = requestInfo.Parameters[1];
            String leng = requestInfo.Parameters[2];
            String deviceId = requestInfo.Parameters[3];
            String pduType = requestInfo.Parameters[4];
            String seq = requestInfo.Parameters[5];
            String settings = requestInfo.Parameters[6]; //减去结尾回车换行符号，减去总的字节数
            String crcs = requestInfo.Parameters[7];

            //print the receving data
            session.Logger.Info("AD接收数据:" + requestInfo.Body);
            session.Logger.Info("当前版本:" + version);
            session.Logger.Info("设备编号:" + deviceId);
            session.Logger.Info("设备类型" + getDeviceTypeByPdu(pduType));
            session.Logger.Info("操作类型"+getOpeTypeByPdu(pduType));

            //update the device code
            devCode = deviceId;

            //upload periodically or config query according to oid
            List<Tag> tags = this.getTags(settings);

            try
            {
                //TODO LIST:处理主动上报的数据、参数查询、参数设置
                TagHandler systemDateHandler = new SystemDateTagHandler();
                TagHandler flowHandler = new FlowTagHandler();
                TagHandler pressHandler = new PressTagHandler();
                TagHandler noiseHandler = new NoiseTagHandler();
                TagHandler liquidHandler = new LiquidTagHandler();
                TagHandler sysTimeHandler = new SystemTimeTagHandler();

                systemDateHandler.NextHandler = flowHandler;
                flowHandler.NextHandler = pressHandler;
                pressHandler.NextHandler = noiseHandler;
                noiseHandler.NextHandler = liquidHandler;
                liquidHandler.NextHandler = sysTimeHandler;

                foreach(Tag tag in tags)
                {
                    //采用责任链的方式来处理各个tag:
                    //目前已经处理噪声、流量、压力、系统日期tag
                    systemDateHandler.handleTag(tag);
                }

                //TODO LIST:必须首先设置SetRequest，并且第一个tag必须是设置系统是时间，相关需要设置的值
                if (isfinishe(pduType))
                {
                    //TODO LIST：查询数据库是否有配置信息，无则下载配置文件中的默认配置信息
                    SensorHub.Model.DeviceConfigInfo conf = (new DeviceConfig()).GetDeviceConfByDeviceCodeAndSensorCode(deviceId, "000034");
                    if (conf == null) 
                    {

                        //TODO LIST:有新的配置项目，按照下面方式追加即可
                      //  byte[] config = ApplicationContext.getInstance().getDefaultADSettting(version,deviceId,pduType,seq);
                        HeadConfig headConfig = new HeadConfig(version, deviceId, pduType, seq);
                        PortConfig portConfig = new PortConfig(headConfig);
                        IpConfig ipConfig = new IpConfig(portConfig);
                        CollectSettingcConfig setConfig = new CollectSettingcConfig(ipConfig);
                        SystemTimeConfig sysTimeConfig = new SystemTimeConfig(setConfig);
                        byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
                        session.Logger.Info("CRC校验前数据：" + BitConverter.ToString(btConfig));
                        
                        //增加CRC校验
                        String strCrc = String.Format("{0:X}", (int)CodeUtils.CRC16_AD(btConfig));
                        byte[] btcrc = { CodeUtils.String2Byte(strCrc.Substring(0, 2)), CodeUtils.String2Byte(strCrc.Substring(2, 2)) };
                        byte[] afcrc = new byte[btConfig.Length + 2];
                        btConfig.CopyTo(afcrc, 0);
                        btcrc.CopyTo(afcrc, btConfig.Length);
                        session.Logger.Info("CRC校验后数据：" + BitConverter.ToString(afcrc));


                        //编码下发：
                        byte[] enCodedConfig = CodeUtils.adEncode(afcrc);
                        session.Send(enCodedConfig, 0, enCodedConfig.Length);
                        session.Logger.Info("编码后下发数据：" + BitConverter.ToString(enCodedConfig));

                        //解码下发数据用于验证
                        byte[] temp = enCodedConfig.CloneRange(0, enCodedConfig.Length - 2);

                        session.Logger.Info("编码前下发数据：" + BitConverter.ToString(CodeUtils.adDecode(temp)));
                    }
                }
            }
            catch (Exception e)
            {
                session.Logger.Error(e.ToString());
            }
        }
    }
}
