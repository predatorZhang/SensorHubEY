using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Common;
using SensorHub.Utility;
namespace SensorHub.Servers
{

    public class AdlerReceiveFilter : BeginEndMarkReceiveFilter<StringRequestInfo>
    {
        //ascii编码 A3开始，\r\n作为结尾
        private readonly static byte[] BeginMark = new byte[] { (byte)0x63 };
        private readonly static byte[] EndMark = new byte[] { 0x0D,0x0A };

        public AdlerReceiveFilter()
            : base(BeginMark, EndMark) 
        {

        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {

            //TODO LIST: decode the readBuffer

            byte[] src = CodeUtils.adDecode(readBuffer.CloneRange(offset, length - 2));

            String data = BitConverter.ToString(src, 0, src.Length).Replace("-", "");

            //TODO: construct the receving adler data
            //String data = Encoding.ASCII.GetString(src, 0, src.Length);
            //preamble(1B) version(1B) leng(2B) deviceId(4B) Pdutype(1B) seq(1B) oid-list/tag-list(N) crc(2B)
            String preamble = data.Substring(0, 2);
            String version = data.Substring(2, 2);
            String leng = data.Substring(4, 4);
            String deviceId = data.Substring(8, 8);
            String pduType = data.Substring(16, 2);
            String seq = data.Substring(18, 2);
            String settings = data.Substring(20, data.Length - 4 - 12 * 2); //减去结尾回车换行符号，减去总的字节数
            String crcs = data.Substring(data.Length - 8, 4);

            String result = "Adler:" + preamble + "," + version + "," +
                leng + "," + deviceId + "," + pduType + "," +
                seq + "," + settings + "," + crcs;

            BasicRequestInfoParser m_Parser = new BasicRequestInfoParser(":", ",");

            return m_Parser.ParseRequestInfo(result);
            
        }
    }

}
