using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;
using SuperSocket.Common;

namespace SensorHub.Servers
{
    public class YLCmdFilter : BeginEndMarkReceiveFilter<BinaryRequestInfo>//继承带起止符的协议
    {

        private readonly static byte[] BeginMark = new byte[] {0x48,0x43};
        private readonly static byte[] EndMark = new byte[] {0xAF};
        private static BasicRequestInfoParser m_Parser = new BasicRequestInfoParser();//用哪个解析器去解析请求数据 返回二进制的请求实体
        
        public YLCmdFilter()
            : base(BeginMark,EndMark)//传入开始标记和结束标记
        { 

        }

        protected override BinaryRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
            //应该是解析帧头和帧尾
            byte[] requestInfo = new byte[1];
            byte[] cutReadBuffer = new byte[ readBuffer.Length-3 ];
            Buffer.BlockCopy(readBuffer, 2, cutReadBuffer, 0, readBuffer.Length - 3);
            Buffer.BlockCopy(cutReadBuffer,4,requestInfo,0,1);
            String requestInfoS ="0x"+ BitConverter.ToString(requestInfo);
            return new BinaryRequestInfo(requestInfoS, cutReadBuffer);

        }
       

    }
}
