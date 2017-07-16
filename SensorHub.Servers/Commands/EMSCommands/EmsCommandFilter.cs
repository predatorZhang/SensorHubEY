using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;

namespace SensorHub.Servers.Commands.EMSCommands
{
    public class EmsCommandFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            try
            {                
                EmsSession session = (EmsSession)commandContext.Session;
                session.Logger.Info("#############################Filter开始执行#############################");
                if (session.Initial)
                {
                    session.Logger.Info("已经登录过啦！");
                    return;
                }
                StringRequestInfo request = (StringRequestInfo)commandContext.RequestInfo;
                string code = request.Parameters[0];
                string[] dataList = code.Split('$');
                string patrolerName = dataList[0];
                string macId = dataList[1];
                byte[] data = new byte[128];

                Model.Patroler patroler = new BLL.Patroler().getPatrolerByName(patrolerName);
                if (null == patroler)
                {
                    session.Logger.Info("没有找到巡检员:" + patrolerName);
                    data = Encoding.UTF8.GetBytes("none" + "\r\n");
                    session.Send(data, 0, data.Length);
                    commandContext.Cancel = true;
                    return;
                }

                Model.Equipment equipment = new BLL.Equipment().getEquipmentByMacId(macId);
                if (null == equipment)
                {
                    session.Logger.Info("没有找到设备:" + macId);
                    data = Encoding.UTF8.GetBytes("none" + "\r\n");
                    session.Send(data, 0, data.Length);
                    commandContext.Cancel = true;
                    return;
                }

                session.Patroler = patroler;
                session.Equipment = equipment;
                session.Initial = true;
                session.Logger.Info("已经找到设备和人了");
                session.Logger.Info("#############################Filter成功执行#############################");
                session.Logger.Info("\n");
                session.Logger.Info("\n");
                session.Logger.Info("\n");
            }
            catch (Exception e)
            {
                commandContext.Session.Logger.Error("#############################EMSFilter执行错误#############################");
                commandContext.Session.Logger.Error(e.ToString());
                commandContext.Session.Logger.Error("\n");
                commandContext.Session.Logger.Error("\n");
                commandContext.Session.Logger.Error("\n");
            }
        }
        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {
        }
    }
}
