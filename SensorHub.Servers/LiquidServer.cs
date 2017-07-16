using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers
{
   public  class LiquidServer : AppServer<LiquidSession>
    {
       public LiquidServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", ",")))
        { 

        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }
    }
}
