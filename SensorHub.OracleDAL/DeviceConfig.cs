using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.OracleClient;
using System.Data;
using SensorHub.Model;

namespace SensorHub.OracleDAL
{
    public class DeviceConfig:SensorHub.IDAL.IDeviceConfig
    {
        private const string TABLE_NAME = "ALARM_DEVICE_CONFIG";

        private const string COLUMN_DBID="DBID";
        private const string COLUMN_DEVID = "DEVID";
        private const string COLUMN_FRAMECONTENT = "FRAMECONTENT";
        private const string COLUMN_SENDTIME = "SENDTIME";
        private const string COLUMN_SENSORID = "SENSORID";
        private const string COLUMN_STATUS = "STATUS";
        private const string COLUMN_WRITETIME = "WRITETIME";
      
        private const string PARM_DBID=":DBID";
        private const string PARM_DEVID = ":DEVID";
        private const string PARM_FRAMECONTENT = ":FRAMECONTENT";
        private const string PARM_SENDTIME = ":SENDTIME";
        private const string PARM_SENSORID = ":SENSORID";
        private const string PARM_STATUS = ":STATUS";
        private const string PARM_WRITETIME = ":WRITETIME";
                
        private const string SQL_SELECT_ALARM_DEVICE_CONFIG_BY_DEVICECODE_AND_SENSORCODE_SEARCH = "SELECT "
                               + COLUMN_DBID +", "
                               + COLUMN_DEVID+", "
                               + COLUMN_FRAMECONTENT+", "
                               + COLUMN_SENDTIME+", "
                               + COLUMN_SENSORID+", "
                               + COLUMN_STATUS+", "
                               + COLUMN_WRITETIME
                               +" FROM " + TABLE_NAME 
                               +" WHERE " + COLUMN_DEVID + " = " + PARM_DEVID + " AND "
                               + COLUMN_SENSORID + " = " + PARM_SENSORID + " AND " + COLUMN_STATUS + " = 0 ORDER BY "
                               + COLUMN_WRITETIME+" DESC";

        private const string SQL_SELECT_LATEST_DEVICE_CONFIG_BY_DEVICECODE_AND_SENSORCODE_SEARCH = "SELECT "
                       + COLUMN_DBID + ", "
                       + COLUMN_DEVID + ", "
                       + COLUMN_FRAMECONTENT + ", "
                       + COLUMN_SENDTIME + ", "
                       + COLUMN_SENSORID + ", "
                       + COLUMN_STATUS + ", "
                       + COLUMN_WRITETIME
                       + " FROM " + TABLE_NAME
                       + " WHERE " + COLUMN_DEVID + " = " + PARM_DEVID + " AND "
                       + COLUMN_SENSORID + " = " + PARM_SENSORID + " AND "
                       + COLUMN_WRITETIME + " = (SELECT MAX(" + COLUMN_WRITETIME + ") FROM " 
                       + TABLE_NAME + " WHERE " + COLUMN_DEVID + " = " + PARM_DEVID + " AND "
                       + COLUMN_SENSORID + " = " + PARM_SENSORID + ") ";

        private const string SQL_SELECT_LATEST_DEVICE_CONFIG_BY_SENSORCODE_SEARCH = "SELECT "
                               + COLUMN_DBID + ", "
                               + COLUMN_DEVID + ", "
                               + COLUMN_FRAMECONTENT + ", "
                               + COLUMN_SENDTIME + ", "
                               + COLUMN_SENSORID + ", "
                               + COLUMN_STATUS + ", "
                               + COLUMN_WRITETIME
                               + " FROM " + TABLE_NAME
                               + " WHERE " + COLUMN_SENSORID + " = " + PARM_SENSORID +" AND "
                               + COLUMN_STATUS + " = 0 ORDER BY "
                               + COLUMN_WRITETIME + " DESC";
        

        private const string SQL_UPDATE_ALARM_DEVICE_CONFIG = "UPDATE " + TABLE_NAME + " SET"
                                + " " + COLUMN_SENDTIME + " = " + PARM_SENDTIME + ","
                                + " " + COLUMN_STATUS + " = " + PARM_STATUS
                                + " WHERE " + COLUMN_DBID + " = " + PARM_DBID;

        /// <summary>
        /// 查询传感器配置信息
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <param name="sensorCode"></param>
        /// <returns></returns>
        public DeviceConfigInfo getDeviceConfigByDeviceCodeAndSensorCode(string deviceCode, string sensorCode)
        {
           // IList bySearch = new ArrayList();

            //Create a parameter
            OracleParameter[] parms = new OracleParameter[]{
                                                        new OracleParameter(PARM_DEVID, OracleType.NVarChar, 40),
                                                        new OracleParameter(PARM_SENSORID, OracleType.NVarChar, 40)
            };

            if (deviceCode != null)
            {
                parms[0].Value = deviceCode;
            }
            else
            {
                parms[0].Value = DBNull.Value;
            }

            if (sensorCode != null)
            {
                parms[1].Value = sensorCode;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }


            //Finally execute the query
            using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_SELECT_ALARM_DEVICE_CONFIG_BY_DEVICECODE_AND_SENSORCODE_SEARCH, parms))//
            {
                while (rdr.Read())
                {
                    DeviceConfigInfo deviceConfig;

                    deviceConfig = GetDeviceConfigByOracleDataReader(rdr);
                    if (deviceConfig != null)
                    {
                        //bySearch.Add(deviceConfig);
                        return deviceConfig;
                    }
                }
            }

            return null;

        }


        /// <summary>
        /// Method to get the work unit document information of the reader current
        /// </summary>
        /// <param name="rdr">OracleDataReader contain the work unit document information</param>
        /// <returns>the device info</returns>
        private DeviceConfigInfo GetDeviceConfigByOracleDataReader(OracleDataReader rdr)
        {
            string[] colsName;
            int[] colsOrdinal;
            const int colsCount = 7;

            DeviceConfigInfo info;
            

            colsName = new string[colsCount] { 
                                            COLUMN_DBID, COLUMN_DEVID, COLUMN_FRAMECONTENT,
                                            COLUMN_SENDTIME,COLUMN_SENSORID,COLUMN_STATUS,
                                            COLUMN_WRITETIME};
            colsOrdinal = new int[colsCount];

            if (GetColumnsOrdinal(rdr, colsName, colsOrdinal) < 0)
            {
                throw (new Exception("Error"));
            }

            info = new DeviceConfigInfo();

            for (int i = 0; i < colsCount; i++)
            {
                if (colsOrdinal[i] < 0 || rdr.IsDBNull(colsOrdinal[i]))
                {
                    continue;
                }
                switch (i)
                {
                    case 0:
                        info.DBID = rdr.GetInt32(colsOrdinal[i]);
                        break;
                    case 1:
                        info.DeviceCode = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 2:
                        info.FrameContent = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 3:
                        info.SendTime = rdr.GetDateTime(colsOrdinal[i]);
                        break;
                    case 4:
                        info.SensorCode = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 5:
                        info.Status = false;
                        break;
                    case 6:
                        info.WriteTime = rdr.GetDateTime(colsOrdinal[i]);
                        break;
                }
            }

            return info;
        }

        /// <summary>
        /// Method to get the column ordinal of reader.
        /// </summary>
        /// <param name="rdr">The OracleDataReader include the recorde</param>
        /// <param name="rdr">An array of columns to search by</param>
        /// <param name="rdr">An array of column ordinals searched</param>
        /// <returns>indicating whether success to get the ordinal</returns>
        private int GetColumnsOrdinal(OracleDataReader rdr, string[] colsName, int[] colsOrdinal)
        {
            for (int i = 0; i < colsName.Length; i++)
            {
                colsOrdinal[i] = rdr.GetOrdinal(colsName[i]);
            }

            return 0;
        }



        /// <summary>
        /// Update a deviceConfig in the database
        /// </summary>
        /// <param name="ate">Adapter information to update</param>
        public void Update(DeviceConfigInfo doc)
        {
            OracleParameter[] parms = GetDeviceConfigInfoParameters();
            SetDeviceConfigInfoParameters(parms, doc);

            try
            {
                int rst = OracleHelper.ExecuteNonQuery(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_UPDATE_ALARM_DEVICE_CONFIG, parms);
              
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// An internal function to bind values parameters for insert
        /// </summary>
        /// <param name="parms">Database parameters</param>
        /// <param name="doc">Values to bind to parameters</param>
        private void SetDeviceConfigInfoParameters(OracleParameter[] parms, DeviceConfigInfo deviceConfig)
        {
            //modified by predator 2013/1/4
            //parms[0].Value = new Guid(doc.DBID);
            //parms[1].Value = new Guid(doc.Adapter.DBID);
            //parms[2].Value = new Guid(doc.Document.DBID);

          //  parms[0].Value = deviceConfig.DBID;
        //    parms[1].Value = deviceConfig.DeviceCode;
         //   parms[2].Value = deviceConfig.FrameContent;
         //   parms[3].Value = deviceConfig.SendTime;
         //   parms[4].Value = deviceConfig.SensorCode;
        //    parms[5].Value = deviceConfig.Status;
         //   parms[6].Value = deviceConfig.WriteTime;
            parms[0].Value = deviceConfig.SendTime;
            parms[1].Value = deviceConfig.Status;
            parms[2].Value = deviceConfig.DBID;
        }


        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetDeviceConfigInfoParameters()
        {
            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_UPDATE_ALARM_DEVICE_CONFIG, "INSERT:UPDADAPTER");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_SENDTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_STATUS, OracleType.Number,1),
                                        new OracleParameter(PARM_DBID, OracleType.Number,19)
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_UPDATE_ALARM_DEVICE_CONFIG, "INSERT:UPDADAPTER", parms);
            }

            return parms;
        }


        public DeviceConfigInfo getLatestConfigByDeviceCodeAndSensorCode(string deviceCode, string sensorCode)
        {
            OracleParameter[] parms = new OracleParameter[]{
                                                        new OracleParameter(PARM_DEVID, OracleType.NVarChar, 40),
                                                        new OracleParameter(PARM_SENSORID, OracleType.NVarChar, 40)
            };

            if (deviceCode != null)
            {
                parms[0].Value = deviceCode;
            }
            else
            {
                parms[0].Value = DBNull.Value;
            }

            if (sensorCode != null)
            {
                parms[1].Value = sensorCode;
            }
            else
            {
                parms[1].Value = DBNull.Value;
            }


            //Finally execute the query
            using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_SELECT_LATEST_DEVICE_CONFIG_BY_DEVICECODE_AND_SENSORCODE_SEARCH, parms))
            {
                while (rdr.Read())
                {
                    DeviceConfigInfo deviceConfig;

                    deviceConfig = GetDeviceConfigByOracleDataReader(rdr);
                    if (deviceConfig != null)
                    {
                        //bySearch.Add(deviceConfig);
                        return deviceConfig;
                    }
                }
            }

            return null;
        }

       public List<DeviceConfigInfo> getDeviceConfigBySensorCode(string sensorCode)
        {
            OracleParameter[] parms = new OracleParameter[]{
                                                        new OracleParameter(PARM_SENSORID, OracleType.NVarChar, 40)
            };

            if (sensorCode != null)
            {
                parms[0].Value = sensorCode;
            }
            else
            {
                parms[0].Value = DBNull.Value;
            }


            //Finally execute the query
            List<Model.DeviceConfigInfo> configs = new List<Model.DeviceConfigInfo>();

            using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_SELECT_LATEST_DEVICE_CONFIG_BY_SENSORCODE_SEARCH, parms))
            {
                while (rdr.Read())
                {
                    DeviceConfigInfo deviceConfig;

                    deviceConfig = GetDeviceConfigByOracleDataReader(rdr);
                    if (deviceConfig != null)
                    {
                        configs.Add(deviceConfig);
                    }
                }
 
            }

            return configs;
        }


       private bool isTimeout(String devCode)
       {
           using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
           {
               String SQL = " SELECT t.ATTEMPTSMAX,t.ATTEMPTSCURRENT " +
                            " FROM ALARM_DEVICE_CONFIGCOUNT t " +
                            " WHERE t.DEVCODE = :devCode ";

               OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", devCode),
                            };
               DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
              
               if (dt.Rows.Count > 0)
               {
                   String maxTry="", currentTry="";
                   if (null != dt.Rows[0]["ATTEMPTSMAX"])
                   {
                       maxTry =  dt.Rows[0]["ATTEMPTSMAX"].ToString();
                   }
                   if (null != dt.Rows[0]["ATTEMPTSCURRENT"])
                   {
                       currentTry = dt.Rows[0]["ATTEMPTSCURRENT"].ToString();
                   }
                   if (float.Parse(maxTry) <= float.Parse(currentTry))
                   {
                       return true;
                   }
                   return false;
               }
           }
           return false;
       }
       public List<String> queryTimeoutDevice(List<DeviceConfigInfo> configs)
       {
           List<String> timeoutDevs = new List<String>();
           foreach (DeviceConfigInfo config in configs)
           {
               String devcode = config.DeviceCode;
               bool result = isTimeout(devcode);
               if (isTimeout(devcode))
               {
                   timeoutDevs.Add(devcode);
               }
           }
           return timeoutDevs;
       }


       private String getNextRetryIndex(String devCode)
       {
           using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
           {
               String SQL = " SELECT t.ATTEMPTSMAX,t.ATTEMPTSCURRENT " +
                            " FROM ALARM_DEVICE_CONFIGCOUNT t " +
                            " WHERE t.DEVCODE = :devCode ";

               OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", devCode),
                            };
               DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];

               if (dt.Rows.Count > 0)
               {
                   String maxTry = "", currentTry = "";
                   if (null != dt.Rows[0]["ATTEMPTSMAX"])
                   {
                       maxTry = dt.Rows[0]["ATTEMPTSMAX"].ToString();
                   }
                   if (null != dt.Rows[0]["ATTEMPTSCURRENT"])
                   {
                       currentTry = dt.Rows[0]["ATTEMPTSCURRENT"].ToString();
                   }
                  
                   if (float.Parse(maxTry) > float.Parse(currentTry))
                   {
                       float retry = float.Parse(currentTry) + 1;
                       return retry + "";
                   }
                   return maxTry;
               }
               return "";
           }
       }
       private void updateCurrentRetryCount(String devCode, String retry)
       {
           try
           {
               OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":devCode",devCode)
                };
               string SQL = " UPDATE ALARM_DEVICE_CONFIGCOUNT "
                          + " SET "
                          + " ATTEMPTSCURRENT = " + retry
                          + " where "
                          + " DEVCODE = :devCode ";

               string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
               using (OracleConnection cnn = new OracleConnection(strCnn))
               {
                   OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL, parms);
               }
           }
           catch (Exception e)
           {
               throw e;
           }
       }
       public void incRetryCountByDevcode(String devCode)
       {
           //TODO LIST:获得设备对应的Max，和currentRetry，将currentRetry+1
           String retry = getNextRetryIndex(devCode);
           if (retry != null && retry != "")
           {
               updateCurrentRetryCount(devCode, retry);
           }
       }


       public void clearDeviceConfig(String devCode)
       {
           try
           {
               OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":devCode",devCode)
                };
               string SQL = " UPDATE ALARM_DEVICE_CONFIG "
                          + " SET "
                          + " STATUS = 1 "
                          + " where "
                          + " DEVID = :devCode ";

               string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
               using (OracleConnection cnn = new OracleConnection(strCnn))
               {
                   OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL, parms);
               }
           }
           catch (Exception e)
           {
               throw e;
           }
       }
    }
}
