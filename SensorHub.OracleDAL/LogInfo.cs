using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class LogInfo : SensorHub.IDAL.ILogInfo
    {
        public void insertLogInfo(Model.LogInfo logInfo)
        {            
            try
            {
                String sql = " insert into LogInfo "
                           + " (EQUIPMENT,OPERATE,OPERATE_TIME,USERNAME,PATROLER_ID)VALUES("
                           + "'" + logInfo.Equipment + "',"
                           + "'" + logInfo.Operate + "',"
                           + "to_date('" + logInfo.Operate_time.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-dd hh24:mi:ss')" + ","
                           + "'" + logInfo.Username + "',"
                           + logInfo.Patroler_id + ")";
                OracleParameter[] parms = null; 
                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, sql, parms);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
