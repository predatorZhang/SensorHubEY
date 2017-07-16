using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers
{
    public class ALSession : AppSession<ALSession, StringRequestInfo>
    {
        private string macID;
        private string deviceId;
        private string regionId;

        public string MacID
        {
            get { return macID; }
            set { macID = value; }
        }

        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        public string RegionId
        {
            get { return RegionId; }
            set { RegionId = value; }
        }
    }
}
