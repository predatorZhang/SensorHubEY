using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using System.Reflection;
using SensorHub.BLL;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketEngine;
using SensorHub.Utility;
namespace TestClass
{
    class Program
    {
        static void Main(string[] args)
        {
            testZZSLNoise();

          //  testAKAlarm();
            //testAK();
            //testWSAlarm();
           // testSLAlarm();
           // testRemoveAlarmRecord();

           // testUpdateSequence();
          // testRemoveAlarmRecord();
            //testLiquidAlarm();
           // testLiquidConfig();
         //   testWellDepth();
          //  testSlNoise();
            //byte[] temp1 = Convert.FromBase64String("Hell");      // Normal.
            //byte[] temp2 = Convert.FromBase64String("Hello Net"); // Normal.（忽略空格）
            //string data = "0101020404028485A1";
            //string crc = CodeUtils.CRC16_Standard(data, 0, data.Length);

            ////测试433的CRC
            ////string testIn = "aa1d124a030014000050001401000034c200050120381612030d050f03";
            ////ushort testOut = CodeUtils.QuickCRC16(testIn, 0, testIn.Length);
            
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();

            var bootstrap = BootstrapFactory.CreateBootstrap();

            try
            {
                if (!bootstrap.Initialize())
                {
                    Console.WriteLine("Failed to initialize!");
                    Console.ReadKey();
                    return;
                }
            }
            catch (Exception e)
            {
                String ss = e.Message;
            }
          

           var result = bootstrap.Start();
       
            Console.WriteLine("Start result: {0}!", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

           Console.WriteLine("Press key 'q' to stop it!");

          while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
               continue;
         }

            Console.WriteLine();

            //Stop the appServer
            bootstrap.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
     
        }

        static void testAKAlarm()
        {
            List<SensorHub.Model.AKFSHJInfo> fshjs = new List<AKFSHJInfo>();
            SensorHub.Model.AKFSHJInfo fs = new AKFSHJInfo();
            fs.UnderWaterIn1 = 1 + "";
            fs.UnderWaterIn2 = 1 + "";
            fs.UnderWaterIn3 = 1 + "";
            fs.UnderWaterIn4 = 1 + "";
            fs.UnderWaterIn5 = 1 + "";
            fs.UnderWaterIn6 = 1 + "";

            fs.UnderVo11 = 2 + "";
            fs.UnderVo12 = 2 + "";
            fs.UnderVo13 = 2 + "";
            fs.UnderVo14 = 2 + "";
            fs.UnderVo15 = 2 + "";
            fs.UnderVo16 = 2 + "";

            fs.OutterTemp1 = 20 + "";
            fs.OutterTemp2 = 20 + "";
            fs.UnderTemp1 = 25 + "";
            fs.UnderTemp2 = 25 + "";
            fs.Cell = 100 + "";
            fs.LogTime = DateTime.Now;
            fs.UpTime = DateTime.Now;

            fs.DEVCODE = "fushi003";

            fshjs.Add(fs);
            new SensorHub.BLL.AKFSHJ().insert(fshjs);
            String pipeType = new SensorHub.BLL.AKFSHJ().getHeatPipeTypeByDevCode("212016090006");
            new SensorHub.BLL.AKFSHJ().saveAlarmInfo(fshjs,pipeType);

            List<SensorHub.Model.AKFSSLInfo> fssls = new List<AKFSSLInfo>();
            SensorHub.Model.AKFSSLInfo sl = new AKFSSLInfo();

            sl.ErrosionRat = 0.5 + "";
            sl.JhResist = "100";
            sl.RyResist = "200";
            sl.OpenCir = "123";
            sl.CurrentDen = "123";
            sl.Cell = 100 + "";
            sl.LogTime = DateTime.Now;
            sl.UpTime = DateTime.Now;
            sl.DEVCODE = "fushi001";
            fssls.Add(sl);
            new SensorHub.BLL.AKFSSL().insert(fssls);
            new SensorHub.BLL.AKFSSL().saveAlarmInfo(fssls);

        }
        static void testAK()
        {
            List<SensorHub.Model.AKFSHJInfo> fshjs = new List<AKFSHJInfo>();
            SensorHub.Model.AKFSHJInfo fs = new AKFSHJInfo();
            fs.UnderWaterIn1 = 1 + "";
            fs.UnderWaterIn2 = 1 + "";
            fs.UnderWaterIn3 = 1 + "";
            fs.UnderWaterIn4 = 1 + "";
            fs.UnderWaterIn5 = 1 + "";
            fs.UnderWaterIn6 = 1 + "";

            fs.UnderVo11 = 2 + "";
            fs.UnderVo12 = 2 + "";
            fs.UnderVo13 = 2 + "";
            fs.UnderVo14 = 2 + "";
            fs.UnderVo15 = 2 + "";
            fs.UnderVo16 = 2 + "";

            fs.OutterTemp1 = 20 + "";
            fs.OutterTemp2 = 20 + "";
            fs.UnderTemp1 = 25 + "";
            fs.UnderTemp2 = 25 + "";
            fs.Cell = 100 + "";
            fs.LogTime = DateTime.Now;
            fs.UpTime = DateTime.Now;

            fs.DEVCODE = "123456";

            fshjs.Add(fs);
            new SensorHub.BLL.AKFSHJ().insert(fshjs);

            List<SensorHub.Model.AKFSSLInfo> fssls = new List<AKFSSLInfo>();
            SensorHub.Model.AKFSSLInfo sl = new AKFSSLInfo();

            sl.ErrosionRat = 0.5 + "";
            sl.JhResist = "100";
            sl.RyResist = "200";
            sl.OpenCir = "123";
            sl.CurrentDen = "123";
            sl.Cell = 100 + "";
            sl.LogTime = DateTime.Now;
            sl.UpTime = DateTime.Now;
            sl.DEVCODE = "234567";
            fssls.Add(sl);
            new SensorHub.BLL.AKFSSL().insert(fssls);
            
        }
        static void testWSAlarm()
        {
            List<SensorHub.Model.AlarmRecordInfo> alarms = new List<SensorHub.Model.AlarmRecordInfo>();
                //TODO LIST:根据李雨龙需求，修改可能发生的4个报警记录合并为一条报警记录
               // 一氧化碳、硫化氢、氧气、甲烷

                SensorHub.Model.AlarmRecordInfo alarm = new SensorHub.Model.AlarmRecordInfo();
                alarm.RECORDCODE = "WS_ALARM_H2S_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                alarm.MESSAGE = "有害气体超标";
                alarm.ITEMNAME = "FIREGAS";
                alarm.DEVICE_ID = (int)362;
                alarm.DEVICE_CODE = "ws2000-201501006";
                alarm.MESSAGE_STATUS = 0;
                alarm.ACTIVE = true;
                alarm.DEVICE_TYPE_NAME = "有害气体监测仪";
                string result = "";
                result = "1,2,3,4";
                alarm.ITEMVALUE = result;
                alarms.Add(alarm);
                new SensorHub.BLL.AlarmRecord().insert(alarms);
 
        }
        static void testGXALARM()
        {
            SensorHub.BLL.NKStressCurve stress = new SensorHub.BLL.NKStressCurve();
            stress.saveAlarm("1,2,3,4","100,300,350,600","GX03",(int)3,"热力泄漏形变光纤");

            SensorHub.Model.NKTemperatureCurveInfo cu = new SensorHub.BLL.NKTemperatureCurve().getLastTempCurve("GX01");
            string disArray = cu.DISTANCE;
            string[] tempArray = cu.TEMPERATURE.Split(',');
            string newTempArray = "";
            for (int i = 0; i < tempArray.Length; i++)
            {
                string newTemp = tempArray[i];
                if (i == 10 || i == 20)
                {
                    newTemp = float.Parse(newTemp) + 10+"";
                }
                newTempArray = newTempArray + newTemp+",";
            }
            
            
            SensorHub.BLL.NKTemperatureCurve temp = new SensorHub.BLL.NKTemperatureCurve();
            temp.saveAlarm(disArray, newTempArray.Substring(0, newTempArray.Length - 1), "GX01", (int)1, "燃气应力温度光纤");

        }
        static void testSLAlarm()
        {
          
          SensorHub.BLL.SlNoise slNoiseDal = new SensorHub.BLL.SlNoise();

          slNoiseDal.testAlarm();
           
        }

        static void testUpdateSequence()
        {
            ApplicationContext.getInstance().updateSequence("SEQ_WS_PERIOD_ID", "DBID", "WS_PERIOD_DATA");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");

            ApplicationContext.getInstance().updateSequence("SEQ_AD_SL_NOISE_ID", "DBID", "AD_SL_NOISE");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");

            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_LIQUID", "DBID", "AD_DJ_LIQUID");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");

            ApplicationContext.getInstance().updateSequence("SEQ_XT_RQ_PERIOD", "DBID", "XT_RQ_PERIOD");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");

            ApplicationContext.getInstance().updateSequence("SEQ_NK_GX_STRESS_CURVE_ID", "DBID", "NK_GX_STRESS_CURVE");
            ApplicationContext.getInstance().updateSequence("SEQ_NK_GX_TEMPERATURE_CURVE_ID", "DBID", "NK_GX_TEMPERATURE_CURVE");
            ApplicationContext.getInstance().updateSequence("SEQ_NK_GX_VIBRATING_CURVE_ID", "DBID", "NK_GX_VIBRATING_CURVE");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");

            ApplicationContext.getInstance().updateSequence("SEQ_POSITION_ID", "DBID", "POSITION");
            ApplicationContext.getInstance().updateSequence("SEQ_LOGINFO_ID", "DBID", "LOGINFO");

            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_LIQUID", "DBID", "AD_DJ_LIQUID");
            ApplicationContext.getInstance().updateSequence("SEQ_AD_SL_NOISE_ID", "DBID", "AD_SL_NOISE");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_PRESS_ID", "DBID", "ALARM_PRESS");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_TEMPERATURE_ID", "DBID", "ALARM_TEMPERATURE");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_WATERQUANTITY_ID", "DBID", "ALARM_WATERQUANTITY");
            ApplicationContext.getInstance().updateSequence("SEQ_CASIC_WELL_INFO_ID", "DBID", "CASIC_WELL_INFO");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");

            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_FLOW_ID", "DBID", "AD_DJ_FLOW");
            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_NOISE_ID", "DBID", "AD_DJ_NOISE");
            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_PRESS_ID", "DBID", "AD_DJ_PRESS");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
          
        }
        static void testRemoveAlarmRecord()
        {
            AlarmRecord alarmRecordDal = new SensorHub.BLL.AlarmRecord();
            List<SensorHub.Model.AlarmRecordInfo> list = new List<SensorHub.Model.AlarmRecordInfo>();
            SensorHub.Model.AlarmRecordInfo record0 = new SensorHub.Model.AlarmRecordInfo();
            record0.DEVICE_CODE = "112015090042";
            record0.MESSAGE = "测试报警01";
            record0.ACTIVE = true;
            record0.DEVICE_ID = Int32.Parse("113");
            record0.DEVICE_TYPE_NAME ="液位监测仪";
            record0.ITEMNAME = "液位";
            record0.ITEMVALUE = "99";
            record0.MESSAGE_STATUS = 0;
            record0.RECORDCODE = "";
            record0.RECORDDATE = System.DateTime.Now;


            SensorHub.Model.AlarmRecordInfo record1 = new SensorHub.Model.AlarmRecordInfo();
            record1.DEVICE_CODE = "112015090042";
            record1.MESSAGE = "测试报警01";
            record1.ACTIVE = true;
            record1.DEVICE_ID = Int32.Parse("113");
            record1.DEVICE_TYPE_NAME = "液位监测仪";
            record1.ITEMNAME = "液位";
            record1.ITEMVALUE = "99";
            record1.MESSAGE_STATUS = 0;
            record1.RECORDCODE = "";
            record1.RECORDDATE = System.DateTime.Now;

            list.Add(record0);
            list.Add(record1);

            alarmRecordDal.insert(list);

         

        }
        static void testLiquidAlarm()
        {

            List<SensorHub.Model.DjLiquidInfo> djs = new List<SensorHub.Model.DjLiquidInfo>();
            SensorHub.Model.DjLiquidInfo model = new SensorHub.Model.DjLiquidInfo();
            model.DEVID="112015090040";
            model.LIQUIDDATA="1";
            djs.Add(model);
            new SensorHub.BLL.DjLiquid().insert(djs);
            new SensorHub.BLL.DjLiquid().saveAlarmInfo(djs);
            new SensorHub.BLL.DjLiquid().updateDevStatus("112015090040");
        }
        static void testLiquidConfig()
        {
            byte ss = byte.Parse(DateTime.Now.ToString("yy"));
            byte ss1 = byte.Parse(DateTime.Now.ToString("MM"));
            byte ss2 = byte.Parse(DateTime.Now.ToString("dd"));
            byte ss3 = byte.Parse(DateTime.Now.ToString("HH"));
            byte ss4 = byte.Parse(DateTime.Now.ToString("mm"));
            byte ss5 = byte.Parse(DateTime.Now.ToString("ss"));
            byte ss7 = byte.Parse("0" + ((int)DateTime.Now.DayOfWeek).ToString());
            DeviceConfigInfo conf;

            conf = (new DeviceConfig()).GetDeviceConfByDeviceCodeAndSensorCode("112015090038", "000034");

            if (conf != null)
            {
                string content = conf.FrameContent;
                string[] items = content.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                byte[] sendData = new byte[items.Length + 9 + 2];//补充2个字节,用于扩展报警界限
                for (int i = 0; i < items.Length - 10; i++)
                {
                    sendData[i + 9] = byte.Parse(items[i], System.Globalization.NumberStyles.HexNumber);
                }
                //修正报警规则信息
                SensorHub.BLL.DjLiquid djBll = new SensorHub.BLL.DjLiquid();
                AlarmRuleInfo rule = djBll.getAlarmRuleByDevcode("112015090038");
                float highValue = rule != null ? rule.HighValue : 1000;
                byte[] btHighValue = BitConverter.GetBytes(highValue);
                sendData[9 + 10 + 0] = btHighValue[0];
                sendData[9 + 10 + 1] = btHighValue[1];
                sendData[9 + 10 + 2] = btHighValue[2];
                sendData[9 + 10 + 3] = btHighValue[3];

              //  if (requestInfo.Parameters[0] == "50")
              //  {
                    //液位时间修正
                    sendData[21 + 2] = ss5;
                    sendData[22 + 2] = ss4;
                    sendData[23 + 2] = ss3;
                    sendData[24 + 2] = ss7;
                    sendData[25 + 2] = ss2;
                    sendData[26 + 2] = ss1;
                    sendData[27 + 2] = ss;
                    sendData[27 + 2 + 1] = 0x03;
             //   }
                sendData[0] = 0XAA;
                sendData[1] = 0X1D;
                sendData[2] = 0x00;
                sendData[3] = 0x00;
                sendData[4] = 0X03;
                sendData[5] = 0X00;
                sendData[6] = System.BitConverter.GetBytes(sendData.Length - 9)[0];
                sendData[7] = 0X00;
                sendData[8] = 0X00;

                String crcIn = "";
                for (int i = 0; i < sendData.Length; i++)
                {
                    crcIn += sendData[i].ToString("X2");
                }
                ushort crcOut = CodeUtils.QuickCRC16(crcIn, 0, crcIn.Length);
                byte[] crcOutByte = BitConverter.GetBytes(crcOut);
                sendData[7] = crcOutByte[1];
                sendData[8] = crcOutByte[0];
            }
        }
        static void testLiquid()
        {
            List<SensorHub.Model.DjLiquidInfo> djs = new List<SensorHub.Model.DjLiquidInfo>();
            DjLiquidInfo liquidInfo = new DjLiquidInfo();
            liquidInfo.CELL = "1.1";
            liquidInfo.LIQUIDDATA = "1.9";
            liquidInfo.LOGTIME = DateTime.Now;
            liquidInfo.UPTIME = DateTime.Now;
            liquidInfo.DEVID = "YWS_YW002";
            djs.Add(liquidInfo);

            new SensorHub.BLL.DjLiquid().saveAlarmInfo(djs);
            new SensorHub.BLL.DjLiquid().updateDevStatus(djs);
        }

        static void testWellDepth()
        {
            SensorHub.BLL.Device device = new SensorHub.BLL.Device();
            float ada = device.getWellDepByDevcode("11201509006");
        }
        static void testSlNoise()
        {
            List<SensorHub.Model.SlNoiseInfo> djs = new List<SensorHub.Model.SlNoiseInfo>();
            SlNoiseInfo slNoiseInfo = new SlNoiseInfo();
            slNoiseInfo.CELL = "1.1";
            slNoiseInfo.LOGTIME = System.DateTime.Now;
            slNoiseInfo.UPTIME = System.DateTime.Now;
            slNoiseInfo.SRCID = "JS_S3_001";
            slNoiseInfo.DSTID = "FFFF";
            slNoiseInfo.DENSEDATA = "8";
            djs.Add(slNoiseInfo);

            new SensorHub.BLL.SlNoise().saveAlarmInfo(djs);
            new SensorHub.BLL.SlNoise().updateDevStatus(djs);
        }

        static void testZZSLNoise()
        {
            List<SensorHub.Model.SlNoiseInfo> djs = new List<SensorHub.Model.SlNoiseInfo>();
            SlNoiseInfo slNoiseInfo = new SlNoiseInfo();
            slNoiseInfo.CELL = "1.1";
            slNoiseInfo.LOGTIME = System.DateTime.Now;
            slNoiseInfo.UPTIME = System.DateTime.Now;
            slNoiseInfo.SRCID = "212016090004";
            slNoiseInfo.DSTID = "FFFF";
            slNoiseInfo.DENSEDATA = "15";

            SlNoiseInfo slNoiseInfo0 = new SlNoiseInfo();
            slNoiseInfo0.CELL = "1.1";
            slNoiseInfo0.LOGTIME = System.DateTime.Now;
            slNoiseInfo0.UPTIME = System.DateTime.Now;
            slNoiseInfo0.SRCID = "212016090004";
            slNoiseInfo0.DSTID = "FFFF";
            slNoiseInfo0.DENSEDATA = "7";

            djs.Add(slNoiseInfo);
            djs.Add(slNoiseInfo0);

            new SensorHub.BLL.SlNoise().SaveZZAlarmInfo(djs);
        }
    }
}
