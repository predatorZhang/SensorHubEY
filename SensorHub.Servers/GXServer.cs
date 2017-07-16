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
    //诺克光纤Server
    public class GXServer : AppServer<GXSession>
    {
        public GXServer()
            : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser(":", ",")))
        { 


        }

        protected override void OnStarted()
        {
            base.OnStarted();
            ApplicationContext.getInstance().updateSequence("SEQ_NK_GX_STRESS_CURVE_ID", "DBID", "NK_GX_STRESS_CURVE");
            ApplicationContext.getInstance().updateSequence("SEQ_NK_GX_TEMPERATURE_CURVE_ID", "DBID", "NK_GX_TEMPERATURE_CURVE");
            ApplicationContext.getInstance().updateSequence("SEQ_NK_GX_VIBRATING_CURVE_ID", "DBID", "NK_GX_VIBRATING_CURVE");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_DEVICE_LOG_ID", "DBID", "ALARM_DEVICE_LOG");
        }

    }
}
