using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class AKFSHJ
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="djs">An adapter entity with information about the new adapter</param>
        public void insert(List<Model.AKFSHJInfo> djs)
        {
            if (djs.Count <= 0)
            {
                return;
            }

            IAKFSHJ dal = SensorHub.DALFactory.AKFSHJ.Create();
            List<Model.AKFSHJInfo> list = new List<AKFSHJInfo>();

            foreach (Model.AKFSHJInfo dj in djs)
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

        //保存腐蚀环境报警数据
        public void saveAlarmInfo(List<Model.AKFSHJInfo> djs,String pipeType)
        {
            float tempThresh = 40;
            if (pipeType == "回水")
            {
                tempThresh = 20;
            }
            List<Model.AlarmRecordInfo> list = new List<Model.AlarmRecordInfo>();

            foreach (Model.AKFSHJInfo dj in djs)
            {
                String itemName = "";
                String itemValue = "";
                /*
               // if (float.Parse(dj.OutterTemp1) > 40 || float.Parse(dj.OutterTemp2) > 40)
                if (Math.Abs(float.Parse(dj.UnderTemp2) - float.Parse(dj.UnderTemp1)) > tempThresh)
                {
                    itemName = itemName + "保温层内外温差过高"+",";
                    itemValue = itemValue + "(" + dj.UnderTemp2 + ":" + dj.UnderTemp1 + ")" + ",";
                }
                if (Math.Abs(float.Parse(dj.OutterTemp1) - float.Parse(dj.OutterTemp1)) > tempThresh)
               // if (float.Parse(dj.UnderTemp1) > 40 || float.Parse(dj.UnderTemp2) > 40)
                {
                    itemName = itemName + "保温层内外温差过高" + ",";
                    itemValue = itemValue + "(" + dj.OutterTemp1 + ":" + dj.OutterTemp1 + ")" + ",";
                }

                if (float.Parse(dj.UnderWaterIn1) != 0 || float.Parse(dj.UnderWaterIn2) != 0 || float.Parse(dj.UnderWaterIn3) != 0 ||
                    float.Parse(dj.UnderWaterIn4) != 0 || float.Parse(dj.UnderWaterIn5) != 0 || float.Parse(dj.UnderWaterIn6) != 0)
                {
                    itemName = itemName + "保温层下浸水" + ",";
                    itemValue = itemValue + "(" + dj.UnderWaterIn1 + ":" + dj.UnderWaterIn2+
                        ":" + dj.UnderWaterIn3 + ":" + dj.UnderWaterIn4 + ":" + dj.UnderWaterIn5 + ":" + dj.UnderWaterIn6+
                         ")" + ",";
                }
                **/
                if (float.Parse(dj.UnderVo11) <= -300 || float.Parse(dj.UnderVo12) <= -300 || float.Parse(dj.UnderVo13) <= -300 ||
                    float.Parse(dj.UnderVo14) <= -300 || float.Parse(dj.UnderVo15) <= -300 || float.Parse(dj.UnderVo16) <= -300)
                {
                    itemName = itemName + "电位异常" + ",";
                    itemValue = itemValue + "(" + dj.UnderVo11 + ":" + dj.UnderVo12 +
                        ":" + dj.UnderVo13 + ":" + dj.UnderVo14 + ":" + dj.UnderVo15 + ":" + dj.UnderVo16 +
                         ")" + ",";
                }
                else
                {
                    new BLL.AlarmRecord().removeByDevCode(dj.DEVCODE);
                    continue;
                }
                AlarmRecordInfo alarmRecordInfo = null;
                if (itemValue != "" || itemName != "")
                {
                    alarmRecordInfo = new AlarmRecordInfo();
                    alarmRecordInfo.ACTIVE = true;
                    alarmRecordInfo.DEVICE_CODE = dj.DEVCODE;
                    alarmRecordInfo.DEVICE_ID = Convert.ToInt32(new BLL.Device().getDeviceIdByCode(dj.DEVCODE));
                    alarmRecordInfo.DEVICE_TYPE_NAME = new BLL.Device().getDevTypeByCode(dj.DEVCODE);
                    alarmRecordInfo.ITEMNAME =itemName.Substring(0,itemName.Length-1);
                    alarmRecordInfo.ITEMVALUE = itemValue.Substring(0,itemValue.Length-1);
                    alarmRecordInfo.MESSAGE = itemName;
                    alarmRecordInfo.MESSAGE_STATUS = 0;
                    alarmRecordInfo.RECORDCODE = "";
                    alarmRecordInfo.RECORDDATE = System.DateTime.Now;
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

        public string getHeatPipeTypeByDevCode(String devCode)
        {
            return SensorHub.DALFactory.AKFSHJ.Create().getHeatPipeTypeByDevCode(devCode);
        }
    }
}
