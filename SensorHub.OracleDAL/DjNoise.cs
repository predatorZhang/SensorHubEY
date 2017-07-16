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
    public class DjNoise:IDjNoise
    {
                // Static constants
        private const string TABLE_NAME = "AD_DJ_NOISE";

        private const string COLUMN_DEVID = "DEVCODE";
        private const string COLUMN_DDATA = "DDATA";
        private const string COLUMN_DBEGIN = "DBEGIN";
        private const string COLUMN_DINTERVAL = "DINTERVAL";
        private const string COLUMN_DCOUNT = "DCOUNT";
        private const string COLUMN_WARELESSOPEN = "WARELESSOPEN";
        private const string COLUMN_WARELESSCLOSE = "WARELESSCLOSE";
        private const string COLUMN_CELL = "CELL";
        private const string COLUMN_SIGNAL = "SIGNAL";
        private const string COLUMN_STATUS = "STATUS";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string PARM_DEVID = ":DEVCODE";
        private const string PARM_DDATA = ":DDATA";
        private const string PARM_DBEGIN = ":DBEGIN";
        private const string PARM_DINTERVAL = ":DINTERVAL";
        private const string PARM_DCOUNT = ":DCOUNT";
        private const string PARM_WARELESSOPEN = ":WARELESSOPEN";
        private const string PARM_WARELESSCLOSE = ":WARELESSCLOSE";
        private const string PARM_CELL = ":CELL";
        private const string PARM_SIGNAL = ":SIGNAL";
        private const string PARM_STATUS = ":STATUS";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";

        private const string SQL_INSERT_DJNOISE = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_DDATA + ","
                        + " " + COLUMN_DBEGIN + ","
                        + " " + COLUMN_DINTERVAL + ","
                        + " " + COLUMN_DCOUNT + ","
                        + " " + COLUMN_WARELESSOPEN + ","
                        + " " + COLUMN_WARELESSCLOSE + ","
                        + " " + COLUMN_CELL + ","
                        + " " + COLUMN_SIGNAL + ","
                        + " " + COLUMN_STATUS + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_AD_DJ_NOISE_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_DDATA + ","
                        + " " + PARM_DBEGIN + ","
                        + " " + PARM_DINTERVAL + ","
                        + " " + PARM_DCOUNT + ","
                        + " " + PARM_WARELESSOPEN + ","
                        + " " + PARM_WARELESSCLOSE + ","
                        + " " + PARM_CELL + ","
                        + " " + PARM_SIGNAL + ","
                        + " " + PARM_STATUS + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(List<Model.DjNoiseInfo> djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.DjNoiseInfo dj in djs)
                    {
                        if (string.IsNullOrEmpty(dj.DEVID))
                        {
                            throw new Exception("设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, dj);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_DJNOISE, parms);
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
        /// <param name="noise">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, DjNoiseInfo noise)
        {
            parms[0].Value = noise.DEVID;

            if (null != noise.DDATA)
            {
                parms[1].Value = noise.DDATA;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != noise.DBEGIN)
            {
                parms[2].Value = noise.DBEGIN;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != noise.DINTERVAL)
            {
                parms[3].Value = noise.DINTERVAL;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != noise.DCOUNT)
            {
                parms[4].Value = noise.DCOUNT;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != noise.WARELESSOPEN)
            {
                parms[5].Value = noise.WARELESSOPEN;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != noise.WARELESSCLOSE)
            {
                parms[6].Value = noise.WARELESSCLOSE;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }

            if (null != noise.CELL)
            {
                parms[7].Value = noise.CELL;
            }
            else
            {
                parms[7].Value = DBNull.Value;
            }

            if (null != noise.SIGNAL)
            {
                parms[8].Value = noise.SIGNAL;
            }
            else
            {
                parms[8].Value = DBNull.Value;
            }

            if (null != noise.STATUS)
            {
                parms[9].Value = noise.STATUS;
            }
            else
            {
                parms[9].Value = DBNull.Value;
            }

            if (null != noise.UPTIME)
            {
                parms[10].Value = noise.UPTIME;
            }
            else
            {
                parms[10].Value = DBNull.Value;
            }

            if (null != noise.LOGTIME)
            {
                parms[11].Value = noise.LOGTIME;
            }
            else
            {
                parms[11].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_DJNOISE, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DEVID, OracleType.VarChar, 19),
                                        new OracleParameter(PARM_DDATA, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DBEGIN, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DINTERVAL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DCOUNT, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_WARELESSOPEN, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_WARELESSCLOSE, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_CELL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_SIGNAL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_STATUS, OracleType.VarChar, 255),                                        
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_DJNOISE, "INSERT:UPDATE", parms);
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
                    String SQL = "SELECT COUNT(*) FROM AD_DJ_NOISE WHERE DEVCODE=:DEVID AND UPTIME=:UPTIME";
                    object count = OracleHelper.ExecuteScalar(conn, CommandType.Text, SQL, oraParams);
                    return Convert.ToInt16(count);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public float getLastData(DjNoiseInfo slNoiseInfo)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {

                String SQL = " SELECT DDATA " +
                         " FROM " +
                         " (SELECT * " +
                         " FROM AD_DJ_NOISE " +
                         " WHERE DEVCODE = :devCode " +
                         " and UPTIME < :uptime " +
                         " order by UPTIME desc) " +
                         " where rownum=1 ";
                /*
                String SQL = " SELECT DDATA " +
                             " FROM AD_DJ_NOISE " +
                             " WHERE DEVCODE = :devCode " +
                             " and UPTIME < :uptime " +
                             " and rownum=1 " +
                             " order by UPTIME desc ";
                 * */
                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", slNoiseInfo.DEVID),
                                new OracleParameter(":uptime", slNoiseInfo.UPTIME)
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    float lastData = float.Parse(dt.Rows[0]["DDATA"].ToString());
                    return lastData;
                }
            }
            return -100;
        }
    }
}
