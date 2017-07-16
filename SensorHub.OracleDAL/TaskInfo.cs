using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class TaskInfo : SensorHub.IDAL.ITaskInfo
    {
        public List<Model.TaskInfo> getTaskInfoByNameAndTwoStates(String name, String state1, String state2)
        {
            try
            {
                String sql = " select * "
                           + " from TASKINFO "
                           + " where userName = '" + name + "'"
                           + " and (taskState = '" + state1 + "'"
                           + " or taskState = '" + state2 + "')";
                OracleParameter[] parms = null;
                DataTable tb = OracleHelper.ExecuteDataset(OracleHelper.ConnectionStringOrderDistributedTransaction,
                               CommandType.Text, sql, parms).Tables[0];
                return getTaskInfoModelList(tb);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void setTaskInfo(Model.TaskInfo taskInfo)
        {
            try
            {
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":TASKSTATE",taskInfo.TaskState),
                new OracleParameter(":dbid",taskInfo.DbId),

                };
                string SQL = " UPDATE TASKINFO "
                           + " SET "
                           + " TASKSTATE = :TASKSTATE "
                           + " where "
                           + " dbid=:dbid ";

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

        public Model.TaskInfo getTaskInfo(string username,string state)
        {            
            try
            {
                String sql = "select * from taskinfo where userName=:username and taskState=:taskstate order by deploytime desc";
                OracleParameter[] parms = new OracleParameter[] { 
                    new OracleParameter(":username", username),
                    new OracleParameter(":taskstate", state)};




                //Finally execute the query
                using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql, parms))
                {
                    while (rdr.Read())
                    {
                        Model.TaskInfo task;

                        task = GetTaskInfoByOracleDataReader(rdr);
                        if (task != null)
                        {
                            //bySearch.Add(deviceConfig);
                            return task;
                        }
                    }
                }

                return null;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public Model.TaskInfo getTaskInfo(long dbid)
        {
            try
            {
                String sql = "select * from taskinfo where dbid=:dbid";
                OracleParameter[] parms = new OracleParameter[] { new OracleParameter(":dbid", dbid) };




                //Finally execute the query
                using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql, parms))
                {
                    while (rdr.Read())
                    {
                        Model.TaskInfo task;

                        task = GetTaskInfoByOracleDataReader(rdr);
                        if (task != null)
                        {
                            //bySearch.Add(deviceConfig);
                            return task;
                        }
                    }
                }

                return null;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private Model.TaskInfo GetTaskInfoByOracleDataReader(OracleDataReader rdr)
        {
            string[] colsName;
            int[] colsOrdinal;
            const int colsCount = 8;

            Model.TaskInfo task;


            colsName = new string[colsCount] { 
                                            "DBID", "DEPLOYTIME", "DESCIRPTION",
                                            "FINISHTIME","STREET","TASKSTATE",
                                            "USERNAME","PATROLER_ID"};
            colsOrdinal = new int[colsCount];

            if (GetColumnsOrdinal(rdr, colsName, colsOrdinal) < 0)
            {
                throw (new Exception("Error"));
            }

            task = new Model.TaskInfo();

            for (int i = 0; i < colsCount; i++)
            {
                if (colsOrdinal[i] < 0 || rdr.IsDBNull(colsOrdinal[i]))
                {
                    continue;
                }
                switch (i)
                {
                    case 0:
                        task.DbId = rdr.GetInt32(colsOrdinal[i]);
                        break;
                    case 1:
                        task.DeployTime = rdr.GetDateTime(colsOrdinal[i]);
                        break;
                    case 2:
                        task.Description = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 3:
                        task.FinishTime = rdr.GetDateTime(colsOrdinal[i]);
                        break;
                    case 4:
                        task.Street = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 5:
                        task.TaskState = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 6:
                        task.UserName = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 7:
                        if (rdr.GetInt32(colsOrdinal[i]) != null)
                        {
                            task.Patroler_id = rdr.GetInt32(colsOrdinal[i]);
                        }
                        break;
                }
            }

            return task;
        }

        private int GetColumnsOrdinal(OracleDataReader rdr, string[] colsName, int[] colsOrdinal)
        {
            for (int i = 0; i < colsName.Length; i++)
            {
                colsOrdinal[i] = rdr.GetOrdinal(colsName[i]);
            }

            return 0;
        }

        private List<Model.TaskInfo> getTaskInfoModelList(DataTable dt)
        {
            List<Model.TaskInfo> taskInfoList = new List<Model.TaskInfo> ();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Model.TaskInfo taskInfo = new Model.TaskInfo();

                if (dt.Rows[i]["DBID"] != null) {
                    taskInfo.DbId = long.Parse(dt.Rows[i]["DBID"].ToString());
                }

                if (dt.Rows[i]["DEPLOYTIME"] != null) {
                    taskInfo.DeployTime = (DateTime) dt.Rows[i]["DEPLOYTIME"];
                }

                if (dt.Rows[i]["DESCIRPTION"] != null) {
                    taskInfo.Description = dt.Rows[i]["DESCIRPTION"].ToString();
                }

                if (dt.Rows[i]["FINISHTIME"] != null && dt.Rows[i]["FINISHTIME"].Equals(""))
                {
                    taskInfo.FinishTime = (DateTime) dt.Rows[i]["FINISHTIME"];
                }

                if (dt.Rows[i]["STREET"] != null) {
                    taskInfo.Street = dt.Rows[i]["STREET"].ToString();
                }

                if (dt.Rows[i]["TASKSTATE"] != null) {
                    taskInfo.TaskState = dt.Rows[i]["TASKSTATE"].ToString();
                }

                if (dt.Rows[i]["USERNAME"] != null) {
                    taskInfo.UserName = dt.Rows[i]["USERNAME"].ToString();
                }

                if (dt.Rows[i]["PATROLER_ID"] != null) {
                    taskInfo.Patroler_id = long.Parse(dt.Rows[i]["PATROLER_ID"].ToString());
                }

                taskInfoList.Add(taskInfo);
            }

            return taskInfoList;
        }
    }
}
