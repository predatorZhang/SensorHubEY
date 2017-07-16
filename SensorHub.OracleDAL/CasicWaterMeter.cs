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
    public class CasicWaterMeter : ICasicWaterMeter
    {
                // Static constants
        private const string TABLE_NAME = "ALARM_WATERQUANTITY";

        private const string COLUMN_DEVID = "DEVCODE";
        private const string COLUMN_DDATA = "DATA";
        private const string COLUMN_LOGTIME = "LOGTIME";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_CELL = "CELL";

        private const string PARM_DEVID = ":DEVCODE";
        private const string PARM_DDATA = ":DATA";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";
        private const string PARM_CELL = ":CELL";

        private const string SQL_INSERT_CASIC_PRESS = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_DDATA + ","
                        + " " + COLUMN_LOGTIME + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_CELL + ")"
                        + " VALUES "
                        + " (SEQ_ALARM_WATERQUANTITY_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_DDATA + ","
                        + " " + PARM_LOGTIME + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_CELL + ")";

        public void insert(List<Model.CasicWaterMeter> djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.CasicWaterMeter dj in djs)
                    {
                        if (string.IsNullOrEmpty(dj.DEVCODE))
                        {
                            throw new Exception("设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, dj);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_CASIC_PRESS, parms);
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
        private void SetAdapterParameters(OracleParameter[] parms, Model.CasicWaterMeter casicPress)
        {
            parms[0].Value = casicPress.DEVCODE;

            if (null != casicPress.Data)
            {
                parms[1].Value = casicPress.Data;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != casicPress.LogTime)
            {
                parms[2].Value = casicPress.LogTime;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != casicPress.UpTime)
            {
                parms[3].Value = casicPress.UpTime;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != casicPress.Cell)
            {
                parms[4].Value = casicPress.Cell;
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

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_CASIC_PRESS, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DEVID, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DDATA, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime),                              
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                         new OracleParameter(PARM_CELL, OracleType.VarChar, 255), 
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_CASIC_PRESS, "INSERT:UPDATE", parms);
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
                        new OracleParameter(":DEVCODE",devId),
                        new OracleParameter(":UPTIME",upTime)
                    };
                    String SQL = "SELECT COUNT(*) FROM ALARM_WATERQUANTITY WHERE DEVCODE=:DEVCODE AND UPTIME=:UPTIME";
                    object count = OracleHelper.ExecuteScalar(conn, CommandType.Text, SQL, oraParams);
                    return Convert.ToInt16(count);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public float getLastData(Model.CasicWaterMeter pressInfo)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = " SELECT DATA " +
                        " FROM " +
                        " (SELECT * " +
                        " FROM ALARM_WATERQUANTITY " +
                        " WHERE DEVCODE = :devCode " +
                        " and UPTIME < :uptime " +
                        " order by UPTIME desc) " +
                        " where rownum=1 "; 
                /*
                String SQL = " SELECT DATA " +
                             " FROM ALARM_WATERQUANTITY " +
                             " WHERE DEVCODE = :devCode " +
                             " and UPTIME < :uptime " +
                             " and rownum=1 " +
                             " order by UPTIME desc ";
                 * */
                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", pressInfo.DEVCODE),
                                new OracleParameter(":uptime", pressInfo.UpTime)
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    float lastData = float.Parse(dt.Rows[0]["DATA"].ToString());
                    return lastData;
                }
            }
            return -1;
        }
    }
}
