using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Servers.Commands;
using SuperSocket.Facility.Protocol;
using SensorHub.Utility;
namespace SensorHub.Servers
{
    //EMS系统 服务器端server
    public class EmsServer : AppServer<EmsSession>
    {
       
        public EmsServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default,new BasicRequestInfoParser(":",","))) // 7 parts but 8 separators
        {
            
        }

        protected override void OnStarted()
        {
            base.OnStarted();

            ApplicationContext.getInstance().updateSequence("SEQ_POSITION_ID", "DBID", "POSITION");
            ApplicationContext.getInstance().updateSequence("SEQ_LOGINFO_ID", "DBID", "LOGINFO");

        }
    }
}
