using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
namespace SensorHub.Servers
{
    public class GXSession : AppSession<GXSession, StringRequestInfo>
    {
        private string macID;

        private long id;

        private string productCompany;

        private string sensorType;

        public string SensorType
        {
            get { return sensorType; }
            set { sensorType = value; }
        }

        public string ProductCompany
        {
            get { return productCompany; }
            set { productCompany = value; }
        }

        public string MacID
        {
            get { return macID; }
            set { macID = value; }
        }

        public long ID
        {
            get { return id; }
            set { id = value; }
        }
    }
}
