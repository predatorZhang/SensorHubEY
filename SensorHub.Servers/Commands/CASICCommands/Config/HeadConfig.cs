using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class HeadConfig:Config
    {
        private byte version; //softwre version
        private byte[] devCode = new byte[6]; //code of device
        private byte[] pduType = new byte[2];
        private byte seq;
        private byte routeFlag;
        private byte[] dstAddr = new byte[2];

        //封装协议头（未加入CRC校验）
        public override byte[] getConfig(byte[] src)
        {
            int srcLen = src.Length;
            
            //[preamble 1] [version 1] [leng 2] [device_id 6] [routeFlag 1] [dstAddr 2] [pdu 2] [seq 1] [taglist N] [crc 2]

            //长度标识位计算
            Int16 intLen = (Int16)(6+1+2+2+1+srcLen);
            byte[] btlens0 = BitConverter.GetBytes(intLen);
            byte[] btlens  = new byte[2];
            btlens[0] = btlens0[1];
            btlens[1] = btlens0[0];
            
            //wrap the data before crc
            byte[] bfsrc = new byte[intLen + 2 + 1 + 1];
            bfsrc[0] = 0xA3;
            bfsrc[1] = this.version;
            btlens.CopyTo(bfsrc, 2);
            this.devCode.CopyTo(bfsrc, 4);
            bfsrc[10] = this.routeFlag;
            this.dstAddr.CopyTo(bfsrc,11);
            this.pduType.CopyTo(bfsrc, 13);
            bfsrc[15] = this.seq;
            src.CopyTo(bfsrc, 16);
            return bfsrc;
        }

      
        public HeadConfig(String version, String devCode, String pduType, String seq,String routeFlag,String dstAddr)
        {

            //获取版本号
            String hiVer = version.Substring(0, 1);
            String lowVer = version.Substring(1, 1);
            this.version = (byte)((CodeUtils.String2Byte(hiVer)) * 16 + CodeUtils.String2Byte(lowVer));

            //获取设备id
            for (int i = 0; i < 6; i++)
            {
                this.devCode[i] = CodeUtils.String2Byte(devCode.Substring(2 * i, 2));
            }

            //获取pduType:与埃德尔有区别
            this.pduType = getRespPduType(pduType);

            //路由Flag信息
            this.routeFlag = CodeUtils.String2Byte(routeFlag);

            //目标节点地址信息 
            for (int i = 0; i < 2; i++)
            {
                this.dstAddr[i] = CodeUtils.String2Byte(dstAddr.Substring(2 * i, 2));
            }

            //获取seq序列ID
            this.seq = CodeUtils.String2Byte(seq);
        }

        /**
         * StartupRequest 8  - StartupResponse 9 
         * OnlineRequest 6 - OnlineResponse 7 
         * GetResponse 2 - SetRequest 3
         * GetResponse 2 - GetRequest 1
         * TrapReque 4 - TrapResponse 5
         * 
         */
        private byte[] getRespPduType(String reqPduType)
        {
            byte[] result = new byte[2];

            Int16 btpduType = Int16.Parse(reqPduType, System.Globalization.NumberStyles.HexNumber);
            int opearType = (btpduType >> 8) & 0xFF;
            switch (opearType)
            {
                case 8:
                     result[0] = 0x09;
                     break;
                case 6:
                     result[0] = 0x07;
                     break;
                case 4:
                     result[0] = 0x05;
                     break;
                default:
                     result[0] = 0x0;
                     result[1] = 0x0;
                     break;
            }
            byte low = (byte)((btpduType & 0xFF)|0x80);
            result[1] = low;
            return result; 
        }

    }
}
