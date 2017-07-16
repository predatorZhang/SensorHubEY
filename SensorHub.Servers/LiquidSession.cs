using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers
{
    public class LiquidSession : AppSession<LiquidSession, StringRequestInfo>
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
    }
}
