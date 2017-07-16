using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers
{
    public class SZSLNoiseSwitchFilter:IReceiveFilter<StringRequestInfo>
    {
        private IReceiveFilter<StringRequestInfo> m_FilterA;
        private byte m_BeginMarkA = (byte)'S';

        private IReceiveFilter<StringRequestInfo> m_FilterB;
        private byte m_BeginMarkB = (byte)'R';

        private IReceiveFilter<StringRequestInfo> m_FilterC;
        private byte m_BeginMarkC = (byte)'I';

        public SZSLNoiseSwitchFilter()
        {
            m_FilterA = new SZSLNoiseCommandLineFilter(this);
            m_FilterB = new SZSLNoiseRepeatFilter(this);
            m_FilterC = new SZSLNoiseIpconfigFilter(this);
        }

        public StringRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = length;
            var flag = readBuffer[offset];

            if (flag == m_BeginMarkA)
                NextReceiveFilter = m_FilterA;
            else if (flag == m_BeginMarkB)
                NextReceiveFilter = m_FilterB;
            else if (flag == m_BeginMarkC)
                NextReceiveFilter = m_FilterC;
            else
                State = FilterState.Error;
            return null;
        }

        public int LeftBufferSize { get; private set; }

        public IReceiveFilter<StringRequestInfo> NextReceiveFilter { get; private set; }

        public void Reset()
        {

        }

        public FilterState State { get; private set; }
    }
}
