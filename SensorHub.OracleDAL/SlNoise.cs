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
    public class SlNoise:ISlNoise
    {
        // Static constants
        private const string TABLE_NAME = "AD_SL_NOISE";

        private const string COLUMN_SRCID = "DEVCODE";
        private const string COLUMN_DSTID = "DSTID";
        private const string COLUMN_DENSEDATA = "DENSEDATA";
        private const string COLUMN_CELL = "CELL";
        private const string COLUMN_SIGNAL = "SIGNAL";
        private const string COLUMN_STATUS = "STATUS";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";
        private const string COLUMN_FREQUENCY = "FREQUENCY";

        private const string PARM_SRCID = ":DEVCODE";
        private const string PARM_DSTID = ":DSTID";
        private const string PARM_DENSEDATA = ":DENSEDATA";
        private const string PARM_CELL = ":CELL";
        private const string PARM_SIGNAL = ":SIGNAL";
        private const string PARM_STATUS = ":STATUS";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";
        private const string PARM_FREQUENCY = ":FREQUENCY";

        private const string SQL_INSERT_AD_SL_NOISE = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_SRCID + ","
                        + " " + COLUMN_DSTID + ","
                        + " " + COLUMN_DENSEDATA + ","
                        + " " + COLUMN_CELL + ","
                        + " " + COLUMN_SIGNAL + ","
                        + " " + COLUMN_STATUS + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ","
                        + " " + COLUMN_FREQUENCY + ")"
                        + " VALUES "
                        + " (SEQ_AD_SL_NOISE_ID.NEXTVAL,"
                        + " " + PARM_SRCID + ","
                        + " " + PARM_DSTID + ","
                        + " " + PARM_DENSEDATA + ","
                        + " " + PARM_CELL + ","
                        + " " + PARM_SIGNAL + ","
                        + " " + PARM_STATUS + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ","
                        + " " + PARM_FREQUENCY + ")";

        public void insert(List<Model.SlNoiseInfo> djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.SlNoiseInfo dj in djs)
                    {
                        if (string.IsNullOrEmpty(dj.SRCID) || string.IsNullOrEmpty(dj.DSTID))
                        {
                            throw new Exception("设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, dj);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_AD_SL_NOISE, parms);
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
        private void SetAdapterParameters(OracleParameter[] parms, SlNoiseInfo noise)
        {
            parms[0].Value = noise.SRCID;
            parms[1].Value = noise.DSTID;

            if (null != noise.DENSEDATA)
            {
                parms[2].Value = noise.DENSEDATA;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != noise.CELL)
            {
                parms[3].Value = noise.CELL;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != noise.SIGNAL)
            {
                parms[4].Value = noise.SIGNAL;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != noise.STATUS)
            {
                parms[5].Value = noise.STATUS;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != noise.UPTIME)
            {
                parms[6].Value = noise.UPTIME;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }

            if (null != noise.LOGTIME)
            {
                parms[7].Value = noise.LOGTIME;
            }
            else
            {
                parms[7].Value = DBNull.Value;
            }

            if (null != noise.FREQUENCY)
            {
                parms[8].Value = noise.FREQUENCY;
            }
            else
            {
                parms[8].Value = DBNull.Value;
            }

        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_AD_SL_NOISE, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_SRCID, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DSTID, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DENSEDATA, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_CELL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_SIGNAL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_STATUS, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_FREQUENCY, OracleType.VarChar, 255)
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_AD_SL_NOISE, "INSERT:UPDATE", parms);
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
                    String SQL = "SELECT COUNT(*) FROM AD_SL_NOISE WHERE DEVCODE=:DEVID AND UPTIME=:UPTIME";
                    object count = OracleHelper.ExecuteScalar(conn, CommandType.Text, SQL, oraParams);
                    return Convert.ToInt16(count);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public float getLastData(SlNoiseInfo slNoiseInfo)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = " SELECT DENSEDATA " +
                             " FROM " +
                             " (SELECT * " +
                             " FROM AD_SL_NOISE " +
                             " WHERE DEVCODE = :devCode " +
                             " and UPTIME < :uptime " +
                             " order by UPTIME desc) " +
                             " where rownum=1 "; 

                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", slNoiseInfo.SRCID),
                                new OracleParameter(":uptime", slNoiseInfo.UPTIME)
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    float lastData = float.Parse(dt.Rows[0]["DENSEDATA"].ToString());
                    return lastData;
                }
            }
            return -100;
        }

        public int getAlarmNumsByArrange(float highValue, DateTime beginTime, DateTime endTime,string devCode)
        {
            string begin = beginTime.ToString("yyyy-MM-dd"); 
            string end = endTime.ToString("yyyy-MM-dd"); 

            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {

                String SQL ="select count(1) as CC from (select substr(mm.UPTIME,0,10) as time, count(1) as count from "+
                    "(select * from ad_sl_noise t where t.densedata> "+ highValue +
                     " and devcode = " + devCode + " and t.uptime < to_date('" + end + "','yyyy-MM-dd')" +
                      " and t.uptime > to_date('" + begin + "','yyyy-MM-dd')) mm GROUP BY  substr(mm.UPTIME,0,10)) tt where tt.count>0";
           
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, null).Tables[0];
                
                List<Model.SlNoiseInfo> list = new List<Model.SlNoiseInfo>();
                int result = 0;
                if (dt.Rows.Count > 0)
                {
                    result = int.Parse(dt.Rows[0]["CC"].ToString());
                }
                return result;
            }
        }

    }
}
