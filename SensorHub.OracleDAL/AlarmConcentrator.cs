using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using SensorHub.Model;

namespace SensorHub.OracleDAL
{
    public class AlarmConcentrator : IDAL.IAlarmConcentrator
    {
        private string concenCode;
        private string location;
        private DateTime lastTime;
        private int status;
        private int active;

        public void setHubOffLine(string concenCode)
        {
            try
            {
                string SQL = " UPDATE ALARM_CONCENTRATOR "
                            + " SET "
                            + " STATUS = " + 0
                            + " where "
                            + " CONCENCODE = " + concenCode;
                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void setHubOnLine(string concenCode)
        {
            try
            {
                string SQL = " UPDATE ALARM_CONCENTRATOR "
                            + " SET "
                            + " STATUS = " + 1
                            + ", LASTTIME = to_date('" + DateTime.Now
                            + "','yyyy-mm-dd hh24:mi:ss') where "
                            + "CONCENCODE = " + concenCode;
                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, SQL);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
