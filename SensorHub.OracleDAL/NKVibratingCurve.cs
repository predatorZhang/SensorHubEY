﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class NKVibratingCurve : SensorHub.IDAL.INKVibratingCurve
    {
        // Static constants
        private const string TABLE_NAME = "NK_GX_VIBRATING_CURVE";

        private const string COLUMN_DEVID = "DEVID";
        private const string COLUMN_DISTANCE = "DISTANCE";
        private const string COLUMN_VIBRATING = "VIBRATING";        
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string PARM_DEVID = ":DEVID";
        private const string PARM_DISTANCE = ":DISTANCE";
        private const string PARM_VIBRATING = ":VIBRATING";        
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";

        private const string SQL_INSERT_NK_GX_VIBRATING_CURVE = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_DISTANCE + ","
                        + " " + COLUMN_VIBRATING + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_NK_GX_VIBRATING_CURVE_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_DISTANCE + ","
                        + " " + PARM_VIBRATING + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(SensorHub.Model.NKVibratingCurveInfo vibratingCurve)
        {
            OracleParameter[] parms = GetAdapterParameters();

            SetAdapterParameters(parms, vibratingCurve);

            try
            {
                OracleHelper.ExecuteNonQuery(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_INSERT_NK_GX_VIBRATING_CURVE, parms);
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
        /// <param name="vibratingCurve">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, SensorHub.Model.NKVibratingCurveInfo vibratingCurve)
        {
            parms[0].Value = vibratingCurve.DEVID;
            if (null != vibratingCurve.DISTANCE)
            {
                parms[1].Value = vibratingCurve.DISTANCE;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }
            if (null != vibratingCurve.VIBRATING)
            {
                parms[2].Value = vibratingCurve.VIBRATING;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }
            if (null != vibratingCurve.UPTIME)
            {
                parms[3].Value = vibratingCurve.UPTIME;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }
            if (null != vibratingCurve.LOGTIME)
            {
                parms[4].Value = vibratingCurve.LOGTIME;
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

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_NK_GX_VIBRATING_CURVE, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DEVID, OracleType.NVarChar, 19),
                                        new OracleParameter(PARM_DISTANCE, OracleType.Clob),
                                        new OracleParameter(PARM_VIBRATING, OracleType.Clob),
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)                                        
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_NK_GX_VIBRATING_CURVE, "INSERT:UPDATE", parms);
            }

            return parms;
        }
    }
}
