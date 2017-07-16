using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class DeviceLog : IDAL.IDeviceLog
    {
        public void add(Model.DeviceLogInfo log)
        {
            try
            {
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":ID",log.DEVICE_ID),
                new OracleParameter(":MSG",log.MESSAGE),
                new OracleParameter(":TP",log.OPERATETYPE),
                new OracleParameter(":TM",log.LOGTIME)
                };
                string SQL = "INSERT INTO ALARM_DEVICE_LOG (DBID,DEVICE_ID,MESSAGE,OPERATETYPE,LOGTIME) "
                    + "VALUES"
                    + " (SEQ_ALARM_DEVICE_LOG_ID.NEXTVAL,:ID,:MSG,:TP,:TM)";

                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL, parms);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
