using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;
namespace SensorHub.Servers.Commands.WSCommands
{
    public class SewAcquireIntervalCmd : CommandBase<WSSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "SewAcquireInterval";
            }
        }

        public override void ExecuteCommand(WSSession session, StringRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("SewAcquireInterval:" + requestInfo.Body);

                if (session.WsConf != null)
                {
                    //如果有更新及更新数据库
                    session.WsConf.Status = true;
                    new DeviceConfig().Update(session.WsConf);
                    session.Logger.Info("成功更新有害气体监测仪配置信息");
                }

            }
            catch (Exception e)
            {
                session.Logger.Error("配置返回异常!");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
        }
    }
}
