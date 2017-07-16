using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class SewPeriodData : IDAL.ISewPeriodData
    {
        // Static constants
        private const string TABLE_NAME = "WS_PERIOD_DATA";

        private const string COLUMN_DEVID = "DEVID";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_CO = "CO";
        private const string COLUMN_O2 = "O2";
        private const string COLUMN_H2S = "H2S";
        private const string COLUMN_FIREGAS = "FIREGAS";

        private const string PARM_DEVID = ":DEVID";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_CO = ":CO";
        private const string PARM_O2 = ":O2";
        private const string PARM_H2S = ":H2S";
        private const string PARM_FIREGAS = ":FIREGAS";

        private const string SQL_INSERT_WS = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_CO + ","
                        + " " + COLUMN_O2 + ","
                        + " " + COLUMN_H2S + ","
                        + " " + COLUMN_FIREGAS + ")"
                        + " VALUES "
                        + " (SEQ_WS_PERIOD_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_CO + ","
                        + " " + PARM_O2 + ","
                        + " " + PARM_H2S + ","
                        + " " + PARM_FIREGAS + ")";

        public void insert(SensorHub.Model.SewPeriodDataInfo ws)
        {
            OracleParameter[] parms = GetAdapterParameters();

            SetAdapterParameters(parms, ws);

            try
            {
                OracleHelper.ExecuteNonQuery(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_INSERT_WS, parms);
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
        /// <param name="RQ">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, Model.SewPeriodDataInfo ws)
        {
            parms[0].Value = ws.DEVID;

            if (null != ws.UPTIME)
            {
                parms[1].Value = ws.UPTIME;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != ws.CO)
            {
                parms[2].Value = ws.CO;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != ws.O2)
            {
                parms[3].Value = ws.O2;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != ws.H2S)
            {
                parms[4].Value = ws.H2S;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != ws.FIREGAS)
            {
                parms[5].Value = ws.FIREGAS;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_WS, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DEVID, OracleType.VarChar, 255),                                        
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_CO, OracleType.VarChar,255),
                                        new OracleParameter(PARM_O2, OracleType.VarChar,255),
                                        new OracleParameter(PARM_H2S, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_FIREGAS, OracleType.VarChar, 255)
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_WS, "INSERT:UPDATE", parms);
            }

            return parms;
        }
    }
}
