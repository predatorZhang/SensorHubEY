using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.IDAL;
using SensorHub.Model;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class DjPress:IDjPress
    {
        // Static constants
        private const string TABLE_NAME = "AD_DJ_PRESS";

        private const string COLUMN_DBID = "DBID";
        private const string COLUMN_DEVID = "DEVCODE";
        private const string COLUMN_PRESSDATA = "PRESSDATA";
        private const string COLUMN_CELL = "CELL";
        private const string COLUMN_SIGNAL = "SIGNAL";
        private const string COLUMN_STATUS = "STATUS";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string PARM_DBID = ":DBID";
        private const string PARM_DEVID = ":DEVCODE";
        private const string PARM_PRESSDATA = ":PRESSDATA";
        private const string PARM_CELL = ":CELL";
        private const string PARM_SIGNAL = ":SIGNAL";
        private const string PARM_STATUS = ":STATUS";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";

        private const string SQL_INSERT_DJPRESS = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_PRESSDATA + ","
                        + " " + COLUMN_CELL + ","
                        + " " + COLUMN_SIGNAL + ","
                        + " " + COLUMN_STATUS + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_AD_DJ_PRESS_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_PRESSDATA + ","
                        + " " + PARM_CELL + ","
                        + " " + PARM_SIGNAL + ","
                        + " " + PARM_STATUS + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(List<Model.DjPressInfo> djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.DjPressInfo dj in djs)
                    {
                        if (string.IsNullOrEmpty(dj.DEVID))
                        {
                            throw new Exception("设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, dj);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_DJPRESS, parms);
                    }

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

        /// <summary>
        /// An internal function to bind values parameters for insert
        /// </summary>
        /// <param name="parms">Database parameters</param>
        /// <param name="press">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, DjPressInfo press)
        {
            parms[0].Value = press.DEVID;

            if (null != press.PRESSDATA)
            {
                parms[1].Value = press.PRESSDATA;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != press.CELL)
            {
                parms[2].Value = press.CELL;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != press.SIGNAL)
            {
                parms[3].Value = press.SIGNAL;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != press.STATUS)
            {
                parms[4].Value = press.STATUS;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != press.UPTIME)
            {
                parms[5].Value = press.UPTIME;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != press.LOGTIME)
            {
                parms[6].Value = press.LOGTIME;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_DJPRESS, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DEVID, OracleType.VarChar, 19),
                                        new OracleParameter(PARM_PRESSDATA, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_CELL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_SIGNAL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_STATUS, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)                                        
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_DJPRESS, "INSERT:UPDATE", parms);
            }

            return parms;
        }


        public int queryCountByDevAndUpTime(string devId, DateTime upTime)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    OracleParameter[] oraParams = new OracleParameter[]{
                        new OracleParameter(":DEVID",devId),
                        new OracleParameter(":UPTIME",upTime)
                    };
                    String SQL = "SELECT COUNT(*) FROM AD_DJ_PRESS WHERE DEVCODE=:DEVID AND UPTIME=:UPTIME";
                    object count = OracleHelper.ExecuteScalar(conn, CommandType.Text, SQL, oraParams);
                    return Convert.ToInt16(count);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public float getLastData(Model.DjPressInfo pressInfo)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = " SELECT PRESSDATA " +
                            " FROM " +
                            " (SELECT * " +
                            " FROM AD_DJ_PRESS " +
                            " WHERE DEVCODE = :devCode " +
                            " and UPTIME < :uptime " +
                            " order by UPTIME desc) " +
                            " where rownum=1 ";

                /*
                String SQL = " SELECT PRESSDATA " +
                             " FROM AD_DJ_PRESS " +
                             " WHERE DEVCODE = :devCode " +
                             " and UPTIME < :uptime " +
                             " and rownum=1 " +
                             " order by UPTIME desc ";
                 * */
                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", pressInfo.DEVID),
                                new OracleParameter(":uptime", pressInfo.UPTIME)
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    float lastData = float.Parse(dt.Rows[0]["PRESSDATA"].ToString());
                    return lastData;
                }
            }
            return -1;
        }
    }
}
