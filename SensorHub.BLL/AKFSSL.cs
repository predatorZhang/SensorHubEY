using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class AKFSSL
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.AKFSSLInfo> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }

            IAKFSSL dal = SensorHub.DALFactory.AKFSSL.Create();
            List<Model.AKFSSLInfo> list = new List<AKFSSLInfo>();

            foreach (Model.AKFSSLInfo dj in djs)
            {
                if (dal.queryCountByDevAndUpTime(dj.DEVCODE, dj.UpTime) <= 0)
                {
                    list.Add(dj);
                }
            }
            if (list.Count > 0)
            {
                dal.insert(djs);
            }
        }

        //保存腐蚀速率报警数据
        public void saveAlarmInfo(List<Model.AKFSSLInfo> djs)
        {
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            
            foreach (Model.AKFSSLInfo dj in djs)
            {
                float errosionRate = float.Parse(dj.ErrosionRat);
                if (errosionRate < 0.025)
                {   
                    //TODO LIST:清除原有腐蚀
                    new BLL.AlarmRecord().removeByDevCode(dj.DEVCODE);
                    continue;
                }

                AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                alarmRecordInfo.ACTIVE = true;
                alarmRecordInfo.DEVICE_CODE = dj.DEVCODE;
                alarmRecordInfo.DEVICE_ID = Convert.ToInt32(new BLL.Device().getDeviceIdByCode(dj.DEVCODE));
                alarmRecordInfo.DEVICE_TYPE_NAME = new BLL.Device().getDevTypeByCode(dj.DEVCODE);
                alarmRecordInfo.ITEMNAME = "腐蚀速率";
                alarmRecordInfo.ITEMVALUE = errosionRate.ToString();
                alarmRecordInfo.MESSAGE_STATUS = 0;
                alarmRecordInfo.RECORDCODE = "";
                alarmRecordInfo.RECORDDATE = System.DateTime.Now;

                if (errosionRate >= 0.025 && errosionRate < 0.13)
                {
                    alarmRecordInfo.MESSAGE = "中度腐蚀";
                }
                else if (errosionRate >= 0.13 && errosionRate < 0.25)
                {
                    alarmRecordInfo.MESSAGE = "严重腐蚀";
                }
                else
                {
                    alarmRecordInfo.MESSAGE = "极严重腐蚀";
                }
               
                if (null != alarmRecordInfo)
                {
                    list.Add(alarmRecordInfo);
                }
            }
            if (list.Count > 0)
            {
                new BLL.AlarmRecord().insert(list);
            }
        }

    }
}
