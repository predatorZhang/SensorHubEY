using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
namespace SensorHub.Servers.Commands
{
    public class LSReceiveLiquidCmd : CommandBase<SZLiquidSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "LSLEVDATA";
            }
        }

        public override void ExecuteCommand(SZLiquidSession session, StringRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("LSLEVDATA：液位监测仪器：" + requestInfo.Body);
                //采集时间
                string year = (Int32.Parse(requestInfo.Parameters[9].Substring(8, 2), System.Globalization.NumberStyles.HexNumber) + 2000).ToString();
                string mon = Int32.Parse(requestInfo.Parameters[9].Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string day = Int32.Parse(requestInfo.Parameters[9].Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string hor = Int32.Parse(requestInfo.Parameters[9].Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string min = Int32.Parse(requestInfo.Parameters[9].Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                DateTime upTime = Convert.ToDateTime(year + "-" + mon + "-" + day + " " + hor + ":" + min + ":00");

                session.MacID = requestInfo.Parameters[5];

                string cfg = requestInfo.Parameters[6];
                string ldata = requestInfo.Parameters[10];

                BLL.DeviceLog bllLog = new DeviceLog();
                BLL.Device bllDevice = new Device();
                Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                log.DEVICE_ID = Convert.ToInt32(bllDevice.getDeviceIdByCode(session.MacID));
                log.MESSAGE = "液位数据上传";
                log.OPERATETYPE = "上报";
                log.LOGTIME = DateTime.Now;
                bllLog.insert(log);

                //采集个数
                int count = Int16.Parse(requestInfo.Parameters[8], System.Globalization.NumberStyles.HexNumber);
                int isAlarm = Int16.Parse(requestInfo.Parameters[7], System.Globalization.NumberStyles.HexNumber);
                
                List<Model.DjLiquidInfo> djs = new List<DjLiquidInfo>();
                List<Model.DjLiquidInfo> alarmDjs = new List<DjLiquidInfo>();
                for (int i = 0; i < count; i++)
                {
                    Model.DjLiquidInfo dj = new DjLiquidInfo();

                    //设备ID
                    dj.DEVID = session.MacID;

                    //液位数据
                    string lStr = ldata.Substring(i * 8, 8);
                    byte[] lBt ={
                                   byte.Parse(lStr.Substring(0,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(lStr.Substring(2,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(lStr.Substring(4,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(lStr.Substring(6,2),System.Globalization.NumberStyles.HexNumber)
                               };
                    float liquidData = verifyLiquidData(BitConverter.ToSingle(lBt, 0), session.MacID, session);
                    dj.LIQUIDDATA = liquidData.ToString();

                    //电池电量
                    dj.CELL = Int16.Parse(requestInfo.Parameters[11], System.Globalization.NumberStyles.HexNumber).ToString();

                    //采集时间
                    dj.UPTIME = upTime;

                    //记录时间
                    dj.LOGTIME = DateTime.Now;

                    djs.Add(dj);

                    upTime = upTime.AddMinutes(60);

                    //如果存在报警
                    if (isAlarm == 1)
                    {
                        alarmDjs.Add(dj);
                    }
                }
                new BLL.DjLiquid().insert(djs);
                new BLL.DjLiquid().saveSZAlarmInfo(alarmDjs);
               // new BLL.DjLiquid().updateDevStatus(session.MacID);

                session.Logger.Info("液位监测仪器：" + session.MacID + "液位数据已经保存! ");

                //TODO LIST:下发上传数据返回帧
                byte[] asck = {
                                  0x50,
                                  0x00,0x09,
                                  0x02,
                                  0x00,0x00,0x34,
                                  0x22,
                                  0x03
                              };
                string head = "LSLEVDATA:";
                byte[] btHead = System.Text.Encoding.Default.GetBytes(head);
                byte[] result = new byte[asck.Length + btHead.Length];

                Buffer.BlockCopy(btHead, 0, result, 0, btHead.Length);
                Buffer.BlockCopy(asck, 0, result, btHead.Length, asck.Length);
                session.Send(result, 0, result.Length);

            }
            catch (Exception e)
            {
                session.Logger.Error("液位数据已经保存失败" + requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
            finally
            {
            }
        }

        private float verifyLiquidData(float src, String devCode, SZLiquidSession session)
        {
            //修正液位数据
            if (src >= 0)
                return src;
            //TODO LIST：根据设备DevCode，找到对应的井深信息
            BLL.Device device = new BLL.Device();
            float result = device.getWellDepByDevcode(devCode);
            if (result == -1)
            {
                session.Logger.Error("旧版液位监测仪器设备编号重复:" + devCode);
                return src;
            }
            else if (result == -2)
            {
                session.Logger.Error("无法找到关联阀门:" + devCode);
                return src;
            }
            else if (result == -3)
            {
                session.Logger.Error("设备关联阀门不唯一:" + devCode);
                return src;
            }
            else {
                session.Logger.Info("液位液位监测:"+devCode+"采集盲区返回井深:"+result);
            }
            return result;
        }
    }
}
