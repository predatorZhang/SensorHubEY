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
    public class YLiang:IYLiang
    {
        private const string TABLE_NAME = "AD_YL_YLIANG";

        private const string COLUMN_SRCID = "SRCID";
        private const string COLUMN_DSTID = "DSTID";
        private const string COLUMN_CURMINUTEYULIANG = "CURMINUTEYULIANG";
        private const string COLUMN_FORMINUTEYULIANG = "FORMINUTEYULIANG";
        private const string COLUMN_TOTALYULIANG = "TOTALYULIANG";
        private const string COLUMN_UPTIME = "UPTIME";
        private const string COLUMN_LOGTIME = "LOGTIME";
        private const String COLUMN_VALUE = "VALUE";



        private const string PARM_SRCID = ":SRCID";
        private const string PARM_DSTID = ":DSTID";
        private const string PARM_CURMINUTEYULIANG = ":CURMINUTEYULIANG";
        private const string PARM_FORMINUTEYULIANG = ":FORMINUTEYULIANG";
        private const string PARM_TOTALYULIANG = ":TOTALYULIANG";
        private const string PARM_UPTIME = ":UPTIME";
        private const string PARM_LOGTIME = ":LOGTIME";
        private const string PARM_VALUE = ":VALUE";
       
       

        private const string SQL_INSERT_YL = "INSERT INTO " + TABLE_NAME
                     + " (DBID,"
                     + " " + COLUMN_SRCID + ","
                     + " " + COLUMN_DSTID + ","
                     + " " + COLUMN_CURMINUTEYULIANG + ","
                     + " " + COLUMN_FORMINUTEYULIANG + ","
                     + " " + COLUMN_TOTALYULIANG + ","
                     + " " + COLUMN_UPTIME + ","
                     + " " + COLUMN_LOGTIME + ","
                     + " " + COLUMN_VALUE + ")"
                     + " VALUES "
                     + " (SEQ_AD_YL_YLIANG_ID.NEXTVAL,"
                     + " " + PARM_SRCID + ","
                     + " " + PARM_DSTID + ","
                     + " " + PARM_CURMINUTEYULIANG + ","
                     + " " + PARM_FORMINUTEYULIANG + ","
                     + " " + PARM_TOTALYULIANG + ","
                     + " " + PARM_UPTIME + ","
                     + " " + PARM_LOGTIME + ","
                     + " " + PARM_VALUE + ")";
      
        public void insert(Model.YLInfo djs)
        {
            OracleTransaction tran = null;
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();


                    if (string.IsNullOrEmpty(djs.SrcId) || string.IsNullOrEmpty(djs.DstId))
                    {
                        throw new Exception("设备ID为空！");
                    }
                    OracleParameter[] parms = GetAdapterParameters();
                    SetAdapterParameters(parms, djs);
                    OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_YL, parms);

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
        private void SetAdapterParameters(OracleParameter[] parms, YLInfo yuliang)
        {
            parms[0].Value = yuliang.SrcId;
            parms[1].Value = yuliang.DstId;

            if (null != yuliang.CurMinuteYuLiang)
            {
                parms[2].Value = yuliang.CurMinuteYuLiang;
            }
            else
            {
                parms[2].Value = DBNull.Value;
            }

            if (null != yuliang.ForMinuteYuLiang)
            {
                parms[3].Value = yuliang.ForMinuteYuLiang;
            }
            else
            {
                parms[3].Value = DBNull.Value;
            }

            if (null != yuliang.TotalYuLiang)
            {
                parms[4].Value = yuliang.TotalYuLiang;
            }
            else
            {
                parms[4].Value = DBNull.Value;
            }

            if (null != yuliang.UpTime)
            {
                parms[5].Value = yuliang.UpTime;
            }
            else
            {
                parms[5].Value = DBNull.Value;
            }

            if (null != yuliang.LogTime)
            {
                parms[6].Value = yuliang.LogTime;
            }
            else
            {
                parms[6].Value = DBNull.Value;
            }
            if (null != yuliang.YLiangValue)
            {
                parms[7].Value = yuliang.YLiangValue;
            }
            else {
                parms[7].Value = DBNull.Value;
            }

        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_INSERT_YL, "INSERT:UPDATE");

            if (parms == null)
            {
                parms = new OracleParameter[]{		                   
                                        new OracleParameter(PARM_SRCID, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_DSTID, OracleType.VarChar, 255),
                                        new OracleParameter(PARM_CURMINUTEYULIANG,OracleType.VarChar, 255),
                                        new OracleParameter(PARM_FORMINUTEYULIANG,OracleType.VarChar, 255),
                                        new OracleParameter(PARM_TOTALYULIANG,OracleType.VarChar, 255),
                                        new OracleParameter(PARM_UPTIME, OracleType.Timestamp),
                                        new OracleParameter(PARM_LOGTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_VALUE, OracleType.Double)
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_INSERT_YL, "INSERT:UPDATE", parms);
            }

            return parms;
        }

        /*
      public void insert(List<Model.YLInfo> djs)
      {
          OracleTransaction tran = null;
          using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
          {
              try
              {
                  conn.Open();
                  tran = conn.BeginTransaction();

                  foreach (Model.YLInfo dj in djs)
                  {
                      if (string.IsNullOrEmpty(dj.SrcId) || string.IsNullOrEmpty(dj.DstId))
                      {
                          throw new Exception("设备ID为空！");
                      }
                      OracleParameter[] parms = GetAdapterParameters();
                      SetAdapterParameters(parms, dj);
                      OracleHelper.ExecuteNonQuery(tran, CommandType.Text, SQL_INSERT_YL, parms);
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
                    String SQL = "SELECT COUNT(*) FROM AD_DJ_YLIANG WHERE DEVCODE=:DEVID AND UPTIME=:UPTIME";
                    object count = OracleHelper.ExecuteScalar(conn, CommandType.Text, SQL, oraParams);
                    return Convert.ToInt16(count);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
       **/
    }
}
