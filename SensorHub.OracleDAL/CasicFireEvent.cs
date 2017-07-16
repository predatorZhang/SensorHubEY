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
    public class CasicFireEvent:ICasicFireEvent
    {
        public Model.CasicFireEvent isFireUp(String cmd)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionStringOrderDistributedTransaction))
                {
                    String SQL = " SELECT t.STATUS,t.VERSION" +
                                 " FROM ALARM_FIREUP_EVENT t" +
                                 " WHERE t.CMD = :CMD ";
                    OracleParameter[] oraParams = new OracleParameter[]{
                                new OracleParameter(":CMD", cmd),
                            };
                    DataTable dt = OracleHelper.ExecuteDataset(conn, CommandType.Text, SQL, oraParams).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Model.CasicFireEvent fireEvent = new Model.CasicFireEvent();

                        if (null != dt.Rows[0]["STATUS"])
                        {
                            fireEvent.Status = dt.Rows[0]["STATUS"].ToString();

                        }
                        if (null != dt.Rows[0]["VERSION"])
                        {
                            fireEvent.Version = dt.Rows[0]["VERSION"].ToString();

                        }
                        return fireEvent;
                    }
                }
            }
            catch (Exception e)
            {
                String ex = e.Message;
            }
            return null;
           
        }

        public void fireOff(String cmd)
        {
            try
            {
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":CMD",cmd)
                };
                string SQL = " UPDATE ALARM_FIREUP_EVENT "
                           + " SET "
                           + " STATUS = 0 "
                           + " where "
                           + " CMD = :CMD ";

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
