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
    public class AKFSHJ : IAKFSHJ
    {
        private const string TABLE_NAME = "ALARM_CORROSION_ENVIRONMENT";

        private const string COLUMN_DEVID = "DEVCODE";
        private const string COLUMN_CELL = "CELL";

        private const string COLUMN_UNDER_TEMP1 = "TEMPERATUREUNDERINSULATION1";
        private const string COLUMN_UNDER_TEMP2 = "TEMPERATUREUNDERINSULATION2";

        private const string COLUMN_OUTTER_TEMP1 = "TEMPERATUREOUTOFINSULATION1";
        private const string COLUMN_OUTTER_TEMP2 = "TEMPERATUREOUTOFINSULATION2";

        private const string COLUMN_UNDER_VOL1 = "POTENTIALUNDERINSULATION1";
        private const string COLUMN_UNDER_VOL2 = "POTENTIALUNDERINSULATION2";
        private const string COLUMN_UNDER_VOL3 = "POTENTIALUNDERINSULATION3";
        private const string COLUMN_UNDER_VOL4 = "POTENTIALUNDERINSULATION4";
        private const string COLUMN_UNDER_VOL5 = "POTENTIALUNDERINSULATION5";
        private const string COLUMN_UNDER_VOL6 = "POTENTIALUNDERINSULATION6";

        private const string COLUMN_UNDER_WATER1 = "WATERININSULATION1";
        private const string COLUMN_UNDER_WATER2 = "WATERININSULATION2";
        private const string COLUMN_UNDER_WATER3 = "WATERININSULATION3";
        private const string COLUMN_UNDER_WATER4 = "WATERININSULATION4";
        private const string COLUMN_UNDER_WATER5 = "WATERININSULATION5";
        private const string COLUMN_UNDER_WATER6 = "WATERININSULATION6";

        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";

        private const string COLUMN_STATUS = "STATUS";

      
        private const string PARM_DEVID = ":DEVCODE";
        private const string PARM_CELL = ":CELL";

        private const string PARM_UNDER_TEMP1 = ":TEMPERATUREUNDERINSULATION1";
        private const string PARM_UNDER_TEMP2 = ":TEMPERATUREUNDERINSULATION2";

        private const string PARM_OUTTER_TEMP1 = ":TEMPERATUREOUTOFINSULATION1";
        private const string PARM_OUTTER_TEMP2 = ":TEMPERATUREOUTOFINSULATION2";

        private const string PARM_UNDER_VOL1 = ":POTENTIALUNDERINSULATION1";
        private const string PARM_UNDER_VOL2 = ":POTENTIALUNDERINSULATION2";
        private const string PARM_UNDER_VOL3 = ":POTENTIALUNDERINSULATION3";
        private const string PARM_UNDER_VOL4 = ":POTENTIALUNDERINSULATION4";
        private const string PARM_UNDER_VOL5 = ":POTENTIALUNDERINSULATION5";
        private const string PARM_UNDER_VOL6 = ":POTENTIALUNDERINSULATION6";

        private const string PARM_UNDER_WATER1 = ":WATERININSULATION1";
        private const string PARM_UNDER_WATER2 = ":WATERININSULATION2";
        private const string PARM_UNDER_WATER3 = ":WATERININSULATION3";
        private const string PARM_UNDER_WATER4 = ":WATERININSULATION4";
        private const string PARM_UNDER_WATER5 = ":WATERININSULATION5";
        private const string PARM_UNDER_WATER6 = ":WATERININSULATION6";

        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";

        private const string PARM_STATUS = ":STATUS";

      
        private const string SQL_INSERT_FSHJ = "INSERT INTO " + TABLE_NAME
                        + " (DBID,"
                        + " " + COLUMN_DEVID + ","
                        + " " + COLUMN_CELL + ","
                        + " " + COLUMN_UNDER_TEMP1 + ","
                        + " " + COLUMN_UNDER_TEMP2 + ","
                        + " " + COLUMN_OUTTER_TEMP1 + ","
                        + " " + COLUMN_OUTTER_TEMP2 + ","
                        + " " + COLUMN_UNDER_VOL1 + ","
                        + " " + COLUMN_UNDER_VOL2 + ","
                        + " " + COLUMN_UNDER_VOL3 + ","
                        + " " + COLUMN_UNDER_VOL4 + ","
                        + " " + COLUMN_UNDER_VOL5 + ","
                        + " " + COLUMN_UNDER_VOL6 + ","
                        + " " + COLUMN_UNDER_WATER1 + ","
                        + " " + COLUMN_UNDER_WATER2 + ","
                        + " " + COLUMN_UNDER_WATER3 + ","
                        + " " + COLUMN_UNDER_WATER4 + ","
                        + " " + COLUMN_UNDER_WATER5 + ","
                        + " " + COLUMN_UNDER_WATER6 + ","
                        + " " + COLUMN_STATUS + ","
                        + " " + COLUMN_UPTIME + ","
                        + " " + COLUMN_LOGTIME + ")"
                        + " VALUES "
                        + " (SEQ_CORROSION_ENVIRONMENT_ID.NEXTVAL,"
                        + " " + PARM_DEVID + ","
                        + " " + PARM_CELL + ","
                        + " " + PARM_UNDER_TEMP1 + ","
                        + " " + PARM_UNDER_TEMP2 + ","
                        + " " + PARM_OUTTER_TEMP1 + ","
                        + " " + PARM_OUTTER_TEMP2 + ","
                        + " " + PARM_UNDER_VOL1 + ","
                        + " " + PARM_UNDER_VOL2 + ","
                        + " " + PARM_UNDER_VOL3 + ","
                        + " " + PARM_UNDER_VOL4 + ","
                        + " " + PARM_UNDER_VOL5 + ","
                        + " " + PARM_UNDER_VOL6 + ","
                        + " " + PARM_UNDER_WATER1 + ","
                        + " " + PARM_UNDER_WATER2 + ","
                        + " " + PARM_UNDER_WATER3 + ","
                        + " " + PARM_UNDER_WATER4 + ","
                        + " " + PARM_UNDER_WATER5 + ","
                        + " " + PARM_UNDER_WATER6 + ","
                        + " " + PARM_STATUS + ","
                        + " " + PARM_UPTIME + ","
                        + " " + PARM_LOGTIME + ")";

        public void insert(List<Model.AKFSHJInfo> djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    foreach (Model.AKFSHJInfo dj in djs)
                    {
                        if (string.IsNullOrEmpty(dj.DEVCODE))
                        {
                            throw new Exception("设备ID为空！");
                        }
                        OracleParameter[] parms = GetAdapterParameters();
                        SetAdapterParameters(parms, dj);
                        OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_FSHJ, parms);
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
        private void SetAdapterParameters(OracleParameter[] parms, Model.AKFSHJInfo fshj)
        {
            parms[0].Value = fshj.DEVCODE;

            if (null != fshj.Cell)
            {
                parms[1].Value = fshj.Cell;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }

            if (null != fshj.UnderTemp1)
            {
                parms[2].Value = fshj.UnderTemp1;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != fshj.UnderTemp2)
            {
                parms[3].Value = fshj.UnderTemp2;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != fshj.OutterTemp1)
            {
                parms[4].Value = fshj.OutterTemp1;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != fshj.OutterTemp2)
            {
                parms[5].Value = fshj.OutterTemp2;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != fshj.UnderVo11)
            {
                parms[6].Value = fshj.UnderVo11;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }

            if (null != fshj.UnderVo12)
            {
                parms[7].Value = fshj.UnderVo12;
            }
            else
            {
                parms[7].Value = DBNull.Value;
            }

            if (null != fshj.UnderVo13)
            {
                parms[8].Value = fshj.UnderVo13;
            }
            else
            {
                parms[8].Value = DBNull.Value;
            }

            if (null != fshj.UnderVo14)
            {
                parms[9].Value = fshj.UnderVo14;
            }
            else
            {
                parms[9].Value = DBNull.Value;
            }

            if (null != fshj.UnderVo15)
            {
                parms[10].Value = fshj.UnderVo15;
            }
            else
            {
                parms[10].Value = DBNull.Value;
            }

            if (null != fshj.UnderVo16)
            {
                parms[11].Value = fshj.UnderVo16;
            }
            else
            {
                parms[11].Value = DBNull.Value;
            }

            if (null != fshj.UnderWaterIn1)
            {
                parms[12].Value = fshj.UnderWaterIn1;
            }
            else
            {
                parms[12].Value = DBNull.Value;
            }

            if (null != fshj.UnderWaterIn2)
            {
                parms[13].Value = fshj.UnderWaterIn2;
            }
            else
            {
                parms[13].Value = DBNull.Value;
            }

            if (null != fshj.UnderWaterIn3)
            {
                parms[14].Value = fshj.UnderWaterIn3;
            }
            else
            {
                parms[14].Value = DBNull.Value;
            }

            if (null != fshj.UnderWaterIn4)
            {
                parms[15].Value = fshj.UnderWaterIn4;
            }
            else
            {
                parms[15].Value = DBNull.Value;
            }

            if (null != fshj.UnderWaterIn5)
            {
                parms[16].Value = fshj.UnderWaterIn5;
            }
            else
            {
                parms[16].Value = DBNull.Value;
            }

            if (null != fshj.UnderWaterIn6)
            {
                parms[17].Value = fshj.UnderWaterIn6;
            }
            else
            {
                parms[17].Value = DBNull.Value;
            }

            if (null != fshj.Status)
            {
                parms[18].Value = fshj.Status;
            }
            else
            {
                parms[18].Value = DBNull.Value;
            }


            if (null != fshj.UpTime)
            {
                parms[19].Value = fshj.UpTime;
            }
            else
            {
                parms[19].Value = DBNull.Value;
            }

            if (null != fshj.LogTime)
            {
                parms[20].Value = fshj.LogTime;
            }
            else
            {
                parms[20].Value = DBNull.Value;
            }

        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {
            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_FSHJ, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{		                   
                                        new OracleParameter(PARM_DEVID, OracleType.NVarChar, 19),
                                        new OracleParameter(PARM_CELL, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_TEMP1, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_TEMP2, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_OUTTER_TEMP1, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_OUTTER_TEMP2, OracleType.NVarChar, 255),

                                        new OracleParameter(PARM_UNDER_VOL1, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_VOL2, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_VOL3, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_VOL4, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_VOL5, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_VOL6, OracleType.NVarChar, 255),

                                        new OracleParameter(PARM_UNDER_WATER1, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_WATER2, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_WATER3, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_WATER4, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_WATER5, OracleType.NVarChar, 255),
                                        new OracleParameter(PARM_UNDER_WATER6, OracleType.NVarChar, 255),

                                        new OracleParameter(PARM_STATUS, OracleType.NVarChar, 255),

                                        new OracleParameter(PARM_UPTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime)                                        
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_FSHJ, "INSERT:UPDATE", parms);
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
                        new OracleParameter(PARM_DEVID,devId),
                        new OracleParameter(PARM_UPTIME,upTime)
                    };
                    String SQL = "SELECT COUNT(*) FROM ALARM_CORROSION_ENVIRONMENT WHERE DEVCODE=:DEVCODE AND UPTIME=:UPTIME";
                    object count = OracleHelper.ExecuteScalar(conn, CommandType.Text, SQL, oraParams);
                    return Convert.ToInt16(count);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public String getHeatPipeTypeByDevCode(String devCode)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = " SELECT d.TURNZ " +
                             " FROM ALARM_DEVICE d " +
                             " WHERE d.DEVCODE = :devCode ";
                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", devCode),
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (null != dt.Rows[0]["TURNZ"])
                    {
                        return dt.Rows[0]["TURNZ"].ToString();
                    }
                }
            }
            return null;
        }

    }
}
