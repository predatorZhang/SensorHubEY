using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using SensorHub.Model;

namespace SensorHub.OracleDAL
{
    public class AlarmRecord:IDAL.IAlarmRecord
    {
        // Static constants 
        private const string TABLE_NAME = "ALARM_ALARM_RECORD";

        private const string COLUMN_DEVICE_CODE = "DEVICE_CODE";
        private const string COLUMN_DEVICE_TYPE_NAME = "DEVICE_TYPE_NAME";
        private const string COLUMN_RECORDCODE = "RECORDCODE";
        private const string COLUMN_MESSAGE = "MESSAGE";
        private const string COLUMN_ITEMNAME = "ITEMNAME";
        private const string COLUMN_ITEMVALUE = "ITEMVALUE";
        private const string COLUMN_DEVICE_ID = "DEVICE_ID";
        private const string COLUMN_RECORDDATE = "RECORDDATE";
        private const string COLUMN_MESSAGE_STATUS = "MESSAGE_STATUS";
        private const string COLUMN_ACTIVE = "ACTIVE";

        private const string PARM_DEVICE_CODE = ":DEVICE_CODE";
        private const string PARM_DEVICE_TYPE_NAME = ":DEVICE_TYPE_NAME";
        private const string PARM_RECORDCODE = ":RECORDCODE";
        private const string PARM_MESSAGE = ":MESSAGE";
        private const string PARM_ITEMNAME = ":ITEMNAME";
        private const string PARM_ITEMVALUE = ":ITEMVALUE";
        private const string PARM_DEVICE_ID = ":DEVICE_ID";
        private const string PARM_RECORDDATE = ":RECORDDATE";
        private const string PARM_MESSAGE_STATUS = ":MESSAGE_STATUS";
        private const string PARM_ACTIVE = ":ACTIVE";

        private const string SQL_INSERT_ALARM_RECORD = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVICE_CODE + ","
                        + " " + COLUMN_DEVICE_TYPE_NAME + ","
                        + " " + COLUMN_RECORDCODE + ","
                        + " " + COLUMN_MESSAGE + ","
                        + " " + COLUMN_ITEMNAME + ","
                        + " " + COLUMN_ITEMVALUE + ","
                        + " " + COLUMN_DEVICE_ID + ","
                        + " " + COLUMN_RECORDDATE + ","
                        + " " + COLUMN_MESSAGE_STATUS + ","
                        + " " + COLUMN_ACTIVE + ")"
                        + " VALUES "
                        + " (SEQ_ALARM_RECORD_ID.NEXTVAL,"
                        + " " + PARM_DEVICE_CODE + ","
                        + " " + PARM_DEVICE_TYPE_NAME + ","
                        + " " + PARM_RECORDCODE + ","
                        + " " + PARM_MESSAGE + ","
                        + " " + PARM_ITEMNAME + ","
                        + " " + PARM_ITEMVALUE + ","
                        + " " + PARM_DEVICE_ID + ","
                        + " " + PARM_RECORDDATE + ","
                        + " " + PARM_MESSAGE_STATUS + ","
                        + " " + PARM_ACTIVE + ")";

        public void insert(List<Model.AlarmRecordInfo> alarms)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();

                    foreach (Model.AlarmRecordInfo alarm in alarms)
                    {
                        //TODO LIST：清除已经有的报警记录
                        tran = conn.BeginTransaction();
                        updateStatus(alarm);
                        if (alarm.DEVICE_ID==null)
                        {
                            throw new Exception(alarm.ITEMNAME + ":设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, alarm);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_ALARM_RECORD, parms);
                        tran.Commit();
                    }

                }
                catch (Exception e)
                {
                    if (null != tran)
                    {
                        tran.Rollback();
                    }
                    throw (e);
                }
            }
        }


        public void updateStatus(Model.AlarmRecordInfo alarm)
        {
            try
            {
                if (alarm.DEVICE_CODE == null)
                {
                    throw new Exception(alarm.ITEMNAME + ":设备编号为空！");
                }
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":DEVICE_CODE",alarm.DEVICE_CODE),
                };
                string SQL = " UPDATE ALARM_ALARM_RECORD "
                            + " SET "
                            + " ACTIVE = " + 0
                            + " where "
                            + " DEVICE_CODE = :DEVICE_CODE ";
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

        public void remove(String devCode)
        {
            try
            {
                if (devCode == null)
                {
                    throw new Exception("待删除设备编号为空！");
                }
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":DEVICE_CODE",devCode),
                };
                string SQL = " UPDATE ALARM_ALARM_RECORD "
                            + " SET "
                            + " ACTIVE = " + 0
                            + " where "
                            + " DEVICE_CODE = :DEVICE_CODE ";
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

        /// <summary>
        /// An internal function to bind values parameters for insert
        /// </summary>
        /// <param name="parms">Database parameters</param>
        /// <param name="noise">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, Model.AlarmRecordInfo alarm)
        {
            if (null != alarm.DEVICE_CODE)
            {
                parms[0].Value = alarm.DEVICE_CODE;
            }
            else
            {
                parms[0].Value = DBNull.Value;
            }

            if (null != alarm.DEVICE_TYPE_NAME)
            {
                parms[1].Value = alarm.DEVICE_TYPE_NAME;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != alarm.RECORDCODE)
            {
                parms[2].Value = alarm.RECORDCODE;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != alarm.MESSAGE)
            {
                parms[3].Value = alarm.MESSAGE;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != alarm.ITEMNAME)
            {
                parms[4].Value = alarm.ITEMNAME;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != alarm.ITEMVALUE)
            {
                parms[5].Value = alarm.ITEMVALUE;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != alarm.DEVICE_ID)
            {
                parms[6].Value = alarm.DEVICE_ID;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }

            if (null != alarm.RECORDDATE)
            {
                parms[7].Value = alarm.RECORDDATE;
            }
            else
            {
                parms[7].Value = DBNull.Value;
            }

            if (null != alarm.MESSAGE_STATUS)
            {
                parms[8].Value = alarm.MESSAGE_STATUS;
            }
            else
            {
                parms[8].Value = DBNull.Value;
            }

            if (null != alarm.ACTIVE)
            {
                parms[9].Value = alarm.ACTIVE;
            }
            else
            {
                parms[9].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {

            OracleParameter[] parms = new OracleParameter[]{	
				                        new OracleParameter(PARM_DEVICE_CODE, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DEVICE_TYPE_NAME, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_RECORDCODE, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_MESSAGE, OracleType.VarChar, 4000),
                                        new OracleParameter(PARM_ITEMNAME, OracleType.VarChar,255),
                                        new OracleParameter(PARM_ITEMVALUE, OracleType.VarChar,4000),
                                        new OracleParameter(PARM_DEVICE_ID, OracleType.Number,19),
                                        new OracleParameter(PARM_RECORDDATE, OracleType.DateTime),
                                        new OracleParameter(PARM_MESSAGE_STATUS, OracleType.Number,1),
                                        new OracleParameter(PARM_ACTIVE, OracleType.Number,1)};
            return parms;
        }

        //保存压力温度光纤报警记录
        //先查询联合查询设备ID、ItemValue是否有报警记录，有的话清楚再保存
        public void saveGXAlarm(Model.AlarmRecordInfo alarm)
        {
            try
            {
                if (alarm.DEVICE_CODE == null)
                {
                    throw new Exception(alarm.ITEMNAME + ":设备编号为空！");
                }
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":DEVICE_CODE",alarm.DEVICE_CODE),
                 new OracleParameter(":ITEMNAME",alarm.ITEMNAME)
                };
                string SQL = " UPDATE ALARM_ALARM_RECORD "
                            + " SET "
                            + " ACTIVE = " + 0
                            + " where "
                            + " DEVICE_CODE = :DEVICE_CODE AND "
                            + " ITEMNAME = :ITEMNAME";
                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL, parms);

                    //保存新的报警记录
                    OracleParameter[] parms0 = GetAdapterParameters();
                    SetAdapterParameters(parms0, alarm);
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL_INSERT_ALARM_RECORD, parms0);    
                }

            }
            catch (Exception e)
            {
                throw e;
            }
 
        }

        //清除设备故障状态
        public void removeDeviceStatus(string devCode, List<int> devStatuslList)
        {
            //清除原有状态
            if (devStatuslList != null && devStatuslList.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (int i in devStatuslList)
                {
                    sb.Append("'" + i + "',");
                }
                string inStr = sb.ToString().Substring(0, sb.ToString().Length - 1);

                 OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":DEVICE_CODE",devCode)
                };
                string UPDATE_SQL = " UPDATE ALARM_ALARM_RECORD "
                                    + " SET "
                                    + " MESSAGE_STATUS = " + -1
                                    + " WHERE "
                                    + " DEVICE_CODE = :DEVICE_CODE AND "
                                    + " MESSAGE IN " + "(" + inStr + ") AND"
                                    + " MESSAGE_STATUS = 0";

                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, UPDATE_SQL, parms);
                }
            }
        }

        public void add(AlarmRecordInfo alarm)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    if (alarm.DEVICE_ID == null)
                    {
                       throw new Exception(alarm.ITEMNAME + ":设备ID为空！");
                    }
                    OracleParameter[] parms = GetAdapterParameters();
                    SetAdapterParameters(parms, alarm);
                    OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_ALARM_RECORD, parms);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    if (null != tran)
                    {
                        tran.Rollback();
                    }
                    throw (e);
                }
            }
        }

    }
}
