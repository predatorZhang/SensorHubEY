using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;
using SuperSocket.Common;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class CollectSettingcConfig:TagConfig
    {
        public CollectSettingcConfig(Config config)
            : base(config)
        {

        }

        private static String CA_NOISE_ENABLE = System.Configuration.ConfigurationSettings.AppSettings["CA_NOISE_ENABLE"];

        private static String CA_NOISE_STIME1 = System.Configuration.ConfigurationSettings.AppSettings["CA_NOISE_STIME1"];
        private static String CA_NOISE_ITRL1 = System.Configuration.ConfigurationSettings.AppSettings["CA_NOISE_ITRL1"];
        private static String CA_NOISE_CNT1 = System.Configuration.ConfigurationSettings.AppSettings["CA_NOISE_CNT1"];
        private static String CA_NOISE_REPEAT = System.Configuration.ConfigurationSettings.AppSettings["CA_NOISE_REPEAT"];
      
        private int getBufferSize()
        {
            int result = 0;


            if (CA_NOISE_ENABLE == "true")
            {
               result++;
            }

            return result;

        }

        public override byte[] getConfig(byte[] src)
        {
            byte []temp = new byte[32*getBufferSize()];
          
            int flag = 0;

            if (CA_NOISE_ENABLE == "true")
            {
                  byte[] noiseSet = this.getNosieSetting();
                  noiseSet.CopyTo(temp,flag);
                  flag=flag+32;     
            }

            byte[] result = new byte[src.Length + temp.Length];
            src.CopyTo(result, 0);
            temp.CopyTo(result, src.Length);

            return base.getConfig(result);
        }

        private byte[] getNosieSetting()
        {
            byte[] oid1 ={0x10,0x00,0x01,0x04,0x00,0x02};
            Int16 stime1 = (Int16)(Convert.ToInt16(CA_NOISE_STIME1.Substring(0, 2)) * 60 + Convert.ToInt16(CA_NOISE_STIME1.Substring(3, 2)));
            byte[] btime1 = BitConverter.GetBytes(stime1);
;             
            byte[] oid2 ={0x10,0x00,0x01,0x05,0x00,0x02};
            Int16 sitr1 = (Convert.ToInt16(CA_NOISE_ITRL1));
            byte[] bsitr1 = BitConverter.GetBytes(sitr1);
              
            byte[] oid3 ={0x10,0x00,0x01,0x06,0x00,0x02};
            Int16 scnt1 = (Convert.ToInt16(CA_NOISE_CNT1));
            byte[] bscnt1 = BitConverter.GetBytes(scnt1);

             byte[] oid4 ={0x10,0x00,0x00,0x0A,0x00,0x02};
             Int16 stime2 = (Convert.ToInt16(CA_NOISE_REPEAT));
            byte[] btime2 = BitConverter.GetBytes(stime2);
              
  
            byte[] result = new byte[32];
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

            return result;


        }

    }
}
