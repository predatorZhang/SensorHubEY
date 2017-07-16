using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Utility;
namespace SensorHub.Servers
{
    //二院：有害监测仪Server
   public  class WSServer : AppServer<WSSession>
    {
       public WSServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", ",")))
        { 

        }

        protected override void OnStarted()
        {
            base.OnStarted();
            ApplicationContext.getInstance().updateSequence("SEQ_WS_PERIOD_ID", "DBID", "WS_PERIOD_DATA");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_DEVICE_LOG_ID", "DBID", "ALARM_DEVICE_LOG");

            //有害气体监测仪
        }
    }
}
