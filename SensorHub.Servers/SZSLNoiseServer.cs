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
    public class SZSLNoiseServer : AppServer<SZSLNoiseSession>
    {
        public SZSLNoiseServer()
            : base(new DefaultReceiveFilterFactory<SZSLNoiseSwitchFilter, StringRequestInfo>())
        {

        }

        protected override void OnStarted()
        {
            base.OnStarted();
            ApplicationContext.getInstance().updateSequence("SEQ_AD_SL_NOISE_ID", "DBID", "AD_SL_NOISE");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_DEVICE_LOG_ID", "DBID", "ALARM_DEVICE_LOG");
        }
    }
}
