using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.RQCommands
{
    public class RQCommandFilter : CommandFilterAttribute
    {
        //命令执行前调用
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            BinaryRequestInfo request = (BinaryRequestInfo)commandContext.RequestInfo;
            byte[] sim = new byte[6];
            byte[] header = headerToByteArray(request.Key);
            byte[] body = request.Body;
            Buffer.BlockCopy(body, 0, sim, 0, 6);
            if (!Utility.CodeUtils.CRC16_validate(header, body))
            {
                commandContext.Cancel = true;
            }

            //sim卡号
            string code = BitConverter.ToString(sim, 0, sim.Length).Replace("-", "").Substring(1);

            //打印燃气设备号
            commandContext.Session.Logger.Info("燃气智能检测终端编号:"+code);

            object obj = new BLL.Device().getDeviceIdByCode(code);
            if (null == obj || string.IsNullOrEmpty(obj.ToString()))
            {
                commandContext.Cancel = true;
            }
            else
            {
                RQSession session = (RQSession)commandContext.Session;
                session.ID = Convert.ToInt32(obj);
            }
        }
        //命令执行后调用
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {
          
        }

        private byte[] headerToByteArray(string header)
        {
            string[] strArray = header.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] btArray = new byte[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                btArray[i] = Convert.ToByte(strArray[i], 16);
            }
            return btArray;
        }
    }
}
