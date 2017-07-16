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
    public class AdlerSession : AppSession<AdlerSession, StringRequestInfo>
    {
        private string macID;
        private string productCompany;
        private uint batch = 0;

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

        public uint Batch
        {
            get { return batch; }
            set { batch = value; }
        }
    }
}
