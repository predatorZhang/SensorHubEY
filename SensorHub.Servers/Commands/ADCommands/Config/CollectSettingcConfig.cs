using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;
using SuperSocket.Common;
namespace SensorHub.Servers.Commands.ADCommands
{
    public class CollectSettingcConfig:TagConfig
    {
        public CollectSettingcConfig(Config config)
            : base(config)
        {

        }

       private  static String  FLOW_ENABLE =System.Configuration.ConfigurationSettings.AppSettings["FLOW_ENABLE"];
       private static String FLOW_STIME1 = System.Configuration.ConfigurationSettings.AppSettings["FLOW_STIME1"];
       private  static String  FLOW_ITRL1=System.Configuration.ConfigurationSettings.AppSettings["FLOW_ITRL1"];
       private  static String  FLOW_CNT1=System.Configuration.ConfigurationSettings.AppSettings["FLOW_CNT1"];
       private  static String  FLOW_STIME2=System.Configuration.ConfigurationSettings.AppSettings["FLOW_STIME2"];
       private  static String  FLOW_ITRL2=System.Configuration.ConfigurationSettings.AppSettings["FLOW_ITRL2"];
       private  static String  FLOW_CNT2=System.Configuration.ConfigurationSettings.AppSettings["FLOW_CNT2"];
       private  static String  FLOW_RTIME=System.Configuration.ConfigurationSettings.AppSettings["FLOW_RTIME"];
       private  static String  FLOW_RCNT =System.Configuration.ConfigurationSettings.AppSettings[ "FLOW_RCNT"];
   
       private  static String  PRESS_ENABLE =System.Configuration.ConfigurationSettings.AppSettings[ "PRESS_ENABLE"];
       private  static String  PRESS_STIME1=System.Configuration.ConfigurationSettings.AppSettings["PRESS_STIME1"];
       private  static String  PRESS_ITRL1=System.Configuration.ConfigurationSettings.AppSettings["PRESS_ITRL1"];
       private  static String  PRESS_CNT1=System.Configuration.ConfigurationSettings.AppSettings["PRESS_CNT1"];
       private  static String  PRESS_STIME2=System.Configuration.ConfigurationSettings.AppSettings["PRESS_STIME2"];
       private  static String  PRESS_ITRL2=System.Configuration.ConfigurationSettings.AppSettings["PRESS_ITRL2"];
       private  static String  PRESS_CNT2=System.Configuration.ConfigurationSettings.AppSettings["PRESS_CNT2"];
       private  static String  PRESS_RTIME=System.Configuration.ConfigurationSettings.AppSettings["PRESS_RTIME"];
       private  static String  PRESS_RCNT =System.Configuration.ConfigurationSettings.AppSettings[ "PRESS_RCNT"];

       private  static String  NOISE_ENABLE =System.Configuration.ConfigurationSettings.AppSettings[ "NOISE_ENABLE"];
       private  static String  NOISE_STIME1=System.Configuration.ConfigurationSettings.AppSettings["NOISE_STIME1"];
       private  static String  NOISE_ITRL1=System.Configuration.ConfigurationSettings.AppSettings["NOISE_ITRL1"];
       private  static String  NOISE_CNT1=System.Configuration.ConfigurationSettings.AppSettings["NOISE_CNT1"];
       private  static String  NOISE_STIME2=System.Configuration.ConfigurationSettings.AppSettings["NOISE_STIME2"];
       private  static String  NOISE_ITRL2=System.Configuration.ConfigurationSettings.AppSettings["NOISE_ITRL2"];
       private  static String  NOISE_CNT2=System.Configuration.ConfigurationSettings.AppSettings["NOISE_CNT2"];
       private  static String  NOISE_RTIME=System.Configuration.ConfigurationSettings.AppSettings["NOISE_RTIME"];
       private  static String  NOISE_RCNT =System.Configuration.ConfigurationSettings.AppSettings[ "NOISE_RCNT"];

       private  static String  LIQUID_ENABLE =System.Configuration.ConfigurationSettings.AppSettings[ "LIQUID_ENABLE"];
       private  static String  LIQUID_STIME1=System.Configuration.ConfigurationSettings.AppSettings["LIQUID_STIME1"];
       private  static String  LIQUID_ITRL1=System.Configuration.ConfigurationSettings.AppSettings["LIQUID_ITRL1"];
       private  static String  LIQUID_CNT1=System.Configuration.ConfigurationSettings.AppSettings["LIQUID_CNT1"];
       private  static String  LIQUID_STIME2=System.Configuration.ConfigurationSettings.AppSettings["LIQUID_STIME2"];
       private  static String  LIQUID_ITRL2=System.Configuration.ConfigurationSettings.AppSettings["LIQUID_ITRL2"];
       private  static String  LIQUID_CNT2=System.Configuration.ConfigurationSettings.AppSettings["LIQUID_CNT2"];
       private  static String  LIQUID_RTIME=System.Configuration.ConfigurationSettings.AppSettings["LIQUID_RTIME"];
       private  static String  LIQUID_RCNT =System.Configuration.ConfigurationSettings.AppSettings[ "LIQUID_RCNT"];
    


        private int getBufferSize()
        {
            int result = 0;
            
         
            if (FLOW_ENABLE == "true")
            {
               result++;
            }


            if (PRESS_ENABLE == "true")
            {
              result++;
            }

            
            if (NOISE_ENABLE == "true")
            {
                  result++;
            }

        
            if (LIQUID_ENABLE == "true")
            {
                  result++;
            }
            return result;

        }

        public override byte[] getConfig(byte[] src)
        {
            byte []temp = new byte[64*getBufferSize()];
          
            int flag = 0;
           
            if(FLOW_ENABLE=="true")
            {
                byte[] flowSet = this.getFlowSetting();
                flowSet.CopyTo(temp,flag);
                flag=flag+64;
            }
      

            if(PRESS_ENABLE=="true")
            {
                byte[] pressSet = this.getPressSetting();
                pressSet.CopyTo(temp,flag);
                flag=flag+64;
            }

              if(NOISE_ENABLE=="true")
            {
                  byte[] noiseSet = this.getNosieSetting();
                  noiseSet.CopyTo(temp,flag);
                  flag=flag+64;
                 
            }

              if(LIQUID_ENABLE=="true")
            {
                  byte[] liquidSet = this.getLiquidSetting();
                  liquidSet.CopyTo(temp,flag);
                  flag=flag+64;
            }

             byte[] result = new byte[src.Length + temp.Length];
            src.CopyTo(result, 0);
            temp.CopyTo(result, src.Length);

            //获取流量压力，
            return base.getConfig(result);
        }

        private byte[] getFlowSetting()
        {
            byte[] oid1 ={0x10,0x00,0x01,0x04,0x00,0x02};
            Int16 stime1 = (Int16)(Convert.ToInt16(FLOW_STIME1.Substring(0,2))*60+Convert.ToInt16(FLOW_STIME1.Substring(3,2)));
            byte[] btime1 = BitConverter.GetBytes(stime1);
;             
            byte[] oid2 ={0x10,0x00,0x01,0x05,0x00,0x02};
            Int16 sitr1 = (Convert.ToInt16(FLOW_ITRL1));
            byte[] bsitr1 = BitConverter.GetBytes(sitr1);
              
            byte[] oid3 ={0x10,0x00,0x01,0x06,0x00,0x02};
            Int16 scnt1 = (Convert.ToInt16(FLOW_CNT1));
            byte[] bscnt1 = BitConverter.GetBytes(scnt1);

             byte[] oid4 ={0x10,0x00,0x01,0x07,0x00,0x02};
            Int16 stime2 =(Int16)( Convert.ToInt16(FLOW_STIME2.Substring(0,2))*60+Convert.ToInt16(FLOW_STIME2.Substring(3,2)));
            byte[] btime2 = BitConverter.GetBytes(stime2);
              
            byte[] oid5 ={0x10,0x00,0x01,0x08,0x00,0x02};
            Int16 sitr2 = Convert.ToInt16(FLOW_ITRL2);
            byte[] bitr2 = BitConverter.GetBytes(sitr2);
             
            byte[] oid6 ={0x10,0x00,0x01,0x09,0x00,0x02};
            Int16 scnt2 = Convert.ToInt16(FLOW_CNT2);
            byte[] bscnt2 =BitConverter.GetBytes(scnt2);

            
            byte[] oid7 ={0x10,0x00,0x01,0x0A,0x00,0x02};
            Int16 srtime =(Int16)( Convert.ToInt16(FLOW_RTIME.Substring(0,2))*60+Convert.ToInt16(FLOW_RTIME.Substring(3,2)));
            byte[] bsrtime = BitConverter.GetBytes(srtime); 

            byte[] oid8 ={0x10,0x00,0x01,0x0B,0x00,0x02};
            Int16 srcnt = Convert.ToInt16(FLOW_RCNT);
            byte[] bsrcnt = BitConverter.GetBytes(srcnt);

            byte[] result = new byte[64];
            oid1.CopyTo(result,0);
            result[6]=btime1[1];
            result[7]=btime1[0];
            
            oid2.CopyTo(result,8);
            result[14] = bsitr1[1];
            result[15] = bsitr1[0];

            oid3.CopyTo(result,16);
            result[22] = bscnt1[1];
            result[23] = bscnt1[0];


            oid4.CopyTo(result,24);
            result[30] = btime2[1];
            result[31] = btime2[0];

            oid5.CopyTo(result,32);
            result[38] = bitr2[1];
            result[39] = bitr2[0];

            oid6.CopyTo(result,40);
            result[46] = bscnt2[1];
            result[47] = bscnt2[0];

            oid7.CopyTo(result,48);
            result[54] = bsrtime[1];
            result[55] = bsrtime[0];

             oid8.CopyTo(result,56);
            result[62] = bsrcnt[1];
            result[63] = bsrcnt[0];
 
            return result;

 
        }

        private byte[] getPressSetting()
        {
            byte[] oid1 ={0x10,0x00,0x02,0x04,0x00,0x02};
            Int16 stime1 = (Int16)(Convert.ToInt16(PRESS_STIME1.Substring(0,2))*60+Convert.ToInt16(PRESS_STIME1.Substring(3,2)));
            byte[] btime1 = BitConverter.GetBytes(stime1);
;             
            byte[] oid2 ={0x10,0x00,0x02,0x05,0x00,0x02};
            Int16 sitr1 = (Convert.ToInt16(PRESS_ITRL1));
            byte[] bsitr1 = BitConverter.GetBytes(sitr1);
              
            byte[] oid3 ={0x10,0x00,0x02,0x06,0x00,0x02};
            Int16 scnt1 = (Convert.ToInt16(PRESS_CNT1));
            byte[] bscnt1 = BitConverter.GetBytes(scnt1);

             byte[] oid4 ={0x10,0x00,0x02,0x07,0x00,0x02};
            Int16 stime2 =(Int16)( Convert.ToInt16(PRESS_STIME2.Substring(0,2))*60+Convert.ToInt16(PRESS_STIME2.Substring(3,2)));
            byte[] btime2 = BitConverter.GetBytes(stime2);
              
            byte[] oid5 ={0x10,0x00,0x02,0x08,0x00,0x02};
            Int16 sitr2 = Convert.ToInt16(PRESS_ITRL2);
            byte[] bitr2 = BitConverter.GetBytes(sitr2);
             
            byte[] oid6 ={0x10,0x00,0x02,0x09,0x00,0x02};
            Int16 scnt2 = Convert.ToInt16(PRESS_CNT2);
            byte[] bscnt2 =BitConverter.GetBytes(scnt2);

            
            byte[] oid7 ={0x10,0x00,0x02,0x0A,0x00,0x02};
            Int16 srtime =(Int16)( Convert.ToInt16(PRESS_RTIME.Substring(0,2))*60+Convert.ToInt16(PRESS_RTIME.Substring(3,2)));
            byte[] bsrtime = BitConverter.GetBytes(srtime); 

            byte[] oid8 ={0x10,0x00,0x02,0x0B,0x00,0x02};
            Int16 srcnt = Convert.ToInt16(PRESS_RCNT);
            byte[] bsrcnt = BitConverter.GetBytes(srcnt);

            byte[] result = new byte[64];
            oid1.CopyTo(result, 0);
            result[6] = btime1[1];
            result[7] = btime1[0];

            oid2.CopyTo(result, 8);
            result[14] = bsitr1[1];
            result[15] = bsitr1[0];

            oid3.CopyTo(result, 16);
            result[22] = bscnt1[1];
            result[23] = bscnt1[0];


            oid4.CopyTo(result, 24);
            result[30] = btime2[1];
            result[31] = btime2[0];

            oid5.CopyTo(result, 32);
            result[38] = bitr2[1];
            result[39] = bitr2[0];

            oid6.CopyTo(result, 40);
            result[46] = bscnt2[1];
            result[47] = bscnt2[0];

            oid7.CopyTo(result, 48);
            result[54] = bsrtime[1];
            result[55] = bsrtime[0];

            oid8.CopyTo(result, 56);
            result[62] = bsrcnt[1];
            result[63] = bsrcnt[0];
 
            return result;

        }

        private byte[] getNosieSetting()
        {
            byte[] oid1 ={0x10,0x00,0x03,0x04,0x00,0x02};
            Int16 stime1 = (Int16)(Convert.ToInt16(NOISE_STIME1.Substring(0,2))*60+Convert.ToInt16(NOISE_STIME1.Substring(3,2)));
            byte[] btime1 = BitConverter.GetBytes(stime1);
;             
            byte[] oid2 ={0x10,0x00,0x03,0x05,0x00,0x02};
            Int16 sitr1 = (Convert.ToInt16(NOISE_ITRL1));
            byte[] bsitr1 = BitConverter.GetBytes(sitr1);
              
            byte[] oid3 ={0x10,0x00,0x03,0x06,0x00,0x02};
            Int16 scnt1 = (Convert.ToInt16(NOISE_CNT1));
            byte[] bscnt1 = BitConverter.GetBytes(scnt1);

             byte[] oid4 ={0x10,0x00,0x03,0x07,0x00,0x02};
            Int16 stime2 =(Int16)( Convert.ToInt16(NOISE_STIME2.Substring(0,2))*60+Convert.ToInt16(NOISE_STIME2.Substring(3,2)));
            byte[] btime2 = BitConverter.GetBytes(stime2);
              
            byte[] oid5 ={0x10,0x00,0x03,0x08,0x00,0x02};
            Int16 sitr2 = Convert.ToInt16(NOISE_ITRL2);
            byte[] bitr2 = BitConverter.GetBytes(sitr2);
             
            byte[] oid6 ={0x10,0x00,0x03,0x09,0x00,0x02};
            Int16 scnt2 = Convert.ToInt16(NOISE_CNT2);
            byte[] bscnt2 =BitConverter.GetBytes(scnt2);

            
            byte[] oid7 ={0x10,0x00,0x03,0x0A,0x00,0x02};
            Int16 srtime =(Int16)( Convert.ToInt16(NOISE_RTIME.Substring(0,2))*60+Convert.ToInt16(NOISE_RTIME.Substring(3,2)));
            byte[] bsrtime = BitConverter.GetBytes(srtime); 

            byte[] oid8 ={0x10,0x00,0x03,0x0B,0x00,0x02};
            Int16 srcnt = Convert.ToInt16(NOISE_RCNT);
            byte[] bsrcnt = BitConverter.GetBytes(srcnt);

            byte[] result = new byte[64];
            oid1.CopyTo(result, 0);
            result[6] = btime1[1];
            result[7] = btime1[0];

            oid2.CopyTo(result, 8);
            result[14] = bsitr1[1];
            result[15] = bsitr1[0];

            oid3.CopyTo(result, 16);
            result[22] = bscnt1[1];
            result[23] = bscnt1[0];


            oid4.CopyTo(result, 24);
            result[30] = btime2[1];
            result[31] = btime2[0];

            oid5.CopyTo(result, 32);
            result[38] = bitr2[1];
            result[39] = bitr2[0];

            oid6.CopyTo(result, 40);
            result[46] = bscnt2[1];
            result[47] = bscnt2[0];

            oid7.CopyTo(result, 48);
            result[54] = bsrtime[1];
            result[55] = bsrtime[0];

            oid8.CopyTo(result, 56);
            result[62] = bsrcnt[1];
            result[63] = bsrcnt[0];
            return result;


        }

        private byte[] getLiquidSetting()
        {
              byte[] oid1 ={0x10,0x00,0x04,0x04,0x00,0x02};
            Int16 stime1 = (Int16)(Convert.ToInt16(LIQUID_STIME1.Substring(0,2))*60+Convert.ToInt16(LIQUID_STIME1.Substring(3,2)));
            byte[] btime1 = BitConverter.GetBytes(stime1);
;             
            byte[] oid2 ={0x10,0x00,0x04,0x05,0x00,0x02};
            Int16 sitr1 = (Convert.ToInt16(LIQUID_ITRL1));
            byte[] bsitr1 = BitConverter.GetBytes(sitr1);
              
            byte[] oid3 ={0x10,0x00,0x04,0x06,0x00,0x02};
            Int16 scnt1 = (Convert.ToInt16(LIQUID_CNT1));
            byte[] bscnt1 = BitConverter.GetBytes(scnt1);

             byte[] oid4 ={0x10,0x00,0x04,0x07,0x00,0x02};
            Int16 stime2 =(Int16)( Convert.ToInt16(LIQUID_STIME2.Substring(0,2))*60+Convert.ToInt16(LIQUID_STIME2.Substring(3,2)));
            byte[] btime2 = BitConverter.GetBytes(stime2);
              
            byte[] oid5 ={0x10,0x00,0x04,0x08,0x00,0x02};
            Int16 sitr2 = Convert.ToInt16(LIQUID_ITRL2);
            byte[] bitr2 = BitConverter.GetBytes(sitr2);
             
            byte[] oid6 ={0x10,0x00,0x04,0x09,0x00,0x02};
            Int16 scnt2 = Convert.ToInt16(LIQUID_CNT2);
            byte[] bscnt2 =BitConverter.GetBytes(scnt2);

            
            byte[] oid7 ={0x10,0x00,0x04,0x0A,0x00,0x02};
            Int16 srtime =(Int16)( Convert.ToInt16(LIQUID_RTIME.Substring(0,2))*60+Convert.ToInt16(LIQUID_RTIME.Substring(3,2)));
            byte[] bsrtime = BitConverter.GetBytes(srtime); 

            byte[] oid8 ={0x10,0x00,0x04,0x0B,0x00,0x02};
            Int16 srcnt = Convert.ToInt16(LIQUID_RCNT);
            byte[] bsrcnt = BitConverter.GetBytes(srcnt);

            byte[] result = new byte[64];
            oid1.CopyTo(result, 0);
            result[6] = btime1[1];
            result[7] = btime1[0];

            oid2.CopyTo(result, 8);
            result[14] = bsitr1[1];
            result[15] = bsitr1[0];

            oid3.CopyTo(result, 16);
            result[22] = bscnt1[1];
            result[23] = bscnt1[0];


            oid4.CopyTo(result, 24);
            result[30] = btime2[1];
            result[31] = btime2[0];

            oid5.CopyTo(result, 32);
            result[38] = bitr2[1];
            result[39] = bitr2[0];

            oid6.CopyTo(result, 40);
            result[46] = bscnt2[1];
            result[47] = bscnt2[0];

            oid7.CopyTo(result, 48);
            result[54] = bsrtime[1];
            result[55] = bsrtime[0];

            oid8.CopyTo(result, 56);
            result[62] = bsrcnt[1];
            result[63] = bsrcnt[0];
            return result;


        }
    }
}
