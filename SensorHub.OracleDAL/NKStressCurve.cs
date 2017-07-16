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
    public class NKStressCurve : INKStressCurve
    {
        // Static constants
        private const string TABLE_NAME = "NK_GX_STRESS_CURVE";

        private const string COLUMN_DEVID = "DEVID";
        private const string COLUMN_DISTANCE = "DISTANCE";
        private const string COLUMN_STRESS = "STRESS";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string PARM_DEVID = ":DEVID";
        private const string PARM_DISTANCE = ":DISTANCE";
        private const string PARM_STRESS = ":STRESS";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";        

        private const string SQL_INSERT_NK_GX_STRESS_CURVE = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_DISTANCE + ","
                        + " " + COLUMN_STRESS + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_NK_GX_STRESS_CURVE_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_DISTANCE + ","
                        + " " + PARM_STRESS + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(SensorHub.Model.NKStressCurveInfo stressCurve)
        {
            OracleParameter[] parms = GetAdapterParameters();

            SetAdapterParameters(parms, stressCurve);

            try
            {
                OracleHelper.ExecuteNonQuery(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_INSERT_NK_GX_STRESS_CURVE, parms);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// An internal function to bind values parameters for insert
        /// </summary>
        /// <param name="parms">Database parameters</param>
        /// <param name="stressCurve">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, SensorHub.Model.NKStressCurveInfo stressCurve)
        {
            parms[0].Value = stressCurve.DEVID;
            if (null != stressCurve.DISTANCE)
            {
                parms[1].Value = stressCurve.DISTANCE;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }
            if (null != stressCurve.STRESS)
            {
                parms[2].Value = stressCurve.STRESS;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }
            if (null != stressCurve.UPTIME)
            {
                parms[3].Value = stressCurve.UPTIME;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }
            if (null != stressCurve.LOGTIME)
            {
                parms[4].Value = stressCurve.LOGTIME;
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

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_NK_GX_STRESS_CURVE, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DEVID, OracleType.NVarChar, 19),
                                        new OracleParameter(PARM_DISTANCE, OracleType.Clob),
                                        new OracleParameter(PARM_STRESS, OracleType.Clob),
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)                                        
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_NK_GX_STRESS_CURVE, "INSERT:UPDATE", parms);
            }

            return parms;
        }
    }
}
