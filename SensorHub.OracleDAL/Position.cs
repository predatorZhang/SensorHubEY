using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class Position : SensorHub.IDAL.IPosition
    {
        public void insertPosition(Model.Position position)
        {
            String sql="";
            try
            {

                if (position.Task_id<0)
                {

                     sql = " insert into Position "
                           + " (LATITUDE,LONGITUDE,RECEIVETIME,USERNAME,PATROLER_ID,TASK_ID)VALUES("
                           + position.Latitude + ","
                           + position.Longitude + ","
                           + "to_date('" + position.ReceiveTime.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-dd hh24:mi:ss')" + ","
                           + "'" + position.Username + "',"
                           + position.Patroler_id + ","
                           + position.Task_id + ")";

                }
                else
                {

                     sql = " insert into Position "
                              + " (LATITUDE,LONGITUDE,RECEIVETIME,USERNAME,PATROLER_ID)VALUES("
                              + position.Latitude + ","
                              + position.Longitude + ","
                              + "to_date('" + position.ReceiveTime.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-dd hh24:mi:ss')" + ","
                              + "'" + position.Username + "',"
                              + position.Patroler_id + ")";

                } 
                OracleParameter[] parms = null;          
                string strCnn = OracleHelper.ConnectionStringOrderDistributedTransaction;
                using (OracleConnection cnn = new OracleConnection(strCnn))
                {
                    OracleHelper.ExecuteNonQuery(cnn, CommandType.Text, sql, parms);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
