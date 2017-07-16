using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using SensorHub.OracleDAL;
namespace SensorHub.Utility
{
    public class ApplicationContext
    {
        private static ApplicationContext ap;

        /*
         * >119.254.110.71;2014!
        **/
        private static string SLIPCONFIG = "SL_BakServer";

        /*
         * SL_Start_Time=02:00
         * SL_DenseData_Interval=3(3分钟上传一次)
         * SL_DenseDate_Num=40(上传40次)
         * SL_LooseData_StartTime=06:00
         * SL_LooseDate_Interval=120（上传间隔120分钟一次）
         * */
        private static string SL_DenseData_StartTime = "SL_DenseData_StartTime";
        private static string SL_DenseData_Interval = "SL_DenseData_Interval";
        private static string SL_DenseData_Num = "SL_DenseData_Num";
        private static string SL_LooseData_StartTime = "SL_LooseData_StartTime";
        private static string SL_LooseDate_Interval = "SL_LooseDate_Interval";

        private ApplicationContext()
        { 
        }
        public static ApplicationContext getInstance()
        {
            if (ap == null)
            {
                ap = new ApplicationContext();
            }
            return ap;
        }

        public  string getSLIpConfig()
        {
            string dst = System.Configuration.ConfigurationSettings.AppSettings[SLIPCONFIG];
            return dst;
        }

        //用于校时
        public byte[] getDefaultLiquidConfigForCALI()
        {

            byte yy = byte.Parse(DateTime.Now.ToString("yy"));
            byte MM = byte.Parse(DateTime.Now.ToString("MM"));
            byte dd = byte.Parse(DateTime.Now.ToString("dd"));
            byte HH = byte.Parse(DateTime.Now.ToString("HH"));
            byte mm = byte.Parse(DateTime.Now.ToString("mm"));
            byte ss = byte.Parse(DateTime.Now.ToString("ss"));
            byte wk = byte.Parse("0" + ((int)DateTime.Now.DayOfWeek).ToString());


            byte[] set2 = { 0x50, 
                                0x00, 0x14,
                                0x01,
                                0x00, 0x00,0x34,
                                0xC2,//C1用于配置，C2用于校时
                                0x00, 0x05,//此数据帧用于校时，采集间隔无效
                                0x01, 0x20,//此数据帧用于校时，发送次数无效
                                ss,  mm,  HH,//秒、分、时
                                wk,  dd,  MM,  yy,//星期、日、月、年
                                0x03};
            return set2;
        }

        public byte[] getDefaultLiquidConfigForUpdate()
        {

            byte yy = byte.Parse(DateTime.Now.ToString("yy"));
            byte MM = byte.Parse(DateTime.Now.ToString("MM"));
            byte dd = byte.Parse(DateTime.Now.ToString("dd"));
            byte HH = byte.Parse(DateTime.Now.ToString("HH"));
            byte mm = byte.Parse(DateTime.Now.ToString("mm"));
            byte ss = byte.Parse(DateTime.Now.ToString("ss"));
            byte wk = byte.Parse("0" + ((int)DateTime.Now.DayOfWeek).ToString());


            byte[] set2 = { 0x50, 
                                0x00, 0x14,
                                0x01,
                                0x00, 0x00,0x34,
                                0xC1,//C1用于配置，C2用于校时
                                0x00, 0x3C,//此数据帧用于校时，采集间隔无效
                                0x00, 0x18,//此数据帧用于校时，发送次数无效
                                ss,  mm,  HH,//秒、分、时
                                wk,  dd,  MM,  yy,//星期、日、月、年
                                0x03};
            return set2;
        }

        public byte[] getDefaultSLConfig()
        {
            byte yy = byte.Parse(DateTime.Now.ToString("yy"));
            byte MM = byte.Parse(DateTime.Now.ToString("MM"));
            byte dd = byte.Parse(DateTime.Now.ToString("dd"));
            byte HH = byte.Parse(DateTime.Now.ToString("HH"));
            byte mm = byte.Parse(DateTime.Now.ToString("mm"));
            byte ss = byte.Parse(DateTime.Now.ToString("ss"));
            byte wk = byte.Parse("0" + ((int)DateTime.Now.DayOfWeek).ToString());

            string denseStartTime = System.Configuration.ConfigurationSettings.AppSettings[SL_DenseData_StartTime];
            string denseInterval = System.Configuration.ConfigurationSettings.AppSettings[SL_DenseData_Interval];
            string denseNum = System.Configuration.ConfigurationSettings.AppSettings[SL_DenseData_Num];
            string looseStartTime = System.Configuration.ConfigurationSettings.AppSettings[SL_LooseData_StartTime];
            string looseInterval = System.Configuration.ConfigurationSettings.AppSettings[SL_LooseDate_Interval];

            byte[] set = { 
                            0xFF, 0x02, 0x16, 0xA1, 
                            Convert.ToByte(denseStartTime.Substring(0,2)),
                            Convert.ToByte(denseStartTime.Substring(2,2)),
                            Convert.ToByte(denseInterval),
                            Convert.ToByte(denseNum),
                            0x04,0x00,
                            0x05,0x00,
                            Convert.ToByte(looseStartTime.Substring(0,2)),
                            Convert.ToByte(looseStartTime.Substring(2,2)),
                            Convert.ToByte(looseInterval),
                            ss,mm,HH,
                            wk,dd,MM,yy,
                            0x41,0xB1,0x03
                        };
            return set;
        }

        public byte[] getSLNoiseDefaultConfig()
        {
            byte[] set = { 
                            0xFF, 
                            0x02,
                            0x01,
                            0x00,0x1C,
                            0x00,0x00,0x32,
                            0xC1,
                            0x02,0x00,
                            0x05,
                            0x1E,
                            0x05,0x00,
                            0x05,0x00,
                            0x08,0x00,
                            0x3C,
                            StringUtil.SEC,StringUtil.MIN,StringUtil.HOR,
                            StringUtil.WEEK,StringUtil.DAY,StringUtil.MON,StringUtil.YEAR,
                            0x03};
            return set;
        }

        public byte[] getLSNoiseCALIConfig()
        {
            byte[] set = { 
                            0xFF, 
                            0x02,
                            0x01,
                            0x00,0x1C,
                            0x00,0x00,0x32,
                            0xC2,//表示该配置帧无效，仅用于校时
                            0x03,0x00,//凌晨3点钟开始采集密集噪声
                            0x03,//3分钟采集一次
                            0x28,//采集40个。
                            0x05,0x00,
                            0x05,0x00,
                            0x06,0x00,//早晨六点开始采集松散噪声。
                            0x3C,
                            StringUtil.SEC,StringUtil.MIN,StringUtil.HOR,
                            StringUtil.WEEK,StringUtil.DAY,StringUtil.MON,StringUtil.YEAR,
                            0x03};
            return set;
        }

        public byte[] getLSSectionCloseConfig()
        {
            byte[] sectionClose = { 
                0xFF, 
                0x00,0x09, 
                0x02, 
                0x00,0x00,0x32, 
                0x00, 
                0x03 };
            return sectionClose;
        }

        public byte[] getLSNoiseDefaultConfig()
        {
            byte[] set = { 
                            0xFF, 
                            0x02,
                            0x01,
                            0x00,0x1C,
                            0x00,0x00,0x32,
                            0xC1,
                            0x03,0x00,//凌晨3点钟开始采集密集噪声
                            0x03,//3分钟采集一次
                            0x28,//采集40个。
                            0x05,0x00,
                            0x05,0x00,
                            0x06,0x00,//早晨六点开始采集松散噪声。
                            0x3C,
                            StringUtil.SEC,StringUtil.MIN,StringUtil.HOR,
                            StringUtil.WEEK,StringUtil.DAY,StringUtil.MON,StringUtil.YEAR,
                            0x03};
            return set;
        }

        public byte[] getLSFlowDefaultConfig()
        {
            byte[] set = { 0x3A, 
                            0x00, 0x0D, 
                            0x01, 
                            0x00, 0x00, 0x31, 
                            0xC1, 
                            0x00, 0x05,//默认五分钟采集一次
                            0x01, 0x20, //默认一天发送288次
                            0x03 };
            return set;
        }

        public byte[] getLSPressDefaultConfig()
        {
            byte[] set = { 0x40, 
                             0x00, 0x0D, 
                             0x01, 
                             0x00, 0x00, 0x33, 
                             0xC1, 
                             0x00, 0x05, 
                             0x00, 0x90, 
                             0x03 };
            return set;
        }

        public byte[] getLiquidEndSectionConfig()
        {
            byte[] set = { 0x50, 
                                 0x00, 0x09,
                                 0x02,
                                 0x00, 0x00,0x34,
                                 0x00,
                                 0x03};
            return set;
        }

        //查询雨量计设备Id号的请求帧
        public byte[] getYuLiangCheckIdConfig()
        { 
            byte[] set ={0x48,
                              0x43,
                              0x00,0x6E,
                              0x00,0x00,
                              0x00,
                              0x00,0x00,0x00,0x00,0x00,0x00,
                              0x00,0x00,
                              0xAF};
            return set;
        }

        //雨量计当前时间的请求帧
        public byte[] getYuLiangTime()
        {
            byte[] set ={0x48,
                              0x43,
                              0xFF,0xFF,
                              0x00,0x00,
                              0x02,
                              0x00,0x00,0x00,0x00,0x00,0x00,
                              0x00,0x00,
                              0xAF};
            return set;
        }

        //更新雨量计的请求帧
        public byte[] getUpdateTime()
        {
            byte yy = byte.Parse(DateTime.Now.ToString("yy"));
            byte MM = byte.Parse(DateTime.Now.ToString("MM"));
            byte dd = byte.Parse(DateTime.Now.ToString("dd"));
            byte HH = byte.Parse(DateTime.Now.ToString("HH"));
            byte mm = byte.Parse(DateTime.Now.ToString("mm"));
            byte ss = byte.Parse(DateTime.Now.ToString("ss"));
            byte wk = byte.Parse("0" + ((int)DateTime.Now.DayOfWeek).ToString());
            
            byte[] set ={0x48,
                              0x43,
                              0x00,0x6E,
                              0x00,0x00,
                              0x05,
                              yy,MM,dd,HH,mm,ss,
                              0x00,0x00,
                              0xAF};
            return set;
        }

        //擦除一小时雨量请求帧
        public byte[] getDeleteOneHourYuLiang()
        {
            byte[] set ={0x48,
                              0x43,
                              0xFF,0xFF,
                              0x00,0x00,
                              0x07,
                              0x03,0x00,0x00,0x00,0x00,0x00,
                              0x00,0x00,
                              0xAF};
            return set;
        }

        public byte[] getDeleteTotalYuLiang()
        {
            byte[] set ={0x48,
                              0x43,
                              0xFF,0xFF,
                              0x00,0x00,
                              0x07,
                              0x00,0x00,0x00,0x00,0x00,0x00,
                              0x00,0x00,
                              0xAF};
            return set;
        }

        //获取当前分钟 前一分钟雨量次数 和所有累计雨量次数的请求帧
        public byte[] getMinuteYuliangCount()
        {
            byte[] set ={0x48,
                              0x43,
                              0x00,0x6E,
                              0x00,0x00,
                              0x10,
                              0x00,0x00,0x00,0x00,0x00,0x00,
                              0x00,0x00,
                              0xAF};
            return set;
        }

        //获取当前小时雨量次数的请求帧
        public byte[] getCurrentHourYuLiangCount()
        {
            byte[] set ={0x48,
                              0x43,
                              0xFF,0xFF,
                              0x00,0x00,
                              0x11,
                              0x00,0x00,0x00,0x00,0x00,0x00,
                              0x00,0x00,
                              0xAF};
            return set;
        }

        //获取三个小时雨量次数的请求帧
        public byte[] getCurrentTHourYuLiangCount()
        {
            byte[] set ={0x48,
                              0x43,
                              0xFF,0xFF,
                              0x00,0x00,
                              0x12,
                              0x00,0x00,0x00,0x00,0x00,0x00,
                              0x00,0x00,
                              0xAF};
            return set;
        }

        //获取埃德尔设备的默认配置信息

        public byte[] getDefaultADSettting(String version, String devId, String pduType, String seq)
        {
            //获取版本号
            String hiVer = version.Substring(0, 1);
            String lowVer = version.Substring(1, 1);
            byte btVer = (byte)((CodeUtils.String2Byte(hiVer)) * 16 + CodeUtils.String2Byte(lowVer));

            //获取设备ID
            byte[] btDevs = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                btDevs[i] = CodeUtils.String2Byte(devId.Substring(2 * i, 2));
            }

            //获取pduType
            byte btPduType = CodeUtils.String2Byte(pduType);
            byte dstPduType = (byte)(0xB0 | (0x0f & btPduType));

            
            //获取系统时间的tag 长度为2个字节！
            byte[] sysTag = { 0x10,0x00,0x00,0x51,0x00,0x06,
            StringUtil.YEAR, StringUtil.MON, StringUtil.DAY, StringUtil.HOR, StringUtil.MIN, StringUtil.SEC };

            //获取iptag信息
            String adIp = System.Configuration.ConfigurationSettings.AppSettings["AD_IP"];
            String adPort = System.Configuration.ConfigurationSettings.AppSettings["AD_PORT"];
            byte[] btIp = ASCIIEncoding.ASCII.GetBytes(adIp);
           
            Int16 ipLen0 = (Int16)btIp.Length;
            byte[] btiplens0 = BitConverter.GetBytes(ipLen0);
            byte[] iplens = new byte[2];
            iplens[0] = btiplens0[1];
            iplens[1] = btiplens0[0];

            byte[] ipOid = {0x10,0x00,0x00,0x22};
            byte[] ipTag = new byte[4 + 2 + btIp.Length];
            ipOid.CopyTo(ipTag, 0);
            iplens.CopyTo(ipTag, 4);
            btIp.CopyTo(ipTag, 6);

            //获取porttag信息
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

            //总长度计算
            Int16 totalLen = (Int16)(4 + 1 + 1 + sysTag.Length + ipTag.Length + portTag.Length);
            byte[] btlens0 = BitConverter.GetBytes(totalLen);
            byte[] btlens = new byte[2];
            btlens[0] = btlens0[1];
            btlens[1] = btlens0[0];

            //crc之前的byte类型数据
            byte[] bfsrc = new byte[totalLen + 2 + 1 + 1];
            bfsrc[0] = 0xA3;
            bfsrc[1] = btVer;
            btlens.CopyTo(bfsrc, 2);
            btDevs.CopyTo(bfsrc,4);
            bfsrc[8] = dstPduType;
            bfsrc[9] = CodeUtils.String2Byte(seq);
            sysTag.CopyTo(bfsrc, 10);
            ipTag.CopyTo(bfsrc, 10 + sysTag.Length);
            portTag.CopyTo(bfsrc, 10 + sysTag.Length+ipTag.Length);


            String strCrc = String.Format("{0:X}", (int)CodeUtils.CRC16_AD(bfsrc));
            byte[] btcrc = { CodeUtils.String2Byte(strCrc.Substring(0, 2)), CodeUtils.String2Byte(strCrc.Substring(2, 2)) };

            byte[] afcrc = new byte[bfsrc.Length + 2];
            bfsrc.CopyTo(afcrc, 0);
            btcrc.CopyTo(afcrc, bfsrc.Length);

            return CodeUtils.adEncode(afcrc);
            
        }
        
        /*
        private static string ADDefaultSetting = "ADDefaultSetting";
        public string getDefaultADSetting()
        {
            string adDefaultSetting = System.Configuration.ConfigurationSettings.AppSettings[ADDefaultSetting];
            String[] settings = adDefaultSetting.Split(',');
            string result = getSystemTimeTag();

            //make sure the first tag is system time tag
            foreach (String setting in settings)
            {
                String[] temp = setting.Split(':');
                String oid = temp[0];
                String len = temp[1];
                String value = temp[2];
                result = result + oid + len + value;
            }
            return "";
        }
        private String getSystemTimeTag()
        {
            //获取第一个系统时间的tag
            String year = (Convert.ToString(StringUtil.YEAR, 16)).Length != 2 ? 0 + Convert.ToString(StringUtil.YEAR, 16) : Convert.ToString(StringUtil.YEAR, 16);
            String month = (Convert.ToString(StringUtil.YEAR, 16)).Length != 2 ? 0 + Convert.ToString(StringUtil.MON, 16) : Convert.ToString(StringUtil.MON, 16);
            String day = (Convert.ToString(StringUtil.YEAR, 16)).Length != 2 ? 0 + Convert.ToString(StringUtil.DAY, 16) : Convert.ToString(StringUtil.DAY, 16);

            String hour = (Convert.ToString(StringUtil.YEAR, 16)).Length != 2 ? 0 + Convert.ToString(StringUtil.HOR, 16) : Convert.ToString(StringUtil.HOR, 16);
            String min = (Convert.ToString(StringUtil.YEAR, 16)).Length != 2 ? 0 + Convert.ToString(StringUtil.MIN, 16) : Convert.ToString(StringUtil.MIN, 16);
            String sec = (Convert.ToString(StringUtil.YEAR, 16)).Length != 2 ? 0 + Convert.ToString(StringUtil.SEC, 16) : Convert.ToString(StringUtil.SEC, 16);

            return "10000051"+"06"+year + month + day + hour + min + sec;

        }
        */

        /**
         *sequeceName:需要更新的序列名称
         *key:主键字段名称
         *tableName:表名称
         */
        public void updateSequence(String sequeceName, String key, String tableName)
        {
             OracleTransaction tran = null;
             using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
             {
                 try
                 {
                      conn.Open();
                      tran = conn.BeginTransaction();

                      String DROP_SEQUENCE = "drop sequence " + sequeceName;
                      OracleHelper.ExecuteNonQuery(tran, CommandType.Text, DROP_SEQUENCE, null);

                      String SQL_MAX_KEY = "select max(" + key + ")" + " from " + tableName;

                      object max = OracleHelper.ExecuteScalar(tran,
                      CommandType.Text, SQL_MAX_KEY, null);

                      int intMax = max.ToString() == "" ? 1 : Convert.ToInt32(max)+1;

                      //TODO LIST:创建序列
                      String CREATE_SEQUENCE = "create sequence " + sequeceName + " minValue 1 maxValue 999999999999999999999999999" +
                          " start with " + intMax + " increment by 1 cache 20";

                      OracleHelper.ExecuteNonQuery(tran, CommandType.Text, CREATE_SEQUENCE, null);

                      tran.Commit();

                 }
                 catch (Exception e)
                {
                    if (null != tran)
                    {
                        tran.Rollback();
                    }
                }
             }
        }
    }
}



