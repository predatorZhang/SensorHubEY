using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class RQPeriod : IDAL.IRQPeriod
    {
        // Static constants
        private const string TABLE_NAME = "XT_RQ_PERIOD";

        private const string COLUMN_ADDRESS = "ADDRESS";
        private const string COLUMN_INPRESS = "INPRESS";
        private const string COLUMN_OUTPRESS = "OUTPRESS";
        private const string COLUMN_FLOW = "FLOW";
        private const string COLUMN_STRENGTH = "STRENGTH";
        private const string COLUMN_TEMPERATURE = "TEMPERATURE";
        private const string COLUMN_CELL = "CELL";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string PARM_ADDRESS = ":ADDRESS";
        private const string PARM_INPRESS = ":INPRESS";
        private const string PARM_OUTPRESS = ":OUTPRESS";
        private const string PARM_FLOW = ":FLOW";
        private const string PARM_STRENGTH = ":STRENGTH";
        private const string PARM_TEMPERATURE = ":TEMPERATURE";
        private const string PARM_CELL = ":CELL";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";

        private const string SQL_INSERT_RQ = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_ADDRESS + ","
                        + " " + COLUMN_INPRESS + ","
                        + " " + COLUMN_OUTPRESS + ","
                        + " " + COLUMN_FLOW + ","
                        + " " + COLUMN_STRENGTH + ","
                        + " " + COLUMN_TEMPERATURE + ","
                        + " " + COLUMN_CELL + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_XT_RQ_PERIOD.NEXTVAL,"
                        + " " + PARM_ADDRESS + ","
                        + " " + PARM_INPRESS + ","
                        + " " + PARM_OUTPRESS + ","
                        + " " + PARM_FLOW + ","
                        + " " + PARM_STRENGTH + ","
                        + " " + PARM_TEMPERATURE + ","
                        + " " + PARM_CELL + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(List<Model.RQPeriodInfo> rqs)
        {

            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.RQPeriodInfo rq in rqs)
                    {
                        if (string.IsNullOrEmpty(rq.ADDRESS))
                        {
                            throw new Exception("讯腾燃气传感器上传设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, rq);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_RQ, parms);
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
        /// <param name="RQ">Values to bind to parameters</param>
        private void SetAdapterParameters(OracleParameter[] parms, Model.RQPeriodInfo RQ)
        {
            if (null != RQ.ADDRESS)
            {
                parms[0].Value = RQ.ADDRESS;
            }
            else
            {
                parms[0].Value = DBNull.Value;
            }

            if (null != RQ.INPRESS)
            {
                parms[1].Value = RQ.INPRESS;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != RQ.OUTPRESS)
            {
                parms[2].Value = RQ.OUTPRESS;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != RQ.FLOW)
            {
                parms[3].Value = RQ.FLOW;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != RQ.STRENGTH)
            {
                parms[4].Value = RQ.STRENGTH;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != RQ.TEMPERATURE)
            {
                parms[5].Value = RQ.TEMPERATURE;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != RQ.CELL)
            {
                parms[6].Value = RQ.CELL;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }

            if (null != RQ.UPTIME)
            {
                parms[7].Value = RQ.UPTIME;
            }
            else
            {
                parms[7].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }

            if (null != RQ.LOGTIME)
            {
                parms[8].Value = RQ.LOGTIME;
            }
            else
            {
                parms[8].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_RQ, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_ADDRESS, OracleType.VarChar, 255),                                        
                                        new OracleParameter(PARM_INPRESS, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_OUTPRESS, OracleType.VarChar,255),
                                        new OracleParameter(PARM_FLOW, OracleType.VarChar,255),
                                        new OracleParameter(PARM_STRENGTH, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_TEMPERATURE, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_CELL, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime, 255)
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_RQ, "INSERT:UPDATE", parms);
            }

            return parms;
        }
    }
}
