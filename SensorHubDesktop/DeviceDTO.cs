using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
namespace SensorHubDesktop
{
    public class DeviceDTO
    {
        //设备名称
        private String name;
        //设备厂商;
        private String company;
        //设备类型
        private String typeName;
        //设备携带配置信息
        private String tag;
        //会话ID
        private String sessionId;

        private String status;

        private String serverName;

        public String ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Company
        {
            get { return company; }
            set { company = value; }
        }

        public String TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public String Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public String SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public String Status
        {
            get { return status; }
            set { status = value; }
        }
        public static DataTable ConvertToTables(IList<DeviceDTO> devs)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Company");
            dt.Columns.Add("TypeName");
            dt.Columns.Add("Tag");
            dt.Columns.Add("SessionId");
            dt.Columns.Add("ServerName");
            dt.Columns.Add("Status");
          
            foreach(DeviceDTO dev in devs)
            {
                DataRow dr = dt.NewRow();
                dr["Name"]=dev.Name;
                dr["Company"] = dev.Company;
                dr["TypeName"] = dev.TypeName;
                dr["Tag"] = dev.Tag;
                dr["SessionId"] = dev.SessionId;
                dr["ServerName"] = dev.ServerName;
                dr["Status"] =  dev.Status;
                dt.Rows.Add(dr);
               
            }
            return dt;
        }
    }
}


  