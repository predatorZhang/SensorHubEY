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
    /*
     * 埃德尔新版通信协议
     */
    public class AdlerServer : AppServer<AdlerSession>
    {
        public AdlerServer()
            : base(new DefaultReceiveFilterFactory<AdlerReceiveFilter, StringRequestInfo>())
        {

        }

        protected override void OnStarted()
        {
            base.OnStarted();
            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_FLOW_ID", "DBID", "AD_DJ_FLOW");
            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_NOISE_ID", "DBID", "AD_DJ_NOISE");
            ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_PRESS_ID", "DBID", "AD_DJ_PRESS");
            ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
        }
    }
}
