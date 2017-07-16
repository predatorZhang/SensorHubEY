using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class NKTemperatureCurve
    {
                /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="temperatureCurve">An adapter entity with information about the new adapter</param>
        public void insert(SensorHub.Model.NKTemperatureCurveInfo temperatureCurve)
        {
            if (string.IsNullOrEmpty(temperatureCurve.DEVID))
            {
                return;
            }
            SensorHub.IDAL.INKTemperatureCurve dal = SensorHub.DALFactory.NKTemperatureCurve.Create();
            dal.insert(temperatureCurve);
        }

        float tempAlarm = float.Parse(System.Configuration.ConfigurationSettings.AppSettings["GX_TEMP_ALARM"]);
        public void saveAlarm(String disList, String tempList, String devCode,
           int devId, String devType)
        {
            string[] tempArray = tempList.Split(',');
            string[] disArray = disList.Split(',');
          //  Model.NKTemperatureCurveInfo lastCurve = 
            Model.NKTemperatureCurveInfo cu = this.getLastTempCurve(devCode);
            string[] lastDisArray = cu.DISTANCE.Split(',');
            string[] lastTempArray = cu.TEMPERATURE.Split(',');

            if (cu == null) { return; }

            for (int i = 0; i < tempArray.Length; i++)
            {
                string dis = disArray[i];
                string temp = tempArray[i];
                if (Math.Abs(float.Parse(temp)-float.Parse(lastTempArray[i]))>tempAlarm)
                {
                    Model.AlarmRecordInfo alarmRecordInfo = new Model.AlarmRecordInfo();
                    alarmRecordInfo.ACTIVE = true;
                    alarmRecordInfo.DEVICE_CODE = devCode;
                    alarmRecordInfo.DEVICE_ID = devId;
                    alarmRecordInfo.DEVICE_TYPE_NAME = devType;
                    alarmRecordInfo.ITEMNAME = "温度:" + dis; //到时候根据这个ID号来取消
                    alarmRecordInfo.ITEMVALUE = dis + "," + temp;
                    alarmRecordInfo.MESSAGE_STATUS = 0;
                    alarmRecordInfo.RECORDCODE = "";
                    alarmRecordInfo.RECORDDATE = System.DateTime.Now;
                    alarmRecordInfo.MESSAGE = "光纤温度超标";
                    alarmRecordInfo.RECORDCODE = "";
                    BLL.AlarmRecord nc = new BLL.AlarmRecord();
                    nc.saveGXPressAlarm(alarmRecordInfo);
                }
            }
        }

        public Model.NKTemperatureCurveInfo getLastTempCurve(string devCode)
        {
            SensorHub.IDAL.INKTemperatureCurve dal = SensorHub.DALFactory.NKTemperatureCurve.Create();
            return dal.getLastCurve(devCode);
        }

    }
}
