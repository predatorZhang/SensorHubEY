using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.YLCommands
{  /**
    * 事务命令：0x05，更新时间，返回的响应
    * */
    public class YLGetTimeCmd : CommandBase<YLSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "0x05";
            }
        }

        public override void ExecuteCommand(YLSession session, BinaryRequestInfo requestInfo)//服务器接收到的二进制的请求实例当做参数被传进来
        {
            session.Logger.Info("雨量计校时返回帧：" + requestInfo.Body);

        }
    }
}
