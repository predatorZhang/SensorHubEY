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
    public class SZLiquidServer : AppServer<SZLiquidSession>
    {
        /*
        public SZLiquidServer()
            : base(new DefaultReceiveFilterFactory<SZLiquidCmdFilter, StringRequestInfo>()) 
        { 

        }
         * */

        public SZLiquidServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", ",")))
        { 


        }

        protected override void OnStarted()
        {
            base.OnStarted();
            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_LIQUID", "DBID", "AD_DJ_LIQUID");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_DEVICE_LOG_ID", "DBID", "ALARM_DEVICE_LOG");
        }

    }
}
