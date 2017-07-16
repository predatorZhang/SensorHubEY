using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;
namespace SensorHub.Servers.Commands.ADCommands
{
    public class HeadConfig:Config
    {
        private byte version; //softwre version
        private byte[] devCode = new byte[4]; //code of device
        private byte pduType;
        private byte seq;     

        //封装协议头（未加入CRC校验）
        public override byte[] getConfig(byte[] src)
        {           
            int srcLen = src.Length;
            
            //[preamble 1] [version 1] [leng 2] [device_id 4] [pdu 1] [seq 1] [taglist N] [crc 2]
            //长度标识位计算
            Int16 intLen = (Int16)(4 + 1 + 1 + srcLen);
            byte[] btlens0 = BitConverter.GetBytes(intLen);
            byte[] btlens  = new byte[2];
            btlens[0] = btlens0[1];
            btlens[1] = btlens0[0];
            
            //wrap the data before crc
            byte[] bfsrc = new byte[intLen + 2 + 1 + 1];
            bfsrc[0] = 0xA3;
            bfsrc[1] = this.version;
            btlens.CopyTo(bfsrc, 2);
            this.devCode.CopyTo(bfsrc,4);
            bfsrc[8] = this.pduType;
            bfsrc[9] = this.seq;
            src.CopyTo(bfsrc, 10);

            return bfsrc;
        }

      
        public HeadConfig(String version, String devCode, String pduType, String seq)
        {

            //获取版本号
            String hiVer = version.Substring(0, 1);
            String lowVer = version.Substring(1, 1);
            this.version = (byte)((CodeUtils.String2Byte(hiVer)) * 16 + CodeUtils.String2Byte(lowVer));

            //获取设备id
            for (int i = 0; i < 4; i++)
            {
                this.devCode[i] = CodeUtils.String2Byte(devCode.Substring(2 * i, 2));
            }

            //获取pduType
            byte btPduType = CodeUtils.String2Byte(pduType);
            this.pduType = (byte)(0xB0 | (0x0f & btPduType));

            //获取seq序列ID
            this.seq = CodeUtils.String2Byte(seq);

        }

    }
}
