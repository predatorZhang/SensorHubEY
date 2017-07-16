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
    public class RqGas:IRqGas
    {
        // Static constants
        private const string TABLE_NAME = "JG_RQ_GAS";

        private const string COLUMN_DBID = "DBID";
        private const string COLUMN_DEVID = "DEVID";
        private const string COLUMN_LEAKDATA = "LEAKDATA";
        private const string COLUMN_INPRESS = "INPRESS";
        private const string COLUMN_OUTPRESS = "OUTPRESS";
        private const string COLUMN_TEMPGAS = "TEMPGAS";
        private const string COLUMN_TEMPROOM = "TEMPROOM";
        private const string COLUMN_CELLPOWER = "CELLPOWER";
        private const string COLUMN_RECORDTIME = "RECORDTIME";

        private const string PARM_DBID = ":DBID";
        private const string PARM_DEVID = ":DEVID";
        private const string PARM_LEAKDATA = ":LEAKDATA";
        private const string PARM_INPRESS = ":INPRESS";
        private const string PARM_OUTPRESS = ":OUTPRESS";
        private const string PARM_TEMPGAS = ":TEMPGAS";
        private const string PARM_TEMPROOM = ":TEMPROOM";
        private const string PARM_CELLPOWER = ":CELLPOWER";
        private const string PARM_RECORDTIME = ":RECORDTIME";

        private const string SQL_INSERT_RQGAS = "INSERT INTO " + TABLE_NAME
                        + "(" + COLUMN_DBID + ","
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_LEAKDATA + ","
                        + " " + COLUMN_INPRESS + ","
                        + " " + COLUMN_OUTPRESS + ","
                        + " " + COLUMN_TEMPGAS + ","
                        + " " + COLUMN_TEMPROOM + ","
                        + " " + COLUMN_CELLPOWER + ","
                        + " " + COLUMN_RECORDTIME + ")"
                        + " VALUES "
                        + "(" + PARM_DBID + ","
                        + " " + PARM_DEVID + ","
                        + " " + PARM_LEAKDATA + ","
                        + " " + PARM_INPRESS + ","
                        + " " + PARM_OUTPRESS + ","
                        + " " + PARM_TEMPGAS + ","
                        + " " + PARM_TEMPROOM + ","
                        + " " + PARM_CELLPOWER + ","
                        + " " + PARM_RECORDTIME + ")";

        public void insert(RqGasInfo gas)
        {
            OracleParameter[] parms = GetAdapterParameters();

            SetAdapterParameters(parms, gas);

            try
            {
                OracleHelper.ExecuteNonQuery(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_INSERT_RQGAS, parms);
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
        /// <param name="gas">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, RqGasInfo gas)
        {
            parms[0].Value = gas.DBID;
            parms[1].Value = gas.DEVID;
            if (null != gas.LEAKDATA)
            {
                parms[2].Value = gas.LEAKDATA;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }
            if (null != gas.INPRESS)
            {
                parms[3].Value = gas.INPRESS;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }
            if (null != gas.OUTPRESS)
            {
                parms[4].Value = gas.OUTPRESS;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }
            if (null != gas.TEMPGAS)
            {
                parms[5].Value = gas.TEMPGAS;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }
            if (null != gas.TEMPROOM)
            {
                parms[6].Value = gas.TEMPROOM;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }
            if (null != gas.CELLPOWER)
            {
                parms[7].Value = gas.CELLPOWER;
            }
            else
            {
                parms[7].Value = DBNull.Value;
            }
            if (null != gas.RECORDTIME)
            {
                parms[8].Value = gas.RECORDTIME;
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

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_RQGAS, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_DBID, OracleType.Number,19),
                                        new OracleParameter(PARM_DEVID, OracleType.NVarChar, 19),
                                        new OracleParameter(PARM_LEAKDATA, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_INPRESS, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_OUTPRESS, OracleType.NVarChar,255),
                                        new OracleParameter(PARM_TEMPGAS, OracleType.NVarChar,255),
                                        new OracleParameter(PARM_TEMPROOM, OracleType.NVarChar, 255),
                                         new OracleParameter(PARM_CELLPOWER, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_RECORDTIME, OracleType.DateTime)                                        
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_RQGAS, "INSERT:UPDATE", parms);
            }

            return parms;
        }
    }
}
