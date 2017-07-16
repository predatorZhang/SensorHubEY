using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;

namespace SensorHub.Servers
{
    public class SZSLNoiseSession : AppSession<SZSLNoiseSession, StringRequestInfo>
    {
        private string macId;
        public string MacID
        {
            get { return macId; }
            set { macId = value; }
        }

        private DeviceConfigInfo conf;
        public DeviceConfigInfo Conf
        {
            get { return conf; }
            set { conf = value; }
        }

       
    }
}
