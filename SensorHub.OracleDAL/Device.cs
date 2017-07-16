using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class Device : SensorHub.IDAL.IDevice
    {
        public object getDevIdByCode(string code)
        {
            String sql = "SELECT DBID FROM ALARM_DEVICE WHERE DEVCODE='" + code + "'";
            try
            {
                return OracleHelper.ExecuteScalar(OracleHelper.ConnectionStringOrderDistributedTransaction,
                CommandType.Text, sql, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public String getDevTypeByCode(String devCode)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = " SELECT t.TYPENAME " +
                             " FROM ALARM_DEVICE d, ALARM_DEVICE_TYPE t " +
                             " WHERE d.DEVICETYPE_ID = t.DBID " +
                             " and d.DEVCODE = :devCode ";
                        OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":devCode", devCode),
                            };
                        DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            if (null != dt.Rows[0]["TYPENAME"])
                            {
                                return dt.Rows[0]["TYPENAME"].ToString();
                            }
                        }
            }
            return null;
        }
       
        public string getDevTypeByDevId(int devId)
        {
            String sql = "SELECT TYPENAME FROM ALARM_DEVICE_TYPE,ALARM_DEVICE WHERE ALARM_DEVICE.DEVICETYPE_ID = ALARM_DEVICE_TYPE.DBID AND ALARM_DEVICE.DBID=" + devId;
            try
            {
                return (string)OracleHelper.ExecuteScalar(OracleHelper.ConnectionStringOrderDistributedTransaction,
                CommandType.Text, sql, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Static constants
        private const string TABLE_NAME = "ALARM_DEVICE";

        private const string COLUMN_DEVCODE = "DEVCODE";
        private const string COLUMN_ACTIVE = "ACTIVE";

        private const string PARM_ACTIVE = ":ACTIVE";


        private const string SQL_SELECT_ALL_ACTIVE_DEVCode = "SELECT"
           + " " + COLUMN_DEVCODE
           + " FROM"
           + " " + TABLE_NAME
           + " WHERE"
           + " " + COLUMN_ACTIVE + " = " + PARM_ACTIVE;

        public float getWellDepthByDevCode(String devCode)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                String SQL = "SELECT TURNX,TURNY FROM  ALARM_DEVICE t" +
               " WHERE t.DEVCODE=:DEVCODE and t.ACTIVE = 1";

                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":DEVCODE", devCode),
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];

                if (dt.Rows.Count != 1)
                {
                    return -1;//设备编号重复
                }

                String layerName = "";
                String featureID = "";

                if (null != dt.Rows[0]["TURNX"])
                {
                    layerName = dt.Rows[0]["TURNX"].ToString();
                }
                if (null != dt.Rows[0]["TURNY"])
                {
                    featureID = dt.Rows[0]["TURNY"].ToString();
                }

                if (layerName ==""|| featureID == "")
                {
                    return -2;//无法找到设备关联阀门井
                }
                
                //TODO LIST: 查找图层对应井深

                 String SQL0 = "SELECT 井深 FROM "+layerName+ " t " +
               " WHERE "+ "编号 = :featureID";

                OracleParameter[] oraParams0 = new OracleParameter[]{
                                new OracleParameter(":featureID", featureID),
                            };
                DataTable dt0 = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL0, oraParams0).Tables[0];

                if (dt.Rows.Count != 1)
                {
                    return -3;//管井编号不唯一
                }

                float depth = Convert.ToSingle(dt0.Rows[0]["井深"].ToString());
                string strDep = depth.ToString("F2");
                return float.Parse(strDep);
            }
        }

        public List<String> getAllDevCode()
        {
            List<String> devCodes = new List<String>();
            try
            {
                OracleParameter[] parms = new OracleParameter[] { new OracleParameter(PARM_ACTIVE, 1) };
                using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, SQL_SELECT_ALL_ACTIVE_DEVCode,
                    parms))
                {
                    while (rdr.Read())
                    {
                        string devCode = GetDevCodeByOracleDataReader(rdr);
                        if (devCode != null)
                        {
                            //bySearch.Add(deviceConfig);
                            devCodes.Add(devCode);
                        }
                    }
                }
                return devCodes;
            }
            catch (Exception e)
            {
                return devCodes;
                throw e;
            }
        }


        private string GetDevCodeByOracleDataReader(OracleDataReader rdr)
        {
            string[] colsName;
            int[] colsOrdinal;
            const int colsCount = 1;

            colsName = new string[colsCount] { "DEVCODE"};
            colsOrdinal = new int[colsCount];

            if (GetColumnsOrdinal(rdr, colsName, colsOrdinal) < 0)
            {
                throw (new Exception("Error"));
            }

            string deviceStatus = null;

            for (int i = 0; i < colsCount; i++)
            {
                if (colsOrdinal[i] < 0 || rdr.IsDBNull(colsOrdinal[i]))
                {
                    continue;
                }
                switch (i)
                {
                    case 0:
                        deviceStatus = rdr.GetString(colsOrdinal[i]);
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

        public List<Model.SensorType> getSensorTypeByDevId(int devId)
        {
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
            {
                 String SQL = "SELECT s.SENSORNAME,ds.SENSORCODE FROM  ALARM_DEVICE_SENSOR ds,ALARM_SENSOR s"+
                " WHERE ds.DEVICEID=:DEVICEID and s.sensorcode = ds.sensorcode";

                OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":DEVICEID", devId),
                            };
                DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                List<Model.SensorType> sensorTypes = new List<Model.SensorType>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Model.SensorType sensorType = new Model.SensorType();
                        if (null != dt.Rows[i]["SENSORNAME"])
                        {
                            sensorType.TypeName = dt.Rows[i]["SENSORNAME"].ToString();
                        }

                        if (null != dt.Rows[i]["SENSORCODE"])
                        {
                            sensorType.TypeCode = dt.Rows[i]["SENSORCODE"].ToString();
                        }
                        sensorTypes.Add(sensorType);
                    }

                }
                return sensorTypes;
            }
            
        }
    }
}
