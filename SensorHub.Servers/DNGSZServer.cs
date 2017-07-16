using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Servers.Commands;
namespace SensorHub.Servers
{
    //苏州旧版，多功能协议
    public class DGNSZServer : AppServer<DGNSZSession>
    {
        public DGNSZServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", ",")))
        {

        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

    }
}
