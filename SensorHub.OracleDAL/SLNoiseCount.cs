using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SensorHub.IDAL;
using SensorHub.Model;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class SLNoiseCount : ISLNoiseCount
    {
        // Static constants

        public bool IncrementSLNoiseAlarmCount(string devCode, int MaxCount)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    string querySQL = "SELECT CUR_COUNT FROM AD_SL_NOISE_ALARM_COUNT" +
                            " WHERE DEVCODE = " + devCode;
                    object cur_count = OracleHelper.ExecuteScalar(conn,
                        CommandType.Text, querySQL, null);
                    if (cur_count == null)
                    {
                        string insertSQL =
                            "INSERT INTO AD_SL_NOISE_ALARM_COUNT (DEVCODE,MAX_COUNT,CUR_COUNT)" +
                            " VALUES (" + devCode + "," + MaxCount + "," + "1 )";
                        OracleHelper.ExecuteNonQuery(conn, CommandType.Text, insertSQL, null);
                        return false;
                    }

                    int cur = Convert.ToInt16(cur_count) + 1;
                    String queryMaxCountSQL = "SELECT MAX_COUNT FROM AD_SL_NOISE_ALARM_COUNT" +
                        " WHERE DEVCODE = " + devCode;
                    object max_count = OracleHelper.ExecuteScalar(conn,
                        CommandType.Text, queryMaxCountSQL, null);
                    if (cur > Convert.ToInt16(MaxCount))
                    {
                        string updateCurCountSQL = " UPDATE AD_SL_NOISE_ALARM_COUNT "
                     + " SET "
                     + " CUR_COUNT = " + 0
                     + " where "
                     + " DEVCODE = " + devCode;
                        OracleHelper.ExecuteNonQuery(conn, CommandType.Text, updateCurCountSQL, null);
                        return true;
                    }

                    //增加Cur
                    string updateCurCountSQL0 = " UPDATE AD_SL_NOISE_ALARM_COUNT "
                   + " SET "
                   + " CUR_COUNT = " + cur
                   + " where "
                   + " DEVCODE = " + devCode;
                    OracleHelper.ExecuteNonQuery(conn, CommandType.Text, updateCurCountSQL0, null);
                    return false;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
