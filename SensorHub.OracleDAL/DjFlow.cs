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
    public class DjFlow:IDjFlow
    {
        // Static constants
        private const string TABLE_NAME = "AD_DJ_FLOW";

        private const string COLUMN_DEVID = "DEVCODE";
        private const string COLUMN_INSDATA = "INSDATA";
        private const string COLUMN_NETDATA = "NETDATA";
        private const string COLUMN_POSDATA = "POSDATA";
        private const string COLUMN_NEGDATA = "NEGDATA";
        private const string COLUMN_SIGNAL = "SIGNAL";
        private const string COLUMN_CELL = "CELL";
        private const string COLUMN_STATUS = "STATUS";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string PARM_DEVID = ":DEVCODE";
        private const string PARM_INSDATA = ":INSDATA";
        private const string PARM_NETDATA = ":NETDATA";
        private const string PARM_POSDATA = ":POSDATA";
        private const string PARM_NEGDATA = ":NEGDATA";
        private const string PARM_SIGNAL = ":SIGNAL";
        private const string PARM_CELL = ":CELL";
        private const string PARM_STATUS = ":STATUS";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";

        private const string SQL_INSERT_DJFLOW = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_INSDATA + ","
                        + " " + COLUMN_NETDATA + ","
                        + " " + COLUMN_POSDATA + ","
                        + " " + COLUMN_NEGDATA + ","
                        + " " + COLUMN_SIGNAL + ","
                        + " " + COLUMN_CELL + ","
                        + " " + COLUMN_STATUS + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_AD_DJ_FLOW_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_INSDATA + ","
                        + " " + PARM_NETDATA + ","
                        + " " + PARM_POSDATA + ","
                        + " " + PARM_NEGDATA + ","
                        + " " + PARM_SIGNAL + ","
                        + " " + PARM_CELL + ","
                        + " " + PARM_STATUS + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(List<Model.DjFlowInfo> djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.DjFlowInfo dj in djs)
                    {
                        if (string.IsNullOrEmpty(dj.DEVID))
                        {
                            throw new Exception("设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, dj);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_DJFLOW, parms);
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
        /// <param name="flow">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, DjFlowInfo flow)
        {
            parms[0].Value = flow.DEVID;

            if (null != flow.INSDATA)
            {
                parms[1].Value = flow.INSDATA;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != flow.NETDATA)
            {
                parms[2].Value = flow.NETDATA;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != flow.POSDATA)
            {
                parms[3].Value = flow.POSDATA;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != flow.NEGDATA)
            {
                parms[4].Value = flow.NEGDATA;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != flow.SIGNAL)
            {
                parms[5].Value = flow.SIGNAL;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != flow.CELL)
            {
                parms[6].Value = flow.CELL;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }

            if (null != flow.STATUS)
            {
                parms[7].Value = flow.STATUS;
            }
            else
            {
                parms[7].Value = DBNull.Value;
            }

            if (null != flow.UPTIME)
            {
                parms[8].Value = flow.UPTIME;
            }
            else
            {
                parms[8].Value = DBNull.Value;
            }

            if (null != flow.LOGTIME)
            {
                parms[9].Value = flow.LOGTIME;
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

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_DJFLOW, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DEVID, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_INSDATA, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_NETDATA, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_POSDATA, OracleType.VarChar,255),
                                        new OracleParameter(PARM_NEGDATA, OracleType.VarChar,255),
                                        new OracleParameter(PARM_SIGNAL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_CELL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_STATUS, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)                                        
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_DJFLOW, "INSERT:UPDATE", parms);
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
                    String SQL = "SELECT COUNT(*) FROM AD_DJ_FLOW WHERE DEVCODE=:DEVID AND UPTIME=:UPTIME";
                    object count = OracleHelper.ExecuteScalar(conn, CommandType.Text, SQL, oraParams);
                    return Convert.ToInt16(count);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public float getLastData(Model.DjFlowInfo pressInfo)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = " SELECT INSDATA " +
                            " FROM " +
                            " (SELECT * " +
                            " FROM AD_DJ_FLOW " +
                            " WHERE DEVCODE = :devCode " +
                            " and UPTIME < :uptime " +
                            " order by UPTIME desc) " +
                            " where rownum=1 ";

                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", pressInfo.DEVID),
                                new OracleParameter(":uptime", pressInfo.UPTIME)
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    float lastData = float.Parse(dt.Rows[0]["INSDATA"].ToString());
                    return lastData;
                }
            }
            return -1;
        }
    }
}
