using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
namespace SensorHub.Servers.Commands.RQCommands
{
    /**
     * 燃气报警数据上报。
     **/
    [RQCommandFilter]
    public class RQAlarmUploadCmd : CommandBase<RQSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "7B-89-00-11";
            }
        }

        public override void ExecuteCommand(RQSession session, BinaryRequestInfo requestInfo)
        {
            try
            {
                //F1-50-11-19-19-23-01-10-00-2D-00-10-20-14-09-17-15-56-36-00-
                //00-42-3C-F5-C3-40-18-80-00-43-6F-00-00-00-00-00-00-00-00-67-27-
                //41-72-00-02-11-6C
                byte[] body = requestInfo.Body;

                session.Logger.Info("接收到燃气报警数据上！" + BitConverter.ToString(body, 0, body.Length));

                byte[] sim = new byte[6];

                Buffer.BlockCopy(body, 0, sim, 0, 6);

                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = BitConverter.ToString(sim).Replace("-", "").Substring(1);
                }

                //获取设备ID
                string devId = session.MacID;

                //将采集上的数据保存到数据库
                byte[] record = new byte[30];
                byte[] dateTime = new byte[6];

                //获取报警记录信息
                Buffer.BlockCopy(body, 13, record, 0, body.Length - 17);

                //获取报警记录时间
                Buffer.BlockCopy(record, 0, dateTime, 0, 6);

                string timeStr=BitConverter.ToString(dateTime).Replace("-","");
                string msg = "燃气设备警告：@devid:@itemname:@itemvalue:@time";
                DateTime date = Convert.ToDateTime("20" + timeStr.Substring(0, 2) 
                    + "-" + timeStr.Substring(2, 2) 
                    + "-" + timeStr.Substring(4, 2) 
                    + " " + timeStr.Substring(6, 2) 
                    + ":" + timeStr.Substring(8, 2) 
                    + ":" + timeStr.Substring(10, 2));
                string inPress = BitConverter.ToSingle(new byte[] { record[7], record[6], record[9], record[8] }, 0).ToString();
                string outPress = BitConverter.ToSingle(new byte[] { record[11], record[10], record[13], record[12] }, 0).ToString();
                string flow = BitConverter.ToSingle(new byte[] { record[15], record[14], record[17], record[16] }, 0).ToString();
                string strength = BitConverter.ToSingle(new byte[] { record[19], record[18], record[21], record[20] }, 0).ToString();
                string temperature = BitConverter.ToSingle(new byte[] { record[23], record[22], record[25], record[24] }, 0).ToString();
                string cell = BitConverter.ToSingle(new byte[] { record[27], record[26], record[29], record[28] }, 0).ToString();

                List<Model.AlarmRecordInfo> alarms = new List<Model.AlarmRecordInfo>();
                if (float.Parse(inPress) != 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "ALARM_XT_INPRESS_" + devId + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                    //alarm.MESSAGE = msg.Replace("@devid", devId).Replace("@itemname", "燃气进站压力")
                       // .Replace("@itemvalue", inPress).Replace("@time", date.ToString());
                   
                    alarm.ITEMNAME = "进站压力超限";
                    alarm.MESSAGE = alarm.ITEMNAME;
                    alarm.ITEMVALUE = inPress;
                    alarm.DEVICE_ID = (int)session.ID;
                    alarm.RECORDDATE = DateTime.Now;
                    alarm.MESSAGE_STATUS = 0;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "燃气智能监测终端";
                    alarms.Add(alarm);

                    //增加进站压力超限设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "燃气进站压力超限报警！";
                    log.OPERATETYPE = "报警数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }
                if (float.Parse(outPress) != 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "ALARM_XT_OUTPRESS_" + devId + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                   // alarm.MESSAGE = msg.Replace("@devid", devId).Replace("@itemname", "燃气出站压力")
                   //     .Replace("@itemvalue", outPress).Replace("@time", date.ToString());
                    alarm.ITEMNAME = "出站压力超限";
                    alarm.MESSAGE = alarm.ITEMNAME;
                    alarm.ITEMVALUE = outPress;
                    alarm.DEVICE_ID = (int)session.ID;
                    alarm.RECORDDATE = DateTime.Now;
                    alarm.MESSAGE_STATUS = 0;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "燃气智能监测终端";
                    alarms.Add(alarm);

                    //增加出站压力超限设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "燃气出站压力超限报警！";
                    log.OPERATETYPE = "报警数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }
                if (float.Parse(flow) != 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "ALARM_XT_FLOW_" + devId + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                 //   alarm.MESSAGE = msg.Replace("@devid", devId).Replace("@itemname", "燃气流量")
                 //       .Replace("@itemvalue", flow).Replace("@time", date.ToString());
                    alarm.ITEMNAME = "流量超限";
                    alarm.MESSAGE = alarm.ITEMNAME;
                    alarm.ITEMVALUE = flow;
                    alarm.DEVICE_ID = (int)session.ID;
                    alarm.RECORDDATE = DateTime.Now;
                    alarm.MESSAGE_STATUS = 0;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "燃气智能监测终端";
                    alarms.Add(alarm);

                    //增加流量超限设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "燃气流量超限报警！";
                    log.OPERATETYPE = "报警数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }
                if (float.Parse(strength) != 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "ALARM_XT_STRENGTH_" + devId + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                   // alarm.MESSAGE = msg.Replace("@devid", devId).Replace("@itemname", "燃气浓度")
                  //      .Replace("@itemvalue", strength).Replace("@time", date.ToString());
                    alarm.ITEMNAME = "浓度超限";
                    alarm.MESSAGE = alarm.ITEMNAME;
                    alarm.ITEMVALUE = strength;
                    alarm.DEVICE_ID = (int)session.ID;
                    alarm.RECORDDATE = DateTime.Now;
                    alarm.MESSAGE_STATUS = 0;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "燃气智能监测终端";
                    alarms.Add(alarm);

                    //增加浓度超限设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "燃气浓度超限报警！";
                    log.OPERATETYPE = "报警数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }
                if (float.Parse(temperature) != 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "ALARM_XT_TEMPERATURE_" + devId + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                   // alarm.MESSAGE = msg.Replace("@devid", devId).Replace("@itemname", "燃气温度")
                   //     .Replace("@itemvalue", temperature).Replace("@time", date.ToString());
                    alarm.ITEMNAME = "温度超限";
                    alarm.MESSAGE = alarm.ITEMNAME;
                    alarm.ITEMVALUE = temperature;
                    alarm.DEVICE_ID = (int)session.ID;
                    alarm.RECORDDATE = DateTime.Now;
                    alarm.MESSAGE_STATUS = 0;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "燃气智能监测终端";
                    alarms.Add(alarm);

                    //增加温度超限设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "燃气温度超限报警！";
                    log.OPERATETYPE = "报警数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }
                if (float.Parse(cell) != 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "ALARM_XT_CELL_" + devId + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                  //  alarm.MESSAGE = msg.Replace("@devid", devId).Replace("@itemname", "电池电量")
                  //      .Replace("@itemvalue", cell).Replace("@time", date.ToString());
                    alarm.ITEMNAME = "电量超限";
                    alarm.MESSAGE = alarm.ITEMNAME;
                    alarm.ITEMVALUE = cell;
                    alarm.DEVICE_ID = (int)session.ID;
                    alarm.RECORDDATE = DateTime.Now;
                    alarm.MESSAGE_STATUS = 0;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "燃气智能监测终端";
                    alarms.Add(alarm);
                }

                new BLL.AlarmRecord().insert(alarms);

                //发送反馈信息
                byte[] first={0x7B,0x89,0x00,0x11,0x00,0x0E};
                byte[] second={0x01,0x10,0x00,0x2D,0x00,0x10};
                byte[] modBusData = new byte[first.Length + sim.Length + second.Length];
                first.CopyTo(modBusData, 0);
                sim.CopyTo(modBusData, first.Length);
                second.CopyTo(modBusData, first.Length + sim.Length);

                byte[] crcData = Utility.CodeUtils.getCrcByModBusData(modBusData);
                byte[] resp = new byte[modBusData.Length + crcData.Length];
                modBusData.CopyTo(resp, 0);
                crcData.CopyTo(resp, modBusData.Length);
                session.Send(resp, 0, resp.Length);
                session.Logger.Info("报警主动上报已确认！" + BitConverter.ToString(body, 0, body.Length));
            }
            catch (Exception e)
            {
                session.Logger.Error("燃气报警数据上传处理异常" + e.ToString());
            }
        }
    }
}
