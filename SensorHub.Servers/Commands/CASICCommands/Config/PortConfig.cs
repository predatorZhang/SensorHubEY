using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;
using SuperSocket.Common;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class PortConfig:TagConfig
    {
        public PortConfig(Config config)
            : base(config)
        {

        }
    
   
        // 10000023 len port
        // port value is encoded with ascii
        public override byte[] getConfig(byte[] src)
        {
            //获取porttag信息
            String adPort = System.Configuration.ConfigurationSettings.AppSettings["AD_PORT"];
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

            byte[] result = new byte[src.Length + portTag.Length];
            src.CopyTo(result, 0);
            portTag.CopyTo(result, src.Length);

            return base.getConfig(result);
        }
    }
}
