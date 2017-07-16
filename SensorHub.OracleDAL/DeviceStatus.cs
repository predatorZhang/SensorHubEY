using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class DeviceStatus : IDAL.IDeviceStatus
    {

        // Static constants
        private const string TABLE_NAME = "ALARM_DEVICE_STATUS";

        private const string COLUMN_DBID = "DBID";
        private const string COLUMN_DEVCODE = "DEVCODE";
        private const string COLUMN_LASTTIME = "LASTTIME";
        private const string COLUMN_STATUS = "STATUS";
   
        private const string PARM_DBID = ":DBID";
        private const string PARM_DEVCODE = ":DEVCODE";
        private const string PARM_LASTTIME = ":LASTTIME";
        private const string PARM_STATUS = ":STATUS";

        private const string SQL_UPDATE_ALARM_DEVICE_STATUS = "UPDATE " + TABLE_NAME + " SET"
                              + " " + COLUMN_LASTTIME + " = " + PARM_LASTTIME + ","
                              + " " + COLUMN_STATUS + " = " + PARM_STATUS
                              + " WHERE " + COLUMN_DBID + " = " + PARM_DBID;

        private const string SQL_DELETE_ALARM_DEVICE_STAUTS = "DELETE FROM " + TABLE_NAME + " WHERE " + COLUMN_DEVCODE + " = " + PARM_DEVCODE;

        private const string SQL_GET_UNALARM_DEVICE_STATUS="SELECT * FROM "+TABLE_NAME+" where "+COLUMN_STATUS+" = "+PARM_STATUS;

        private const string SQL_GET_ALL_DEVICE_STATUS = "SELECT * FROM " + TABLE_NAME;


       public List<Model.DeviceStatus> getAllAlarmDeviceStatus()
       {
           List<Model.DeviceStatus> devs = new List<Model.DeviceStatus>();
           try
           {
              // OracleParameter[] parms = new OracleParameter[] { new OracleParameter(PARM_STATUS, OracleType.Number,1) };
               
               using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_GET_ALL_DEVICE_STATUS, null))
               {
                   while (rdr.Read())
                   {
                       Model.DeviceStatus deviceStatus;

                       deviceStatus = GetDeviceStatusByOracleDataReader(rdr);
                       if (deviceStatus != null)
                       {
                           //bySearch.Add(deviceConfig);
                           devs.Add(deviceStatus);
                       }
                   }
               }

               return devs;

           }
           catch (Exception e)
           {
               return devs;
               throw e;
           }

       }

       public List<Model.DeviceStatus> getUnAlarmDeviceStatus()
       {
           List<Model.DeviceStatus> devs = new List<Model.DeviceStatus>();
            try
            {
                OracleParameter[] parms = new OracleParameter[] { new OracleParameter(PARM_STATUS, (object)0)};
               
                using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_GET_UNALARM_DEVICE_STATUS, parms))
                {
                    while (rdr.Read())
                    {
                        Model.DeviceStatus deviceStatus;

                        deviceStatus = GetDeviceStatusByOracleDataReader(rdr);
                        if (deviceStatus != null)
                        {
                            //bySearch.Add(deviceConfig);
                            devs.Add(deviceStatus);
                        }
                    }
                }

                return devs;

            }
            catch (Exception e)
            {
                return devs;
                throw e;
            }

       }

       private Model.DeviceStatus GetDeviceStatusByOracleDataReader(OracleDataReader rdr)
       {
           string[] colsName;
           int[] colsOrdinal;
           const int colsCount = 5;

           Model.DeviceStatus deviceStatus;


           colsName = new string[colsCount] { 
                                            "DBID", "DEVCODE", "LASTTIME",
                                            "STATUS","SENSORTYPECODE"};
           colsOrdinal = new int[colsCount];

           if (GetColumnsOrdinal(rdr, colsName, colsOrdinal) < 0)
           {
               throw (new Exception("Error"));
           }

           deviceStatus = new Model.DeviceStatus();

           for (int i = 0; i < colsCount; i++)
           {
               //if (colsOrdinal[i] < 0 || rdr.IsDBNull(colsOrdinal[i]))
               if (colsOrdinal[i] < 0)
               {
                   continue;
               }
               switch (i)
               {
                   case 0:
                       deviceStatus.DBID = rdr.GetInt32(colsOrdinal[i]);
                       break;
                   case 1:
                       deviceStatus.DEVCODE = rdr.GetString(colsOrdinal[i]);
                       break;
                   case 2:
                       if(!rdr.IsDBNull(colsOrdinal[i]))
                       {
                           deviceStatus.LASTTIME = rdr.GetDateTime(colsOrdinal[i]);
                       }
                       break;
                   case 3:
                       deviceStatus.STATUS = false;
                       break;
                   case 4:
                       deviceStatus.SensorTypeCode = rdr.GetString(colsOrdinal[i]);
                       break;
                
               }
           }

           return deviceStatus;
       }

       private int GetColumnsOrdinal(OracleDataReader rdr, string[] colsName, int[] colsOrdinal)
       {
           for (int i = 0; i < colsName.Length; i++)
           {
               colsOrdinal[i] = rdr.GetOrdinal(colsName[i]);
           }

           return 0;
       }


        //根据设备号，删除设备记录表
       public  bool removeByDevCode(String devCode)
        {
            OracleParameter parm = new OracleParameter(PARM_DEVCODE, OracleType.VarChar, 255);
            parm.Value = devCode;
            try
            {
                OracleHelper.ExecuteNonQuery(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_DELETE_ALARM_DEVICE_STAUTS, parm);
                return true;
            }
            catch
            {
                return false;
                throw;
            }

        }


        public bool add(Model.DeviceStatus devStatus)
        {
            try
            {
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":DEVCODE",devStatus.DEVCODE),
                new OracleParameter(":STATUS",(object)0),
                new OracleParameter(":SENSORTYPECODE",devStatus.SensorTypeCode)
                };
                string SQL = "INSERT INTO ALARM_DEVICE_STATUS (DBID,DEVCODE,STATUS,SENSORTYPECODE) "
                    + "VALUES"
                    + " (SEQ_ALARM_DEVICE_STATUS_ID.NEXTVAL,:DEVCODE,:STATUS,:SENSORTYPECODE)";

                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL, parms);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
        }

        /// <summary>
        /// Update a deviceConfig in the database
        /// </summary>
        /// <param name="ate">Adapter information to update</param>
        public void update(Model.DeviceStatus devStatus)
        {
            OracleParameter[] parms = GetDeviceStatusParameters();
            SetDeviceStatusParameters(parms, devStatus);

            try
            {
                int rst = OracleHelper.ExecuteNonQuery(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_UPDATE_ALARM_DEVICE_STATUS, parms);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// An internal function to bind values parameters for insert
        /// </summary>
        /// <param name="parms">Database parameters</param>
        /// <param name="doc">Values to bind to parameters</param>
        private void SetDeviceStatusParameters(OracleParameter[] parms, Model.DeviceStatus devStatus)
        {

            parms[0].Value = devStatus.LASTTIME;
            parms[1].Value = devStatus.STATUS;
            parms[2].Value = devStatus.DBID;
         
        }


        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetDeviceStatusParameters()
        {
            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_UPDATE_ALARM_DEVICE_STATUS, "INSERT:UPDADAPTER");

            if (parms == null)
            {
                parms = new OracleParameter[]{					                            
                                        new OracleParameter(PARM_LASTTIME, OracleType.DateTime),
                                        new OracleParameter(PARM_STATUS, OracleType.Number,1),
                                         new OracleParameter(PARM_DBID, OracleType.Number,19)
                };

                OracleHelperParameterCache.CacheParameterSet(SQL_UPDATE_ALARM_DEVICE_STATUS, "INSERT:UPDADAPTER", parms);
            }

            return parms;
        }

        public Model.DeviceStatus getByDevCode(String devCode)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = " SELECT * " +
                             " FROM ALARM_DEVICE_STATUS " +
                             " WHERE DEVCODE = :DEVCODE ";
                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":DEVCODE", devCode)
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Model.DeviceStatus deviceStatus = new Model.DeviceStatus();
                    if (dt.Rows[0]["DBID"] != null)
                    {
                        deviceStatus.DBID = long.Parse(dt.Rows[0]["DBID"].ToString());
                    }

                    if (dt.Rows[0]["DEVCODE"] != null)
                    {
                        deviceStatus.DEVCODE = dt.Rows[0]["DEVCODE"].ToString();
                    }

                    if (dt.Rows[0]["LASTTIME"] != null && !dt.Rows[0]["LASTTIME"].ToString().Equals(""))
                    {
                        deviceStatus.LASTTIME = (DateTime) dt.Rows[0]["LASTTIME"];
                    }

                    if (dt.Rows[0]["STATUS"] != null)
                    {
                        if (dt.Rows[0]["STATUS"].ToString().Equals("0"))
                        {
                            deviceStatus.STATUS = false;
                        }
                        else
                        {
                            deviceStatus.STATUS = true;
                        }
                    }
                    return deviceStatus;
                }
            }
            return null;
        }

        public Model.DeviceStatus getByDevCodeAndType(String devCode, String sensorTypeCode)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = " SELECT * " +
                             " FROM ALARM_DEVICE_STATUS " +
                             " WHERE DEVCODE = :DEVCODE AND SENSORTYPECODE= :SENSORTYPECODE";
                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":DEVCODE", devCode),
                                 new OracleParameter(":SENSORTYPECODE", sensorTypeCode)
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Model.DeviceStatus deviceStatus = new Model.DeviceStatus();
                    if (dt.Rows[0]["DBID"] != null)
                    {
                        deviceStatus.DBID = long.Parse(dt.Rows[0]["DBID"].ToString());
                    }

                    if (dt.Rows[0]["DEVCODE"] != null)
                    {
                        deviceStatus.DEVCODE = dt.Rows[0]["DEVCODE"].ToString();
                    }

                    if (dt.Rows[0]["LASTTIME"] != null && !dt.Rows[0]["LASTTIME"].ToString().Equals(""))
                    {
                        deviceStatus.LASTTIME = (DateTime)dt.Rows[0]["LASTTIME"];
                    }

                    if (dt.Rows[0]["STATUS"] != null)
                    {
                        if (dt.Rows[0]["STATUS"].ToString().Equals("0"))
                        {
                            deviceStatus.STATUS = false;
                        }
                        else
                        {
                            deviceStatus.STATUS = true;
                        }
                    }
                    return deviceStatus;
                }
            }
            return null;
        }

        public bool isNotExist(String devCode)
        {

            String sql = "SELECT COUNT(*) FROM ALARM_DEVICE_STATUS WHERE DEVCODE='" + devCode + "'";
            try
            {
                object dad = OracleHelper.ExecuteScalar(OracleHelper.ConnectionStringOrderDistributedTransaction,
                CommandType.Text, sql, null);
                int count = Convert.ToInt32(dad);
                return count == 0 ? true : false;
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
        }

        public bool isNotExist(String devCode,String sensorTypeCode)
        {

            String sql = "SELECT COUNT(*) FROM ALARM_DEVICE_STATUS WHERE DEVCODE='" + devCode + "'"+" AND SENSORTYPECODE = "+sensorTypeCode;
            try
            {
                object dad = OracleHelper.ExecuteScalar(OracleHelper.ConnectionStringOrderDistributedTransaction,
                CommandType.Text, sql, null);
                int count = Convert.ToInt32(dad);
                return count == 0 ? true : false;
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
        }
    }
}
