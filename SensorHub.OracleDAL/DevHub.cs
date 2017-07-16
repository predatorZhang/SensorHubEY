using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace SensorHub.OracleDAL
{
    public class DevHub : SensorHub.IDAL.IDevHub
    {
        public List<Model.DevHubInfo> getAll()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
                {
                    String SQL = "SELECT t.DEVCODE,t.CONCENCODE FROM  ALARM_DEVICE_CONCENTRATOR t WHERE ACTIVE = 1";

                    DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, null).Tables[0];
                    List<Model.DevHubInfo> devHubs = new List<Model.DevHubInfo>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Model.DevHubInfo devHub = new Model.DevHubInfo();
                            if (null != dt.Rows[i]["DEVCODE"])
                            {
                                devHub.DevCode = dt.Rows[i]["DEVCODE"].ToString();
                            }

                            if (null != dt.Rows[i]["CONCENCODE"])
                            {
                                devHub.HubCode = dt.Rows[i]["CONCENCODE"].ToString();
                            }
                            devHubs.Add(devHub);
                        }

                    }
                    return devHubs;
                }
            }
            catch (Exception e)
            {
                String ss = e.Message;
                return null;
            }
        }

        public List<Model.DevHubInfo> getAllRealTimeDT()
        {
            try
            {
                using (
                    OracleConnection conn =
                        new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
                {
                    String SQL =
                        "SELECT t.DEVCODE,t.HUBCODE FROM  DEVREALDATA t WHERE STATUS = 1";
                    DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, null).Tables[0];
                    List<Model.DevHubInfo> devHubs = new List<Model.DevHubInfo>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Model.DevHubInfo devHub = new Model.DevHubInfo();
                            if (null != dt.Rows[i]["DEVCODE"])
                            {
                                devHub.DevCode = dt.Rows[i]["DEVCODE"].ToString();
                            }

                            if (null != dt.Rows[i]["HUBCODE"])
                            {
                                devHub.HubCode = dt.Rows[i]["HUBCODE"].ToString();
                            }

                            devHubs.Add(devHub);
                        }

                    }
                    return devHubs;
                }
            }
            catch (Exception e)
            {
                String ss = e.Message;
                return null;
            }
        }

        public void changeRealSerachDevStatus(String devCode)
        {
            try
            {
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":DEVCODE",devCode)
                };
                string SQL = " UPDATE DEVREALDATA "
                           + " SET "
                           + " STATUS = '"+ 0
                           + "' where "
                           + " DEVCODE = :DEVCODE ";

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

        public void changeOnLineStatus(String devCode, bool isOnline)
        {
            try
            {
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":DEVCODE",devCode),
                new OracleParameter(":STATUS",isOnline)
                };
                string SQL = " UPDATE ALARM_DEVICE_CONCENTRATOR "
                           + " SET "
                           + " STATUS = :STATUS "
                           + " where "
                           + " DEVCODE = :DEVCODE "
                           + " AND ACTIVE = 1";

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
        public void setOffLine()
        {
            try
            {
                string SQL = " UPDATE ALARM_DEVICE_CONCENTRATOR "
                           + " SET "
                           + " STATUS = 0 "
                           + " where "
                           + " ACTIVE = 1";

                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL, null);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
