using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
namespace SensorHub.Servers
{
    public class WSSession : AppSession<WSSession, StringRequestInfo>
    {
        public string macID;
        public string MacID
        {
            get { return macID; }
            set { macID = value; }
        }

        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }


        private DeviceConfigInfo wsConf;

        public DeviceConfigInfo WsConf
        {
            get { return wsConf; }
            set { wsConf = value; }
        }

    }
}
