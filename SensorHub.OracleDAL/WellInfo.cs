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
    public class WellInfo:IWellInfo
    {
                // Static constants
        private const string TABLE_NAME = "CASIC_WELL_INFO";

        private long dbId;
        private string devId;
        private string cell;
        private string status;
        private string descn;
        private DateTime logTime;

        private const string COLUMN_DEVID = "DEVCODE";
        private const string COLUMN_CELL = "CELL";
        private const string COLUMN_DESCN = "DESCN";
        private const string COLUMN_STATUS = "STATUS";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string PARM_DEVID = ":DEVCODE";
        private const string PARM_CELL = ":CELL";
        private const string PARM_DESCN = ":DESCN";
        private const string PARM_STATUS = ":STATUS";
        private const string PARM_LOGTIME = ":LOGTIME";

        private const string SQL_INSERT_WELLINFO = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_CELL + ","
                        + " " + COLUMN_DESCN + ","
                        + " " + COLUMN_STATUS + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_CASIC_WELL_INFO_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_CELL + ","
                        + " " + PARM_DESCN + ","
                        + " " + PARM_STATUS + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(List<Model.WellSensorInfo> djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.WellSensorInfo dj in djs)
                    {
                        if (string.IsNullOrEmpty(dj.DEVID))
                        {
                            throw new Exception("设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, dj);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_WELLINFO, parms);
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
        private void SetAdapterParameters(OracleParameter[] parms, WellSensorInfo wellInfo)
        {
            /*
                new OracleParameter(PARM_DEVID, OracleType.VarChar, 19),
                                        new OracleParameter(PARM_CELL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DESCN, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_STATUS, OracleType.VarChar, 255),                                        
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)
            */
            parms[0].Value = wellInfo.DEVID;

            if (null != wellInfo.CELL)
            {
                parms[1].Value = wellInfo.CELL;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != wellInfo.DESCN)
            {
                parms[2].Value = wellInfo.DESCN;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != wellInfo.STATUS)
            {
                parms[3].Value = wellInfo.STATUS;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != wellInfo.LOGTIME)
            {
                parms[4].Value = wellInfo.LOGTIME;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {
           
            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_WELLINFO, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DEVID, OracleType.VarChar, 19),
                                        new OracleParameter(PARM_CELL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DESCN, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_STATUS, OracleType.VarChar, 255),                                        
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_WELLINFO, "INSERT:UPDATE", parms);
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
    }
}
