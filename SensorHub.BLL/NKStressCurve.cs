using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class NKStressCurve
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="stressCurve">An adapter entity with information about the new adapter</param>
        public void insert(SensorHub.Model.NKStressCurveInfo stressCurve)
        {
            if (string.IsNullOrEmpty(stressCurve.DEVID))
            {
                return;
            }
            SensorHub.IDAL.INKStressCurve dal = SensorHub.DALFactory.NKStressCurve.Create();
            dal.insert(stressCurve);
        }

        float pressAlarm = float.Parse(System.Configuration.ConfigurationSettings.AppSettings["GX_PRESS_ALARM"]);

        public void saveAlarm(String disList, String pressList, String devCode,
            int devId, String devType)
        {
            string[] pressArray = pressList.Split(',');
            string[] disArray = disList.Split(',');

            for (int i = 0; i < pressArray.Length; i++)
            {
                if (float.Parse(pressArray[i]) > pressAlarm)
                {
                    string dis = disArray[i];
                    string press =pressArray[i];

                    Model.AlarmRecordInfo alarmRecordInfo = new Model.AlarmRecordInfo();
                    alarmRecordInfo.ACTIVE = true;
                    alarmRecordInfo.DEVICE_CODE =devCode;
                    alarmRecordInfo.DEVICE_ID = devId;
                    alarmRecordInfo.DEVICE_TYPE_NAME = devType;
                    alarmRecordInfo.ITEMNAME = "压力:"+dis; //到时候根据这个ID号来取消
                    alarmRecordInfo.ITEMVALUE = dis+","+press;
                    alarmRecordInfo.MESSAGE_STATUS = 0;
                    alarmRecordInfo.RECORDCODE = "";
                    alarmRecordInfo.RECORDDATE = System.DateTime.Now;
                    alarmRecordInfo.MESSAGE = "光纤压力超标";
                    BLL.AlarmRecord nc = new BLL.AlarmRecord();
                    nc.saveGXPressAlarm(alarmRecordInfo);
                    
                }
            }

        }
    }
}
