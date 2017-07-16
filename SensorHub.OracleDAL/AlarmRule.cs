using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using SensorHub.Model;

namespace SensorHub.OracleDAL
{
    public class AlarmRule:IDAL.IAlarmRule
    {
        // Static constants
        private const string TABLE_NAME = "ALARM_ALARM_RULE";

        private const string COLUMN_DBID = "DBID";
        private const string COLUMN_HIGHVALUE = "HIGHVALUE";
        private const string COLUMN_LOWVALUE = "LOWVALUE";
        private const string COLUMN_OVERTIME = "OVERTIME";
        private const string COLUMN_SALTATION = "SALTATION";
        private const string COLUMN_SENSORCODE= "SENSORCODE";
        private const string COLUMN_DEVICE_ID = "DEVICE_ID";
  
        private const string PARM_DBID = ":DBID";
        private const string PARM_HIGHVALUE = ":HIGHVALUE";
        private const string PARM_LOWVALUE = ":LOWVALUE";
        private const string PARM_OVERTIME = ":OVERTIME";
        private const string PARM_SALTATION = ":SALTATION";
        private const string PARM_SENSORCODE = ":SENSORCODE";
        private const string PARM_DEVICE_ID = ":DEVICE_ID";

        public object getOverTimeByDevId(int devId)
        {
          string SQL_SELECT_OVERTIME_BY_DEVID = "SELECT"
            + " " + COLUMN_OVERTIME
            + " FROM"
            + " " + TABLE_NAME
            + " WHERE"
            + " " + COLUMN_DEVICE_ID + "=" + devId;        

            try
            {
                return OracleHelper.ExecuteScalar(OracleHelper.ConnectionStringOrderDistributedTransaction,
                CommandType.Text, SQL_SELECT_OVERTIME_BY_DEVID, null);
            }
            catch (Exception e)
            {
                throw e;
            }
 
        }

        public AlarmRuleInfo getAlarmRule(String devCode)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    String SQL = " SELECT r.* " +
                                 " FROM ALARM_ALARM_RULE r, ALARM_DEVICE d " +
                                 " WHERE r.DEVICE_ID = d.DBID " +
                                 " and d.DEVCODE = :devCode ";
                    OracleParameter[] oraParams = new OracleParameter[]{
                            new OracleParameter(":devCode", devCode),
                        };
                    DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        AlarmRuleInfo alarmRuleInfo = new AlarmRuleInfo();
                        if (dt.Rows[0]["DBID"] != null)
                        {
                            alarmRuleInfo.Dbid = long.Parse(dt.Rows[0]["DBID"].ToString());
                        }

                        if (dt.Rows[0]["HIGHVALUE"] != null)
                        {
                            alarmRuleInfo.HighValue = float.Parse(dt.Rows[0]["HIGHVALUE"].ToString());
                        }

                        if (dt.Rows[0]["LOWVALUE"] != null)
                        {
                            alarmRuleInfo.LowValue = float.Parse(dt.Rows[0]["LOWVALUE"].ToString());
                        }

                        if (dt.Rows[0]["OVERTIME"] != null)
                        {
                            alarmRuleInfo.Overtime = float.Parse(dt.Rows[0]["OVERTIME"].ToString());
                        }

                        if (dt.Rows[0]["SALTATION"] != null)
                        {
                            alarmRuleInfo.Saltation = float.Parse(dt.Rows[0]["SALTATION"].ToString());
                        }

                        if (dt.Rows[0]["SENSORCODE"] != null)
                        {
                            alarmRuleInfo.SensorCode = dt.Rows[0]["SENSORCODE"].ToString();
                        }

                        if (dt.Rows[0]["DEVICE_ID"] != null)
                        {
                            alarmRuleInfo.DeviceId = int.Parse(dt.Rows[0]["DEVICE_ID"].ToString()); ;
                        }
                        return alarmRuleInfo;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return null;
        }

        public AlarmRuleInfo getAlarmRule(String devCode,String sensorType)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    String SQL = " SELECT r.* " +
                                 " FROM ALARM_ALARM_RULE r, ALARM_DEVICE d " +
                                 " WHERE r.DEVICE_ID = d.DBID " +
                                 " and d.DEVCODE = :devCode and r.SENSORCODE=:SENSORCODE";
                    OracleParameter[] oraParams = new OracleParameter[]{
                            new OracleParameter(":devCode", devCode),
                            new OracleParameter(":SENSORCODE", sensorType)
                        };
                    DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        AlarmRuleInfo alarmRuleInfo = new AlarmRuleInfo();
                        if (dt.Rows[0]["DBID"] != null)
                        {
                            alarmRuleInfo.Dbid = long.Parse(dt.Rows[0]["DBID"].ToString());
                        }

                        if (dt.Rows[0]["HIGHVALUE"] != null)
                        {
                            alarmRuleInfo.HighValue = float.Parse(dt.Rows[0]["HIGHVALUE"].ToString());
                        }

                        if (dt.Rows[0]["LOWVALUE"] != null)
                        {
                            alarmRuleInfo.LowValue = float.Parse(dt.Rows[0]["LOWVALUE"].ToString());
                        }

                        if (dt.Rows[0]["OVERTIME"] != null)
                        {
                            alarmRuleInfo.Overtime = float.Parse(dt.Rows[0]["OVERTIME"].ToString());
                        }

                        if (dt.Rows[0]["SALTATION"] != null)
                        {
                            alarmRuleInfo.Saltation = float.Parse(dt.Rows[0]["SALTATION"].ToString());
                        }

                        if (dt.Rows[0]["SENSORCODE"] != null)
                        {
                            alarmRuleInfo.SensorCode = dt.Rows[0]["SENSORCODE"].ToString();
                        }

                        if (dt.Rows[0]["DEVICE_ID"] != null)
                        {
                            alarmRuleInfo.DeviceId = int.Parse(dt.Rows[0]["DEVICE_ID"].ToString()); ;
                        }
                        return alarmRuleInfo;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return null;
        }

        public object getOverTimeByIdAndType(int devId, String typeCode)
        {
            string SQL_SELECT_OVERTIME_BY_DEVID = "SELECT"
           + " " + COLUMN_OVERTIME
           + " FROM"
           + " " + TABLE_NAME
           + " WHERE"
           + " " + COLUMN_DEVICE_ID + "=" + devId
           + " AND " + COLUMN_SENSORCODE + " = " + typeCode;
            try
            {
                return OracleHelper.ExecuteScalar(OracleHelper.ConnectionStringOrderDistributedTransaction,
                CommandType.Text, SQL_SELECT_OVERTIME_BY_DEVID, null);
            }
            catch (Exception e)
            {
                throw e;
            }
 
        }
    }
}
