using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class Config : IDAL.IConfig
    {

        // Static constants
        private const string TABLE_NAME = "ALARM_DEVICE_CONFIG";

        private const string COLUMN_DBID = "DBID";
        private const string COLUMN_DEVID = "DEVID";
        private const string COLUMN_FRAMECONTENT = "FRAMECONTENT";
        private const string COLUMN_WRITETIME = "WRITETIME";
        private const string COLUMN_SENDTIME = "SENDTIME";
        private const string COLUMN_STATUS = "STATUS";

        private const string PARM_DBID = ":DBID";
        private const string PARM_DEVID = ":DEVID";
        private const string PARM_FRAMECONTENT = ":FRAMECONTENT";
        private const string PARM_WRITETIME = ":WRITETIME";
        private const string PARM_SENDTIME = ":SENDTIME";
        private const string PARM_STATUS = ":STATUS";


        private const string SQL_SELECT_CFG_DEV = "SELECT"
            + " " + COLUMN_DBID + ","
            + " " + COLUMN_DEVID + ","
            + " " + COLUMN_FRAMECONTENT + ","
            + " " + COLUMN_WRITETIME + ","
            + " " + COLUMN_SENDTIME + ","
            + " " + COLUMN_STATUS
            + " FROM"
            + " " + TABLE_NAME
            + " WHERE"
            + " " + COLUMN_STATUS + "=0 AND"
            + " " + COLUMN_DEVID + "=" + PARM_DEVID;        

        public Model.ConfigInfo getConfigByDevId(string devId)
        {
            OracleParameter[] parms = GetAdapterParameters();
            SetAdapterParameters(parms, devId);
            DataTable tb = OracleHelper.ExecuteDataset(OracleHelper.ConnectionStringOrderDistributedTransaction,
                CommandType.Text, SQL_SELECT_CFG_DEV, parms).Tables[0];
            return getModel(tb);
        }

        private Model.ConfigInfo getModel(DataTable dt)
        {
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            if (dt.Rows.Count >1)
            {
                throw new Exception("查找数据不唯一，无法实例化配置信息！");
            }

            Model.ConfigInfo cfg = new Model.ConfigInfo();

            if (dt.Rows[0][COLUMN_DBID] != null)
            {
                cfg.DBID = long.Parse(dt.Rows[0][COLUMN_DBID].ToString());
            }

            if (dt.Rows[0][COLUMN_DEVID] != null)
            {
                cfg.DEVID = dt.Rows[0][COLUMN_DEVID].ToString();
            }

            if (dt.Rows[0][COLUMN_FRAMECONTENT] != null)
            {
                cfg.FRAMECONTENT = (string)dt.Rows[0][COLUMN_FRAMECONTENT].ToString();
            }

            if (dt.Rows[0][COLUMN_WRITETIME] != null)
            {
                cfg.WRITETIME = (DateTime)dt.Rows[0][COLUMN_WRITETIME];
            }

            if (dt.Rows[0][COLUMN_SENDTIME] != null)
            {
                cfg.SENDTIME = (DateTime)dt.Rows[0][COLUMN_SENDTIME];
            }

            if (dt.Rows[0][COLUMN_STATUS] != null)
            {
                if ("1".Equals(dt.Rows[0][COLUMN_STATUS].ToString()))
                {
                    cfg.STATUS = true;
                }
                if ("0".Equals(dt.Rows[0][COLUMN_STATUS].ToString()))
                {
                    cfg.STATUS = false;
                }
            }

            return cfg;
        }

        private void SetAdapterParameters(OracleParameter[] parms, string devId)
        {
            if (string.IsNullOrEmpty(devId))
            {
                parms[0].Value = DBNull.Value;
            }
            else
            {
                parms[0].Value = devId;
            }
        }

        /// <summary>
        /// An internal function to get the database parameters
        /// </summary>
        /// <returns>Parameter array</returns>
        private static OracleParameter[] GetAdapterParameters()
        {

            OracleParameter[] parms = OracleHelperParameterCache.GetCachedParameterSet(SQL_SELECT_CFG_DEV, "SELECT");

            if (parms == null)
            {
                parms = new OracleParameter[] { new OracleParameter(PARM_DEVID, OracleType.VarChar, 255) };
                OracleHelperParameterCache.CacheParameterSet(SQL_SELECT_CFG_DEV, "SELECT", parms);
            }

            return parms;
        }
    }
}
