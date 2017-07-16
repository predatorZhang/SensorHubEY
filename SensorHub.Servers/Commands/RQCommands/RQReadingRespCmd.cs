using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
namespace SensorHub.Servers.Commands.RQCommands
{
    /**
     * 读取采集器配置信息
     * 平台先发送读取采集器配置信息
     * 设备将自身配置信息发送回来
     **/
    [RQCommandFilter]
    public class RQReadingRespCmd : CommandBase<RQSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "7B-89-00-12";
            }
        }

        public override void ExecuteCommand(RQSession session, BinaryRequestInfo requestInfo)
        {
            try
            {
                byte[] rbody = requestInfo.Body;
                session.Logger.Info("接收到燃气配置信息:" + BitConverter.ToString(rbody, 0, rbody.Length));

                //((RQServer)session.AppServer).predatorServer.DispatchMessage("",
                //    BitConverter.ToString(rbody, 0, rbody.Length));
            }
            catch (Exception e)
            {
                session.Logger.Error("处理燃气设备采集器信息异常!" + e.ToString());
            }
        }
    }


}
