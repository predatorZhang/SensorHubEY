using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
namespace SensorHub.Servers.Commands.RQCommands
{
    /**
     * 设置燃气设置信息
     * 平台-》设备配置信息请求
     * 设备->平台配置成功。
     * */
    [RQCommandFilter]
    public class RQTimeSettingRespCmd : CommandBase<RQSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "7B-89-00-14";
            }
        }

        public override void ExecuteCommand(RQSession session, BinaryRequestInfo requestInfo)
        {
            byte[] rBody = requestInfo.Body;
            session.Logger.Info("燃气校时配置成功:" + BitConverter.ToString(rBody, 0, rBody.Length));
            session.Message = "修改时间配置成功";
            //((RQServer)session.AppServer).predatorServer.DispatchMessage("",
            //      BitConverter.ToString(rBody, 0, rBody.Length));
        }
    }
}
