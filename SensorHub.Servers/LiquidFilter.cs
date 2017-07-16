using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;
using SuperSocket.Common;
using SensorHub.Utility;

namespace SensorHub.Servers
{
    class LiquidFilter : BeginEndMarkReceiveFilter<StringRequestInfo>
    {
        #region

        private readonly static byte[] BeginMark = new byte[] { 0xAA };
        private readonly static byte[] EndMark = new byte[] { 0x0D, 0x0A };

        public LiquidFilter()
            : base(BeginMark, EndMark)
        {

        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
            string set = "heartbeat";
            String com = "";

            if (length == set.Length + 15)
            {
                com = Encoding.UTF8.GetString(readBuffer.CloneRange(offset + 1, set.Length));
            }

            if ((length == set.Length + 15) && (com == set))
            {
                String command = Encoding.UTF8.GetString(readBuffer.CloneRange(offset + 1, set.Length));
                String body = Encoding.UTF8.GetString(readBuffer.CloneRange(offset, length));
                String mBody = body.Substring(0, body.Length - 2);
                String[] param = mBody.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //Console.WriteLine("液位监测仪器指令头：" + Encoding.UTF8.GetString(header.Array, header.Offset, 9)); //shao debug
                return new StringRequestInfo(command, mBody, param);
            }
            else if (length > 19 && 170 == readBuffer[offset])
            {
                byte[] readBufferLocal = readBuffer.CloneRange(offset, length);
                //检查数据正确性
                //1.检查数据中间是否有AA
                for (int i = 1; i < readBufferLocal.Length; i++)
                {
                    if (170 == readBufferLocal[i])
                    {
                        readBufferLocal = readBufferLocal.CloneRange(0, i);
                        break;
                    }
                }
                //2.检查数据长度
                if (readBufferLocal[6] + 9 > readBufferLocal.Length)
                {
                    String bodyError = "";
                    for (int i = 19; i < readBufferLocal.Length; i++)
                    {
                        bodyError += readBufferLocal[i].ToString("X2");
                    }
                    String mBodyError = "数据长度不够," + bodyError.Substring(0, bodyError.Length);
                    String[] paramError = mBodyError.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return new StringRequestInfo("ERROR:", mBodyError, paramError);
                }
                if (readBufferLocal[6] + 9 < readBufferLocal.Length)
                {
                    readBufferLocal = readBufferLocal.CloneRange(0, readBufferLocal[6] + 9);
                }
                //3.CRC校验
                String crcIn = "";
                byte crcHigh = readBufferLocal[7];
                byte crcLow = readBufferLocal[8];
                readBufferLocal[7] = 0;
                readBufferLocal[8] = 0;
                for (int i = 0; i < readBufferLocal.Length; i++)
                {
                    crcIn += readBufferLocal[i].ToString("X2");
                }
                ushort crcOut = CodeUtils.QuickCRC16(crcIn, 0, crcIn.Length);
                byte[] crcOutByte = BitConverter.GetBytes(crcOut);
                if (crcHigh != crcOutByte[1] || crcLow != crcOutByte[0])
                {
                    String bodyError = "";
                    for (int i = 19; i < readBufferLocal.Length; i++)
                    {
                        bodyError += readBufferLocal[i].ToString("X2");
                    }
                    String mBodyError = "CRC校验错误," + bodyError.Substring(0, bodyError.Length);
                    String[] paramError = mBodyError.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return new StringRequestInfo("ERROR:", mBodyError, paramError);
                }

                //处理信息
                String header9 = "";
                for (int i = 0; i < 9; i++)
                {
                    header9 += readBufferLocal[i].ToString("X2");
                }
                String command = Encoding.UTF8.GetString(readBufferLocal.CloneRange(9, 10));
                String body = Encoding.UTF8.GetString(readBufferLocal.CloneRange(19, readBufferLocal[6] - 10));

                String mBody = body.Substring(0, body.Length - 2) + ',' + header9;
                String[] param = mBody.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                return new StringRequestInfo(command, mBody, param);
            }

            return new StringRequestInfo("NULL", string.Empty, Encoding.ASCII.GetString(Convert.FromBase64String(String.Empty)).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)); //return null;
        }
        #endregion
    }

}
