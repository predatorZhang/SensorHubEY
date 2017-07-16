using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;


namespace SensorHub.Servers.Commands.WSCommands
{
     [WSCommandFilter]
    public class WSSewAlarmDataCmd : CommandBase<WSSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "SewAlarmData";
            }
        }

        public override void ExecuteCommand(WSSession session, StringRequestInfo requestInfo)
        {
            try
            {
                //SewAlarmData：设备ID，上传时间，CO数据，O2氧气数据，H2S数据，可燃气体数据\r\n
                session.Logger.Info("污水报警数据上传!");
                session.Logger.Info(requestInfo.Body);

                string[] bt = requestInfo.Body.Split(',');
                if (string.IsNullOrEmpty(session.MacID))
                {
                    session.MacID = bt[0];
                }
                List<Model.AlarmRecordInfo> alarms = new List<Model.AlarmRecordInfo>();
                //TODO LIST:根据李雨龙需求，修改可能发生的4个报警记录合并为一条报警记录
               // 一氧化碳、硫化氢、氧气、甲烷

                Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                alarm.RECORDCODE = "WS_ALARM_H2S_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                alarm.MESSAGE = "有害气体超标";
                alarm.ITEMNAME = "FIREGAS";
                alarm.DEVICE_ID = session.ID;
                alarm.DEVICE_CODE = session.MacID;
                alarm.MESSAGE_STATUS = 0;
                alarm.ACTIVE = true;
                string stime = bt[1];
                string sdate = stime.Substring(0, 4) + "-" + stime.Substring(4, 2) + "-" + stime.Substring(6, 2)
                  + " " + stime.Substring(8, 2) + ":" + stime.Substring(10, 2) + ":" + stime.Substring(12, 2);
                alarm.RECORDDATE = Convert.ToDateTime(sdate);
                alarm.DEVICE_TYPE_NAME = "有害气体监测仪";
                string result = "";
                double coAlarm = Convert.ToDouble(bt[2]) > 50.0?Convert.ToDouble(bt[2]):0;
                double o2Alarm = Convert.ToDouble(bt[3]) > 18 && Convert.ToDouble(bt[3]) < 23 ? 0 : Convert.ToDouble(bt[3]);
                double h2sAlarm = Convert.ToDouble(bt[4]) > 10.0 ? Convert.ToDouble(bt[4]) : 0;
                double firegasAlarm = Convert.ToDouble(bt[5]) > 2 ? Convert.ToDouble(bt[5]) : 0;
                result = coAlarm + "," +
                     h2sAlarm + "," +
                     o2Alarm + "," +
                       firegasAlarm;
                alarm.ITEMVALUE = result;

                alarms.Add(alarm);
                /*
                if (Convert.ToDouble(bt[3]) > 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "WS_ALARM_CO_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                    //alarm.MESSAGE = "污水一氧化碳报警:【值：" + bt[3] + "】【时间："+bt[1] + "】";
                    alarm.MESSAGE = "CO超标";
                    alarm.ITEMNAME = "CO";
                    alarm.ITEMVALUE = bt[3];
                    alarm.DEVICE_ID = session.ID;
                    alarm.MESSAGE_STATUS = false;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "有害气体监测仪";
                    alarms.Add(alarm);

                    //增加CO气体浓度报警设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "CO浓度超限！";
                    log.OPERATETYPE = "有害气体数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }
                if (Convert.ToDouble(bt[4]) > 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "WS_ALARM_O2_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                   // alarm.MESSAGE = "污水氧气报警:【值：" + bt[3] + "】【时间："+bt[1] + "】";
                    alarm.MESSAGE = "氧气过低";
                    alarm.ITEMNAME = "O2";
                    alarm.ITEMVALUE = bt[4];
                    alarm.DEVICE_ID = session.ID;
                    alarm.MESSAGE_STATUS = false;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "有害气体监测仪";
                    alarms.Add(alarm);

                    //增加O2气体浓度报警设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "O2浓度超限！";
                    log.OPERATETYPE = "有害气体数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }
                if (Convert.ToDouble(bt[5]) > 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "WS_ALARM_H2S_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                  //  alarm.MESSAGE = "污水H2S报警:【值：" + bt[4] + "】【时间：" +bt[1] + "】";
                    alarm.MESSAGE = "H2S超标";
                    alarm.ITEMNAME = "H2S";
                    alarm.ITEMVALUE = bt[5];
                    alarm.DEVICE_ID = session.ID;
                    alarm.MESSAGE_STATUS = false;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.DEVICE_TYPE_NAME = "有害气体监测仪";
                    alarms.Add(alarm);

                    //增加H2S气体浓度报警设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "H2S浓度超限！";
                    log.OPERATETYPE = "有害气体数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }

                if (Convert.ToDouble(bt[6]) > 0)
                {
                    Model.AlarmRecordInfo alarm = new Model.AlarmRecordInfo();
                    alarm.RECORDCODE = "WS_ALARM_H2S_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                  //  alarm.MESSAGE = "污水可燃气体报警:【值：" + bt[5] + "】【时间：" + bt[1] + "】";
                    alarm.MESSAGE = "甲烷超标";
                    alarm.ITEMNAME = "FIREGAS";
                    alarm.ITEMVALUE = bt[6];
                    alarm.DEVICE_ID = session.ID;
                    alarm.DEVICE_CODE = session.MacID;
                    alarm.MESSAGE_STATUS = false;
                    alarm.ACTIVE = true;
                    alarm.DEVICE_TYPE_NAME = "有害气体监测仪";
                    alarms.Add(alarm);

                    //增加可燃气体报警设备运行日志thc20150610
                    Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                    log.DEVICE_ID = session.ID;
                    log.MESSAGE = "可燃气体浓度超限！";
                    log.OPERATETYPE = "有害气体数据上报";
                    log.LOGTIME = DateTime.Now;
                    new BLL.DeviceLog().insert(log);
                }
                **/
                new BLL.AlarmRecord().insert(alarms);

                Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                log.DEVICE_ID = session.ID;
                log.MESSAGE = "有害气体浓度超标！";
                log.OPERATETYPE = "有害气体数据上报";
                log.LOGTIME = DateTime.Now;
                new BLL.DeviceLog().insert(log);

                //send the success cmd
                string sdata0 = "SewAlarmData:success";
                byte[] data0 = new byte[sdata0.Length + 2];
                Encoding.ASCII.GetBytes(sdata0, 0, sdata0.Length, data0, 0);
                data0[sdata0.Length] = 0x0D;
                data0[sdata0.Length + 1] = 0x0A;
                session.Send(data0, 0, data0.Length);
       
            }
            catch (Exception e)
            {
                session.Logger.Error("污水报警数据保存异常!");
                session.Logger.Error(requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
        }
    }
}
