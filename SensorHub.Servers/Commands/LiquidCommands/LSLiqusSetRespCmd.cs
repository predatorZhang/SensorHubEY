using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;
using SensorHub.Utility;

namespace SensorHub.Servers.Commands
{
    public class LSLiqusSetRespCmd : CommandBase<SZLiquidSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "LSSETRESP";
            }
        }

        public override void ExecuteCommand(SZLiquidSession session, StringRequestInfo requestInfo)
        {
           
            try
            {
                session.Logger.Info("LSSETRESP：" + requestInfo.Body + "服务器下发液位设置成功：");
                if (session.LiquidConf != null)
                {
                    //如果有更新及更新数据库
                    session.LiquidConf.Status = true;
                    new DeviceConfig().Update(session.LiquidConf);
                    session.Logger.Info("成功更新液位计配置信息");
                }

                byte[] setBody = {  0x50, 
                                0x00, 0x09,
                                0x02,
                                0x00, 0x00,0x34,
                                0x00,
                                0x03};

                string head = "LSSETRESP:";
                byte[] btHead = System.Text.Encoding.Default.GetBytes(head);
                byte[] result = new byte[setBody.Length + btHead.Length];
                Buffer.BlockCopy(btHead,0,result,0,btHead.Length);
                Buffer.BlockCopy(setBody, 0, result, btHead.Length, setBody.Length);
                session.Send(result, 0, result.Length);
                session.Logger.Info("发送SECTION帧完成:" + BitConverter.ToString(result));

            }
            catch (Exception e)
            {
                session.Logger.Error("发送SECTION帧失败！");
                session.Logger.Error(e.ToString());
            }
        }
    }
}
