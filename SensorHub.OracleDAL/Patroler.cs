using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class Patroler : SensorHub.IDAL.IPatroler
    {
        public Model.Patroler getPatrolerByName(string name)
        {            
            try
            {
                String sql = "select * from PATROLER where username=:name2";
                OracleParameter[] parms = new OracleParameter[]{new OracleParameter(":name2", name)};
                //Finally execute the query
                using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql, parms))
                {
                    while (rdr.Read())
                    {
                        Model.Patroler patroler;

                        patroler = GetPatrolerByOracleDataReader(rdr);
                        if (patroler != null)
                        {
                            return patroler;
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

        private Model.Patroler GetPatrolerByOracleDataReader(OracleDataReader rdr)
        {
            string[] colsName;
            int[] colsOrdinal;
            const int colsCount = 8;

            Model.Patroler patroler;


            colsName = new string[colsCount] { 
                                            "DBID", "ACCOUNTSTATE", "AGE",
                                            "PASSWORD","PHONENUM","SEX",
                                            "USERNAME","REGION_ID"};
            colsOrdinal = new int[colsCount];

            if (GetColumnsOrdinal(rdr, colsName, colsOrdinal) < 0)
            {
                throw (new Exception("Error"));
            }

            patroler = new Model.Patroler();

            for (int i = 0; i < colsCount; i++)
            {
                if (colsOrdinal[i] < 0 || rdr.IsDBNull(colsOrdinal[i]))
                {
                    continue;
                }
                switch (i)
                {
                    case 0:
                        patroler.DbId = rdr.GetInt32(colsOrdinal[i]);
                        break;
                    case 1:
                        patroler.AccountState = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 2:
                        patroler.Age = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 3:
                        patroler.Password = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 4:
                        patroler.PhoneNum = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 5:
                        patroler.Sex = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 6:
                        patroler.UserName = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 7:
                        if (rdr.GetInt32(colsOrdinal[i]) != null)
                        {
                            patroler.Region_id = rdr.GetInt32(colsOrdinal[i]);
                        }
                        break;
                }
            }

            return patroler;
        }

        private int GetColumnsOrdinal(OracleDataReader rdr, string[] colsName, int[] colsOrdinal)
        {
            for (int i = 0; i < colsName.Length; i++)
            {
                colsOrdinal[i] = rdr.GetOrdinal(colsName[i]);
            }

            return 0;
        }

        public bool isExist(string name)
        {
            string sql = "select count(*) from username where username=:name";
            OracleParameter[] parms = {new OracleParameter(":name",name)};
            object obj = OracleHelper.ExecuteScalar(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql, parms);
            if (null != obj && int.Parse(obj.ToString()) > 0)
            {
                return true;
            }
            return false;
        }

        public void setPatrolerAccountStateByName(Model.Patroler patroler)
        {
            try
            {
                OracleParameter[] parms = new OracleParameter[]{
                new OracleParameter(":ACCOUNTSTATE",patroler.AccountState),
                new OracleParameter(":username",patroler.UserName),

                };
                string SQL = " UPDATE PATROLER "
                           + " SET "
                           + " ACCOUNTSTATE = :ACCOUNTSTATE "
                           + " where "
                           + " username = :username ";

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

        private List<Model.Patroler> getTaskInfoModelList(DataTable dt)
        {
            List<Model.Patroler> patrolerList = new List<Model.Patroler>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Model.Patroler patroler = new Model.Patroler();

                if (dt.Rows[i]["DBID"] != null)
                {
                    patroler.DbId = long.Parse(dt.Rows[i]["DBID"].ToString());
                }

                if (dt.Rows[i]["ACCOUNTSTATE"] != null)
                {
                    patroler.AccountState = dt.Rows[i]["ACCOUNTSTATE"].ToString();
                }

                if (dt.Rows[i]["AGE"] != null)
                {
                    patroler.Age = dt.Rows[i]["AGE"].ToString();
                }

                if (dt.Rows[i]["PASSWORD"] != null)
                {
                    patroler.Password = dt.Rows[i]["PASSWORD"].ToString();
                }

                if (dt.Rows[i]["PHONENUM"] != null)
                {
                    patroler.PhoneNum = dt.Rows[i]["PHONENUM"].ToString();
                }

                if (dt.Rows[i]["SEX"] != null)
                {
                    patroler.Sex = dt.Rows[i]["SEX"].ToString();
                }

                if (dt.Rows[i]["USERNAME"] != null)
                {
                    patroler.UserName = dt.Rows[i]["USERNAME"].ToString();
                }

                if (dt.Rows[i]["REGION_ID"] != null )
                    if (dt.Rows[i]["REGION_ID"].ToString() == "")
                    {
                        patroler.Region_id = 0;
                    }
                    else {
                        patroler.Region_id = long.Parse(dt.Rows[i]["REGION_ID"].ToString());
                    }

                patrolerList.Add(patroler);
            }

            return patrolerList;
        }
    }
}
