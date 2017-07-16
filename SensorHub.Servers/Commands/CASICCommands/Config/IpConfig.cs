using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;
using SuperSocket.Common;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class IpConfig:TagConfig
    {
        public IpConfig(Config config)
            : base(config)
        {

        }
    
        // 10000051 0006 年月日时分秒
        // 10000022 
        public override byte[] getConfig(byte[] src)
        {
            //获取iptag信息
            String adIp = System.Configuration.ConfigurationSettings.AppSettings["AD_IP"];
            String adPort = System.Configuration.ConfigurationSettings.AppSettings["AD_PORT"];
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

            byte[] result = new byte[src.Length + ipTag.Length];
            src.CopyTo(result, 0);
            ipTag.CopyTo(result, src.Length);

            return base.getConfig(result);
        }
    }
}
