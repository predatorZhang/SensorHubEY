using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.BYCommands
{
    public class BYVibratingPositionCmd : CommandBase<BYSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "AlarmRpt";
            }
        }
        public override void ExecuteCommand(BYSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //AlarmRpt：系统标示，命令字、设备ID，报警ID，报警状态，报警个数，报警位置|报警值#报警位置|报警值，报警时间/r/n
                session.Logger.Info("震动曲线开始:\n" + requestInfo.Body);                         
                string[] body = requestInfo.Body.Split(',');
                string alarmID = body[3];
                string systemID = body[0];
                string commandId = body[1];
                string[] locations = body[6].Split('#');
                string strDate = body[7];
                string alarmStatus = body[4];
                string devCode = body[2];
                object obj = new BLL.Device().getDeviceIdByCode(devCode);
                if(obj==null)
                {
                    session.Logger.Info("光纤编号 "+devCode+"：未注册:\n");
                    return;
                }

                int devId = Convert.ToInt32(obj);
                string devType = new BLL.Device().getDevTypeByDevId(devId);
                if (devType == null)
                {
                    session.Logger.Info("设备类型表中，无该类型");
                    return;
                }

                List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();
                foreach (string loc in locations)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();

                    if (alarmStatus == "1")
                    {
                        //开挖报警
                        alarm.MESSAGE = "管线开挖报警";
                        alarm.ITEMNAME = "开挖报警";
                    }
                    else if (alarmStatus == "-99")
                    {
                        //断纤报警
                        alarm.MESSAGE = "断纤报警";
                        alarm.ITEMNAME = "断纤报警";
                    }
                    else
                    {
                        //断纤报警自动清除
                        alarm.MESSAGE = "断纤报警自动清除";
                        alarm.ITEMNAME = "断纤报警自动清除";
                    }

                    alarm.ITEMVALUE = loc.Replace('|',',');
                    alarm.DEVICE_ID = devId;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = devCode;
                    alarm.DEVICE_TYPE_NAME = devType;
                    alarm.MESSAGE_STATUS = 0;
                    alarm.RECORDDATE = Convert.ToDateTime(strDate);
                    list.Add(alarm);
                }
                new BLL.AlarmRecord().insert(list);

                //发送返回信息AlarmRptR：系统标示码、命令号、报警ID\r\n
                string sdata0 = "AlarmRptsR:" + systemID + "," + commandId + "," + devCode + "," + alarmID;
                byte[] data0 = new byte[sdata0.Length + 2];
                Encoding.ASCII.GetBytes(sdata0, 0, sdata0.Length, data0, 0);
                data0[sdata0.Length] = 0x0D;
                data0[sdata0.Length + 1] = 0x0A;
                session.Send(data0, 0, data0.Length);
                session.Logger.Info("光纤回复信息:" + sdata0);
            }
            catch (Exception e)
            {
                session.Logger.Error("振动定位数据采集失败！\n" + e.ToString());

            }
        }
    }
}
