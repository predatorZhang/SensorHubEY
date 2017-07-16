using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.BLL;

namespace SensorHub.Servers.Commands.WSCommands
{
    [WSCommandFilter]
    public class WSSewPeriodDataCmd : CommandBase<WSSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "SewPeriodData";
            }
        }

        public override void ExecuteCommand(WSSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //SewPeriodData：设备ID，上传时间，CO数据，O2氧气数据，H2S数据，可燃气体数据\r\n
                session.Logger.Info("污水数据已经上传!");
                session.Logger.Info(requestInfo.Body);

                string[] bt = requestInfo.Body.Split(',');

                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = bt[0];
                }

                Model.SewPeriodDataInfo sew = new Model.SewPeriodDataInfo();

                sew.DEVID = bt[0];
                //转换bt[1] 20131212101010 bug fix by predator
                string sdate=bt[1].Substring(0,4)+"-"+bt[1].Substring(4,2)+"-"+bt[1].Substring(6,2)+" "+bt[1].Substring(8,2)+":"+bt[1].Substring(10,2)+":"+bt[1].Substring(12,2);
                sew.UPTIME = Convert.ToDateTime(sdate);
                sew.CO = bt[2];
                sew.O2 = bt[3];
                sew.H2S = bt[4];
                sew.FIREGAS = bt[5];
                new BLL.SewPeriodData().insert(sew);

                //send the time calibrating cmd
                /*
                 string sdata = "SewTiming:" + bt[0]+","+DateTime.Now.ToString("yyyyMMddHHmmss");
                byte[] data = new byte[sdata.Length + 2];
                Encoding.ASCII.GetBytes(sdata, 0, sdata.Length, data, 0);
                data[sdata.Length] =  0x0D;
                data[sdata.Length + 1] = 0x0A;
                session.Send(data, 0, data.Length);
                 * */

                //send the success cmd;
                string sdata0 = "SewPeriodData:success";
                byte[] data0 = new byte[sdata0.Length + 2];
                Encoding.ASCII.GetBytes(sdata0, 0, sdata0.Length, data0, 0);
                data0[sdata0.Length] = 0x0D;
                data0[sdata0.Length + 1] = 0x0A;
                session.Send(data0, 0, data0.Length);

                //增加设备运行日志
                BLL.DeviceLog bllLog = new DeviceLog();
                BLL.Device bllDevice = new Device();
                Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                log.MESSAGE = "污水数据上报";
                log.OPERATETYPE = "上报";
                log.LOGTIME = DateTime.Now;
                bllLog.insert(log);
             
            }
            catch (Exception e)
            {
                session.Logger.Error("污水数据保存失败!");
                session.Logger.Error(e.ToString());
            }
        }
    }
}
