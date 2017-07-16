using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
namespace SensorHub.Servers
{
   public  class SZSLNoiseIpconfigFilter:TerminatorReceiveFilter
   {
       private SZSLNoiseSwitchFilter switchFilter;
        
        private static Encoding m_Encoding= Encoding.Default;
        private static byte[] m_Terminator = Encoding.Default.GetBytes("G");
        private static BasicRequestInfoParser m_Parser = new BasicRequestInfoParser(":", ",");

        public SZSLNoiseIpconfigFilter(SZSLNoiseSwitchFilter switcher)
            : base(m_Terminator,m_Encoding,m_Parser)
        {
            switchFilter = switcher;
        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
          //  var requestInfo = m_Parser.ParseRequestInfo(Encoding.ASCII.GetString(readBuffer, offset, length));
          
            NextReceiveFilter = switchFilter;
            return new StringRequestInfo("IPCONFIG", string.Empty, Encoding.ASCII.GetString(Convert.FromBase64String(String.Empty)).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));;
        }

   }
}
