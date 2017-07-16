using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Common;
using SuperSocket.Facility.Protocol;

namespace SensorHub.Servers.Commands
{
    public class RQFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
       public RQFilter()
            :base(6)
        { 

        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
           return (int)header[offset + 4] * 256 + (int)header[offset + 5];
    
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new BinaryRequestInfo(BitConverter.ToString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }
     }
}
