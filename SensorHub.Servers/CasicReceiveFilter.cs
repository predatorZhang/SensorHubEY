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

    public class CasicReceiveFilter : FixedHeaderReceiveFilter<StringRequestInfo>
    {
        //固定header共6个字节，preamble（1字节） version（1字节）长度（2字节）
        /*
        private readonly static byte[] BeginMark = new byte[] { (byte)0xA3 };
        private readonly static byte[] EndMark = new byte[] { 0x0D,0x0A };
         * */

        public CasicReceiveFilter()
            : base(4)
        {

        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return (int)(header[offset + 2] * 256 + (int)header[offset + 3]+2);
        }


        protected override StringRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {

            byte[] tmpHead = header.Array;
            byte[] tmpBody = bodyBuffer.CloneRange(offset, length);

            byte[] src = new byte[tmpHead.Length + tmpBody.Length];

            tmpHead.CopyTo(src, 0);
            tmpBody.CopyTo(src, tmpHead.Length);

            String data = BitConverter.ToString(src, 0, src.Length).Replace("-", "");

            //TODO: construct the receving casic data
            String preamble = data.Substring(0, 2);
            String version = data.Substring(2, 2);
            String leng = data.Substring(4, 4);
            String deviceId = data.Substring(8, 12);
            String routeFlag = data.Substring(20, 2);
            String dstNodeAddr = data.Substring(22, 4);
            String pduType = data.Substring(26, 4);
            String seq = data.Substring(30, 2);
            String settings = data.Substring(32, data.Length - 18 * 2);
            String crcs = data.Substring(data.Length - 4, 4);

            String result = "Casic:" + preamble + "," + version + "," +
                leng + "," + deviceId + "," + routeFlag + "," + dstNodeAddr + "," + pduType + "," +
                seq + "," + settings + "," + crcs;

            BasicRequestInfoParser m_Parser = new BasicRequestInfoParser(":", ",");

            return m_Parser.ParseRequestInfo(result);
          //  return new BinaryRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }
    }

}
