
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class LogInfo
    {
        private long dbId;
        private string equipment;
        private string operate;
        private System.DateTime operate_time;
        private string username;
        private long patroler_id;

        public long DbId
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public string Equipment
        {
            get { return equipment; }
            set { equipment = value; }
        }

        public string Operate
        {
            get { return operate; }
            set { operate = value; }
        }

        public System.DateTime Operate_time
        {
            get { return operate_time; }
            set { operate_time = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public long Patroler_id
        {
            get { return patroler_id; }
            set { patroler_id = value; }
        }
    }
}
