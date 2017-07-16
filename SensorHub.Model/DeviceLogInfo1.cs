using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DeviceLogInfo1
    {
        private long dbid;
        private string devcode;
        private string devType;
        private string message;
        private string operateType;
        private DateTime logTime;

        public long DBID
        {
            get { return dbid; }
            set { dbid = value; }
        }

        public string DEVTYPE
        {
            get { return devType; }
            set { devType = value; }
        }

        public string DEVICECODE
        {
            get { return devcode; }
            set { devcode = value; }
        }

        public string MESSAGE
        {
            get { return message; }
            set { message = value; }
        }

        public string OPERATETYPE
        {
            get { return operateType; }
            set { operateType = value; }
        }

        public DateTime LOGTIME
        {
            get { return logTime; }
            set { logTime = value; }
        }
    }
}
