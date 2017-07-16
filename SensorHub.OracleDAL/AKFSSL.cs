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
    public class AKFSSL : IAKFSSL
    {
        private const string TABLE_NAME = "ALARM_CORROSION_RATE";

        private const string COLUMN_DEVID = "DEVCODE";
        private const string COLUMN_OPENCIRCUITPOTENTIAL = "OPENCIRCUITPOTENTIAL";
        private const string COLUMN_INSULANCE = "INSULANCE";
        private const string COLUMN_CORROSIONRATE = "CORROSIONRATE";
        private const string COLUMN_POLARRESISTIVITY = "POLARRESISTIVITY";
        private const string COLUMN_COCURRENTDENSITY = "COCURRENTDENSITY";
        private const string COLUMN_CELL = "CELL";
        private const string COLUMN_STATUS = "STATUS";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string PARM_DEVID = ":DEVCODE";
        private const string PARM_OPENCIRCUITPOTENTIAL = ":OPENCIRCUITPOTENTIAL";
        private const string PARM_INSULANCE = ":INSULANCE";
        private const string PARM_CORROSIONRATE = ":CORROSIONRATE";
        private const string PARM_POLARRESISTIVITY = ":POLARRESISTIVITY";
        private const string PARM_COCURRENTDENSITY = ":COCURRENTDENSITY";
        private const string PARM_CELL = ":CELL";
        private const string PARM_STATUS = ":STATUS";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";


        private const string SQL_INSERT_ERROSION = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_OPENCIRCUITPOTENTIAL + ","
                        + " " + COLUMN_INSULANCE + ","
                        + " " + COLUMN_CORROSIONRATE + ","
                        + " " + COLUMN_POLARRESISTIVITY + ","
                        + " " + COLUMN_COCURRENTDENSITY + ","
                        + " " + COLUMN_CELL + ","
                        + " " + COLUMN_STATUS + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_CORROSION_RATE_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_OPENCIRCUITPOTENTIAL + ","
                        + " " + PARM_INSULANCE + ","
                        + " " + PARM_CORROSIONRATE + ","
                        + " " + PARM_POLARRESISTIVITY + ","
                        + " " + PARM_COCURRENTDENSITY + ","
                        + " " + PARM_CELL + ","
                        + " " + PARM_STATUS + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(List<Model.AKFSSLInfo> djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.AKFSSLInfo dj in djs)
                    {
                        if (string.IsNullOrEmpty(dj.DEVCODE))
                        {
                            throw new Exception("设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, dj);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_ERROSION, parms);
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
        /// <param name="liquid">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, Model.AKFSSLInfo fsslInfo)
        {
            parms[0].Value = fsslInfo.DEVCODE;

            if (null != fsslInfo.OpenCir)
            {
                parms[1].Value = fsslInfo.OpenCir;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != fsslInfo.RyResist)
            {
                parms[2].Value = fsslInfo.RyResist;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != fsslInfo.ErrosionRat)
            {
                parms[3].Value = fsslInfo.ErrosionRat;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != fsslInfo.JhResist)
            {
                parms[4].Value = fsslInfo.JhResist;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != fsslInfo.CurrentDen)
            {
                parms[5].Value = fsslInfo.CurrentDen;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != fsslInfo.Cell)
            {
                parms[6].Value = fsslInfo.Cell;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }

            if (null != fsslInfo.Status)
            {
                parms[7].Value = fsslInfo.Status;
            }
            else
            {
                parms[7].Value = DBNull.Value;
            }

            if (null != fsslInfo.UpTime)
            {
                parms[8].Value = fsslInfo.UpTime;
            }
            else
            {
                parms[8].Value = DBNull.Value;
            }

            if (null != fsslInfo.LogTime)
            {
                parms[9].Value = fsslInfo.LogTime;
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
            /*
             *   private const string PARM_DEVID = ":DEVCODE";
        private const string PARM_OPENCIRCUITPOTENTIAL = ":OPENCIRCUITPOTENTIAL";
        private const string PARM_INSULANCE = ":INSULANCE";
        private const string PARM_CORROSIONRATE = ":CORROSIONRATE";
        private const string PARM_POLARRESISTIVITY = ":POLARRESISTIVITY";
        private const string PARM_COCURRENTDENSITY = ":COCURRENTDENSITY";
        private const string PARM_CELL = ":CELL";
        private const string PARM_STATUS = ":STATUS";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";
             * */

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_ERROSION, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{		                   
                                        new OracleParameter(PARM_DEVID, OracleType.NVarChar, 19),
                                        new OracleParameter(PARM_OPENCIRCUITPOTENTIAL, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_INSULANCE, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_CORROSIONRATE, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_POLARRESISTIVITY, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_COCURRENTDENSITY, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_CELL, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_STATUS, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)                                        
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_ERROSION, "INSERT:UPDATE", parms);
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
                    String SQL = "SELECT COUNT(*) FROM ALARM_CORROSION_RATE WHERE DEVCODE=:DEVID AND UPTIME=:UPTIME";
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
