using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Utility;

namespace SensorHub.Servers.Commands
{
    public class LSStalkOverCmd : CommandBase<SZLiquidSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "STALKOVER";
            }
        }

        public override void ExecuteCommand(SZLiquidSession session, StringRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("成功接收终端上传的会话结束帧:" + requestInfo.Body);
            }
            catch (Exception e)
            {
                session.Logger.Error("接收终端上传的会话结束帧失败" + requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
            finally
            {
            }
        }
    }
}
