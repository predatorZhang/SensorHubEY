using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Servers.Commands;
using System.Timers;
using SensorHub.BLL;
using SensorHub.Utility;
using SensorHub.Servers.Commands.CASICCommands;
using System.Threading;

namespace SensorHub.Servers
{
    /*
     * 航天二院最新服务器协议
     */
    public class CasicSender 
    {
        private CasicServer server;

        public CasicSender(CasicServer server)
        {
            this.server = server;
        }

        public Int16 getPduType(String devCode)
        {
            String devType = devCode.Substring(0,2);
            if (devType == "11")
            { 
                //液位监测仪
                return 7;
            }
            else if (devType == "21")
            { 
                //噪声记录仪
                return 2;
            }
            else if (devType == "31")
            { 
                //燃气智能监测终端
                return 4;
            }
            else if (devType == "41")
            {
                //井盖传感器
                return 6;

            }
            else if (devType == "52")
            {
                //远传水表
                return 10;

            }
            else if (devType == "81" || devType == "82" || devType == "83")
            {
                //温度压力监测仪
                return 8;
            }
            else if (devType == "91")
            {
                //腐蚀速率监测仪
                return 11;
            }
            else if (devType == "92")
            {
                //腐蚀速率监测仪
                return 12;
            }
            else
            {
                return 0;
            }
        }

        public void SendGetDataReq(Dictionary<String, String> maps, Model.CasicFireEvent fireEvent)
        {
            try
            {
                foreach (CasicSession session in server.GetAllSessions())
                {
                    string hubAddr = session.HubAddr;
                    foreach (KeyValuePair<String, String> kvp in maps)
                    {
                        if (kvp.Value == hubAddr)
                        {
                            String devCode = kvp.Key;
                      
                            String preamble = "A3";
                            byte btpreamble = byte.Parse(preamble, System.Globalization.NumberStyles.HexNumber);

                            String version = fireEvent.Version;
                            byte btVersion = byte.Parse(version, System.Globalization.NumberStyles.HexNumber);

                            String deviceId = devCode;//6个字节
                            byte[] btDevId = new byte[6];
                            btDevId[0] = byte.Parse(deviceId.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[1] = byte.Parse(deviceId.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[2] = byte.Parse(deviceId.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[3] = byte.Parse(deviceId.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[4] = byte.Parse(deviceId.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[5] = byte.Parse(deviceId.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
                            
                            String routeFlag = "1";
                            byte btRouteFlag = byte.Parse(routeFlag, System.Globalization.NumberStyles.HexNumber);

                            String dstNodeAddr = devCode.Substring(8);//2个字节 ** ****** ****
                            byte[] btDstNode = new byte[2];
                            btDstNode[0] = byte.Parse(dstNodeAddr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            btDstNode[1] = byte.Parse(dstNodeAddr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

                            Int16 btpduType = this.getPduType(devCode);
                            Int16 opearType = (Int16)(btpduType & 0x7F);
                            Int16 pdu = (Int16)(4992 + opearType); //0X1380
                            byte[] btPdu0 = BitConverter.GetBytes(pdu);
                            byte[] btPdu = { btPdu0[1], btPdu0[0] };

                            String seq = "1";
                            byte btSeq = byte.Parse(seq, System.Globalization.NumberStyles.HexNumber);

                            byte[] tag = { 0x60, 0x00, 0x02, 0x00, 0x00, 0x01, 0x01 };

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

                            session.Logger.Info("噪声设备下发GetData：" + "设备编号：" + deviceId + "" + BitConverter.ToString(afcrc));
                            session.Send(afcrc, 0, afcrc.Length);

                            //由于集中器无法立即接收全部数据
                            int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GET_DATA_ITR"]);
                            Thread.Sleep(configItr);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                String ss = e.Message;
            }
           
        }

        public void SendGetRealTimeDataReq(Dictionary<string, string> maps)
        {

            foreach (CasicSession session in server.GetAllSessions())
            {
                string hubAddr = session.HubAddr;
                foreach (KeyValuePair<String, String> kvp in maps)
                {
                    if (kvp.Value == hubAddr)
                    {
                        try
                        {
                            String devCode = kvp.Key;
                            String preamble = "A3";//0xA3
                            byte btpreamble = byte.Parse(preamble, System.Globalization.NumberStyles.HexNumber);

                            String version = "20";
                            byte btVersion = byte.Parse(version, System.Globalization.NumberStyles.HexNumber);

                            String deviceId = devCode;//6个字节
                            byte[] btDevId = new byte[6];
                            btDevId[0] = byte.Parse(deviceId.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[1] = byte.Parse(deviceId.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[2] = byte.Parse(deviceId.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[3] = byte.Parse(deviceId.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[4] = byte.Parse(deviceId.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[5] = byte.Parse(deviceId.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);

                            String routeFlag = "1";//ZHANGFAN
                            byte btRouteFlag = byte.Parse(routeFlag, System.Globalization.NumberStyles.HexNumber);
                            
                            String dstNodeAddr = devCode.Substring(8);//2个字节 ** ****** ****
                            byte[] btDstNode = new byte[2];
                            btDstNode[0] = byte.Parse(dstNodeAddr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            btDstNode[1] = byte.Parse(dstNodeAddr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

                            Int16 btpduType = this.getPduType(devCode);
                            Int16 opearType = (Int16)(btpduType & 0x7F);
                            Int16 pdu = (Int16)(384 + opearType); //0X0180
                            byte[] btPdu0 = BitConverter.GetBytes(pdu);
                            byte[] btPdu = { btPdu0[1], btPdu0[0] };

                            //A3-20-00-10-52-20-16-04-50-13-01-50-13-
                            //01-8A-
                            //01-20-00-00-01-75-75

                            String seq = "1";
                            byte btSeq = byte.Parse(seq, System.Globalization.NumberStyles.HexNumber);

                            //"0x20000001"oid所有设备一样
                            byte[] oidList = { 0x20, 0x00, 0x00, 0x01 };//
                            
                            byte[] totoalLen = { 0x00, 0x10 };

                            //wrap the whole data
                            byte[] result = new byte[1 + 1 + 2 + 6 + 1 + 2 + 2 + 1 + 4];
                            result[0] = btpreamble;
                            result[1] = btVersion;
                            totoalLen.CopyTo(result, 2);
                            btDevId.CopyTo(result, 4);
                            result[10] = btRouteFlag;
                            btDstNode.CopyTo(result, 11);
                            btPdu.CopyTo(result, 13);
                            result[15] = btSeq;
                            oidList.CopyTo(result, 16);

                            //增加CRC校验
                            String strCrc = StringUtil.To16HexString(String.Format("{0:X}", (int)CodeUtils.CRC16_AD(result)));
                            byte[] btcrc = { CodeUtils.String2Byte(strCrc.Substring(0, 2)), CodeUtils.String2Byte(strCrc.Substring(2, 2)) };
                            byte[] afcrc = new byte[result.Length + 2];
                            result.CopyTo(afcrc, 0);
                            btcrc.CopyTo(afcrc, result.Length);

                            session.Logger.Info("实时监测数据查看下发GetRequest：" + "设备编号：" + deviceId + "，" + BitConverter.ToString(afcrc));
                            session.Send(afcrc, 0, afcrc.Length);
                            new BLL.DevHub().setRealSerachDevStatus(devCode);
                            //由于集中器无法立即接收全部数据
                            int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GET_DATA_ITR"]);
                            Thread.Sleep(configItr);
                        }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }
                        
                    }
                }
            }
        }

        public void sendWakeUpCmd(Dictionary<String, String> maps,Model.CasicFireEvent fireEvent)
        {
            try
            {
                foreach (CasicSession session in server.GetAllSessions())
                {
                    string hubAddr = session.HubAddr;
                    foreach (KeyValuePair<String, String> kvp in maps)
                    {
                        if (kvp.Value == hubAddr)
                        {
                            String devCode = kvp.Key;
                            //TODO: construct the receving casic data

                            String preamble = "A3";
                            byte btpreamble = byte.Parse(preamble, System.Globalization.NumberStyles.HexNumber);

                            String version = fireEvent.Version;
                            byte btVersion = byte.Parse(version, System.Globalization.NumberStyles.HexNumber);

                            String deviceId = devCode;//6个字节
                            byte[] btDevId = new byte[6];
                            btDevId[0] = byte.Parse(deviceId.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[1] = byte.Parse(deviceId.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[2] = byte.Parse(deviceId.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[3] = byte.Parse(deviceId.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[4] = byte.Parse(deviceId.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
                            btDevId[5] = byte.Parse(deviceId.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);

                            String routeFlag = "1";
                            byte btRouteFlag = byte.Parse(routeFlag, System.Globalization.NumberStyles.HexNumber);

                            String dstNodeAddr = devCode.Substring(8);//2个字节 ** ****** ****
                            byte[] btDstNode = new byte[2];
                            btDstNode[0] = byte.Parse(dstNodeAddr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            btDstNode[1] = byte.Parse(dstNodeAddr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

                            Int16 btpduType = this.getPduType(devCode);
                            Int16 opearType = (Int16)(btpduType & 0x7F);
                            Int16 pdu = (Int16)(2688 + opearType); //0X0A80
                            byte[] btPdu0 = BitConverter.GetBytes(pdu);
                            byte[] btPdu = { btPdu0[1], btPdu0[0] };

                            String seq = "1";
                            byte btSeq = byte.Parse(seq, System.Globalization.NumberStyles.HexNumber);

                            byte[] tag = { 0x60, 0x00, 0x02, 0x00, 0x00, 0x01, 0x01 };

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

                            session.Logger.Info("下发Wakeup唤醒信息：" + "设备编号：" + deviceId + "" + BitConverter.ToString(afcrc));
                            session.Send(afcrc, 0, afcrc.Length);

                            //由于集中器无法立即接收全部数据
                            int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DEVICE_ITR"]);
                            Thread.Sleep(configItr);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                String ss = e.Message;
            }
           
        }

        public void sendWaterMeterConfig(List<Model.DeviceConfigInfo> configs, Dictionary<String, String> maps)
        {
            foreach (CasicSession session in server.GetAllSessions())
            {
                string hubAddr = session.HubAddr;

                foreach (Model.DeviceConfigInfo config in configs)
                {
                    string devCode = config.DeviceCode;
                    if (maps.ContainsKey(devCode) && maps[devCode] == hubAddr)
                    {
                        String content = config.FrameContent;
                        String[] slParams = content.Split(',');
                        //远传水表
                        String ver = slParams[0]; //版本号
                        String water_netid = slParams[1]; //网络ID

                        Dictionary<String, String> settings = new Dictionary<String, String>();
                        settings.Add("water_netid", water_netid);

                        this.doSendCasicConfig(devCode, ver, "远传水表", settings, session,"1");

                        //由于集中器无法立即接收全部数据
                        int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DEVICE_ITR"]);
                        Thread.Sleep(configItr);
                    }
                }
            }

        }

        public void sendTempAndPressConfig(List<Model.DeviceConfigInfo> configs, Dictionary<String, String> maps)
        {
            foreach (CasicSession session in server.GetAllSessions())
            {
                string hubAddr = session.HubAddr;

                foreach (Model.DeviceConfigInfo config in configs)
                {
                    string devCode = config.DeviceCode;
                    if (maps.ContainsKey(devCode) && maps[devCode] == hubAddr)
                    {
                        String content = config.FrameContent;
                        String[] slParams = content.Split(',');
                        //温度压力
                        String ver = slParams[0]; //版本号
                        String itr = slParams[1]; //采集间隔
                        String rep = slParams[2]; //重传次数
                        String cnt = slParams[3]; //采集次数
                        String stime = slParams[4]; //采集开始时间
                        String tmp_netoid = slParams[5]; //网络ID

                        Dictionary<String, String> settings = new Dictionary<String, String>();
                        settings.Add("tmp_itr", itr);
                        settings.Add("tmp_rept", rep);
                        settings.Add("tmp_cnt", cnt);
                        settings.Add("tmp_stime", stime);
                        settings.Add("tmp_netoid", tmp_netoid);

                        this.doSendCasicConfig(devCode, ver, "温度压力监测仪", settings, session,"1");

                        //由于集中器无法立即接收全部数据
                        int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DEVICE_ITR"]);
                        Thread.Sleep(configItr);
                    }
                }
            }

        }

        public void sendRqConfig(List<Model.DeviceConfigInfo> configs, Dictionary<String, String> maps)
        {
            foreach (CasicSession session in server.GetAllSessions())
            {
                string hubAddr = session.HubAddr;

                foreach (Model.DeviceConfigInfo config in configs)
                {
                    string devCode = config.DeviceCode;
                    if (maps.ContainsKey(devCode) && maps[devCode] == hubAddr)
                    {
                        String content = config.FrameContent;
                        String[] rqParams = content.Split(',');
                        //燃气
                        String ver = rqParams[0]; //版本号
                        String rep = rqParams[1]; //重传次数
                        String itr = rqParams[2]; //采集间隔
                        String period = rqParams[3]; //上传周期
                        String rq_netoid = rqParams[4]; //网络ID

                        Dictionary<String, String> settings = new Dictionary<String, String>();
                        settings.Add("rq_itr", itr);
                        settings.Add("rq_rept", rep);
                        settings.Add("rq_period", period);
                        settings.Add("rq_netoid", rq_netoid);

                        this.doSendCasicConfig(devCode, ver, "燃气智能监测终端", settings, session,"1");

                        //由于集中器无法立即接收全部数据
                        int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DEVICE_ITR"]);
                        Thread.Sleep(configItr);
                    }
                }
            }

        }

        public void sendSLConfig(List<Model.DeviceConfigInfo> configs, Dictionary<String, String> maps)
        {
            foreach (CasicSession session in server.GetAllSessions())
            {
                string hubAddr = session.HubAddr;
             
                foreach (Model.DeviceConfigInfo config in configs)
                {
                    string devCode = config.DeviceCode;
                    if (maps.ContainsKey(devCode) && maps[devCode] == hubAddr)
                    {
                        String content = config.FrameContent;
                        String[] slParams = content.Split(',');
                        //噪声
                        String ver = slParams[0];//版本号
                        String stime = slParams[1];//获取采集开始时间
                        String itrl = slParams[2];//采集间隔
                        String cnt = slParams[3];//采集次数
                        String rept = slParams[4];//重试次数
                        String sl_netid = slParams[5];//网络ID

                        Dictionary<String, String> setMap = new Dictionary<String, String>();
                        setMap.Add("sl_stime",stime);
                        setMap.Add("sl_itrl",itrl);
                        setMap.Add("sl_cnt",cnt);
                        setMap.Add("sl_rept",rept);
                        setMap.Add("sl_netid", sl_netid);

                        this.doSendCasicConfig(devCode, ver, "噪声记录仪", setMap, session,"1");
                        
                        //由于集中器无法立即接收全部数据
                        int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DEVICE_ITR"]);
                        Thread.Sleep(configItr);
                    }
                }
            }
       
        }

        public void sendFSSLConfig(List<Model.DeviceConfigInfo> configs, Dictionary<string, string> maps)
        {
            foreach (CasicSession session in server.GetAllSessions())
            {
                string hubAddr = session.HubAddr;

                foreach (Model.DeviceConfigInfo config in configs)
                {
                    string devCode = config.DeviceCode;
                    if (maps.ContainsKey(devCode) && maps[devCode] == hubAddr)
                    {
                        String content = config.FrameContent;
                        String[] slParams = content.Split(',');
                        String ver = slParams[0];

                        String rept = slParams[1];
                        String stime = slParams[2];
                        String itrl = slParams[3];
                       
                        Dictionary<String, String> setMap = new Dictionary<String, String>();
                        setMap.Add("fssl_stime", stime);
                        setMap.Add("fssl_itrl", itrl);
                        setMap.Add("fssl_rept", rept);

                        this.doSendCasicConfig(devCode, ver, "保温层下腐蚀速率监测仪", setMap, session,"1");

                        //由于集中器无法立即接收全部数据
                        int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DEVICE_ITR"]);
                        Thread.Sleep(configItr);
                    }
                }
            }
        }

        public void sendFSHJConfig(List<Model.DeviceConfigInfo> configs, Dictionary<string, string> maps)
        {
            foreach (CasicSession session in server.GetAllSessions())
            {
                string hubAddr = session.HubAddr;

                foreach (Model.DeviceConfigInfo config in configs)
                {
                    string devCode = config.DeviceCode;
                    if (maps.ContainsKey(devCode) && maps[devCode] == hubAddr)
                    {
                        String content = config.FrameContent;
                        String[] slParams = content.Split(',');
                        String ver = slParams[0];

                        String rept = slParams[1];
                        String stime = slParams[2];
                        String itrl = slParams[3];

                        Dictionary<String, String> setMap = new Dictionary<String, String>();
                        setMap.Add("fshj_stime", stime);
                        setMap.Add("fshj_itrl", itrl);
                        setMap.Add("fshj_rept", rept);

                        this.doSendCasicConfig(devCode, ver, "保温层下腐蚀环境监测仪", setMap, session,"1");

                        //由于集中器无法立即接收全部数据
                        //TODO LIST:定期会监测数据库，下发配置信息
                        int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DEVICE_ITR"]);
                        Thread.Sleep(configItr);
                    }
                }
            }
        }

        //发送液位配置信息
        public void sendYWConfig(List<Model.DeviceConfigInfo> configs, Dictionary<string, string> maps)
        {
            foreach (CasicSession session in server.GetAllSessions())
            {
                string hubAddr = session.HubAddr;

                foreach (Model.DeviceConfigInfo config in configs)
                {
                    string devCode = config.DeviceCode;
                    if (maps.ContainsKey(devCode) && maps[devCode] == hubAddr)
                    {
                        String content = config.FrameContent;
                        String[] slParams = content.Split(',');

                        //液位计
                        String ver = slParams[0];//版本号
                        String yw_thresh = slParams[1];//报警阈值
                        String yw_netid = slParams[2];//网络ID 0-255
                        String yw_rept = slParams[3];//重传次数
                        String yw_height = slParams[4];//高度
                        
                        Dictionary<String, String> setMap = new Dictionary<String, String>();
                        setMap.Add("yw_thresh", yw_thresh);
                        setMap.Add("yw_netid", yw_netid);
                        setMap.Add("yw_rept", yw_rept);
                        setMap.Add("yw_height", yw_height);
                        this.doSendCasicConfig(devCode, 
                            ver, "液位监测仪", setMap, session,"1");
                        int configItr = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DEVICE_ITR"]);
                        Thread.Sleep(configItr);
                       
                    }
                }
            }
        }

        public bool doSendCasicConfig(String devCode,String ver, String typeName,
        Dictionary<String, String> settings,CasicSession session,string flag)
        {
            //TODO: construct the receving casic data
            String preamble = "A3";
            byte btpreamble = byte.Parse(preamble, System.Globalization.NumberStyles.HexNumber);

            String version = ver;
            byte btVersion = byte.Parse(version, System.Globalization.NumberStyles.HexNumber);

            String deviceId = devCode;//6个字节
            byte[] btDevId = new byte[6];
            btDevId[0] = byte.Parse(deviceId.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[1] = byte.Parse(deviceId.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[2] = byte.Parse(deviceId.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[3] = byte.Parse(deviceId.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[4] = byte.Parse(deviceId.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            btDevId[5] = byte.Parse(deviceId.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);

            String routeFlag = flag;
            byte btRouteFlag = byte.Parse(routeFlag, System.Globalization.NumberStyles.HexNumber);

            String dstNodeAddr = devCode.Substring(8);//2个字节 ** ****** ****
            byte[] btDstNode = new byte[2];
            btDstNode[0] = byte.Parse(dstNodeAddr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            btDstNode[1] = byte.Parse(dstNodeAddr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

            String seq = "1";
            byte btSeq = byte.Parse(seq, System.Globalization.NumberStyles.HexNumber);

            byte[] btPdu = new byte[2]; //2个字节
            byte[] tag = null;

            switch (typeName)
            {
                case "噪声记录仪":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x82;
                    tag = this.getNosieTag(settings);
                    break;
                case "保温层下腐蚀速率监测仪":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x8B;
                    tag = this.getFSSLTag(settings);
                    break;
                case "保温层下腐蚀环境监测仪":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x8C;
                    tag = this.getFSHJTag(settings);
                    break;
                case "液位监测仪":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x87;
                    tag = this.getYWTag(settings);
                    break;
                case "温度压力监测仪":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x88;
                    tag = this.getTempTag(settings);
                    break;
                case "燃气智能监测终端":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x84;
                    tag = this.getRQTag(settings);
                    break;
                case "远传水表":
                    btPdu[0] = 0x03;
                    btPdu[1] = 0x8A;
                    tag = this.getWaterMeterTag(settings);
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

            //增加CRC校验
            String strCrc = StringUtil.To16HexString(String.Format("{0:X}", (int)CodeUtils.CRC16_AD(result)));
            byte[] btcrc = { CodeUtils.String2Byte(strCrc.Substring(0, 2)), CodeUtils.String2Byte(strCrc.Substring(2, 2)) };
            byte[] afcrc = new byte[result.Length + 2];
            result.CopyTo(afcrc, 0);
            btcrc.CopyTo(afcrc, result.Length);

            //TODO:发送数据
            session.Logger.Info("设备类型："+typeName+" 设备编号:"+devCode+" 自动下发配置信息：" + BitConverter.ToString(afcrc));
            session.Send(afcrc, 0, afcrc.Length);

            //TODO LIST:当前发送次数+1; devCode
            new BLL.DeviceConfig().incRetryCount(devCode);
            return true;
        }

        public byte[] getWaterMeterTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            byte[] result = new byte[getWaterMeterTagCount(settings)];

            if (settings.ContainsKey("water_netid"))
            {
                //网络ID0x10001001
                byte[] water_oid = { 0x10, 0x00, 0x10, 0x01, 0x00, 0x01 };
                String water_netid = settings["water_netid"];
                byte rept = (Convert.ToByte(water_netid));
                byte[] setting_water_netid = new byte[7];
                water_oid.CopyTo(setting_water_netid, 0);
                setting_water_netid[6] = rept;
                setting_water_netid.CopyTo(result, flag);
                flag = flag + 7;
            }
            
            SystemTimeConfig sysTimeConfig = new SystemTimeConfig(null);
            byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
            btConfig.CopyTo(result, flag);

            return result;

        }

        private int getWaterMeterTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;

            if (settings.ContainsKey("water_netid"))
            {
                flag = flag + 7;
            }
            flag = flag + 12;//校时
            return flag;
        }

        public byte[] getRQTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            byte[] result = new byte[getRQTagCount(settings)];
            if (settings.ContainsKey("rq_itr"))
            {
                //采集间隔
                byte[] yw_oid2 = { 0x10, 0x00, 0x01, 0x05, 0x00, 0x02 };
                String yw_itr = settings["rq_itr"];
                Int16 yw_sitr1 = (Convert.ToInt16(yw_itr));
                byte[] yw_bsitr1 = BitConverter.GetBytes(yw_sitr1);
                byte[] setting_yw_itr = new byte[8];
                yw_oid2.CopyTo(setting_yw_itr, 0);
                setting_yw_itr[6] = yw_bsitr1[1];
                setting_yw_itr[7] = yw_bsitr1[0];
                setting_yw_itr.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("rq_rept"))
            {
                //重试次数
                String sl_rept = settings["rq_rept"];
                byte[] oid4 = { 0x10, 0x00, 0x00, 0x0A, 0x00, 0x01 };
                byte rept = (Convert.ToByte(sl_rept));
                //byte[] brept = BitConverter.GetBytes(rept);
                byte[] setting_sl_rept = new byte[7];
                oid4.CopyTo(setting_sl_rept, 0);
                setting_sl_rept[6] = rept;
                setting_sl_rept.CopyTo(result, flag);
                flag = flag + 7;
            }

            if (settings.ContainsKey("rq_period"))
            {
                //周期
                String yw_per = settings["rq_period"];
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

            if (settings.ContainsKey("rq_netid"))
            {
                //网络ID0x10001001
                byte[] rq_oid = { 0x10, 0x00, 0x10, 0x01, 0x00, 0x01 };
                String rq_netid = settings["rq_netid"];
                byte rept = (Convert.ToByte(rq_netid));
                byte[] setting_rq_netid = new byte[7];
                rq_oid.CopyTo(setting_rq_netid, 0);
                setting_rq_netid[6] = rept;
                setting_rq_netid.CopyTo(result, flag);
                flag = flag + 7;
            }
            SystemTimeConfig sysTimeConfig = new SystemTimeConfig(null);
            byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
            btConfig.CopyTo(result, flag);

            return result;

        }

        private int getRQTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;

            if (settings.ContainsKey("rq_itr"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("rq_rept"))
            {
                flag = flag + 7;
            }

            if (settings.ContainsKey("rq_period"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("rq_netid"))
            {
                flag = flag + 7;
            } 
            flag = flag + 12;//校时
            return flag;
        }

        /**
      * 获取温度数据的tag信息
      * **/
        private byte[] getTempTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            byte[] result = new byte[this.getTempTagCount(settings)];

            if (settings.ContainsKey("tmp_itr"))
            {
                //采集间隔
                byte[] tmp_oid2 = { 0x10, 0x00, 0x01, 0x05, 0x00, 0x02 };
                String tmp_itr = settings["tmp_itr"];
                Int16 tmp_sitr1 = (Convert.ToInt16(tmp_itr));
                byte[] tmp_bsitr1 = BitConverter.GetBytes(tmp_sitr1);
                byte[] setting_tmp_itr = new byte[8];
                tmp_oid2.CopyTo(setting_tmp_itr, 0);
                setting_tmp_itr[6] = tmp_bsitr1[1];
                setting_tmp_itr[7] = tmp_bsitr1[0];
                setting_tmp_itr.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("tmp_rept"))
            {
                //重试次数
                String tmp_rept = settings["tmp_rept"];
                byte[] oid4 = { 0x10, 0x00, 0x00, 0x0A, 0x00, 0x01 };
                byte rept = (Convert.ToByte(tmp_rept));
                byte[] setting_tmp_rept = new byte[7];
                oid4.CopyTo(setting_tmp_rept, 0);
                setting_tmp_rept[6] = rept;
                setting_tmp_rept.CopyTo(result, flag);
                flag = flag + 7;
            }
            if (settings.ContainsKey("tmp_cnt"))
            {
                //采集次数
                String tmp_cnt = settings["tmp_cnt"];
                byte[] tmp_oid3 = { 0x10, 0x00, 0x01, 0x06, 0x00, 0x02 };
                Int16 tmp_scnt1 = (Convert.ToInt16(tmp_cnt));
                byte[] tmp_bscnt1 = BitConverter.GetBytes(tmp_scnt1);
                byte[] setting_tmp_cnt = new byte[8];
                tmp_oid3.CopyTo(setting_tmp_cnt, 0);
                setting_tmp_cnt[6] = tmp_bscnt1[1];
                setting_tmp_cnt[7] = tmp_bscnt1[0];
                setting_tmp_cnt.CopyTo(result, flag);
                flag = flag + 8;
            }

            //获取采集开始时间
            if (settings.ContainsKey("tmp_stime"))
            {
                byte[] oid1 = { 0x10, 0x00, 0x01, 0x04, 0x00, 0x02 };
                String tmp_stime = settings["tmp_stime"];
                Int16 stime1 = (Int16)(Convert.ToInt16(tmp_stime.Substring(0, 2)) * 60 + Convert.ToInt16(tmp_stime.Substring(3, 2)));
                byte[] btime1 = BitConverter.GetBytes(stime1);
                byte[] setting_tmp_stime = new byte[8];
                oid1.CopyTo(setting_tmp_stime, 0);
                setting_tmp_stime[6] = btime1[1];
                setting_tmp_stime[7] = btime1[0];
                setting_tmp_stime.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("tmp_netid"))
            {
                //网络ID0x10001001
                byte[] tmp_oid1 = { 0x10, 0x00, 0x10, 0x01, 0x00, 0x01 };
                String tmp_netid = settings["tmp_netid"];
                byte rept = (Convert.ToByte(tmp_netid));
                byte[] setting_tmp_netid = new byte[7];
                tmp_oid1.CopyTo(setting_tmp_netid, 0);
                setting_tmp_netid[6] = rept;
                setting_tmp_netid.CopyTo(result, flag);
                flag = flag + 7;
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
            if (settings.ContainsKey("tmp_itr"))
            {
                flag = flag + 8;
            }
            if (settings.ContainsKey("tmp_rept"))
            {
                flag = flag + 7;
            }
            if (settings.ContainsKey("tmp_cnt"))
            {
                flag = flag + 8;
            }
            if (settings.ContainsKey("tmp_stime"))
            {
                flag = flag + 8;
            }
            if (settings.ContainsKey("tmp_netid"))
            {
                flag = flag + 7;
            }
            flag = flag + 12;
            return flag;
        }


        /**
         * 获取液位数据的tag信息
         * **/
        private byte[] getYWTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            byte[] result = new byte[getYWTagCount(settings)];

            if (settings.ContainsKey("yw_thresh"))
            {
                //阈值0x10000901
                byte[] yw_oid0 = { 0x10, 0x00, 0x09, 0x01, 0x00, 0x04 };
                String yw_thresh = settings["yw_thresh"];
                float height = (Convert.ToSingle(yw_thresh));
                byte[] bHeight = BitConverter.GetBytes(height);
                byte[] setting_yw_thresh = new byte[10];
                yw_oid0.CopyTo(setting_yw_thresh, 0);
                setting_yw_thresh[6] = bHeight[0];
                setting_yw_thresh[7] = bHeight[1];
                setting_yw_thresh[8] = bHeight[2];
                setting_yw_thresh[9] = bHeight[3];
                setting_yw_thresh.CopyTo(result, flag);
                flag = flag + 10;
            }
            if (settings.ContainsKey("yw_netid"))
            {
                //网络ID0x10001001
                byte[] yw_oid1 = { 0x10, 0x00, 0x10, 0x01, 0x00, 0x01 };
                String yw_netid = settings["yw_netid"];
                byte rept = (Convert.ToByte(yw_netid));
                byte[] setting_yw_netid = new byte[7];
                yw_oid1.CopyTo(setting_yw_netid, 0);
                setting_yw_netid[6] = rept;
                setting_yw_netid.CopyTo(result, flag);
                flag = flag + 7;
            }
            if (settings.ContainsKey("yw_rept"))
            {
                //重试次数
                String yw_rept = settings["yw_rept"];
                byte[] oid4 = { 0x10, 0x00, 0x00, 0x0A, 0x00, 0x01 };
                byte rept = (Convert.ToByte(yw_rept));
                byte[] setting_yw_rept = new byte[7];
                oid4.CopyTo(setting_yw_rept, 0);
                setting_yw_rept[6] = rept;
                setting_yw_rept.CopyTo(result, flag);
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

            if (settings.ContainsKey("yw_thresh"))
            {
                flag = flag + 10;

            }

            if (settings.ContainsKey("yw_netid"))
            {
                flag = flag + 7;
            }

            if (settings.ContainsKey("yw_rept"))
            {
                flag = flag + 7;
            }

            if (settings.ContainsKey("yw_height"))
            {
                flag = flag + 10;
            }
            flag = flag + 12;//校时
            return flag;
        }

        /**
        * 获取腐蚀环境数据的tag信息
        * **/
        private byte[] getFSHJTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            int cc = getFSHJTagCount(settings);
            byte[] result = new byte[cc];

            SystemTimeConfig sysTimeConfig = new SystemTimeConfig(null);
            byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
            btConfig.CopyTo(result, flag);
            flag = flag + 12;

            //获取采集开始时间
            if (settings.ContainsKey("fshj_stime"))
            {
                byte[] oid1 = { 0x10, 0x00, 0x01, 0x04, 0x00, 0x02 };
                String sl_stime = settings["fshj_stime"];
                Int16 stime1 = (Int16)(Convert.ToInt16(sl_stime.Substring(0, 2)) * 60 + Convert.ToInt16(sl_stime.Substring(3, 2)));
                byte[] btime1 = BitConverter.GetBytes(stime1);
                byte[] setting_sl_stime = new byte[8];
                oid1.CopyTo(setting_sl_stime, 0);
                setting_sl_stime[6] = btime1[1];
                setting_sl_stime[7] = btime1[0];
                setting_sl_stime.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("fshj_itrl"))
            {
                //采集间隔
                byte[] oid2 = { 0x10, 0x00, 0x01, 0x05, 0x00, 0x02 };
                String sl_itr = settings["fshj_itrl"];
                Int16 sitr1 = (Convert.ToInt16(sl_itr));
                byte[] bsitr1 = BitConverter.GetBytes(sitr1);
                byte[] setting_sl_itr = new byte[8];
                oid2.CopyTo(setting_sl_itr, 0);
                setting_sl_itr[6] = bsitr1[1];
                setting_sl_itr[7] = bsitr1[0];
                setting_sl_itr.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("fshj_rept"))
            {
                //重试次数
                String sl_rept = settings["fshj_rept"];
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
         * 获取腐蚀环境配置个数
         * **/
        private int getFSHJTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;
            if (settings.ContainsKey("fshj_stime"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("fshj_itrl"))
            {
                flag = flag + 8;

            }

            if (settings.ContainsKey("fshj_rept"))
            {
                flag = flag + 8;
            }
            flag = flag + 12;//校时
            return flag;
        }
        
        /**
     * 获取腐蚀渗漏数据的tag信息
     * **/
        private byte[] getFSSLTag(Dictionary<String, String> settings)
        {
            int flag = 0;
            int cc = getFSSLTagCount(settings);
            byte[] result = new byte[cc];

            SystemTimeConfig sysTimeConfig = new SystemTimeConfig(null);
            byte[] btConfig = sysTimeConfig.getConfig(new byte[0]);
            btConfig.CopyTo(result, flag);
            flag = flag + 12;

            //获取采集开始时间
            if (settings.ContainsKey("fssl_stime"))
            {
                byte[] oid1 = { 0x10, 0x00, 0x01, 0x04, 0x00, 0x02 };
                String sl_stime = settings["fssl_stime"];
                Int16 stime1 = (Int16)(Convert.ToInt16(sl_stime.Substring(0, 2)) * 60 + Convert.ToInt16(sl_stime.Substring(3, 2)));
                byte[] btime1 = BitConverter.GetBytes(stime1);
                byte[] setting_sl_stime = new byte[8];
                oid1.CopyTo(setting_sl_stime, 0);
                setting_sl_stime[6] = btime1[1];
                setting_sl_stime[7] = btime1[0];
                setting_sl_stime.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("fssl_itrl"))
            {
                //采集间隔
                byte[] oid2 = { 0x10, 0x00, 0x01, 0x05, 0x00, 0x02 };
                String sl_itr = settings["fssl_itrl"];
                Int16 sitr1 = (Convert.ToInt16(sl_itr));
                byte[] bsitr1 = BitConverter.GetBytes(sitr1);
                byte[] setting_sl_itr = new byte[8];
                oid2.CopyTo(setting_sl_itr, 0);
                setting_sl_itr[6] = bsitr1[1];
                setting_sl_itr[7] = bsitr1[0];
                setting_sl_itr.CopyTo(result, flag);
                flag = flag + 8;
            }

            if (settings.ContainsKey("fssl_rept"))
            {
                //重试次数
                String sl_rept = settings["fssl_rept"];
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
         * 获取腐蚀渗漏配置个数
         * **/
        private int getFSSLTagCount(Dictionary<String, String> settings)
        {
            int flag = 0;
            if (settings.ContainsKey("fssl_stime"))
            {
                flag = flag + 8;
            }

            if (settings.ContainsKey("fssl_itrl"))
            {
                flag = flag + 8;

            }

            if (settings.ContainsKey("fssl_rept"))
            {
                flag = flag + 8;
            }
            flag = flag + 12;//校时
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
                setting_sl_cnt.CopyTo(result, flag);
                flag = flag + 8;
            }
            if (settings.ContainsKey("sl_rept"))
            {
                //重试次数
                String sl_rept = settings["sl_rept"];
                byte[] oid4 = { 0x10, 0x00, 0x00, 0x0A, 0x00, 0x01 };
                byte rept = (Convert.ToByte(sl_rept));
                byte[] setting_sl_rept = new byte[7];
                oid4.CopyTo(setting_sl_rept, 0);
                setting_sl_rept[6] = rept;
                setting_sl_rept.CopyTo(result, flag);
                flag = flag + 7;
            }
            if (settings.ContainsKey("sl_netid"))
            {
                //网络ID0x10001001
                byte[] sl_oid5 = { 0x10, 0x00, 0x10, 0x01, 0x00, 0x01 };
                String sl_netid = settings["sl_netid"];
                byte rept = (Convert.ToByte(sl_netid));
                byte[] setting_sl_netid = new byte[7];
                sl_oid5.CopyTo(setting_sl_netid, 0);
                setting_sl_netid[6] = rept;
                setting_sl_netid.CopyTo(result, flag);
                flag = flag + 7;
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
                flag = flag + 7;
            }

            if (settings.ContainsKey("sl_netid"))
            {
                flag = flag + 7;
            }
            flag = flag + 12;//校时
            return flag;
        }

    }
}
