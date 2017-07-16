using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace SensorHub.OracleDAL
{
    public class Equipment : SensorHub.IDAL.IEquipment
    {
        public Model.Equipment getEquipmentByMacId(string macId)
        {
            try
            {
                String sql = "select * from equipment where macid=:macid";
                OracleParameter[] parms = new OracleParameter[] { new OracleParameter(":macid", macId) };
                using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql, parms))
                {
                    while (rdr.Read())
                    {
                        Model.Equipment equipment;

                        equipment = GetEquipmentByOracleDataReader(rdr);
                        if (equipment != null)
                        {
                            //bySearch.Add(deviceConfig);
                            return equipment;
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

        private Model.Equipment GetEquipmentByOracleDataReader(OracleDataReader rdr)
        {
            string[] colsName;
            int[] colsOrdinal;
            const int colsCount = 5;

            Model.Equipment equipment;


            colsName = new string[colsCount] { 
                                            "DBID", "DESCIRPTION", "MACID",
                                            "OWNER","STATUS"};
            colsOrdinal = new int[colsCount];

            if (GetColumnsOrdinal(rdr, colsName, colsOrdinal) < 0)
            {
                throw (new Exception("Error"));
            }

            equipment = new Model.Equipment();

            for (int i = 0; i < colsCount; i++)
            {
                if (colsOrdinal[i] < 0 || rdr.IsDBNull(colsOrdinal[i]))
                {
                    continue;
                }
                switch (i)
                {
                    case 0:
                        equipment.DbId = rdr.GetInt32(colsOrdinal[i]);
                        break;
                    case 1:
                        equipment.Description = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 2:
                        equipment.MacId = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 3:
                        equipment.Owner = rdr.GetString(colsOrdinal[i]);
                        break;
                    case 4:
                        equipment.Status = rdr.GetString(colsOrdinal[i]);
                        break;
                }
            }

            return equipment;
        }

        private int GetColumnsOrdinal(OracleDataReader rdr, string[] colsName, int[] colsOrdinal)
        {
            for (int i = 0; i < colsName.Length; i++)
            {
                colsOrdinal[i] = rdr.GetOrdinal(colsName[i]);
            }

            return 0;
        }

        public bool isExist(string macId)
        {
            string sql = "select count(*) from equipment where macid=:id";
            OracleParameter[] parms = { new OracleParameter(":id", macId) };
            object obj = OracleHelper.ExecuteScalar(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql, parms);
            if (null != obj && int.Parse(obj.ToString()) > 0)
            {
                return true;
            }
            return false;
        }

        private List<Model.Equipment> getEquipmentList(DataTable dt)
        {
            List<Model.Equipment> equipmentList = new List<Model.Equipment>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Model.Equipment equipment = new Model.Equipment();

                if (dt.Rows[i]["DBID"] != null)
                {
                    equipment.DbId = long.Parse(dt.Rows[i]["DBID"].ToString());
                }

                if (dt.Rows[i]["MACID"] != null)
                {
                    equipment.MacId = dt.Rows[i]["MACID"].ToString();
                }

                if (dt.Rows[i]["OWNER"] != null)
                {
                    equipment.Owner = dt.Rows[i]["OWNER"].ToString();
                }

                if (dt.Rows[i]["STATUS"] != null)
                {
                    equipment.Status = dt.Rows[i]["STATUS"].ToString();
                }

                if (dt.Rows[i]["DESCIRPTION"] != null)
                {
                    equipment.Description = dt.Rows[i]["DESCIRPTION"].ToString();
                }

                equipmentList.Add(equipment);
            }

            return equipmentList;
        }
    }
}
