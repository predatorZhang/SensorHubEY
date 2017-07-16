using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.BLL
{
    public class AlarmRule
    {
       /// <summary>
       /// 根据设备号查询对应的超时数值
       /// </summary>
       /// <param name="devId"></param>
       /// <returns></returns>
        public object getOverTimeByDevId(int devId)
        {
            IDAL.IAlarmRule dal = SensorHub.DALFactory.AlarmRule.Create();
            return dal.getOverTimeByDevId(devId);
        }

        public object getOverTimeByIdAndType(int devId,String sensorTypeCode)
        {
            IDAL.IAlarmRule dal = SensorHub.DALFactory.AlarmRule.Create();
            return dal.getOverTimeByIdAndType(devId, sensorTypeCode);
        }

        public AlarmRuleInfo getAlarmRule(String devCode, String sensorType)
        {
            IDAL.IAlarmRule dal = SensorHub.DALFactory.AlarmRule.Create();
            return dal.getAlarmRule(devCode, sensorType);
        }


      

       
    }
}
