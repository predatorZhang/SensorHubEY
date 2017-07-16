using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Servers.Commands.YLCommands;

namespace SensorHub.Servers.Commands.YLCommands
{
    public class YLGetDeIdCmd : CommandBase<YLSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "0x00";
            }
        }

        public override void ExecuteCommand(YLSession session, BinaryRequestInfo requestInfo)
        {
            byte[] result = requestInfo.Body;
            
            //将采集到的数据解析出来
        }
    }
}
