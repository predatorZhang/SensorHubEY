using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;
using SuperSocket.Common;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class TrapRespConfig:TagConfig
    {
        public TrapRespConfig(Config config)
            : base(config)
        {

        }

        private UInt16 noSeq;
        public UInt16 NoSeq
        {
            get { return noSeq; }
            set { noSeq = value; }
        }

       
        // 6000 0300 len port
        // port value is encoded with ascii
        public override byte[] getConfig(byte[] src)
        {
            

            byte[] btRespOidAndLen = { 0x60, 0x00, 0x03, 0x00,0x00,0x02 };
           // byte[] btSeq = BitConverter.GetBytes(noSeq);
            String strCrc = String.Format("{0:X}", noSeq);
            String strDst = this.wrap16String(strCrc);
            byte[] btSeq = { CodeUtils.String2Byte(strDst.Substring(0, 2)), CodeUtils.String2Byte(strDst.Substring(2, 2)) };
                 
            byte[] portResp = new byte[4 + 2 + 2];
            btRespOidAndLen.CopyTo(portResp, 0);
            btSeq.CopyTo(portResp, 6);

            byte[] result = new byte[src.Length + portResp.Length];
            src.CopyTo(result, 0);
            portResp.CopyTo(result, src.Length);

            return base.getConfig(result);
        }

        private String wrap16String(String src)
        {
            if (src.Length > 4)
            {
                return "";
            }
            String dst = src;
            for (int i = 0; i < 4 - src.Length; i++)
            {
                dst = "0" + dst;
            }
            return dst;
        }
    }
}
