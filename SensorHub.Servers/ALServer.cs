using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers
{
    //埃德尔报警推送server
    public class ALServer : AppServer<ALSession>
    {
        public ALServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", ",")))
        { 

        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }
    }
}
