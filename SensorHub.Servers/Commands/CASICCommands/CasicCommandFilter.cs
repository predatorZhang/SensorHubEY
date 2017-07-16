using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class CasicCommandFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            try
            {
                StringRequestInfo requestInfo = (StringRequestInfo)commandContext.RequestInfo;

                //TODO: construct the receving casic data
                String deviceId = requestInfo.Parameters[3];
                String pduType = requestInfo.Parameters[6];

                //print the receving data
                String devType = new CasicCmd().getDeviceTypeByPdu(pduType);
                String operType = new CasicCmd().getOpeTypeByPdu(pduType);

                if (devType != "集中器") {
                    Model.DeviceLogInfo1 log = new Model.DeviceLogInfo1();
                    log.DEVICECODE = deviceId;
                    log.DEVTYPE = devType;
                    log.OPERATETYPE = operType;
                    log.LOGTIME = DateTime.Now;
                    log.MESSAGE = operType;
                    new BLL.DeviceLog1().insert(log);
                }
               
            }
            catch (Exception e)
            {
                ((CasicSession)commandContext.Session).Logger.Error(e.Message);
            }
        }
        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {
         
        }



       
    }
}
