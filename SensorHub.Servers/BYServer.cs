using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Servers.Commands;
using SensorHub.Utility;
namespace SensorHub.Servers
{
    //北洋光纤Server
    public class BYServer : AppServer<BYSession>
    {
        public BYServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", ",")))
        { 


        }

        protected override void OnStarted()
        {
            base.OnStarted();
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
        }

    }
}
