using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.IDAL;
using SensorHub.Model;
namespace SensorHub.BLL
{
    public class RQPeriod
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="rqs">An adapter entity with information about the new adapter</param>
        private const string RQ_TYPE = "000044";
        private const string TypeName = "可燃气体泄露监测仪";
        public enum RQAlarmEnum
        {
            NO_DATA = 0, //7天无数据
            OVER_THRESH = 1, //浓度超标
        }
        public void insert(List<Model.RQPeriodInfo> rqs)
        {
            IDAL.IRQPeriod dal = SensorHub.DALFactory.RQPeriod.Create();
            dal.insert(rqs);

        }

        public void saveAlarmInfo(List<Model.RQPeriodInfo> djs)
        {
            double thresh = 25;//燃气超标上线
            foreach (RQPeriodInfo rq in djs)
            {
                if (Double.Parse(rq.STRENGTH) >= thresh)
                {
                    AlarmRecordInfo alarmRecordInfo = new AlarmRecordInfo();
                    alarmRecordInfo.ACTIVE = true;
                    alarmRecordInfo.DEVICE_CODE = rq.ADDRESS;
                    alarmRecordInfo.DEVICE_ID = Convert.ToInt32(new BLL.Device().getDeviceIdByCode(rq.ADDRESS));
                    alarmRecordInfo.DEVICE_TYPE_NAME = TypeName;
                    alarmRecordInfo.MESSAGE = (int) RQAlarmEnum.OVER_THRESH + "";
                    alarmRecordInfo.ITEMNAME = "浓度超标";
                    alarmRecordInfo.ITEMVALUE = rq.STRENGTH;
                    alarmRecordInfo.MESSAGE_STATUS = 0;
                    alarmRecordInfo.RECORDCODE = "";
                    alarmRecordInfo.RECORDDATE = System.DateTime.Now;

                    List<int> devStatusList = new List<int>();
                    devStatusList.Add((int) RQAlarmEnum.OVER_THRESH);
                    new BLL.AlarmRecord().deleteByMessage(rq.ADDRESS, devStatusList);

                    new BLL.AlarmRecord().save(alarmRecordInfo);

                }
            }
        }
    }
}
