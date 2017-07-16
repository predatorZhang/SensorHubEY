using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class DeviceLog1 : IDAL.IDeviceLog1
    {
        public void add(Model.DeviceLogInfo1 log)
        {
            try
            {
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":DEVCODE",log.DEVICECODE),
                new OracleParameter(":DEVTYPE",log.DEVTYPE),
                new OracleParameter(":MESSAGE",log.MESSAGE),
                new OracleParameter(":TP",log.OPERATETYPE),
                new OracleParameter(":TM",log.LOGTIME)
                };
                string SQL = "INSERT INTO ALARM_DEVICE_LOG1 (DBID,DEVICECODE,DEVICETYPE,MESSAGE,OPERATETYPE,LOGTIME) "
                    + "VALUES"
                    + " (SEQ_ALARM_DEVICE_LOG1_ID.NEXTVAL,:DEVCODE,:DEVTYPE,:MESSAGE,:TP,:TM)";

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
