using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;
using SuperSocket.Common;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class SystemTimeConfig:TagConfig
    {
        public SystemTimeConfig(Config config):base(config)
        {

        }
        //byte[] crc = new byte[0]; in case of empty src

        // 10000051 0006 年月日时分秒
        public override byte[] getConfig(byte[] src)
        {
            byte[] sysTag = { 0x10,0x00,0x00,0x51,0x00,0x06,
            StringUtil.YEAR, StringUtil.MON, StringUtil.DAY, StringUtil.HOR, StringUtil.MIN, StringUtil.SEC };

            byte[] result = new byte[src.Length + sysTag.Length];
            src.CopyTo(result,0);
            sysTag.CopyTo(result,src.Length);

            return base.getConfig(result);
        }
    }
}
