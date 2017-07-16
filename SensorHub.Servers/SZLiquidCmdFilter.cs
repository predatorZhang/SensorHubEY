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
    public class SZLiquidCmdFilter : BeginEndMarkReceiveFilter<StringRequestInfo>
    {
        /*
        private static Encoding m_Encoding = Encoding.Default;
        private static byte[] m_Terminator = Encoding.Default.GetBytes("\r\n");
        private static BasicRequestInfoParser m_Parser = new BasicRequestInfoParser(":", ",");
         * */

        private readonly static byte[] BeginMark = new byte[] { 0xAA };
        private readonly static byte[] EndMark = new byte[] { 0x0D, 0x0A };
        private static BasicRequestInfoParser m_Parser = new BasicRequestInfoParser(":", ",");

        public SZLiquidCmdFilter()
            : base(BeginMark, EndMark)
        {

        }
        /*
        public SZLiquidCmdFilter()
            : base(m_Terminator, m_Encoding, m_Parser)
        {

        }
         * */

        protected override StringRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
            byte[] head = new byte[9];
            Buffer.BlockCopy(readBuffer, offset, head, 0, 9);
           
            var requestInfo = m_Parser.ParseRequestInfo(Encoding.ASCII.GetString(readBuffer, offset + 9, length - 9-2) + ',' +
                                             BitConverter.ToString(head).Replace("-",""));

            // var requestInfo = m_Parser.ParseRequestInfo(Encoding.ASCII.GetString(readBuffer, offset + 9, length - 9));
            return requestInfo;
        }
    }
}
