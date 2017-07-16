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
     * 
     * 平台先发送配置信息
     * 设备返回配置信息
     **/
    public class RQSettingRespCmd : CommandBase<RQSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "7B-89-00-13";
            }
        }

        public override void ExecuteCommand(RQSession session, BinaryRequestInfo requestInfo)
        {
            try
            {
                byte[] rbody = requestInfo.Body;
                session.Logger.Info("燃气智能监测终端配置修改成功:" + BitConverter.ToString(rbody, 0, rbody.Length));

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
