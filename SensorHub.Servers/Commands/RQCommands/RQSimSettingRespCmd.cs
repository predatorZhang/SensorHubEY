using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.RQCommands
{
    public class RQSimSettingRespCmd : CommandBase<RQSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "7B-89-00-15";
            }
        }

        public override void ExecuteCommand(RQSession session, BinaryRequestInfo requestInfo)
        {
            byte[] body = requestInfo.Body;
            //if (body[13] == 0x01)
            //{
            //    session.Logger.Info("燃气SIM卡配置成功");
            //}
            //if (body[13] == 0x00)
            //{
            //    session.Logger.Info("燃气SIM卡配置失败");
            //}
            session.Logger.Info(BitConverter.ToString(body, 0, body.Length));

            ((RQServer)session.AppServer).predatorServer.DispatchMessage("",
                BitConverter.ToString(body, 0, body.Length));
        }
    }
}
